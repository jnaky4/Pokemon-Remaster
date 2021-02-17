using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon
{
    public class GameController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Awake ()
        {
            DontDestroyOnLoad(transform.gameObject);

            Pokemon.all_base_stats = BattleSystem.load_CSV("BASE_STATS");
            Moves.all_moves = BattleSystem.load_CSV("MOVES");
            Type.type_attack = BattleSystem.load_CSV("TYPE_ATTACK");
            Type.type_defend = BattleSystem.load_CSV("TYPE_DEFEND");
            Learnset.all_learnset = BattleSystem.load_CSV("LEARNSET");
            Pokedex.all_pokedex = BattleSystem.load_CSV("POKEMON");
            Route.all_routes = BattleSystem.load_CSV("ROUTES");
            Type.load_type();
            Moves.load_moves();

            Debug.Log("CSV's have been loaded");
        }

    }
}
