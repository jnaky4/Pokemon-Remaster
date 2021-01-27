using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace Pokemon
{
    class Type
    {
        public static string[][] type_chart = new string[15][];
        public static Dictionary<string, Type> type_dictionary = new Dictionary<string, Type>();
        public string type;
        public int index;

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

        public Type(string type)
        {
            this.type = type;
            //Console.WriteLine("Creating " + this.type +" type");
            //load_type_chart();
            //get_type();

        }
        public Type(string type, double a_fire, double a_water, double a_grass,
            double a_electric, double a_ice, double a_fighting, double a_poison,
            double a_ground, double a_flying, double a_psychic, double a_bug,
            double a_rock, double a_ghost, double a_dragon, double a_normal,
            double d_fire, double d_water, double d_grass,
            double d_electric, double d_ice, double d_fighting, double d_poison,
            double d_ground, double d_flying, double d_psychic, double d_bug,
            double d_rock, double d_ghost, double d_dragon, double d_normal)
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
        public static void load_type(string path)
        {
            
            //var path = @"C:\Users\Hyperlight Drifter\Desktop\Pokemon\Pokemon\TYPE.csv";


            using (TextFieldParser csvParser = new TextFieldParser(path, System.Text.Encoding.Default))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;
                //skip headers
                csvParser.ReadLine();

                int j = 0;
                while (!csvParser.EndOfData)
                {
                    string[] row = csvParser.ReadFields();
                    //Console.WriteLine(String.Join(" ", row));
                    
                    Type.type_chart[j] = row;
                    j++;
                }



            }
            for (int i = 0; i < type_chart.Length; i++)
            {
                string temp_type_str = type_chart[i][0];
                double attack_fire = Convert.ToDouble(type_chart[i][1]);
                double attack_water = Convert.ToDouble(type_chart[i][2]);
                double attack_grass = Convert.ToDouble(type_chart[i][3]);
                double attack_electric = Convert.ToDouble(type_chart[i][4]);
                double attack_ice = Convert.ToDouble(type_chart[i][5]);
                double attack_fighting = Convert.ToDouble(type_chart[i][6]);
                double attack_poison = Convert.ToDouble(type_chart[i][7]);
                double attack_ground = Convert.ToDouble(type_chart[i][8]);
                double attack_flying = Convert.ToDouble(type_chart[i][9]);
                double attack_psychic = Convert.ToDouble(type_chart[i][10]);
                double attack_bug = Convert.ToDouble(type_chart[i][11]);
                double attack_rock = Convert.ToDouble(type_chart[i][12]);
                double attack_ghost = Convert.ToDouble(type_chart[i][13]);
                double attack_dragon = Convert.ToDouble(type_chart[i][14]);
                double attack_normal = Convert.ToDouble(type_chart[i][15]);




                double defend_fire = Convert.ToDouble(type_chart[0][i + 1]);
                double defend_water = Convert.ToDouble(type_chart[1][i + 1]);
                double defend_grass = Convert.ToDouble(type_chart[2][i + 1]);
                double defend_electric = Convert.ToDouble(type_chart[3][i + 1]);
                double defend_ice = Convert.ToDouble(type_chart[4][i + 1]);
                double defend_fighting = Convert.ToDouble(type_chart[5][i + 1]);
                double defend_poison = Convert.ToDouble(type_chart[6][i + 1]);
                double defend_ground = Convert.ToDouble(type_chart[7][i + 1]);
                double defend_flying = Convert.ToDouble(type_chart[8][i + 1]);
                double defend_psychic = Convert.ToDouble(type_chart[9][i + 1]);
                double defend_bug = Convert.ToDouble(type_chart[10][i + 1]);
                double defend_rock = Convert.ToDouble(type_chart[11][i + 1]);
                double defend_ghost = Convert.ToDouble(type_chart[12][i + 1]);
                double defend_dragon = Convert.ToDouble(type_chart[13][i + 1]);
                double defend_normal = Convert.ToDouble(type_chart[14][i + 1]);

                Type temp_type = new Type(temp_type_str, attack_fire, attack_water, attack_grass,
                    attack_electric, attack_ice, attack_fighting, attack_poison,
                    attack_ground, attack_flying, attack_psychic, attack_bug,
                    attack_rock, attack_ghost, attack_dragon, attack_normal,
                    defend_fire, defend_water, defend_grass,
                    defend_electric, defend_ice, defend_fighting, defend_poison,
                    defend_ground, defend_flying, defend_psychic, defend_bug,
                    defend_rock, defend_ghost, defend_dragon, defend_normal);


                Type.type_dictionary.Add(temp_type_str, temp_type);
            }

            
        }
        public static Type get_type(string type)
        {
            return type_dictionary[type];
        }

    }

}
