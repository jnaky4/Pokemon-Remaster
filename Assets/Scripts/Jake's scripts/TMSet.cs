using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;


namespace Pokemon
{
    public class TMSet
    {
        public static List<Dictionary<string, object>> all_TMSet = new List<Dictionary<string, object>>();

        string TM_name;
        Moves move;

        public static Dictionary<string, Moves> get_TMSet(int dexNum)
        {
            Dictionary<string, Moves> TM_Set = new Dictionary<string, Moves>();
            bool at_index = false;
            if (dexNum <= 75)
            {
                for (int i = 0; i < all_TMSet.Count; i++)
                {
                    if (int.Parse(all_TMSet[i]["DexNum"].ToString()) == dexNum)
                    {
                        //Debug.Log("TM_NAME" + all_TMSet[i]["Move_Name"].ToString());
                        TM_Set.Add(all_TMSet[i]["Move_Name"].ToString(), Moves.get_move(all_TMSet[i]["Move_Name"].ToString()));
                        at_index = true;
                    }
                    else if (at_index == true)
                    {
                        //Debug.Log("Breaking Out");
                        break;
                    }
                    //Debug.Log("In for Loop still");
                }
            }
            else
            {
                for (int i = all_TMSet.Count - 1; i > 0; i--)
                {
                    if (int.Parse(all_TMSet[i]["DexNum"].ToString()) == dexNum)
                    {
                        //Debug.Log("TM_NAME" + all_TMSet[i]["Move_Name"].ToString());
                        TM_Set.Add(all_TMSet[i]["Move_Name"].ToString(), Moves.get_move(all_TMSet[i]["Move_Name"].ToString()));
                        at_index = true;
                    }
                    else if (at_index == true)
                    {
                        // Debug.Log("Breaking Out");
                        break;
                    }
                    //Debug.Log("In for Loop still");
                }
                //Debug.Log("Broke out");
            }

            return TM_Set;

        }
    }
}