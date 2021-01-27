using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System;
using System.Collections;
using System.Collections.Generic;
/*using UnityEngine;*/
using System.Linq;

/*
Pokemon Object
    A Pokemon will be derived from a pokemon object 
        Each pokemon generated in the game will have the following Example Attributes:
        name: Charizard
        pokemonImage: PokemonImages/Charizard.jpg
        dexnum: 5
        level: 55
        type1: Fire
        type2: Flying
        Learnset{<Slash,36>,<Flamethrower,46>,<Firespin,55>}
        LearnableTms{Mega Punch, Sword Dance, Mega Kick, Toxic, 
            Body Slam, Take Down, Double-Edge, Hyper Beam, Submission,
            Counter, Seismic Toss, Rage, Dragon Rage, Earthquake, 
            Fissure, Dig, Mimic, Double Team, Reflect, Bide, Fire Blast, 
            Swift, Skull Bash, Rest, Substitute}
        LearnableHMs: {Cut, Strength, Fly}
        currentMoves: {Fire Blast, Earthquake, Sword Dance, Hyper Beam}
        EXP100: 1600000  -> this is the amount of EXP to get to lvl 100
        currentEXP: 650245
        

*/


public class Pokemon /*: MonoBehaviour*/
{
    public static Dictionary<int, string> all_pokemon = new Dictionary<int, string> {
            {1, "Bulbasaur" },{2 ,"Ivysaur"},{3 ,"Venusaur"},
            {4 ,"Charmander"},{5 ,"Charmeleon"},{6 ,"Charizard"},
            {7 ,"Squirtle"},{8 ,"Wartortle"},{9 ,"Blastoise"},
            {10 ,"Caterpie"},{11 ,"Metapod"},{12 ,"Butterfree"},
            {13 ,"Weedle"},{14 ,"Kakuna"},{15 ,"Beedrill"},
            {16 ,"Pidgey"},{17 ,"Pidgeotto"},{18 ,"Pidgeot"},
            {19 ,"Rattata"},{20 ,"Raticate"},
            {21 ,"Spearow"},{22 ,"Fearow"},
            {23 ,"Ekans"},{24 ,"Arbok"},
            {25 ,"Pikachu"},{26 ,"Raichu"},
            {27 ,"Sandshrew"},{28 ,"Sandslash"},
            {29 ,"Nidoran"},{30 ,"Nidorina"},{31 ,"Nidoqueen"},
            {32 ,"Nidoran"},{33 ,"Nidorino"},{34 ,"Nidoking"},
            {35 ,"Clefairy"},{36 ,"Clefable"},
            {37 ,"Vulpix"},{38 ,"Ninetales"},
            {39 ,"Jigglypuff"},{40 ,"Wigglytuff"},
            {41 ,"Zubat"},{42 ,"Golbat"},
            {43 ,"Oddish"},{44 ,"Gloom"},{45 ,"Vileplume"},
            {46 ,"Paras"},{47 ,"Parasect"},
            {48 ,"Venonat"},{49 ,"Venomoth"},
            {50 ,"Diglett"},{51 ,"Dugtrio"},
            {52 ,"Meowth"},{53 ,"Persian"},
            {54 ,"Psyduck"},{55 ,"Golduck"},
            {56 ,"Mankey"},{57 ,"Primeape"},
            {58 ,"Growlithe"},{59 ,"Arcanine"},
            {60 ,"Poliwag"},{61 ,"Poliwhirl"},{62 ,"Poliwrath"},
            {63 ,"Abra"},{64 ,"Kadabra"},{65 ,"Alakazam"},
            {66 ,"Machop"},{67 ,"Machoke"},{68 ,"Machamp"},
            {69 ,"Bellsprout"},{70 ,"Weepinbell"},{71 ,"Victreebel"},
            {72 ,"Tentacool"},{73 ,"Tentacruel"},
            {74 ,"Geodude"},{75 ,"Graveler"},{76 ,"Golem"},
            {77 ,"Ponyta"},{78 ,"Rapidash"},
            {79 ,"Slowpoke"},{80 ,"Slowbro"},
            {81 ,"Magnemite"},{82 ,"Magneton"},
            {83 ,"Farfetch'd"},
            {84 ,"Doduo"},{85 ,"Dodrio"},
            {86 ,"Seel"},{87 ,"Dewgong"},
            {88 ,"Grimer"},{89 ,"Muk"},
            {90 ,"Shellder"},{91 ,"Cloyster"},
            {92 ,"Gastly"},{93 ,"Haunter"},{94 ,"Gengar"},
            {95 ,"Onix"},
            {96 ,"Drowzee"},{97 ,"Hypno"},
            {98 ,"Krabby"},{99 ,"Kingler"},
            {100 ,"Voltorb"},{101 ,"Electrode"},
            {102 ,"Exeggcute"},{103 ,"Exeggutor"},
            {104 ,"Cubone"},{105 ,"Marowak"},
            {106 ,"Hitmonlee"},
            {107 ,"Hitmonchan"},
            {108 ,"Lickitung"},
            {109 ,"Koffing"},{110 ,"Weezing"},
            {111 ,"Rhyhorn"},{112 ,"Rhydon"},
            {113 ,"Chansey"},
            {114 ,"Tangela"},
            {115 ,"Kangaskhan"},
            {116 ,"Horsea"},{117 ,"Seadra"},
            {118 ,"Goldeen"},{119 ,"Seaking"},
            {120 ,"Staryu"},{121 ,"Starmie"},
            {122 ,"Mr.Mime"},
            {123 ,"Scyther"},
            {124 ,"Jynx"},
            {125 ,"Electabuzz"},
            {126 ,"Magmar"},
            {127 ,"Pinsir"},
            {128 ,"Tauros"},
            {129 ,"Magikarp"},{130 ,"Gyarados"},
            {131 ,"Lapras"},
            {132 ,"Ditto"},
            {133 ,"Eevee"},{134 ,"Vaporeon"},{135 ,"Jolteon"},{136 ,"Flareon"},
            {137 ,"Porygon"},
            {138 ,"Omanyte"},{139 ,"Omastar"},
            {140 ,"Kabuto"},{141 ,"Kabutops"},
            {142 ,"Aerodactyl"},
            {143 ,"Snorlax"},
            {144 ,"Articuno"},
            {145 ,"Zapdos"},
            {146 ,"Moltres"},
            {147 ,"Dratini"},{148 ,"Dragonair"},{149 ,"Dragonite"},
            {150 ,"Mewtwo"},
            {151 ,"Mew"}
    };
    private string name;

