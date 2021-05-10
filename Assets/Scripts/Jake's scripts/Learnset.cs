using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
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
            
            //add all moves to learnable moves that are equal or lower lvl than the wild pokemon
            foreach (Learnset learnable_move in pokemons_learnset)
            {
                if (learnable_move.level <= level)
                {

                    learnable_moves.Add(learnable_move);
                    Debug.Log("Initally: " + learnable_move);
                }
            }
            

            int num_available_moves = learnable_moves.Count;
            int num_available_moves_left = num_available_moves;
            Debug.Log("Start Count: " + num_available_moves);
            for (int i = 0; i < 4; i++)
            {
                //if there are less available moves than 4 slots, stop getting new moves
                if (i >= num_available_moves)
                {
                    //foreach (Moves current_move in temp_pokemon.currentMoves) Debug.Log("List of Moves 80: " + current_move.name);
                    return temp_pokemon;
                }
                else
                {
                    bool move_added = false;
                    //while there are still learnable moves and one hasnt been added
                    while (learnable_moves.Count != 0 && !move_added) { 
                        
                        //Debug.Log("Size of Learnable Moves: " + learnable_moves.Count);

                        //max is inclusive, get a random value to pick move
                        int random = UnityEngine.Random.Range(0, learnable_moves.Count);

                        //Debug.Log("Random Pick Index: " + random);
                    
                        bool has_move = false;
                        
                        //create move object from move
                        Moves move = Moves.get_move(learnable_moves[random].move.name);
                        Debug.Log("Grabbed " + move.name);
                        //iterate through current moves
                        foreach (Moves current_move in temp_pokemon.currentMoves)
                        {
                            //slot can be null
                            if(current_move != null)
                            {
                                //if pokemon alredy has the move
                                if (move.name == current_move.name)
                                {

                                    //has move is true, remove from potential learnable moves
                                    has_move = true;
                                    learnable_moves.RemoveAt(random);
                                }
                            }

                        }
                        if (!has_move)
                        {
                            Debug.Log("No Dupe, Adding Move: " + move.name);
                            temp_pokemon.currentMoves[i] = move;
                            move_added = true;
                            //make sure not to add duplicates
                            learnable_moves.Remove(learnable_moves[random]);
                            Debug.Log("Learnable Moves Left: " + learnable_moves.Count);
                        }
                        //Debug.Log("Move Learned: " + move.move);

                        
                        
                    }
                }
            }
            //foreach (Moves current_move in temp_pokemon.currentMoves) Debug.Log("List of Moves 131: " + current_move.name);
            return temp_pokemon;


            //currentMoves = new Moves[4];
        }

    }

}
