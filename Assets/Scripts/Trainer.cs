using System.Collections.Generic;


namespace Pokemon
{
    public class Trainer
    {
        public static List<Dictionary<string, object>> all_trainers = new();
        public Pokemon[] trainer_team = new Pokemon[6];


        public string route;
        public string name;
        public string type;
        public int money_mult;
        public string intro_dialogue;
        public string exit_dialogue;

        public Trainer(string name, string type, int money_mult, string intro_dialogue, string exit_dialogue,
        Pokemon pokemon1 = null, Pokemon pokemon2 = null, Pokemon pokemon3 = null, Pokemon pokemon4 = null, Pokemon pokemon5 = null, Pokemon pokemon6 = null)
        {
            this.name = name;
            this.type = type;
            this.money_mult = money_mult;
            this.intro_dialogue = intro_dialogue;
            this.exit_dialogue = exit_dialogue;

            trainer_team[0] = pokemon1;
            trainer_team[1] = pokemon2;
            trainer_team[2] = pokemon3;
            trainer_team[3] = pokemon4;
            trainer_team[4] = pokemon5;
            trainer_team[5] = pokemon6;


        }

        //grabs a dictionary of trainers from the route, to add to route
        public static Dictionary<string, Trainer> get_route_trainers(string route)
        {
            Dictionary<string, Trainer> route_trainers = new();
            for (var i = 0; i < all_trainers.Count; i++)
            {
                if (route == Trainer.all_trainers[i]["Route"].ToString())
                {
                    Pokemon pokemon1 = null;
                    Pokemon pokemon2 = null;
                    Pokemon pokemon3 = null;
                    Pokemon pokemon4 = null;
                    Pokemon pokemon5 = null;
                    Pokemon pokemon6 = null;

                    if (int.Parse(Trainer.all_trainers[i]["P1_DexNum"].ToString()) != 0)
                    {
                        /*                        Debug.Log("DEXNUM " + Trainer.all_trainers[i]["P1_DexNum"].ToString());
                                                Debug.Log("LVL FROM MAX " + Trainer.all_trainers[i]["P1_Level_From_Max"].ToString());
                                                Debug.Log("MOVES " + Trainer.all_trainers[i]["P1_Moves"].ToString());
                                                Debug.Log("PERM " + Trainer.all_trainers[i]["P1_Perm_Moves"].ToString());*/
                        pokemon1 = make_trainer_pokemon(
                        int.Parse(Trainer.all_trainers[i]["P1_DexNum"].ToString()),
                        int.Parse(Trainer.all_trainers[i]["P1_Level_From_Max"].ToString()),
                        Trainer.all_trainers[i]["P1_Moves"].ToString(),
                        Trainer.all_trainers[i]["P1_Perm_Moves"].ToString()
                        );
                    }


                    if (int.Parse(Trainer.all_trainers[i]["P2_DexNum"].ToString()) != 0)
                    {
                        pokemon2 = make_trainer_pokemon(
                            int.Parse(Trainer.all_trainers[i]["P2_DexNum"].ToString()),
                            int.Parse(Trainer.all_trainers[i]["P2_Level_From_Max"].ToString()),
                            Trainer.all_trainers[i]["P2_Moves"].ToString(),
                            Trainer.all_trainers[i]["P2_Perm_Moves"].ToString()
                        );
                    }

                    if (int.Parse(Trainer.all_trainers[i]["P3_DexNum"].ToString()) != 0)
                    {
                        pokemon3 = make_trainer_pokemon(
                            int.Parse(Trainer.all_trainers[i]["P3_DexNum"].ToString()),
                            int.Parse(Trainer.all_trainers[i]["P3_Level_From_Max"].ToString()),
                            Trainer.all_trainers[i]["P3_Moves"].ToString(),
                            Trainer.all_trainers[i]["P3_Perm_Moves"].ToString()
                        );
                    }
                    if (int.Parse(Trainer.all_trainers[i]["P4_DexNum"].ToString()) != 0)
                    {
                        pokemon4 = make_trainer_pokemon(
                            int.Parse(Trainer.all_trainers[i]["P4_DexNum"].ToString()),
                            int.Parse(Trainer.all_trainers[i]["P4_Level_From_Max"].ToString()),
                            Trainer.all_trainers[i]["P4_Moves"].ToString(),
                            Trainer.all_trainers[i]["P4_Perm_Moves"].ToString()
                        );
                    }

                    if (int.Parse(Trainer.all_trainers[i]["P5_DexNum"].ToString()) != 0)
                    {
                        pokemon5 = make_trainer_pokemon(
                            int.Parse(Trainer.all_trainers[i]["P5_DexNum"].ToString()),
                            int.Parse(Trainer.all_trainers[i]["P5_Level_From_Max"].ToString()),
                            Trainer.all_trainers[i]["P5_Moves"].ToString(),
                            Trainer.all_trainers[i]["P5_Perm_Moves"].ToString()
                        );
                    }

                    if (int.Parse(Trainer.all_trainers[i]["P6_DexNum"].ToString()) != 0)
                    {
                        pokemon6 = make_trainer_pokemon(
                            int.Parse(Trainer.all_trainers[i]["P6_DexNum"].ToString()),
                            int.Parse(Trainer.all_trainers[i]["P6_Level_From_Max"].ToString()),
                            Trainer.all_trainers[i]["P6_Moves"].ToString(),
                            Trainer.all_trainers[i]["P6_Perm_Moves"].ToString()
                        );
                    }

                    //make_trainer_pokemon -> add to Trainers trainer_team
                    /*string route, 
                     * string name, 
                     * string type, 
                     * int money_mult, 
                     * string intro_dialogue, 
                     * string exit_dialogue,
                     * Pokemon pokemon1 = null, 
                     * Pokemon pokemon2 = null, 
                     * Pokemon pokemon3 = null, 
                     * Pokemon pokemon4 = null, 
                     * Pokemon pokemon5 = null, 
                     * Pokemon pokemon6 = null*/

                    Trainer temp_trainer = new(
                        Trainer.all_trainers[i]["Name"].ToString(),
                        Trainer.all_trainers[i]["Type"].ToString(),
                        int.Parse(Trainer.all_trainers[i]["Money"].ToString()),
                        Trainer.all_trainers[i]["Intro_Dialogue"].ToString(),
                        Trainer.all_trainers[i]["Exit_Dialogue"].ToString(),
                        (pokemon1 != null ? pokemon1 : null),
                        (pokemon2 != null ? pokemon2 : null),
                        (pokemon3 != null ? pokemon3 : null),
                        (pokemon4 != null ? pokemon4 : null),
                        (pokemon5 != null ? pokemon5 : null),
                        (pokemon6 != null ? pokemon6 : null)
                        );
                    //add to dictionary
                    route_trainers.Add(temp_trainer.name, temp_trainer);
                }
            }
            //return dictionary
            return route_trainers;
        }
        public static Pokemon make_trainer_pokemon(int dexNum, int level_from_cap, string moves, string p_moves)
        {

            List<Learnset> pokemons_learnset;
            List<Learnset> learnable_moves = new();

            Moves[] temp_moves = new Moves[4];
            //count of how many moves are going to be added after permanent moves
            int new_moves_count;
            //temporary dexnum that will be incremented
            int temp_dexNum = dexNum;
            //string moves example: 'Tail Whip|Tackle'
            string[] moves_list = moves.Split('|');
            string[] permanent_moves_list = p_moves.Split('|');


            //level_from_cap is a negative number
            //create temp pokemon to get evolve level
            Pokemon temp_pokemon = new(temp_dexNum, (GameController.level_cap + level_from_cap));
            //while pokemons current level > evolve_level, evolve to next pokemon
            while ((temp_pokemon.pokedex_entry.evolve_level != -1) && (temp_pokemon.pokedex_entry.evolve_level <= (GameController.level_cap + level_from_cap)))
            {
                temp_dexNum++;
                temp_pokemon = new Pokemon(temp_dexNum, (GameController.level_cap + level_from_cap));
            }


            new_moves_count = 4 - permanent_moves_list.Length;
            if (permanent_moves_list[0] != "null")
            {

                //base case we set all moves ahead of time
                if (permanent_moves_list.Length == 4)
                {
                    temp_pokemon = new Pokemon(temp_pokemon.dexnum, temp_pokemon.level, permanent_moves_list[0], permanent_moves_list[1], permanent_moves_list[2], permanent_moves_list[3]);
                    return temp_pokemon;
                }
                //check for null
                //add permanent moves to temp_moves         
                else
                {
                    for (int i = 0; i < permanent_moves_list.Length; i++)
                    {
                        temp_moves[i] = Moves.get_move(permanent_moves_list[i]);
                    }

                }
            }
            else
            {
                new_moves_count = 4;
            }
            //get new moves up to 4 - permanent_moves
            //now we have evolved pokemon, grab its learnset

            pokemons_learnset = temp_pokemon.learnset;
            //get number of available moves
            foreach (Learnset learnable_move in pokemons_learnset)
            {
                if (learnable_move.level <= temp_pokemon.level)
                {
                    //Debug.Log(learnable_move.move.name);
                    learnable_moves.Add(learnable_move);
                }
            }


            int total_moves_available;
            //condition ? statement 1 : statement 2
            //if # of avialble moves in learnset is > new moves, use learnset count
            total_moves_available = new_moves_count > learnable_moves.Count ? learnable_moves.Count : new_moves_count;

            //Debug.Log("TOTAL MOVES AVAILABLE " + total_moves_available);

            int index_tracker = 4 - new_moves_count;
            //now we have total moves left to grab, grab that many from the end of learnset
            for (int i = learnable_moves.Count - 1; i > learnable_moves.Count - total_moves_available - 1; i--)
            {
                if (index_tracker < 4)
                {
                    /*                    Debug.Log("ISIZE " + i);
                                        Debug.Log("INDEXTRACKER " + index_tracker);
                                        Debug.Log("LAST 4 MOVES " + learnable_moves[i].move.name);*/
                    temp_moves[index_tracker] = Moves.get_move(learnable_moves[i].move.name);
                    index_tracker++;
                }

            }
            /*            Debug.Log("DEXNUM " + temp_pokemon.dexnum);
                        Debug.Log("LEVEL " + temp_pokemon.level);
                        Debug.Log("TEMP MOVE 1 " + (temp_moves[0] != null ? temp_moves[0].name : "null"));
                        Debug.Log("TEMP MOVE 2 " + (temp_moves[1] != null ? temp_moves[1].name : "null"));
                        Debug.Log("TEMP MOVE 3 " + (temp_moves[2] != null ? temp_moves[2].name : "null"));
                        Debug.Log("TEMP MOVE 4 " + (temp_moves[3] != null ? temp_moves[3].name : "null"));*/

            //we have all the moves now, We just have to make the pokemon
            temp_pokemon = new Pokemon(
                temp_pokemon.dexnum,
                temp_pokemon.level,
                (temp_moves[0] != null ? temp_moves[0].name : null),
                (temp_moves[1] != null ? temp_moves[1].name : null),
                (temp_moves[2] != null ? temp_moves[2].name : null),
                (temp_moves[3] != null ? temp_moves[3].name : null)

                );
            return temp_pokemon;
        }
    }
}
