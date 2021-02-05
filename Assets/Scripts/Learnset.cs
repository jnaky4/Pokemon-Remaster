using System;
using System.Collections.Generic;
using System.Text;
//using Microsoft.VisualBasic.FileIO;

namespace Pokemon
{
    public class Learnset
    {
        //public static string[][] all_learnset = new string[1089][];
        public static List<Dictionary<string, object>> all_learnset = new List<Dictionary<string, object>>();
        int level;
        Moves move;
        public override string ToString()
        {
            return "Level: " + level + "   Move: " + move.move;
        }

        public Learnset(int level, string move)
        {
            this.level = level;
            this.move = Moves.get_move(move); 
        }

        public static List<Learnset> get_learnset(int dexnum, List<Learnset> pokemons_learnset)
        {
            //Console.WriteLine(Pokemon.learnset.Length);
            for (var i = 0; i < all_learnset.Count; i++)
            {
                //int.Parse(Pokemon.all_base_stats[this.dexnum - 1]["HP"].ToString());
                
                if (dexnum == int.Parse(Learnset.all_learnset[i]["dexnum"].ToString()))
                {
                    
                    Learnset temp = new Learnset(int.Parse(Learnset.all_learnset[i]["lvl"].ToString()), Learnset.all_learnset[i]["move"].ToString());
                    pokemons_learnset.Add(temp);
                }
            }
            return pokemons_learnset;
        }
        public int get_lvl()
        {
            return this.level;
        }
        public Moves get_move()
        {
            return this.move;
        }
    }

}