    private int dexnum;
    public int level;
    /*
        private int hp = 78;
        private int attack = 84;
        private int defense = 78;
        private int sp_attack = 109;
        private int sp_defense = 85;
        private int speed = 100;
        private static ArrayList currentMoves = new ArrayList { "Slash", "Flamethrower", "Wing Attack", "Earthquake" };
        Image pokemonImage { get; set; }
        private type type1 { get; set; }
        private type type2 { get; set; }
        Dictionary<string, int> learnset { get; set; }
        ArrayList learnableTms { get; set; }
        ArrayList learnableHMs { get; set; }
        private int EXP100 { get; set; }
             */


    public int hp;
    public int attack;
    public int defense;
    public int sp_attack;
    public int sp_defense;
    public int speed;
    public string type1;
    public string type2;
    public ArrayList currentMoves;
    public int currentEXP;


    public Pokemon(int level, int dexNum)
    {
        this.level = level;
        this.dexnum = dexNum;
        this.name = all_pokemon[dexNum];
        Console.WriteLine(this.name);
    }
    static void Main(string[] args)
    {
        Pokemon Charizard = new Pokemon(50, 6);
        Console.WriteLine(Charizard.name);
    }
}




/*
public class type
{
    Dictionary<string, int> attacking { get; set; }
    Dictionary<string, int> defending { get; set; }
    /*public type()
    {
        attacking = new Dictionary<string, int>();
        defending = new Dictionary<string, int>();
    }
}
public class pokedex
{
    Dictionary<pokemon, int> dex { get; set; }
    public pokedex()
    {
        dex = new Dictionary<pokemon, int>();
    }
}

public class player
{
    string name { get; set; }
    bool gender { get; set; }
    int money { get; set; }
    pokedex playerDex { get; set; }
    Dictionary<string, bool> gymsBeaten = new Dictionary<string, bool>();
    Dictionary<string, int> items = new Dictionary<string, int>();

}

public class move
{
    int name;
    int pp;
    int baseDamage;
    type type;
    bool priority;
    //how to decide between HM, TM, Learnable move
    //list of pokemon that can use the move
}*/
