using System.Collections;
using System.Collections.Generic;
using System.Drawing;

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


/*public class Pokemon
{
    //private pokedex name { get; set; }
    //Image pokemonImage { get; set; }
    private int dexnum { get; set; }
    public int level { get; set; }
    //private type type1 { get; set; }
    //private type type2 { get; set; }
    //Dictionary<move, int> learnset { get; set; }
    ArrayList learnableTms { get; set; }
    ArrayList learnableHMs { get; set; }
    ArrayList currentMoves { get; set; }
    private int EXP100 { get; set; }
    private int currentEXP { get; set; }
    public Pokemon(string name, int level)
    {
        this.name = name;
        this.level = level;
        //pokemonImage = Image.fromFile(Imagename);
        this.currentMoves = new ArrayList();
        this.learnableHMs = new ArrayList();
        this.learnableTms = new ArrayList();
        this.learnset = new Dictionary<move, int>();
    }
}

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
