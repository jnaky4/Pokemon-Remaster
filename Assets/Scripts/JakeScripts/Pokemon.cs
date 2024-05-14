using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
Pokemon Object
    A Pokemon will be derived from a pokemon object
        Each pokemon generated in the game will have the following Example Attributes:

        name: Charizard
        dexnum: 6
        level: 55
        type1: Fire
        type2: Flying
        Learnset{<Slash,36>,<Flamethrower,46>,<Firespin,55>}
        currentMoves: {Fire Blast, Earthquake, Sword Dance, Hyper Beam}
        private int hp = 78;
        private int attack = 84;
        private int defense = 78;
        private int sp_attack = 109;
        private int sp_defense = 85;
        private int speed = 100;

        //TODO
        pokemonImage: PokemonImages/Charizard.jpg
        LearnableTms{Mega Punch, Sword Dance, Mega Kick, Toxic,
            Body Slam, Take Down, Double-Edge, Hyper Beam, Submission,
            Counter, Seismic Toss, Rage, Dragon Rage, Earthquake,
            Fissure, Dig, Mimic, Double Team, Reflect, Bide, Fire Blast,
            Swift, Skull Bash, Rest, Substitute}
        LearnableHMs: {Cut, Strength, Fly}

        EXP100: 1600000  -> this is the amount of EXP to get to lvl 100
        currentEXP: 650245

        private static ArrayList currentMoves = new ArrayList { "Slash", "Flamethrower", "Wing Attack", "Earthquake" };
        Image pokemonImage { get; set; }
        private type type1 { get; set; }
        private type type2 { get; set; }
        Dictionary<string, int> learnset { get; set; }
        ArrayList learnableTms { get; set; }
        ArrayList learnableHMs { get; set; }
        private int EXP100 { get; set; }
*/

namespace Pokemon
{
    public class Pokemon
    {
        //create pokemon with specific moves, chains the 2 arg constructor
        public Pokemon(int dexNum, int level, string move1 = null, string move2 = null, string move3 = null, string move4 = null,
            int current_exp = 0, int current_hp = 0, int current_attack = 0, int current_special_attack = 0, int current_defense = 0, int current_special_defense = 0, int current_speed = 0, ArrayList statuses = null)
        {
            this.level = level;
            dexnum = dexNum;

            //check if null before using get_move, otherwise set to null
            currentMoves[0] = move1 != null ? Moves.get_move(move1) : null;
            currentMoves[1] = move2 != null ? Moves.get_move(move2) : null;
            currentMoves[2] = move3 != null ? Moves.get_move(move3) : null;
            currentMoves[3] = move4 != null ? Moves.get_move(move4) : null;

            pokedex_entry = new Pokedex(dexNum);
            //grab name from pokedex
            name = pokedex_entry.name;

            
            //set exp for new pokemon, default 0,
            lvl_speed = pokedex_entry.leveling_speed;
            this.current_exp = current_exp;

            //sets: base_level_exp, current_exp if current_exp above is 0, next_level_exp
            SetExp();

            //grabs base and current stats, calculated from base_stats for this pokemon
            CalculateStats(current_hp);

            //adds persisting status to pokemon if it had prior to evolving
            if (statuses != null) this.statuses = statuses;

            //get learnset added to learnset_dictionary for this pokemon
            learnset = Learnset.get_learnset(dexnum);
            //this.TM_Set = TMSet.get_TMSet(this.dexnum);

            //gets type for this pokemon from pokedex and creates type object
            type1 = Type.get_type(pokedex_entry.get_type1());
            if (pokedex_entry.get_type2() != "-")
            {
                type2 = Type.get_type(pokedex_entry.get_type2());
            }
            else
            {
                type2 = Type.get_type("Null");
            }

            gender = SetGender(pokedex_entry.gender_ratio);

            GetImagePath();


        }

        //gets loaded in once, have to call load_base_stats before creating new pokemon
        public static List<Dictionary<string, object>> all_base_stats = new List<Dictionary<string, object>>();

        public Dictionary<string, Moves> TM_Set = new Dictionary<string, Moves>();

        //basic pokemon info
        public string name;

        public int dexnum;
        public int level;

        //Pokex Object of Pokemon
        public Pokedex pokedex_entry;

        //each pokemon has up to 2 types
        public Type type1;

        public Type type2;

        public char gender;

