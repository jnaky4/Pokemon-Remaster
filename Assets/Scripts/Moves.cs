using System;
using System.Collections.Generic;
using System.Text;
//using Microsoft.VisualBasic.FileIO;

namespace Pokemon
{
    public class Moves
    {
        public string move = "default";
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
        //public static string[][] move_list = new string[151][];
        public static Dictionary<string, Moves> move_dictionary = new Dictionary<string, Moves>();
        public static List<Dictionary<string, object>> all_moves = new List<Dictionary<string, object>>();

        public static void load_moves()
        {
            for (var i = 0; i < all_moves.Count; i++)
            {
                //creates a new move object in dictionary to access later
                Moves move = new Moves(
                    all_moves[i]["Name"].ToString(),
                    Type.get_type(all_moves[i]["Type"].ToString()),
                    int.Parse(all_moves[i]["PP"].ToString()),
                    int.Parse(all_moves[i]["Att."].ToString()),
                    int.Parse(all_moves[i]["Acc."].ToString()),
                    all_moves[i]["Effect"].ToString());


                move_dictionary.Add(move.move, move);
            }

        }
        //grab any move from dictionary
        public static Moves get_move(string move)
        {
            return Moves.move_dictionary[move];

        }
        
    }
}
