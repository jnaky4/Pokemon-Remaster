using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace Pokemon
{
    class Type
    {
        public static string[][] type_chart = new string[15][];
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
            load_type_chart();
            get_type();

        }
        public void load_type_chart()
        {
            
            var path = @"E:\Pokemon\Pokemon\TYPE.csv";


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
            

        }

        public void get_type()
        {
            for(int i = 0; i < type_chart.Length; i++)
            {
                //Console.WriteLine(type_chart[i][0]);
                //Console.WriteLine(this.type);
                if (type_chart[i][0].Contains(this.type))
                {
                    //Console.WriteLine("FOUND");

                    this.index = i;
                    this.attack_fire = Convert.ToDouble(type_chart[i][1]);
                    this.attack_water = Convert.ToDouble(type_chart[i][2]);
                    this.attack_grass = Convert.ToDouble(type_chart[i][3]);
                    this.attack_electric = Convert.ToDouble(type_chart[i][4]);
                    this.attack_ice = Convert.ToDouble(type_chart[i][5]);
                    this.attack_fighting = Convert.ToDouble(type_chart[i][6]);
                    this.attack_poison = Convert.ToDouble(type_chart[i][7]);
                    this.attack_ground = Convert.ToDouble(type_chart[i][8]);
                    this.attack_flying = Convert.ToDouble(type_chart[i][9]);
                    this.attack_psychic = Convert.ToDouble(type_chart[i][10]);
                    this.attack_bug = Convert.ToDouble(type_chart[i][11]);
                    this.attack_rock = Convert.ToDouble(type_chart[i][12]);
                    this.attack_ghost = Convert.ToDouble(type_chart[i][13]);
                    this.attack_dragon = Convert.ToDouble(type_chart[i][14]);
                    this.attack_normal = Convert.ToDouble(type_chart[i][15]);




                    this.defend_fire = Convert.ToDouble(type_chart[0][i + 1]);
                    this.defend_water = Convert.ToDouble(type_chart[1][i + 1]);
                    this.defend_grass = Convert.ToDouble(type_chart[2][i + 1]);
                    this.defend_electric = Convert.ToDouble(type_chart[3][i + 1]);
                    this.defend_ice = Convert.ToDouble(type_chart[4][i + 1]);
                    this.defend_fighting = Convert.ToDouble(type_chart[5][i + 1]);
                    this.defend_poison = Convert.ToDouble(type_chart[6][i + 1]);
                    this.defend_ground = Convert.ToDouble(type_chart[7][i + 1]);
                    this.defend_flying = Convert.ToDouble(type_chart[8][i + 1]);
                    this.defend_psychic = Convert.ToDouble(type_chart[9][i + 1]);
                    this.defend_bug = Convert.ToDouble(type_chart[10][i + 1]);
                    this.defend_rock = Convert.ToDouble(type_chart[11][i + 1]);
                    this.defend_ghost = Convert.ToDouble(type_chart[12][i + 1]);
                    this.defend_dragon = Convert.ToDouble(type_chart[13][i + 1]);
                    this.defend_normal = Convert.ToDouble(type_chart[14][i + 1]);

/*                    Console.WriteLine("attacking fire " + ": " + this.attack_fire);
                    Console.WriteLine("attacking water" + ": " + this.attack_water);
                    Console.WriteLine("attacking grass" + ": " + this.attack_grass);

                    Console.WriteLine("defending fire " + ": " + this.defend_fire);
                    Console.WriteLine("defending water" + ": " + this.defend_water);
                    Console.WriteLine("defending grass" + ": " + this.defend_grass);*/

                    //Console.WriteLine("FOUND at " + i);
                    //Console.WriteLine("index[0][i+1]" + type_chart[0][i + 1]);
                    //Console.WriteLine("index[1][i+1]" + type_chart[1][i + 1]);
                    /*                    Console.WriteLine("DEFEND FIRE for: " + this.defend_fire);
                                        Console.WriteLine("ATTACK FIRE for: " + this.attack_fire);*/



                }
            }

        }
    }

}
