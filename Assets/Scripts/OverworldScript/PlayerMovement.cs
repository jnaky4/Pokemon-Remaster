using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pokemon
{
    public class PlayerMovement : MonoBehaviour
    {
        //public float moveSpeed = 5f;
        public float moveSpeed;
        public LayerMask solidObjectsLayer;
        public LayerMask interactableLayer;
        public LayerMask grassLayer;
        public VectorValue startingPosition;
        public LayerMask boundary;
        public string location = "Route 1";

        //public Rigidbody2D rb;
        public Animator animator;

        private bool isMoving;
        Vector2 movement;

        private void Awake()
        {
            Pokemon.all_base_stats = BattleSystem.load_CSV("BASE_STATS");
            Moves.all_moves = BattleSystem.load_CSV("MOVES");
            Type.type_attack = BattleSystem.load_CSV("TYPE_ATTACK");
            Type.type_defend = BattleSystem.load_CSV("TYPE_DEFEND");
            Learnset.all_learnset = BattleSystem.load_CSV("LEARNSET");
            Pokedex.all_pokedex = BattleSystem.load_CSV("POKEMON");
            Route.all_routes = BattleSystem.load_CSV("ROUTES");
            Type.load_type();
            Moves.load_moves();

            animator = GetComponent<Animator>();
            transform.position = startingPosition.initialValue;
        }

        // Update is called once per frame
        void Update()
        {
            if (!isMoving)
            {
                // Input
                movement.x = Input.GetAxisRaw("Horizontal");
                movement.y = Input.GetAxisRaw("Vertical");

                if (movement.x != 0) movement.y = 0;

                if (movement != Vector2.zero)
                {
                    animator.SetFloat("Horizontal", movement.x);
                    animator.SetFloat("Vertical", movement.y);

                    var targetPos = transform.position;
                    targetPos.x += movement.x;
                    targetPos.y += movement.y;

                    if (IsWalkable(targetPos))
                        StartCoroutine(Move(targetPos));
                }
            }

            animator.SetBool("IsMoving", isMoving);

            if (Input.GetKeyDown(KeyCode.Z))
                Interact();
        }

        void Interact()
        {
            var faceDir = new Vector3(animator.GetFloat("Horizontal"), animator.GetFloat("Vertical"));
            var interactPos = transform.position + faceDir;

            //Debug.DrawLine(transform.position, interactPos, Color.green, 0.5f);
            var collider = Physics2D.OverlapCircle(interactPos, 0.3f, interactableLayer);
            if (collider != null)
            {
                collider.GetComponent<Interactable>()?.Interact();
            }
        }

        IEnumerator Move(Vector3 targetPos)
        {
            isMoving = true;

            while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = targetPos;

            isMoving = false;

            CheckForEncounters();
        }

        private bool IsWalkable(Vector3 targetPos)
        {
            if (Physics2D.OverlapCircle(targetPos, 0.125f, solidObjectsLayer | interactableLayer) != null)
            {
                return false;
            }
            return true;
        }

        private void CheckForEncounters()
        {
            if (Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null)
            {
                if (Random.Range(1, 101) <= 10)
                {
                    Dictionary<string, Route> route1_dic = Route.get_route(location);
                    string terrain = "Grass";
                    //list of spcific completed badges
                    Dictionary<string, int> badges_completed = new Dictionary<string, int>()
                    {
                    };

                    Pokemon wild_spawn = generate_wild_pokemon(route1_dic, terrain, badges_completed, badges_completed.Count);
                    Debug.Log("Wild Pokemon! " + wild_spawn.name);
                    Debug.Log("Level: " + wild_spawn.level);
                    //SceneManager.LoadScene("BattleScene");
                }
            }
        }

        //Gets all available pokemon to spawn from dictionary, Current # gym badges, list of specific Gyms Beaten
        Pokemon generate_wild_pokemon(Dictionary<string, Route> route, string terrain, Dictionary<string, int> Gyms_beaten, int badges)

        {
            //dictionary of gyms beaten
            double sum_probability = 0;
            Dictionary<string, Route> possible_spawns = new Dictionary<string, Route>();
            List<Dictionary<string, Route>> final_list = new List<Dictionary<string, Route>>();

            //make a new dictionary of possible spawning pokemon 
            foreach (KeyValuePair<string, Route> wild_spawn in route)
            {


                if (
                    //required badges for pokemon spawn less than or equal to current player badges
                    (wild_spawn.Value.required_badges <= badges
                    ||
                    //either they have the gym required, or the pokemon spawns at any
                    (Gyms_beaten.ContainsKey(wild_spawn.Value.gym_available.ToString())
                    ||
                    wild_spawn.Value.gym_available.ToString() == "any"))
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


            double temp = 0;
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
                    int level_cap = ((badges * 10) + 10);

                    if (level_cap > wild_spawn.Value.cap)
                    {
                        level_cap = wild_spawn.Value.cap;
                    }
                    level_cap += random_level;

                    Pokemon temp_pokemon = new Pokemon(wild_spawn.Value.dexnum, level_cap);
                    return temp_pokemon;
                }

            }

            //If no pokemon found to spawn? spawn a lvl 69 Slowbro
            Pokemon temp_pokemon2 = new Pokemon(80, 69);
            return temp_pokemon2;
        }

        /*private void FixedUpdate()
        {
            // Movement
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }*/
    }
}
