using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace Pokemon
{
    class Pokedex
    {
        public static string[][] pokedex = new string[152][];
        public static Dictionary<int,Pokedex> pokedex_dictionary = new Dictionary<int, Pokedex>();

        public int dexnum;
        public string type1;
        public string type2;
        public string stage;
        public string gender_ratio;
        public double height;
        public double weight;
        public string description;
        public string category;
        public string image1;
        public string image2;

        public Pokedex(int dexnum)
        {
            this.dexnum = dexnum;

            //load_pokemon_into_pokedex();
            get_pokemon_info();
        }

        public static void load_pokemon_into_pokedex()
        {
            var path = @"E:\Pokemon\Pokemon\POKEMON.csv";


            using (TextFieldParser csvParser = new TextFieldParser(path, System.Text.Encoding.Default))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;
                //skip headers
                //csvParser.ReadLine();

                int j = 0;
                while (!csvParser.EndOfData)
                {
                    string[] row = csvParser.ReadFields();
                    //Console.WriteLine(String.Join(" ", row));
                    //Pokemon.base_stats.Add(row);
                    Pokedex.pokedex[j] = row;
                    j++;
                }



            }

        }

        public void get_pokemon_info()
        {
            this.type1 = pokedex[dexnum][2];
            this.type2 = pokedex[dexnum][3];
            this.stage = pokedex[dexnum][4];
            this.gender_ratio = pokedex[dexnum][5];
            this.height = Convert.ToDouble(pokedex[dexnum][6]);
            this.weight = Convert.ToDouble(pokedex[dexnum][7]);
            this.description = pokedex[dexnum][8];
            this.category = pokedex[dexnum][9];
            //this.image1 = pokedex[dexnum][10];
            //this.image2 = pokedex[dexnum][11];
        }
        public string get_type1()
        {
            //Console.WriteLine("TYPE1: " + this.type1);
            return this.type1;
        }
        public string get_type2()
        {
            //Console.WriteLine("TYPE2: " + this.type2);
            return this.type2;
        }
    }
}
