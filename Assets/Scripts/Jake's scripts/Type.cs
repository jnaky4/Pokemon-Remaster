using System.Collections.Generic;

namespace Pokemon
{
    public class Type
    {
        //CSV's get loaded into these Lists
        public static List<Dictionary<string, object>> type_attack = new List<Dictionary<string, object>>();
        public static List<Dictionary<string, object>> type_defend = new List<Dictionary<string, object>>();
        //After a Static Dictionary of Types is created with load_type()
        public static Dictionary<string, Type> type_dictionary = new Dictionary<string, Type>();
        public string type;

        //for defenses
        public double defend_fire;
        public double defend_water;
        public double defend_grass;
        public double defend_electric;
        public double defend_ice;
        public double defend_fighting;
        public double defend_poison;
        public double defend_ground;
        public double defend_flying;
        public double defend_psychic;
        public double defend_bug;
        public double defend_rock;
        public double defend_ghost;
        public double defend_dragon;
        public double defend_normal;


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
        public double attack_normal;

        public Type(
            string type,

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
            double a_normal,

            double d_fire,
            double d_water,
            double d_grass,
            double d_electric,
            double d_ice,
            double d_fighting,
            double d_poison,
            double d_ground,
            double d_flying,
            double d_psychic,
            double d_bug,
            double d_rock,
            double d_ghost,
            double d_dragon,
            double d_normal)

        {
            this.type = type;
            //for attacking
            this.attack_fire = a_fire;
            this.attack_water = a_water;
            this.attack_grass = a_grass;
            this.attack_electric = a_electric;
            this.attack_ice = a_ice;
            this.attack_fighting = a_fighting;
            this.attack_poison = a_poison;
            this.attack_ground = a_ground;
            this.attack_flying = a_flying;
            this.attack_psychic = a_psychic;
            this.attack_bug = a_bug;
            this.attack_rock = a_rock;
            this.attack_ghost = a_ghost;
            this.attack_dragon = a_dragon;
            this.attack_normal = a_normal;

            //for defending
            this.defend_fire = d_fire;
            this.defend_water = d_water;
            this.defend_grass = d_grass;
            this.defend_electric = d_electric;
            this.defend_ice = d_ice;
            this.defend_fighting = d_fighting;
            this.defend_poison = d_poison;
            this.defend_ground = d_ground;
            this.defend_flying = d_flying;
            this.defend_psychic = d_psychic;
            this.defend_bug = d_bug;
            this.defend_rock = d_rock;
            this.defend_ghost = d_ghost;
            this.defend_dragon = d_dragon;
            this.defend_normal = d_normal;



        }
        public static void load_type()
        {
            for (var i = 0; i < type_defend.Count; i++)
            {
                string temp_type_str = type_defend[i]["Defending"].ToString();


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
                double attack_normal = double.Parse(type_attack[i]["Normal"].ToString());




                double defend_fire = double.Parse(type_defend[i]["Fire"].ToString());
                double defend_water = double.Parse(type_defend[i]["Water"].ToString());
                double defend_grass = double.Parse(type_defend[i]["Grass"].ToString());
                double defend_electric = double.Parse(type_defend[i]["Electric"].ToString());
                double defend_ice = double.Parse(type_defend[i]["Ice"].ToString());
                double defend_fighting = double.Parse(type_defend[i]["Fighting"].ToString());
                double defend_poison = double.Parse(type_defend[i]["Poison"].ToString());
                double defend_ground = double.Parse(type_defend[i]["Ground"].ToString());
                double defend_flying = double.Parse(type_defend[i]["Flying"].ToString());
                double defend_psychic = double.Parse(type_defend[i]["Psychic"].ToString());
                double defend_bug = double.Parse(type_defend[i]["Bug"].ToString());
                double defend_rock = double.Parse(type_defend[i]["Rock"].ToString());
                double defend_ghost = double.Parse(type_defend[i]["Ghost"].ToString());
                double defend_dragon = double.Parse(type_defend[i]["Dragon"].ToString());
                double defend_normal = double.Parse(type_defend[i]["Normal"].ToString());

                Type temp_type = new Type
                    (
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
                    attack_normal,

                    defend_fire,
                    defend_water,
                    defend_grass,
                    defend_electric,
                    defend_ice,
                    defend_fighting,
                    defend_poison,
                    defend_ground,
                    defend_flying,
                    defend_psychic,
                    defend_bug,
                    defend_rock,
                    defend_ghost,
                    defend_dragon,
                    defend_normal
                    );



                type_dictionary.Add(temp_type_str, temp_type);
            }
        }

        public static Type get_type(string type)
        {
            /*Console.WriteLine(type);*/
            return type_dictionary[type];
        }

    }
}
