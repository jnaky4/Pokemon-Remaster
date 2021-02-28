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
        public int level;
        public Moves move;
        public override string ToString()
        {
            return "Level: " + level + "   Move: " + move.name;
        }

        public Learnset(int level, string move)
        {
            this.level = level;
            this.move = Moves.get_move(move);
        }

        public static List<Learnset> get_learnset(int dexnum)
        {
            List<Learnset> pokemons_learnset = new List<Learnset>();
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

        public int Get_lvl()
        {
            return this.level;
        }
        Moves Get_move()
        {
            return this.move;
        }
        public static Pokemon add_wild_moves(int dexnum, int level)
        {

            //create a temp pokemon to grab the learnset
            Pokemon temp_pokemon = new Pokemon(dexnum, level);
            List<Learnset> pokemons_learnset = temp_pokemon.learnset;
            List<Learnset> learnable_moves = new List<Learnset>();
            foreach (Learnset learnable_move in pokemons_learnset)
            {
                if (learnable_move.level <= level)
                {
                    learnable_moves.Add(learnable_move);
                }
            }
            int num_available_moves = learnable_moves.Count;
            for (int i = 0; i < 4; i++)
            {
                if (i >= num_available_moves)
                {
                    return temp_pokemon;
                }
                else
                {
                    //Debug.Log("Size of Learnable Moves: " + learnable_moves.Count);
                    int random = UnityEngine.Random.Range(0, learnable_moves.Count);
                    //Debug.Log("Random Pick Index: " + random);
                    Moves move = Moves.get_move(learnable_moves[random].move.name);
                    //Debug.Log("Move Learned: " + move.move);
                    temp_pokemon.currentMoves[i] = move;
                    //make sure not to add duplicates
                    learnable_moves.Remove(learnable_moves[random]);
                }
            }
            return temp_pokemon;


            //currentMoves = new Moves[4];
        }

    }

}
