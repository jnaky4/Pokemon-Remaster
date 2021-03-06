﻿using System.Collections.Generic;
//using Microsoft.VisualBasic.FileIO;

namespace Pokemon
{
    public class Moves
    {
        public string name = "default";
        public Type move_type;
        public int current_pp;
        public int maxpp;
        public int base_power;
        public int accuracy;
        public string category;
        public Status status;
        public double status_chance;
        public string status_target;
        public int max_per_turn;
        public int min_per_turn;
        public int max_turns;
        public int min_turns;
        public string target;
        public double chance_stat_change;
        public string current_stat_change;
        public int stat_change_amount;
        public int priority;
        public double heal;
        public string description;


        public Moves(string move)
        {
            //this.Moves = move_dictionary[move];
            this.name = move;

        }

        public Moves(string move, Type move_type, int pp, int base_power,
            int accuracy, string category, Status status, double status_chance,
            string status_target, int max_per_turn, int min_per_turn, int max_turns, int min_turns,
            string target, double chance_stat_change, string current_stat_change,
            int stat_change_amount, int priority, double heal, string description)
        {
            this.name = move;
            this.move_type = move_type;
            this.current_pp = pp;
            this.maxpp = pp;
            this.base_power = base_power;
            this.accuracy = accuracy;
            this.category = category;
            this.status = status;
            this.status_chance = status_chance;
            this.status_target = status_target;
            this.max_per_turn = max_per_turn;
            this.min_per_turn = min_per_turn;
            this.max_turns = max_turns;
            this.min_turns = min_turns;
            this.target = target;
            this.chance_stat_change = chance_stat_change;
            this.current_stat_change = current_stat_change;
            this.stat_change_amount = stat_change_amount;
            this.priority = priority;
            this.heal = heal;
            this.description = description;
        }
        //public static string[][] move_list = new string[151][];
        public static Dictionary<string, Moves> move_dictionary = new Dictionary<string, Moves>();
        public static List<Dictionary<string, object>> all_moves = new List<Dictionary<string, object>>();

        public static void load_moves()
        {
            move_dictionary.Clear();
            for (var i = 0; i < all_moves.Count; i++)
            {

                //creates a new move object in dictionary to access later
                Moves move = new Moves(
                    all_moves[i]["Name"].ToString(),
                    Type.get_type(all_moves[i]["Type"].ToString()),
                    int.Parse(all_moves[i]["PP"].ToString()),
                    int.Parse(all_moves[i]["Att."].ToString()),
                    int.Parse(all_moves[i]["Acc."].ToString()),
                    all_moves[i]["Category"].ToString(),
                    Status.get_status(all_moves[i]["Status"].ToString()),
                    double.Parse(all_moves[i]["Status_Chance"].ToString()),
                    all_moves[i]["Status_Target"].ToString(),
                    int.Parse(all_moves[i]["Max_Per_Turn"].ToString()),
                    int.Parse(all_moves[i]["Min_Per_Turn"].ToString()),
                    int.Parse(all_moves[i]["Max_Turns"].ToString()),
                    int.Parse(all_moves[i]["Min_Turns"].ToString()),
                    all_moves[i]["Target"].ToString(),
                    double.Parse(all_moves[i]["Chance_Stat_Change"].ToString()),
                    all_moves[i]["Current_Stat_Change"].ToString(),
                    int.Parse(all_moves[i]["Stat_Change_Amount"].ToString()),
                    int.Parse(all_moves[i]["Priority"].ToString()),
                    double.Parse(all_moves[i]["Heal"].ToString()),
                    all_moves[i]["Effect"].ToString()
                    ); ;


                move_dictionary.Add(move.name, move);
            }

        }
        //grab any move from dictionary
        public static Moves get_move(string move)
        {
            Moves move_reference = Moves.move_dictionary[move];
            Moves new_move = new Moves(move_reference.name, move_reference.move_type,
                move_reference.maxpp, move_reference.base_power, move_reference.accuracy,
                move_reference.category, move_reference.status, move_reference.status_chance,
                move_reference.status_target, move_reference.max_per_turn, move_reference.min_per_turn,
                move_reference.max_turns, move_reference.min_turns, move_reference.target,
                move_reference.chance_stat_change, move_reference.current_stat_change,
                move_reference.stat_change_amount, move_reference.priority, move_reference.heal,
                move_reference.description);

            return new_move;

        }

    }
}
