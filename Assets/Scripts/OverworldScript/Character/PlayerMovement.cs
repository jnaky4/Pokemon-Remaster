using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon
{
    public class PlayerMovement : MonoBehaviour
    {
        //normal spawnrate
        //private int spawnRate = 10;

        //uncomment this for high spawn rate
        private int spawnRate = 101;

        //uncomment this for no spawn rate
        //private int spawnRate = 0;

        public float moveSpeed;
        public VectorValue startingPosition;
        public string location = "Route 1";
        public GameObject dialogBox;

        private Character character;

        //public static DialogController Instance { get; private set; }

        private Vector2 movement;

        private void Awake()
        {
            character = GetComponent<Character>();
            transform.position = startingPosition.initialValue;
        }

        // Update is called once per frame
        private void Update()
        {
            if (!character.IsMoving)
            {
                // Input
                movement.x = Input.GetAxisRaw("Horizontal");
                movement.y = Input.GetAxisRaw("Vertical");

                //prevents diagonal movement
                if (movement.x != 0) movement.y = 0;

                //find the next target position when a player attempts to move
                if (movement != Vector2.zero && !GameController.inCombat && GameController.state == GameState.Roam)
                {
                    //makes sure an area is walkable before allowing a player move
                    StartCoroutine(character.Move(movement, OnMoveOver));
                }
            }

            character.HandleUpdate();

            if (Input.GetKeyDown(KeyCode.Z) && !GameController.inCombat && GameController.state == GameState.Roam)
            {
                Debug.Log("Pressed Z in the player controller");
                Interact();
            }
        }

        private void Interact()
        {
            var faceDir = new Vector3(character.Animator.MoveX, character.Animator.MoveY);
            var interactPos = transform.position + faceDir;

            var collider = Physics2D.OverlapCircle(interactPos, 0.3f, GameplayLayers.i.InteractLayer);
            if (collider != null)
            {
                //Debug.Log("Pressed Z in player");
                collider.GetComponent<Interactable>()?.Interact(transform);
            }
        }

        private void OnMoveOver()
        {
            CheckForEncounters();
            CheckIfInTrainerView();
        }

        private void CheckForEncounters()
        {
            if (Physics2D.OverlapCircle(transform.position, 0.1f, GameplayLayers.i.GrassLayer) != null && !GameController.triggerCombat && !GameController.inCombat)
            {
                if (UnityEngine.Random.Range(1, 101) <= spawnRate)
                {
                    //new code
                    character.Animator.IsMoving = false;

                    Dictionary<string, Route> route1_dic = Route.get_route(location);
                    string terrain = "Grass";
                    //list of spcific completed badges

                    Pokemon wild_spawn = generate_wild_pokemon(route1_dic, terrain);
                    GameController.opponentPokemon[0] = wild_spawn;
                    GameController.music = "Battle Wild Begin";
                    GameController.isCatchable = true;
                    GameController.triggerCombat = true;

                    //overworldCam.SetActive(false);
                    //SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
                }
            }
        }

        private void CheckIfInTrainerView()
        {
            //string trainer;
            var collider = Physics2D.OverlapCircle(transform.position, 0.2f, GameplayLayers.i.FovLayer);

            if (collider != null && !GameController.triggerCombat && !GameController.inCombat)
            {
                var trainerInfo = collider.GetComponentInParent<TrainerController>();

                if (trainerInfo.isBeaten == false)
                {
                    GameController.state = GameState.TrainerEncounter;
                    GameController.music = "Encounter Trainer";
                    character.Animator.IsMoving = false;
                    StartCoroutine(trainerInfo.TriggerTrainerBattle(transform.position));
                }
            }
        }

        //Gets all available pokemon to spawn from dictionary, Current # gym badges, list of specific Gyms Beaten
        private Pokemon generate_wild_pokemon(Dictionary<string, Route> route, string terrain)
        {
            GameController.update_level_cap();
            int num_badges = GameController.badges_completed.Count;
            //dictionary of gyms beaten
            double sum_probability = 0;
            Dictionary<string, Route> possible_spawns = new Dictionary<string, Route>();
            List<Dictionary<string, Route>> final_list = new List<Dictionary<string, Route>>();

            //make a new dictionary of possible spawning pokemon
            foreach (KeyValuePair<string, Route> wild_spawn in route)
            {
                if (
                    //required badges for pokemon spawn less than or equal to current player badges
                    (wild_spawn.Value.required_badges <= num_badges)
                    ||
                    //either they have the gym required, or the pokemon spawns at any
                    (GameController.badges_completed.ContainsKey(wild_spawn.Value.gym_available.ToString()) || wild_spawn.Value.gym_available.ToString() == "any")
                    &&
                    //the pokemon spawns in the specified terrain
                    terrain == wild_spawn.Value.terrain
                    )
                {
                    sum_probability += wild_spawn.Value.spawn_chance;
                    possible_spawns.Add(wild_spawn.Key, wild_spawn.Value);
                }
            }

            //Debug.Log("Possible Spawns: " + possible_spawns.Count);

            //double temp = 0;
            //sum_probability: sum of chance of all pokemon that can spawn
            //possibile_spawns: Dictionary of <dexnum, Route object> of all pokemon that can spawn in that route after filtering
            foreach (KeyValuePair<string, Route> wild_spawn in possible_spawns)
            {
                wild_spawn.Value.spawn_chance /= sum_probability;
            }

            double random = UnityEngine.Random.Range(0.0f, 1.0f);
            //Debug.Log("Random: " + random);
            double cumulativeProbability = 0;

            /*
            cumulativeProbability += item.probability();
            if (p <= cumulativeProbability) {
            return item;
            */

            foreach (KeyValuePair<string, Route> wild_spawn in possible_spawns)
            {
                cumulativeProbability += wild_spawn.Value.spawn_chance;
                if (random <= cumulativeProbability)
                {
                    //level_min and level_max are negative values, ie take away this many levels from the level cap
                    int random_level = UnityEngine.Random.Range(wild_spawn.Value.level_min - 1, wild_spawn.Value.level_max) + 1;
                    //Debug.Log("Random Level: " + random_level);

                    // if level_cap > pokemon_cap
                    // pokemon_level = pokemon_cap
                    // add in random_level takeaway from cap
                    int level_cap = GameController.level_cap;

                    if (level_cap > wild_spawn.Value.cap)
                    {
                        level_cap = wild_spawn.Value.cap;
                    }
                    level_cap += random_level;

                    //new Pokemon(wild_spawn.Value.dexnum, level_cap);
                    Pokemon temp_pokemon = Learnset.add_wild_moves(wild_spawn.Value.dexnum, level_cap);

                    return temp_pokemon;
                }
            }

            //If no pokemon found to spawn? spawn a lvl 69 Slowbro
            Pokemon temp_pokemon2 = new Pokemon(80, 69, "Hyper Beam", "Flamethrower", "Psychic", "Recover");
            return temp_pokemon2;
        }
    }
}