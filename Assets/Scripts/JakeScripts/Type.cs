using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Pokemon
{
    public class Type
    {
        //Not Used
        public enum Types { Fire, Water, Grass, Electric, Ice, Fighting, Poison, Ground, Flying, Psychic, Bug, Rock, Ghost, Dragon, Dark, Steel, Normal, Null }


        //Atk typ           Fr  Wa  Gr  El  Ic  Fg  Po  Gd  Fl  Py  Bg  Ro  Gh  Dr  No  Nu    
        ///*Fire*/	        .5,	.5,	2,	1,	2,	1,	1,	1,	1,	1,	2,	.5,	1,	.5,	1   1 
        ///*Water*/	        2,	.5,	.5,	1,	1,	1,	1,	2,	1,	1,	1,	2,	1,	.5,	1   1 
        ///*Grass*/	        .5,	2,	.5,	1,	1,	1,	.5,	2,	.5,	1,	.5,	2,	1,	.5,	1   1 
        ///*Electric*/	    1,	2,	.5,	.5,	1,	1,	1,	0,	2,	1,	1,	1,	1,	.5,	1   1 
        ///*Ice*/	        1,	.5,	2,	1,	.5,	1,	1,	2,	2,	1,	1,	1,	1,	2,	1   1 
        ///*Fighting*/	    1,	1,	1,	1,	2,	1,	.5,	1,	.5,	.5,	.5,	2,	0,	1,	2   1 
        ///*Poison*/	    1,	1,	2,	1,	1,	1,	.5,	.5,	1,	1,	2,	.5,	.5,	1,	1   1
        ///*Ground*/        2,	1,	.5,	2,	1,	1,	2,	1,	0,	1,	.5,	2,	1,	1,	1   1
        ///*Flying*/	    1,	1,	2,	.5,	1,	2,	1,	1,	1,	1,	2,	.5,	1,	1,	1   1
        ///*Psychic*/	    1,	1,	1,	1,	1,	2,	2,	1,	1,	.5,	1,	1,	1,	1,	1   1
        ///*Bug*/	        .5,	1,	2,	1,	1,	.5,	2,	1,	.5,	2,	1,	1,	.5,	1,	1   1
        ///*Rock*/	        2,	1,	1,	1,	2,	.5,	1,	.5,	2,	1,	2,	1,	1,	1,	1   1
        ///*Ghost*/	        1,	1,	1,	1,	1,	1,	1,	1,	1,	0,	1,	1,	2,	1,	0   1
        ///*Dragon*/	    1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	2,	1   1
        ///*Normal*/	    1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	.5,	0,	1,	1   1
        ///*Null*/          1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1   1

        /*
         * 
         *  Attacking,  Fr Wa Gr El Ic Fg Po Gd Fl Py Bg Ro Gh Dr Dk St No Nu
            Fi          .5 .5 2  1  2  1  1  1  1  1  2  .5 1  .5 1  2  1  1
            Wa          2  .5 .5 1  1  1  1  2  1  1  1  2  1  .5 1  1  1  1
            Gr          5  2  .5 1  1  1  .5 2  .5 1  .5 2  1  .5 1  .5 1  1
            El          1  2  .5 .5 1  1  1  0  2  1  1  1  1  .5 1  1  1  1
            Ic          1  .5 1,0.5,1,1,2,2,1,1,1,1,2,1,0.5,1,1
            Fg          1,1,1,1,2,1,0.5,1,0.5,0.5,0.5,2,0,1,2,2,2,1
            Po          1,1,2,1,1,1,0.5,0.5,1,1,2,0.5,0.5,1,1,0,1,1
            Gd          2,1,0.5,2,1,1,2,1,0,1,0.5,2,1,1,1,2,1,1
            Fl          1,1,2,0.5,1,2,1,1,1,1,2,0.5,1,1,1,0.5,1,1
            Ps          1,1,1,1,1,2,2,1,1,0.5,1,1,1,1,0,0.5,1,1
            Bg          0.5,1,2,1,1,0.5,2,1,0.5,2,1,1,0.5,1,2,0.5,1,1
            Ro          2,1,1,1,2,0.5,1,0.5,2,1,2,1,1,1,1,0.5,1,1
            Gh          1,1,1,1,1,1,1,1,1,0,1,1,2,1,0.5,0.5,0,1
            Dr          1,1,1,1,1,1,1,1,1,1,1,1,1,2,1,0.5,1,1
            Dk          1,1,1,1,1,0.5,1,1,1,2,1,1,2,1,0.5,0.5,1,1
            St          0.5,0.5,1,0.5,2,1,1,1,1,1,1,2,1,1,1,0.5,1,1
            No          1,1,1,1,1,1,1,1,1,1,1,0.5,0,1,1,0.5,1,1
            Nu          1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1
         * 
         * 
         * 
         */



        //CSV's get loaded into these Lists
        public static List<Dictionary<string, object>> type_attack = new();
        //public static List<Dictionary<string, object>> type_defend = new List<Dictionary<string, object>>();
        //After a Static Dictionary of Types is created with load_type()
        public static Dictionary<string, Type> type_dictionary = new();

        public static Dictionary<string, Dictionary<string, double>> attacking_type_dict = new();

        public string name;


        //for attacking
        public double attack_fire;
        public double attack_water;
        public double attack_grass;
        public double attack_electric;
        public double attack_ice;
        public double attack_fighting;
        public double attack_poison;
        public double attack_ground;
        public double attack_flying;
        public double attack_psychic;
        public double attack_bug;
        public double attack_rock;
        public double attack_ghost;
        public double attack_dragon;
        public double attack_dark;
        public double attack_steel;
        public double attack_normal;
        public double attack_null;

        public Type(
            string name,

            double a_fire,
            double a_water,
            double a_grass,
            double a_electric,
            double a_ice,
            double a_fighting,
            double a_poison,
            double a_ground,
            double a_flying,
            double a_psychic,
            double a_bug,
            double a_rock,
            double a_ghost,
            double a_dragon,
            double a_dark,
            double a_steel,
            double a_normal,
            double a_null)

        {
            this.name = name;
            //for attacking
            attack_fire = a_fire;
            attack_water = a_water;
            attack_grass = a_grass;
            attack_electric = a_electric;
            attack_ice = a_ice;
            attack_fighting = a_fighting;
            attack_poison = a_poison;
            attack_ground = a_ground;
            attack_flying = a_flying;
            attack_psychic = a_psychic;
            attack_bug = a_bug;
            attack_rock = a_rock;
            attack_ghost = a_ghost;
            attack_dragon = a_dragon;
            attack_dark = a_dark;
            attack_steel = a_steel;
            attack_normal = a_normal;
            attack_null = a_null;
        }
        public static void load_type()
        {
            type_dictionary.Clear();
            for (var i = 0; i < type_attack.Count; i++)
            {
                string temp_type_str = type_attack[i]["Attacking"].ToString();

                double attack_fire = double.Parse(type_attack[i]["Fire"].ToString());
                double attack_water = double.Parse(type_attack[i]["Water"].ToString());
                double attack_grass = double.Parse(type_attack[i]["Grass"].ToString());
                double attack_electric = double.Parse(type_attack[i]["Electric"].ToString());
                double attack_ice = double.Parse(type_attack[i]["Ice"].ToString());
                double attack_fighting = double.Parse(type_attack[i]["Fighting"].ToString());
                double attack_poison = double.Parse(type_attack[i]["Poison"].ToString());
                double attack_ground = double.Parse(type_attack[i]["Ground"].ToString());
                double attack_flying = double.Parse(type_attack[i]["Flying"].ToString());
                double attack_psychic = double.Parse(type_attack[i]["Psychic"].ToString());
                double attack_bug = double.Parse(type_attack[i]["Bug"].ToString());
                double attack_rock = double.Parse(type_attack[i]["Rock"].ToString());
                double attack_ghost = double.Parse(type_attack[i]["Ghost"].ToString());
                double attack_dragon = double.Parse(type_attack[i]["Dragon"].ToString());
                double attack_dark = double.Parse(type_attack[i]["Dark"].ToString());
                double attack_steel = double.Parse(type_attack[i]["Steel"].ToString());
                double attack_normal = double.Parse(type_attack[i]["Normal"].ToString());
                double attack_null = double.Parse(type_attack[i]["Null"].ToString());


                Type temp_type = new                    (
                    temp_type_str,
                    attack_fire,
                    attack_water,
                    attack_grass,
                    attack_electric,
                    attack_ice,
                    attack_fighting,
                    attack_poison,
                    attack_ground,
                    attack_flying,
                    attack_psychic,
                    attack_bug,
                    attack_rock,
                    attack_ghost,
                    attack_dragon,
                    attack_dark,
                    attack_steel,
                    attack_normal,
                    attack_null
                    );

                type_dictionary.Add(temp_type_str, temp_type);
            }



            attacking_type_dict.Clear();
            //initial dictionary loaded into is type_defend
            for (int i = 0; i < type_attack.Count; i++)
            {


                Dictionary<string, double> temp = new()
                {
                    ["Fire"] = double.Parse(type_attack[i]["Fire"].ToString()),
                    ["Water"] = double.Parse(type_attack[i]["Water"].ToString()),
                    ["Grass"] = double.Parse(type_attack[i]["Grass"].ToString()),
                    ["Electric"] = double.Parse(type_attack[i]["Electric"].ToString()),
                    ["Ice"] = double.Parse(type_attack[i]["Ice"].ToString()),
                    ["Fighting"] = double.Parse(type_attack[i]["Fighting"].ToString()),
                    ["Poison"] = double.Parse(type_attack[i]["Poison"].ToString()),
                    ["Ground"] = double.Parse(type_attack[i]["Ground"].ToString()),
                    ["Flying"] = double.Parse(type_attack[i]["Flying"].ToString()),
                    ["Psychic"] = double.Parse(type_attack[i]["Psychic"].ToString()),
                    ["Bug"] = double.Parse(type_attack[i]["Bug"].ToString()),
                    ["Rock"] = double.Parse(type_attack[i]["Rock"].ToString()),
                    ["Ghost"] = double.Parse(type_attack[i]["Ghost"].ToString()),
                    ["Dragon"] = double.Parse(type_attack[i]["Dragon"].ToString()),
                    ["Dark"] = double.Parse(type_attack[i]["Dark"].ToString()),
                    ["Steel"] = double.Parse(type_attack[i]["Steel"].ToString()),
                    ["Normal"] = double.Parse(type_attack[i]["Normal"].ToString()),
                    ["Null"] = double.Parse(type_attack[i]["Null"].ToString()),
                };


                string type = type_attack[i]["Attacking"].ToString();
                attacking_type_dict[type] = temp;
            }
        }

        
        public static Type get_type(string type)
        {
            /*Console.WriteLine(type);*/
            return type_dictionary[type];
        }

    }
}