        //list of learnable moves
        public List<Learnset> learnset = new List<Learnset>();

        //list of current moves the pokemon has
        public Moves[] currentMoves = new Moves[4];

        public Moves struggle = Moves.get_move("Struggle");

        //images of pokemon
        public string image1;

        public string image2;
        public string shiny_image;

        //used for caculating max stats
        public int iv = 30;

        /// <summary>
        /// base HP 
        /// </summary>
        public int b_hp;
        public int b_atk;
        public int b_def;
        public int b_spAtk;
        public int b_spDef;
        public int b_spd;

        //current stats for the pokemon
        public int current_hp;

        //This means the maximum at this level, or full health
        public int max_hp;
        public int max_attack;
        public int max_defense;
        public int max_sp_attack;
        public int max_sp_defense;
        public int max_speed;

        //stages range from -6 to 6, used during damage calculation
        public int stage_critical = 0;

        public int stage_attack = 0;
        public int stage_defense = 0;
        public int stage_sp_attack = 0;
        public int stage_sp_defense = 0;
        public int stage_speed = 0;
        public int stage_accuracy = 0;
        public int stage_evasion = 0;

        //stores name of unable to attack move
        public Status UnableToAttackStatus = null;
        //public string UnableToAttackStatusName = "";
        public bool can_attack = true;

        //used to calculate exp and evolution
        public double lvl_speed;

        public int base_lvl_exp;
        public int current_exp;
        public int next_lvl_exp;
        public bool time_to_evolve;
        public bool want_to_evolve;

        //used to display changes in stats at level up
        public int change_hp;
        public int change_attack;
        public int change_defense;
        public int change_sp_attack;
        public int change_sp_defense;
        public int change_speed;

        public bool learned_new_move;
        public Moves learned_move;
        public bool gained_a_level;

        public ArrayList statuses = new();

        public void ResetStages()
        {
            stage_attack = 0;
            stage_defense = 0;
            stage_sp_attack = 0;
            stage_sp_defense = 0;
            stage_speed = 0;
            stage_accuracy = 0;
            stage_evasion = 0;
            stage_critical = 0;
        }

        public void CalculateStats(int current_hp = 0)
        {
            //var test = Pokemon.all_base_stats[this.dexnum - 1]["HP"];
            b_hp = int.Parse(all_base_stats[dexnum - 1]["HP"].ToString());
            b_atk = int.Parse(all_base_stats[dexnum - 1]["Attack"].ToString());
            b_def = int.Parse(all_base_stats[dexnum - 1]["Defense"].ToString());
            b_spAtk = int.Parse(all_base_stats[dexnum - 1]["Sp. Atk"].ToString());
            b_spDef = int.Parse(all_base_stats[dexnum - 1]["Sp. Def"].ToString());
            b_spd = int.Parse(all_base_stats[dexnum - 1]["Speed"].ToString());

            //sets all max_stats calculated from base stats & lvl
            UpdateCurrentStats();

            //pokemon may have evolved so we need to pass in its current stats
            //after updating stats, checks arguments passed, if 0 set to max_stat, otherwise set the value
            this.current_hp = current_hp == 0 ? max_hp : current_hp;
            //Debug.LogWarning("Hey motherfucker, current defense for " + this.name + " is " + this.current_defense + " and the current sp defense is " + this.current_sp_defense);
        }


        /// <summary>
        /// Sets image path for pokemon
        /// </summary>
        public void GetImagePath()
        {
            var path = "Assets/Resources/Images/PokemonImages/";

            image1 = path + dexnum.ToString().PadLeft(3, '0') + name + ".png";
            image2 = path + dexnum.ToString().PadLeft(3, '0') + name + "2.png";

        }

