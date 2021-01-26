using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace Pokemon
{
    class Moves
    {
        public string move;
        public Type move_type;
        public int pp;
        public int base_power;
        public int accuracy;
        public string effect;
        public int prioirity;


        public Moves(string move)
        {
            //this.Moves = move_dictionary[move];
            this.move = move;
        }
        public Moves(string move, Type move_type, int pp, int base_power, int accuracy, string effect)
        {
            this.move = move;
            this.move_type = move_type;
            this.pp = pp;
            this.base_power = base_power;
            this.accuracy = accuracy;
            this.effect = effect;
            
        }
        public static string[][] move_list = new string[151][];
        public static Dictionary<string, Moves> move_dictionary = new Dictionary<string, Moves>();

        public static void load_moves()
        {
            var path = @"E:\Pokemon\Pokemon\MOVES.csv";


            using (TextFieldParser csvParser = new TextFieldParser(path, System.Text.Encoding.Default))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;
                //skip headers
                csvParser.ReadLine();

                //int j = 0;
                while (!csvParser.EndOfData)
                {
                    string[] row = csvParser.ReadFields();
                    //Console.WriteLine(String.Join(" ", row));

                    //creates a new move object in dictionary to access later
                    Moves move = new Moves(row[0], new Type(row[1]), Convert.ToInt32(row[2]), Convert.ToInt32(row[3]), Convert.ToInt32(row[4]), row[5]);
                    move_dictionary.Add(move.move, move);
                }
            }
        }

        //grab any move from dictionary
        public static Moves get_move(string move)
        {
            return Moves.move_dictionary[move];
        }
    }
}
