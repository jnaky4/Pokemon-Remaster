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


        //for defenses
        public int defend_fire;
        public int defend_water;
        public int defend_grass;
        public int defend_electric;
        public int defend_fighting;
        public int defend_poison;
        public int defend_ground;
        public int defend_flying;
        public int defend_psychic;
        public int defend_bug;
        public int defend_rock;
        public int defend_ghost;
        public int defend_dragon;
        public int defend_normal;


        //for attacking
        public int attack_fire;
        public int attack_water;
        public int attack_grass;
        public int attack_electric;
        public int attack_fighting;
        public int attack_poison;
        public int attack_ground;
        public int attack_flying;
        public int attack_psychic;
        public int attack_bug;
        public int attack_rock;
        public int attack_ghost;
        public int attack_dragon;
        public int attack_normal;

        public Type(string type)
        {
            this.type = type;

            

        }
        public void get_base_stats()
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
                    //Console.WriteLine(String.Join(" ", row.Cast<string>()));
                    Type.type_chart[j] = row;
                    j++;
                }



            }
        }
    }

}