        /// <summary>
        /// Sets the base exp and next level exp, after 50, exp is constant increase of 7500
        /// forumla <50 : base: lvl_speed * level^3, next: lvl_speed * (level + 1)^3
        /// formula >50 : base: lvl_speed * 50^3 + 7500*(lvl-50), next: lvl_speed * 50^3 + 7500*(lvl-50 + 1)
        /// </summary>
        public void SetExp()
        {
            //leveling is multiplier * level^3 up to level 50
            if (level <= 50)
            {
                //Debug.Log("Pokemon is Under lvl 50");
                base_lvl_exp = (int)(lvl_speed * Math.Pow(level, 3));
                next_lvl_exp = (int)(lvl_speed * Math.Pow(level + 1, 3));
                //Debug.Log("SET_EXP() base_lvl_exp: " + this.base_lvl_exp);
                //Debug.Log("SET_EXP() next_lvl_exp: " + this.next_lvl_exp);

                //Debug.Log("BEFORE SET_EXP() current_lvl_exp " + this.current_exp);
                current_exp = current_exp < base_lvl_exp ? base_lvl_exp : current_exp;

                //Debug.Log("AFTER SET_EXP() current_lvl_exp " + this.current_exp);
            }

            //after level 50, next level is 7500 exp from 51 to 100
            else
            {
                base_lvl_exp = (int)(lvl_speed * Math.Pow(50, 3)) + (7500 * (level - 50));
                next_lvl_exp = (int)(lvl_speed * Math.Pow(50, 3)) + (7500 * (level + 1 - 50));
                if (current_exp == 0 || current_exp < base_lvl_exp)
                {
                    current_exp = base_lvl_exp;
                }
            }
        }

        /// <summary>
        /// takes the base stats and level and caculates the max_stats
        /// </summary>
        public void UpdateCurrentStats()
        {
            //iv is set to 30
            //ev set for +20 for each stat
            //510 total ev / 4 = 127.5 / 6 = 21.25, each pokemon should have +21.25 to each stat when using all evs
            max_hp = (((((b_hp + iv) * 2) + 20) * level) / 100) + level + 10;
            max_attack = (((((b_atk + iv) * 2) + 20) * level) / 100) + 5;
            max_defense = (((((b_def + iv) * 2) + 20) * level) / 100) + 5;
            max_sp_attack = (((((b_spAtk + iv) * 2) + 20) * level) / 100) + 5;
            max_sp_defense = (((((b_spDef + iv) * 2) + 20) * level) / 100) + 5;
            max_speed = (((((b_spd + iv) * 2) + 20) * level) / 100) + 5;
        }

        /// <summary>
        /// Checks if a new move is learnable at its level after leveling up
        /// </summary>
        /// <returns>Moves object of learnable move</returns>
        public Moves CheckLearnset()
        {
            learned_move = null;
            foreach (Learnset available_move in learnset)
            {
                //if the learnables moves level to learn is equal to the pokemons new level, it can learn it
                if (level == available_move.level)
                {

                    //check if pokemon already has the move
                    bool has_move = HasMove(available_move.move);

                    //if the pokemon doesnt have the move then the move returned is the move, otherwise null
                    if (!has_move)
                    {
                        learned_move = available_move.move;

                        return learned_move;
                    }
                }
            }

            //no moves have been learned, check if one of the currentmoves can be upgraded
/*            Moves upgraded_move = null;
            for(int i = 0; i < 4; i++)
            {
                //if upgraded move is not null we have upgraded a move already and should stop
                if(upgraded_move == null) upgraded_move = upgrade_a_move(currentMoves[i]) != null ? this.upgrade_a_move(currentMoves[i]) : learned_move;

            }

            if (upgraded_move != null) this.learned_move = upgraded_move;*/

            return learned_move;
        }

        /// <summary>
        /// Checks if the pokemon is ready to evovle
        /// </summary>
        /// <returns>true if ready to evolve</returns>
        public bool CheckEvolve()
        {
            if (pokedex_entry.evolve_level == -1) return false;

            if (level >= pokedex_entry.evolve_level) return true;
                
            return false;
        }

