using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace Pokemon { 
    
    public class Main : MonoBehaviour
    {

        void Start()
        {
            Pokemon.all_base_stats = load_CSV("BASE_STATS");
            Moves.all_moves = load_CSV("MOVES");
            Type.type_attack = load_CSV("TYPE_ATTACK");
            Type.type_defend = load_CSV("TYPE_DEFEND");
            Learnset.all_learnset = load_CSV("LEARNSET");
            Pokedex.all_pokedex = load_CSV("POKEMON");
            Type.load_type();
            Moves.load_moves();


            //Debug.Log("all_learnset Count " + Learnset.all_learnset.Count);
            //Debug.Log("learnset Dict Count " + Learnset.get_learnset.Count);


            print_pokemon();
            //print_moves();

        }

        // Update is called once per frame
        void Update()
        {
            
        }

        List<Dictionary<string, object>> load_CSV(string name)
        {
            List<Dictionary<string, object>> data = CSVReader.Read(name);
            
/*            for (var i = 0; i < data.Count; i++)
            {
                Debug.Log("Name " + data[i]["Name"] + " " +
                       "Type " + data[i]["Type"] + " " +
                       "PP " + data[i]["PP"] + " " +
                       "Base Power " + data[i]["Att."] + " " +
                       "Accuracy " + data[i]["Acc."] + " " +
                       "Effect " + data[i]["Effect"]
                       );
            }*/
            return data;
        }

        public void print_pokemon()
        {

            for (int i = 1; i < 152; i++)
            {
                Pokemon TestPokemon = new Pokemon(i, 50, "Flamethrower", "Earthquake", "Wing Attack", "Slash");
/*                Debug.Log("Name " + TestPokemon.name);
                Debug.Log("Base Attack " + TestPokemon.base_attack + " Current Attack " + TestPokemon.current_attack);
                Debug.Log("Type1: " + TestPokemon.type1.type);
                if (TestPokemon.type2 != null)
                {
                    Debug.Log("Type2: " + TestPokemon.type2.type);
                }*/
                
                foreach(Learnset learned in TestPokemon.learnset)
                {
                    Debug.Log(learned.ToString());
/*                    Debug.Log("PP " + learned.Get_move().pp);
                    Debug.Log("TYPE " + learned.Get_move().move_type.type);*/

                }
                    
            }
        }
        public void print_moves()
        {
            foreach(KeyValuePair<string, Moves> move in Moves.move_dictionary)
            {
                Debug.Log(move.Key);
            }
        }

    }
}
