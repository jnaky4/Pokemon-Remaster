using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

namespace Pokemon {

    
    public class Main : MonoBehaviour
    {
        Pokemon wildPokemon;
        void Start()
        {
            
            Pokemon.all_base_stats = load_CSV("BASE_STATS");
            Moves.all_moves = load_CSV("MOVES");
            Type.type_attack = load_CSV("TYPE_ATTACK");
            Type.type_defend = load_CSV("TYPE_DEFEND");
            Learnset.all_learnset = load_CSV("LEARNSET");
            Pokedex.all_pokedex = load_CSV("POKEMON");
            Route.all_routes = load_CSV("ROUTES");
            Type.load_type();
            Moves.load_moves();



            //Random Pokemon Example

            //creates a dictionary for the route
            Dictionary<string,Route> route1_dic = Route.get_route("Route 2");
            
            string terrain = "Grass";
            //list of spcific completed badges
            Dictionary<string, int> badges_completed = new Dictionary<string, int>()
            {{"Rock",1},{"Water",1}//,{"Electric",1},{"Grass",1},{"Poison",1},{"Psychic",1},{"Fire",1},{"Ground",1}
            };

            //reoute1_dic: Dictionary<string, int> Generate a wild pokemon based on dictionary
            //terrain: string: filters available pokemon based on terrain spawned from
            //badges_completed: int: Calculate lvl based on 
            //num_gym_completed: int: filters available pokemon
            Pokemon wild_spawn = generate_wild_pokemon(route1_dic, terrain, badges_completed, badges_completed.Count);
            Debug.Log("Wild Pokemon! " + wild_spawn.name);
            Debug.Log("Wild Level: " + wild_spawn.level);



            //DEBUGGING
            //this.wildPokemon = new Pokemon(1, 50);


            //Debug.Log("all_learnset Count " + Learnset.all_learnset.Count);
            //Debug.Log("learnset Dict Count " + Learnset.get_learnset.Count);


            //print_pokemon();
            //print_moves();

        }

        // Update is called once per frame
        void Update()
        {

            //Debugging
            if (Input.GetMouseButtonDown(0))
            {
                //creates a dictionary for the route
                Dictionary<string, Route> route1_dic = Route.get_route("Route 2");


                string terrain = "Grass";
                //list of spcific completed badges
                Dictionary<string, int> badges_completed = new Dictionary<string, int>()
                {{"Rock",1},{"Water",1},{"Electric",1},{"Grass",1},{"Poison",1},{"Psychic",1},{"Fire",1},{"Ground",1}
                };

                Pokemon wild_spawn = generate_wild_pokemon(route1_dic, terrain, badges_completed, badges_completed.Count);
                Debug.Log("Wild Pokemon! " + wild_spawn.name);
                Debug.Log("Wild Level: " + wild_spawn.level);


                //Debugging

/*                int temp = (wildPokemon.dexnum % 151) + 1;
                //Debug.Log(temp);
                this.wildPokemon = new Pokemon(temp, 50, "Wing Attack");
                Debug.Log(wildPokemon.currentMoves[0].move_type.attack_bug);
                Debug.Log(wildPokemon.name);*/

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
                    wild_spawn.Value.gym_available.ToString() == "any") )
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
            foreach(KeyValuePair<string, Route> wild_spawn in possible_spawns)
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
                if(random <= cumulativeProbability)
                {

                    
                    //level_min and level_max are negative values, ie take away this many levels from the level cap
                    int random_level = UnityEngine.Random.Range(wild_spawn.Value.level_min - 1, wild_spawn.Value.level_max) + 1;
                    //Debug.Log("Random Level: " + random_level);


                    // if level_cap > pokemon_cap 
                    // pokemon_level = pokemon_cap
                    // add in random_level takeaway from cap
                    int level_cap = ((badges * 10) + 10 );

                    if(level_cap > wild_spawn.Value.cap)
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

        List<Dictionary<string, object>> load_CSV(string name)
        {
            List<Dictionary<string, object>> data = CSVReader.Read(name);
            return data;
        }
        public void print_pokemon()
        {

            for (int i = 1; i <= 151; i++)
            {

                Pokemon test = new Pokemon(6, 50);

                Pokemon TrainerPokemon = new Pokemon(i, 50, "Flamethrower", "Earthquake", "Wing Attack", "Slash");
                Pokemon WildPokemon = new Pokemon(i, 50, "Flamethrower", "Earthquake", "Wing Attack", "Slash");

                /*                Debug.Log("Name " + TestPokemon.name);
                                Debug.Log("Base Attack " + TestPokemon.base_attack + " Current Attack " + TestPokemon.current_attack);
                                Debug.Log("Type1: " + TestPokemon.type1.type);
                                if (TestPokemon.type2 != null)
                                {
                                    Debug.Log("Type2: " + TestPokemon.type2.type);
                                }*/

                foreach (Learnset learned in TrainerPokemon.learnset)
                {
                    Debug.Log(learned.ToString());
                    Debug.Log("PP " + learned.get_move().pp);
                    Debug.Log("TYPE " + learned.get_move().move_type.type);

                }
                    
            }
        }
        public void print_moves()
        {
            foreach(KeyValuePair<string, Moves> move in Moves.move_dictionary)
            {
                Debug.Log(move.Key);
            }
        }

    }
}
