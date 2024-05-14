using System.Collections.Generic;
using UnityEngine;


namespace Pokemon
{

    public class Main : MonoBehaviour
    {

        public void Start()
        {
            Pokemon.all_base_stats = LoadCSV("BASE_STATS");
            Moves.all_moves = LoadCSV("MOVES");
            Type.type_attack = LoadCSV("TYPE_ATTACK");
            //Type.type_defend = load_CSV("TYPE_DEFEND");
            Learnset.all_learnset = LoadCSV("LEARNSET");
            Pokedex.all_pokedex = LoadCSV("POKEMON");
            Type.load_type();
            Moves.load_moves();

            // JsonManager.SaveDataToJSON(Pokedex.all_pokedex, "Assets/Scripts/Resources/pokedex_data.json");
        }

        // Update is called once per frame
        void Update()
        {

        }

        List<Dictionary<string, object>> LoadCSV(string name)
        {
            List<Dictionary<string, object>> data = CSVReader.Read(name);
            return data;
        }

    }
}