        //for: trainer_wild_multiplier, if it is a trainer pokemon, needs to be set to 1.5, default 1
        public int Gain_EXP(int enemy_level, int enemy_base_exp, int num_player_pokemon_used = 1, double trainer_wild_multipllier = 1)
        {
            //if pokemon is at the level cap, do nothing
            if (level >= GameController.level_cap) return 0;

            gained_a_level = false;
            //EXP = (a * t * e * b * L) / (7 * s)
            //a is wild or trainer pokemon: 1 or 1.5
            //t is trainer_pokemon or traded, always 1 for game
            //e is if lucky egg, always 1
            //b is enemy_base_exp
            //L is enemy_level
            int exp_gained = (int)(trainer_wild_multipllier * 1 * 1 * enemy_base_exp * enemy_level) / (7 * num_player_pokemon_used);
            //Debug.Log(this.name + " gained " + exp_gained + "!");
            current_exp += exp_gained;

/*            Debug.Log("base exp " + this.base_lvl_exp);
            Debug.Log("current exp " + this.current_exp);
            Debug.Log("next level exp " + this.next_lvl_exp);*/

            if (current_exp >= next_lvl_exp)
            {
                //Debug.Log("Pokemon Leveled!");
                level += 1;
                gained_a_level = true;

                //initially sets change_stats to previous level stats
                change_hp = max_hp;
                change_attack = max_attack;
                change_defense = max_defense;
                change_speed = max_speed;
                change_sp_attack = max_sp_attack;
                change_sp_defense = max_sp_defense;

                UpdateCurrentStats();

                //sets change_stats to new level max - prev level max stat
                change_hp = max_hp - change_hp;
                change_attack = max_attack - change_attack;
                change_defense = max_defense - change_defense;
                change_speed = max_speed - change_speed;
                change_sp_attack = max_sp_attack - change_sp_attack;
                change_sp_defense = max_sp_defense - change_sp_defense;

/*                Debug.Log("Your Pokemon Gained " + change_hp + " HP!");
                Debug.Log("Your Pokemon Gained " + change_attack + " ATK!");
                Debug.Log("Your Pokemon Gained " + change_attack + " ATK!");
                Debug.Log("Your Pokemon Gained " + change_defense + " DFN!");
                Debug.Log("Your Pokemon Gained " + change_speed + " SPD!");
                Debug.Log("Your Pokemon Gained " + change_sp_attack + " SPA!");
                Debug.Log("Your Pokemon Gained " + change_sp_defense + " SPD!");*/


                //check if learnable move
                Moves temp_new_move = CheckLearnset();
                if (temp_new_move != null)
                {
                    //Debug.Log("Your pokemon learned a new move! " + temp_new_move.name);
                    learned_new_move = true;
                }

                //check evolve
                time_to_evolve = CheckEvolve();
                if (time_to_evolve)


                //update exp
                SetExp();
                /*                Debug.Log("New base EXP " + this.base_lvl_exp);
                                Debug.Log("Current EXP " + this.current_exp);
                                Debug.Log("New Next levl EXP " + this.next_lvl_exp);*/
            }

            return exp_gained;
        }

        /// <summary>
        /// Evolves the current pokemon into its dex+1 and recalculates its new stats, grabs new CSV's
        /// </summary>
        public void Evolve()
        {
            //save old stats
            change_hp = max_hp;
            change_attack = max_attack;
            change_defense = max_defense;
            change_speed = max_speed;
            change_sp_attack = max_sp_attack;
            change_sp_defense = max_sp_defense;

            Pokemon evolved_pokemon = new Pokemon(dexnum + 1, level);

            //update pokemon data 
            learnset = evolved_pokemon.learnset;
            pokedex_entry = evolved_pokemon.pokedex_entry;
            learnset = evolved_pokemon.learnset;
            type1 = evolved_pokemon.type1;
            type2 = evolved_pokemon.type2;

            dexnum = evolved_pokemon.dexnum;
            name = evolved_pokemon.name;
            CalculateStats(current_hp);

            image1 = evolved_pokemon.image1;
            image2 = evolved_pokemon.image2;

            want_to_evolve = false;

            //update change to new stats
            change_hp = evolved_pokemon.max_hp - change_hp;
            change_attack = evolved_pokemon.max_attack - change_attack;
            change_defense = evolved_pokemon.max_defense - change_defense;
            change_speed = evolved_pokemon.max_speed - change_speed;
            change_sp_attack = evolved_pokemon.max_sp_attack - change_sp_attack;
            change_sp_defense = evolved_pokemon.max_sp_defense - change_sp_defense;

/*          Debug.Log("Your Pokemon Gained " + change_hp + " HP!");
            Debug.Log("Your Pokemon Gained " + change_attack + " ATK!");
            Debug.Log("Your Pokemon Gained " + change_attack + " ATK!");
            Debug.Log("Your Pokemon Gained " + change_defense + " DFN!");
            Debug.Log("Your Pokemon Gained " + change_speed + " SPD!");
            Debug.Log("Your Pokemon Gained " + change_sp_attack + " SPA!");
            Debug.Log("Your Pokemon Gained " + change_sp_defense + " SPD!");*/
        }


