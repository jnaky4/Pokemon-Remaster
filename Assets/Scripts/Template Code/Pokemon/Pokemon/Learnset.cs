using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace Pokemon
{
    class Learnset
    {
        public static string[][] all_learnset = new string[1089][];
        int level;
        Moves move;
        
        public Learnset(int level, string move)
        {
            this.level = level;
            this.move = Moves.get_move(move);
        }
        public static void load_learnset(string path)
        {
            //var path = @"C:\Users\Hyperlight Drifter\Desktop\Pokemon\Pokemon\LEARNSET.csv";


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
                    //Console.WriteLine(j);
                    //Console.WriteLine(String.Join(" ", row.Cast<string>()));
                    Learnset.all_learnset[j] = row;
                    j++;
                }



            }

        }

        public static List<Learnset> get_learnset(int dexnum, List<Learnset> learnset)
        {
            //Console.WriteLine(Pokemon.learnset.Length);

            for (int i = 0; i < Pokemon.all_learnset.Length - 1; i++)
            {
                //Console.WriteLine(i);
                //Console.WriteLine(Pokemon.learnset[i][0]);
                if (dexnum == Int32.Parse(Learnset.all_learnset[i][0]))
                {
                    
                    Learnset temp = new Learnset(Int32.Parse(Learnset.all_learnset[i][1]), Learnset.all_learnset[i][2]);
                    //Console.WriteLine("Added to Learnset: " + temp.level + " " + temp.move.move);
                    learnset.Add(temp);


                    //[i][1] level
                    //[i][2] movename
                }
            }
            return learnset;
        }
    }

}
