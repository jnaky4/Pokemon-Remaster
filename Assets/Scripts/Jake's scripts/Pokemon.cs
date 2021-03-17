using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Microsoft.VisualBasic.FileIO;


/*
Pokemon Object
    A Pokemon will be derived from a pokemon object
        Each pokemon generated in the game will have the following Example Attributes:

        //DONE
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
        public Pokemon(int dexNum, int level, string move1 = null, string move2 = null, string move3 = null, string move4 = null)
        {
            this.level = level;
            this.dexnum = dexNum;

            //check if null before using get_move, otherwise set to null
            this.currentMoves[0] = move1 != null ? Moves.get_move(move1) : null;
            this.currentMoves[1] = move2 != null ? Moves.get_move(move2) : null;
            this.currentMoves[2] = move3 != null ? Moves.get_move(move3) : null;
            this.currentMoves[3] = move4 != null ? Moves.get_move(move4) : null;

            this.pokedex_entry = new Pokedex(dexNum);

            //grab name from pokedex
            this.name = pokedex_entry.name;

            //grabs base and current stats, calculated from base_stats for this pokemon
            calculate_stats();
            //get learnset added to learnset_dictionary for this pokemon
            this.learnset = Learnset.get_learnset(this.dexnum);
            this.TM_Set = TMSet.get_TMSet(this.dexnum);

            //gets type for this pokemon from pokedex and creates type object
            this.type1 = Type.get_type(this.pokedex_entry.get_type1());
            if (this.pokedex_entry.get_type2() != "-")
            {
                this.type2 = Type.get_type(this.pokedex_entry.get_type2());
            }
            else
            {
                this.type2 = null;
            }
            get_image_path();
        }

        public static Dictionary<int, string> dictionary_pokemon = new Dictionary<int, string> {
            {1, "Bulbasaur" },{2 ,"Ivysaur"},{3 ,"Venusaur"},
            {4 ,"Charmander"},{5 ,"Charmeleon"},{6 ,"Charizard"},
            {7 ,"Squirtle"},{8 ,"Wartortle"},{9 ,"Blastoise"},
            {10 ,"Caterpie"},{11 ,"Metapod"},{12 ,"Butterfree"},
            {13 ,"Weedle"},{14 ,"Kakuna"},{15 ,"Beedrill"},
            {16 ,"Pidgey"},{17 ,"Pidgeotto"},{18 ,"Pidgeot"},
            {19 ,"Rattata"},{20 ,"Raticate"},
            {21 ,"Spearow"},{22 ,"Fearow"},
            {23 ,"Ekans"},{24 ,"Arbok"},
            {25 ,"Pikachu"},{26 ,"Raichu"},
            {27 ,"Sandshrew"},{28 ,"Sandslash"},
            {29 ,"Nidoran"},{30 ,"Nidorina"},{31 ,"Nidoqueen"},
            {32 ,"Nidoran"},{33 ,"Nidorino"},{34 ,"Nidoking"},
            {35 ,"Clefairy"},{36 ,"Clefable"},
            {37 ,"Vulpix"},{38 ,"Ninetales"},
            {39 ,"Jigglypuff"},{40 ,"Wigglytuff"},
            {41 ,"Zubat"},{42 ,"Golbat"},
            {43 ,"Oddish"},{44 ,"Gloom"},{45 ,"Vileplume"},
            {46 ,"Paras"},{47 ,"Parasect"},
            {48 ,"Venonat"},{49 ,"Venomoth"},
            {50 ,"Diglett"},{51 ,"Dugtrio"},
            {52 ,"Meowth"},{53 ,"Persian"},
            {54 ,"Psyduck"},{55 ,"Golduck"},
            {56 ,"Mankey"},{57 ,"Primeape"},
            {58 ,"Growlithe"},{59 ,"Arcanine"},
            {60 ,"Poliwag"},{61 ,"Poliwhirl"},{62 ,"Poliwrath"},
            {63 ,"Abra"},{64 ,"Kadabra"},{65 ,"Alakazam"},
            {66 ,"Machop"},{67 ,"Machoke"},{68 ,"Machamp"},
            {69 ,"Bellsprout"},{70 ,"Weepinbell"},{71 ,"Victreebel"},
            {72 ,"Tentacool"},{73 ,"Tentacruel"},
            {74 ,"Geodude"},{75 ,"Graveler"},{76 ,"Golem"},
            {77 ,"Ponyta"},{78 ,"Rapidash"},
            {79 ,"Slowpoke"},{80 ,"Slowbro"},
            {81 ,"Magnemite"},{82 ,"Magneton"},
            {83 ,"Farfetch'd"},
            {84 ,"Doduo"},{85 ,"Dodrio"},
            {86 ,"Seel"},{87 ,"Dewgong"},
            {88 ,"Grimer"},{89 ,"Muk"},
            {90 ,"Shellder"},{91 ,"Cloyster"},
            {92 ,"Gastly"},{93 ,"Haunter"},{94 ,"Gengar"},
            {95 ,"Onix"},
            {96 ,"Drowzee"},{97 ,"Hypno"},
            {98 ,"Krabby"},{99 ,"Kingler"},
            {100 ,"Voltorb"},{101 ,"Electrode"},
            {102 ,"Exeggcute"},{103 ,"Exeggutor"},
            {104 ,"Cubone"},{105 ,"Marowak"},
            {106 ,"Hitmonlee"},
            {107 ,"Hitmonchan"},
            {108 ,"Lickitung"},
            {109 ,"Koffing"},{110 ,"Weezing"},
            {111 ,"Rhyhorn"},{112 ,"Rhydon"},
            {113 ,"Chansey"},
            {114 ,"Tangela"},
            {115 ,"Kangaskhan"},
            {116 ,"Horsea"},{117 ,"Seadra"},
            {118 ,"Goldeen"},{119 ,"Seaking"},
            {120 ,"Staryu"},{121 ,"Starmie"},
            {122 ,"Mr.Mime"},
            {123 ,"Scyther"},
            {124 ,"Jynx"},
            {125 ,"Electabuzz"},
            {126 ,"Magmar"},
            {127 ,"Pinsir"},
            {128 ,"Tauros"},
            {129 ,"Magikarp"},{130 ,"Gyarados"},
            {131 ,"Lapras"},
            {132 ,"Ditto"},
            {133 ,"Eevee"},{134 ,"Vaporeon"},{135 ,"Jolteon"},{136 ,"Flareon"},
            {137 ,"Porygon"},
            {138 ,"Omanyte"},{139 ,"Omastar"},
            {140 ,"Kabuto"},{141 ,"Kabutops"},
            {142 ,"Aerodactyl"},
            {143 ,"Snorlax"},
            {144 ,"Articuno"},
            {145 ,"Zapdos"},
            {146 ,"Moltres"},
            {147 ,"Dratini"},{148 ,"Dragonair"},{149 ,"Dragonite"},
            {150 ,"Mewtwo"},
            {151 ,"Mew"}
    };
        //gets loaded in once, have to call load_base_stats before creating new pokemon
        public static List<Dictionary<string, object>> all_base_stats = new List<Dictionary<string, object>>();
        public Dictionary<string, Moves> TM_Set = new Dictionary<string, Moves>();


        public string name;
        public int dexnum;
        public int level;
        public Pokedex pokedex_entry;

        public Type type1;
        public Type type2 = null;
        public List<Learnset> learnset = new List<Learnset>();


        public string image1;
        public string image2;
        public string shiny_image;

        public int iv = 30;

        public int base_hp;
        public int base_attack;
        public int base_defense;
        public int base_sp_attack;
        public int base_sp_defense;
        public int base_speed;

        public int current_hp;
        public double current_attack;
        public double current_defense;
        public double current_sp_attack;
        public double current_sp_defense;
        public double current_speed;
        public double current_accuracy = 1;
        public double current_evasion = 1;

        //This means the maximum at this level
        public int max_hp;
        public int max_attack;
        public int max_defense;
        public int max_sp_attack;
        public int max_sp_defense;
        public int max_speed;

        public int critical_stage = 0;
        public int attack_stage = 0;
        public int defense_stage = 0;
        public int sp_attack_stage = 0;
        public int sp_defense_stage = 0;
        public int speed_stage = 0;
        public int accuracy_stage = 0;
        public int evasion_stage = 0;

        public int type = 1;
        public int burn = 1;

        public Moves[] currentMoves = new Moves[4];
        public Moves struggle = Moves.get_move("Struggle");
        public int currentEXP;

        public double lvl_speed;
        public int base_lvl_exp;
        public int current_exp;
        public int next_lvl_exp;
        public bool time_to_evolve;

        public void reset_stages()
        {
            critical_stage = 0;
            attack_stage = 0;
            defense_stage = 0;
            sp_attack_stage = 0;
            sp_defense_stage = 0;
            speed_stage = 0;
            accuracy_stage = 0;
            evasion_stage = 0;
    }

        public void calculate_stats()
        {
            //var test = Pokemon.all_base_stats[this.dexnum - 1]["HP"];
            this.base_hp = int.Parse(Pokemon.all_base_stats[this.dexnum - 1]["HP"].ToString());
            this.base_attack = int.Parse(Pokemon.all_base_stats[this.dexnum - 1]["Attack"].ToString());
            this.base_defense = int.Parse(Pokemon.all_base_stats[this.dexnum - 1]["Defense"].ToString());
            this.base_sp_attack = int.Parse(Pokemon.all_base_stats[this.dexnum - 1]["Sp. Atk"].ToString());
            this.base_sp_defense = int.Parse(Pokemon.all_base_stats[this.dexnum - 1]["Sp. Def"].ToString());
            this.base_speed = int.Parse(Pokemon.all_base_stats[this.dexnum - 1]["Speed"].ToString());

            update_current_stats();

            this.current_hp = this.max_hp;
            this.current_attack = this.max_attack;
            this.current_defense = this.max_defense;
            this.current_sp_attack = this.max_sp_attack;
            this.current_sp_defense = this.max_sp_defense;
            this.current_speed = this.max_speed;
        }

        public void print_pokemon()
        {
            //Console.WriteLine(this.name + " type1 and type2" + ((this.type1) + (this.type1)));

            Console.WriteLine(this.name + "s type1 is: " + this.type1);
            if(this.type2 != null)
            {
                Console.WriteLine(this.name + "s type2 is: " + this.type2);
            }

            Console.WriteLine(this.name + "s attack1 is: " + this.currentMoves[0].name);
            Console.WriteLine(this.name + "s attack1 dmg is: " + this.currentMoves[0].base_power);
            Console.WriteLine(this.name + "s attack2 is: " + this.currentMoves[1].name);
            Console.WriteLine(this.name + "s attack3 is: " + this.currentMoves[2].name);
            Console.WriteLine(this.name + "s attack4 is: " + this.currentMoves[3].name);


        }
        //loads an image of the pokemon when created
        public void get_image_path()
        {
            var path = Directory.GetCurrentDirectory();

            if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
            {
                //Debug.Log("Does something happen here?");
                this.image1 = path + "/Images/PokemonImages/" + (this.dexnum).ToString().PadLeft(3, '0') + this.name + ".png";
                this.image2 = path + "/Images/PokemonImages/" + (this.dexnum).ToString().PadLeft(3, '0') + this.name + "2.png";
            }
            else
            {
                this.image1 = path + "\\Images\\PokemonImages\\" + (this.dexnum).ToString().PadLeft(3, '0') + this.name + ".png";
                this.image2 = path + "\\Images\\PokemonImages\\" + (this.dexnum).ToString().PadLeft(3, '0') + this.name + "2.png";
            }
            //Debug.Log(image1);
            //Debug.Log(image2);
        }

        public void set_exp()
        {
            //leveling is multiplier * level^3 up to level 50
            if (this.level <= 50)
            {
                this.base_lvl_exp = (int)(this.lvl_speed * Math.Pow(this.level, 3));
                this.next_lvl_exp = (int)(this.lvl_speed * Math.Pow(this.level + 1, 3));

                if (this.current_exp == 0 || this.current_exp < this.base_lvl_exp)
                {
                    this.current_exp = this.base_lvl_exp;
                }
            }

            //after level 50, next level is 7500 exp from 51 to 100
            else
            {
                this.base_lvl_exp = (int)(this.lvl_speed * Math.Pow(50, 3)) + (7500 * (this.level - 50));
                this.next_lvl_exp = (int)(this.lvl_speed * Math.Pow(50, 3)) + (7500 * (this.level + 1 - 50));
                if (this.current_exp == 0)
                {
                    this.current_exp = this.base_lvl_exp;
                }
            }

        }

        public void update_current_stats()
        {
            //iv is set to 30 
            //ev set for +20 for each stat
            //510 total ev / 4 = 127.5 / 6 = 21.25, each pokemon should have +21.25 to each stat when using all evs
            this.max_hp = (((((this.base_hp + this.iv) * 2) + 20) * this.level) / 100) + this.level + 10;
            this.max_attack = (((((this.base_attack + this.iv) * 2) + 20) * this.level) / 100) + 5;
            this.max_defense = (((((this.base_defense + this.iv) * 2) + 20) * this.level) / 100) + 5;
            this.max_sp_attack = (((((this.base_sp_attack + this.iv) * 2) + 20) * this.level) / 100) + 5;
            this.max_sp_defense = (((((this.base_sp_defense + this.iv) * 2) + 20) * this.level) / 100) + 5;
            this.max_speed = (((((this.base_speed + this.iv) * 2) + 20) * this.level) / 100) + 5;
        }

        public Moves check_learnset()
        {
            foreach (Learnset move in this.learnset)
            {
                if (this.level == move.level)
                {
                    return move.move;
                }
            }


            return null;
        }


        public bool check_evolve()
        {
            if (this.pokedex_entry.evolve_level != -1)
            {
                if (this.level >= this.pokedex_entry.evolve_level)
                {
                    return true;
                }
            }
            return false;

        }


        //for: trainer_wild_multiplier, if it is a trainer pokemon, needs to be set to 1.5, default 1
        public int gain_exp(int enemy_level, int enemy_base_exp, int num_player_pokemon_used, double trainer_wild_multipllier = 1)
        {
            //EXP = (a * t * e * b * L) / (7 * s)
            //a is wild or trainer pokemon: 1 or 1.5
            //t is trainer_pokemon or traded, always 1 for game
            //e is if lucky egg, always 1
            //b is enemy_base_exp
            //L is enemy_level
            int exp_gained = (int)(trainer_wild_multipllier * 1 * 1 * enemy_base_exp * enemy_level) / (7 * num_player_pokemon_used);
            this.current_exp += exp_gained;

            Debug.Log("base exp " + this.base_lvl_exp);
            Debug.Log("current exp " + this.current_exp);
            Debug.Log("next level exp " + this.next_lvl_exp);

            if (this.current_exp >= this.next_lvl_exp)
            {
                Debug.Log("Pokemon Leveled!");
                this.level += 1;

                //update stats
                this.update_current_stats();


                //check if learnable move
                Moves temp_new_move = this.check_learnset();
                if (temp_new_move != null)
                {
                    Debug.Log("Your pokemon learned a new move! " + temp_new_move.name);

                }

                //check evolve
                this.time_to_evolve = this.check_evolve();
                if (this.time_to_evolve)
                {
                    Debug.Log("Your Pokemon is ready to evolve!");
                }

                //update exp
                this.set_exp();
                Debug.Log("New base EXP " + this.base_lvl_exp);
                Debug.Log("Current EXP " + this.current_exp);
                Debug.Log("New Next levl EXP " + this.next_lvl_exp);

            }

            return exp_gained;

        }
    }
}