        /// <summary>
        /// Resets stats of pokemon at Beginning of Battle / end of battle / fainted / swapping pokemon
        /// </summary>
        public void ResetBattleStats()
        {
            //check if pokemon fainted
            if (IsFainted())
            {
                //remove all statuses
                statuses.Clear();
            }
            //pokemon isnt fainted, remove all non persisting status effects
            else
            {

                foreach (KeyValuePair<string,Status> status_item in Status.all_status_effects)
                {
                    //if status doesnt persist, remove
                    if (!status_item.Value.persistence && statuses.Contains(Status.GetStatus(status_item.Value.name))) statuses.Remove(Status.GetStatus(status_item.Value.name));
                }

                //this.current_speed = this.HasStatus("Paralysis") ? this.current_speed : this.max_speed;
                //this.current_attack = this.HasStatus("Burn") ? this.current_attack : this.max_attack;
            }

            //reset stages
            ResetStages();
        }

        /// <summary>
        /// For AI Pokemon
        /// looks at all moves a pokemon has and checks if they are at a level to auto upgrade to the next tier
        /// Upgraded Moves:
        /// Start Moves  -> lvl 20               -> lvl 38       -> lvl 50
        /// Ember        -> lvl flame Wheel      -> Flamethrower -> Fire Blast
        /// Bubble       -> Water Gun            -> Surf         -> Hydropump
        /// Vinewhip     -> Mega Drain           -> Razor Leaf   -> Solar Beam
        /// Thundershock -> Volt Switch          -> Thunder Bolt -> Thunder
        /// </summary>
        public void UpgradeMoves()
        {
            Moves Upgraded_move = null;
            for(int i = 0; i < 4; i++)
            {
                if (currentMoves[i] != null)
                {

                    Moves move = currentMoves[i];
                    switch (move.name)
                    {
                        case "Ember":
                        case "Flame Wheel":
                        case "Flamethrower":
                            
                            //Upgraded_move = this.level >= 20 ? Moves.get_move("Flame Wheel") : Upgraded_move;
                            Upgraded_move = level >= 38 ? Moves.get_move("Flamethrower") : Upgraded_move;
                            Upgraded_move = level >= 50 ? Moves.get_move("Fire Blast") : Upgraded_move;
                            //Debug.Log("Upgraded a Move! : " + Upgraded_move.name);
                            break;
                        case "Bubble":
                        case "Water Gun":
                        case "Surf":
                            
                            Upgraded_move = level >= 20 ? Moves.get_move("Water Gun") : Upgraded_move;
                            Upgraded_move = level >= 38 ? Moves.get_move("Surf") : Upgraded_move;
                            Upgraded_move = level >= 50 ? Moves.get_move("Hydro Pump") : Upgraded_move;
                            //Debug.Log("Upgraded a Move! : " + Upgraded_move.name);
                            break;
                        case "Vine Whip":
                        case "Mega Drain":
                        case "Razor Leaf":
                            
                            Upgraded_move = level >= 20 ? Moves.get_move("Vine Whip") : Upgraded_move;
                            //Upgraded_move = this.level >= 38 ? Moves.get_move("Mega Drain") : Upgraded_move;
                            Upgraded_move = level >= 50 ? Moves.get_move("Razor Leaf") : Upgraded_move;
                            //Debug.Log("Upgraded a Move! : " + Upgraded_move.name);
                            break;
                        case "Thunder Shock":
                        case "Volt Switch":
                        case "Thunderbolt":
                            
                            //Upgraded_move = this.level >= 20 ? Moves.get_move("Volt Switch") : Upgraded_move;
                            Upgraded_move = level >= 38 ? Moves.get_move("Thunderbolt") : Upgraded_move;
                            Upgraded_move = level >= 50 ? Moves.get_move("Thunder") : Upgraded_move;
                            //Debug.Log("Upgraded a Move! : " + Upgraded_move.name);
                            break;
                        default:
                            break;
                    }
                    //if the player doesnt have the move or the upgraded move != null, replace with upgraded move
                    currentMoves[i] = HasMove(Upgraded_move) || Upgraded_move == null ? currentMoves[i] : Upgraded_move;
                }
            
            }


            /*foreach (Moves move in this.currentMoves){


                if(move != null)
                {
                    
                    switch (move.name)
                    {
                        case "Ember":
                        case "Flame Wheel":
                        case "Flamethrower":
                            //Upgraded_move = this.level >= 20 ? Moves.get_move("Flame Wheel") : Upgraded_move;
                            Upgraded_move = this.level >= 38 ? Moves.get_move("Flamethrower") : Upgraded_move;
                            Upgraded_move = this.level >= 50 ? Moves.get_move("Fire Blast") : Upgraded_move;
                            break;
                        case "Bubble":                       
                        case "Water Gun":
                        case "Surf":
                            Upgraded_move = this.level >= 20 ? Moves.get_move("Water Gun") : Upgraded_move;
                            Upgraded_move = this.level >= 38 ? Moves.get_move("Surf") : Upgraded_move;
                            Upgraded_move = this.level >= 50 ? Moves.get_move("Hydro Pump") : Upgraded_move;
                            break;
                        case "Vine Whip":
                        case "Mega Drain":
                        case "Razor Leaf":
                            Upgraded_move = this.level >= 20 ? Moves.get_move("Vine Whip") : Upgraded_move;
                            //Upgraded_move = this.level >= 38 ? Moves.get_move("Mega Drain") : Upgraded_move;
                            Upgraded_move = this.level >= 50 ? Moves.get_move("Razor Leaf") : Upgraded_move;
                            break;
                        case "Thunder Shock":
                        case "Volt Switch":
                        case "Thunderbolt":
                            //Upgraded_move = this.level >= 20 ? Moves.get_move("Volt Switch") : Upgraded_move;
                            Upgraded_move = this.level >= 38 ? Moves.get_move("Thunderbolt") : Upgraded_move;
                            Upgraded_move = this.level >= 50 ? Moves.get_move("Thunder") : Upgraded_move;
                            break;
                        default:
                            break;
                    }
                    

                }
                move = have_move(Upgraded_move) ? null : Upgraded_move;*/

            //}


            
             

            
        }

