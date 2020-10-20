public class pokemon
{
    private pokedex name;
    Image pokemonImage;
    private int dexnum;
    private int level;
    private type type1;
    private type type2;
    Dictionary<move, int> learnset = new Dictionary<move, int>();
    private int EXP;
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

}