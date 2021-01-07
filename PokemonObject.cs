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


public class Pokemon
{
    private pokedex name;
    Image pokemonImage;
    private int dexnum;
    private int level;
    private type type1;
    private type type2;
    Dictionary<move, int> learnset = new Dictionary<move, int>();
    ArrayList learnableTms = new ArrayList();
    ArrayList learnableHMs = new ArrayList();
    ArrayList currentMoves = new ArrayList();
    private int EXP100;
    private int currentEXP;
    public pokemon(pokedex name, int level, string Imagename)
    {
        name = name;
        level = level;
        pokemonImage = Image.fromFile(Imagename);


    }
}

public class type
{
    Dictionary<string, int> attacking = new Dictionary<string, int>();
    Dictionary<string, int> defending = new Dictionary<string, int>();
}
public class pokedex
{
    Dictionary<pokemon, int> dex = new Dictionary<pokemon, int>();
}

public class player
{
    string name;
    bool gender;
    int money;
    pokedex playerDex;
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
}