        public Moves UpgradeAMove(Moves move)
        {
            if (move == null) return move;

            Moves Upgraded_move = null;
            switch (move.name)
            {
                case "Ember":
                case "Flame Wheel":
                case "Flamethrower":
                    
                    //Upgraded_move = this.level >= 20 ? Moves.get_move("Flame Wheel") : Upgraded_move;
                    Upgraded_move = level >= 38 ? Moves.get_move("Flamethrower") : Upgraded_move;
                    Upgraded_move = level >= 50 ? Moves.get_move("Fire Blast") : Upgraded_move;
                    //Debug.Log("Upgraded a Move! : " + Upgraded_move.name);
                    break;
                case "Bubble":
                case "Water Gun":
                case "Surf":
                    
                    Upgraded_move = level >= 20 ? Moves.get_move("Water Gun") : Upgraded_move;
                    Upgraded_move = level >= 38 ? Moves.get_move("Surf") : Upgraded_move;
                    Upgraded_move = level >= 50 ? Moves.get_move("Hydro Pump") : Upgraded_move;
                    //Debug.Log("Upgraded a Move! : " + Upgraded_move.name);
                    break;
                case "Vine Whip":
                case "Mega Drain":
                case "Razor Leaf":
                    
                    Upgraded_move = level >= 20 ? Moves.get_move("Vine Whip") : Upgraded_move;
                    //Upgraded_move = this.level >= 38 ? Moves.get_move("Mega Drain") : Upgraded_move;
                    Upgraded_move = level >= 50 ? Moves.get_move("Razor Leaf") : Upgraded_move;
                    //Debug.Log("Upgraded a Move! : " + Upgraded_move.name);
                    break;
                case "Thunder Shock":
                case "Volt Switch":
                case "Thunderbolt":
                    
                    //Upgraded_move = this.level >= 20 ? Moves.get_move("Volt Switch") : Upgraded_move;
                    Upgraded_move = level >= 38 ? Moves.get_move("Thunderbolt") : Upgraded_move;
                    Upgraded_move = level >= 50 ? Moves.get_move("Thunder") : Upgraded_move;
                    //Debug.Log("Upgraded a Move! : " + Upgraded_move.name);
                    break;
                default:
                    break;

            }
            return Upgraded_move;
        }

        /// <summary>
        /// Check if pokemon has a move
        /// </summary>
        /// <param name="new_move">Move Object to check</param>
        /// <returns>True if has the move</returns>
        public bool HasMove(Moves new_move)
        {
            bool has_move = false;
            //check if pokemon has the move already
            if (new_move != null)
            {
                foreach (Moves move in currentMoves)
                {
                    if (move != null)
                    {
                        has_move = move.name == new_move.name ? true : has_move;

                    }

                }

            }
            return has_move;

        }

        //returns number of moves the pokemon currently has checking for null
        public int CountMoves()
        {
            int count = 0;
            foreach (Moves move in currentMoves)
            {
                if (move != null) count++;
            }
            return count;
        }


        //checks all statuses in pokemon.statuses for unable to attack chance, returns true if able to still attack
        public bool RollCanAttack()
        {
            foreach (Status status in statuses)
            {
                if (status.unable_to_attack_chance == 1.0)
                {
                    UnableToAttackStatus = status;
                    return can_attack = false;
                }
                if (status.unable_to_attack_chance > 0)
                {
                    System.Random rnd = new System.Random();
                    //returns a number >= 0.0 AND < 1.0 : [0.0 to .99999]
                    double num = rnd.NextDouble();
                    if (num <= status.unable_to_attack_chance)
                    {
                        UnableToAttackStatus = status;
                        return can_attack = false;
                        
                    }
                }
            }
            
            return can_attack;
        }

        /// <summary>
        /// Checks if pokemon is fainted
        /// </summary>
        /// <returns>returns true if pokemon health is <= 0</returns>
        public bool IsFainted() { return current_hp <= 0; }

        /// <summary>
        /// At start of turn checks if any statuses remaining turns = 0
        /// </summary>
        /// <returns>String of the status removed</returns>
        public string StartTurnStatusUpdate()
        {
            string remove_status = "";
            foreach (Status status in statuses)
            {
                //Debug.Log("STATUS NAME:" + status.name);
                //Debug.Log("REMAINING TURNS: " + status.remaining_turns);
                //checks if remaining_number of turns reaches 0 to remove status
                if (status.remaining_turns == 0)
                {
                    
                    //Debug.Log("Removed " + status.name);
                    statuses.Remove(status);
                    return status.name;

                }
                //checks if there is a removal chance to roll for removal
                if (status.removal_chance > 0)
                {
                    System.Random rnd = new System.Random();
                    //returns a number >= 0.0 AND < 1.0 : [0.0 to .99999]
                    double num = rnd.NextDouble();
                    if (num <= status.removal_chance)
                    {
                        statuses.Remove(status);
                        return status.name;
                    }
                
                }
            }
            return remove_status;
        }
        
        
        /// <summary>
        /// Decrements all statuses remaning turns by 1
        /// Sets Unable to attack back to true
        /// </summary>
        public void EndTurnStatusUpdate()
        {
            can_attack = true;
            foreach (Status status in statuses)
            {
                status.remaining_turns = status.remaining_turns -1;
                
            }



        }

        /// <summary>
        /// Returns true if the string name matches the name of a status in the statuses array
        /// </summary>
        /// <param name="name">name of the status</param>
        /// <returns>true/false</returns>
        public bool HasStatus(string name)
        {
            foreach (Status status in statuses) if (status.name == name) return true;
            return false;
        }

        /// <summary>
        /// Checks the pokemon.statuses array to see if the pokemon already has a persisting status.
        /// only one can be applied at a time
        /// </summary>
        /// <returns>true if the pokemon has a persisting status</returns>
        public bool HasPersistenceStatus()
        {
            foreach(Status status in statuses) if (status.persistence) return true;
            return false; 
        }


        public bool ImmuneToStatus(Status attacking_status)
        {
            if (attacking_status.ignore_type == type1.name) return true;
            if (attacking_status.ignore_type == type2.name) return true;
            return false;
        }


        public int getMoveIndex(string name)
        {

            for(int i = 0; i < CountMoves(); i++) if (currentMoves[i].name == name) return i;

            Debug.Log("ERROR: Move not found on Pokemon");
            return 0;
        }
        private char SetGender(string ratio)
        {
            int[] ratio_arr = ratio.Split('|').Select(int.Parse).ToArray();
            switch (ratio_arr[0], ratio_arr[1])
            {
                case (-1, -1):
                    return 'u';
                case (-1, 1):
                    return 'm';
                case (1, -1):
                    return 'f';
                default:
                    int roll = Utility.rnd.Next(ratio_arr[0] + ratio_arr[1]);
                    return roll < ratio_arr[0] ? 'f' : 'm';
            }


        }
    }
}