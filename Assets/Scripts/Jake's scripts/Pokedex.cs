using System.Collections.Generic;
//using Microsoft.VisualBasic.FileIO;

namespace Pokemon
{
    public class Pokedex
    {
        public static string[][] pokedex = new string[152][];
        public static Dictionary<int, Pokedex> pokedex_dictionary = new Dictionary<int, Pokedex>();
        public static List<Dictionary<string, object>> all_pokedex = new List<Dictionary<string, object>>();

        public int dexnum;
        public string name;
        public string type1;
        public string type2;
        public string stage;
        public int evolve_level;
        public string gender_ratio;
        public double height;
        public double weight;
        public string description;
        public string category;
        public string image1;
        public string image2;
        public double leveling_speed;
        public int base_exp;

        public Pokedex(int dexnum)
        {
            this.dexnum = dexnum;

            //load_pokemon_into_pokedex();
            get_pokemon_info();
        }



        public void get_pokemon_info()
        {

            //int.Parse(Pokedex.all_pokedex[this.dexnum - 1]["HP"].ToString());

            this.name = Pokedex.all_pokedex[this.dexnum - 1]["Pokemon_Name"].ToString();
            this.type1 = Pokedex.all_pokedex[this.dexnum - 1]["Type1"].ToString();
            this.type2 = Pokedex.all_pokedex[this.dexnum - 1]["Type2"].ToString();
            this.stage = Pokedex.all_pokedex[this.dexnum - 1]["Stage"].ToString();
            this.evolve_level = int.Parse(Pokedex.all_pokedex[this.dexnum - 1]["Evolve_Level"].ToString());
            this.gender_ratio = Pokedex.all_pokedex[this.dexnum - 1]["Gender_Ratio"].ToString();
            this.height = double.Parse(Pokedex.all_pokedex[this.dexnum - 1]["Height"].ToString());
            this.weight = double.Parse(Pokedex.all_pokedex[this.dexnum - 1]["Weight"].ToString());
            this.description = Pokedex.all_pokedex[this.dexnum - 1]["Description"].ToString();
            this.category = Pokedex.all_pokedex[this.dexnum - 1]["Category"].ToString();
            this.leveling_speed = double.Parse(Pokedex.all_pokedex[this.dexnum - 1]["Leveling_Speed"].ToString());
            this.base_exp = int.Parse(Pokedex.all_pokedex[this.dexnum - 1]["Base_Exp"].ToString());


            /*            this.type1 = pokedex[dexnum][2];
                        this.type2 = pokedex[dexnum][3];
                        this.stage = pokedex[dexnum][4];
                        this.gender_ratio = pokedex[dexnum][5];
                        this.height = Convert.ToDouble(pokedex[dexnum][6]);
                        this.weight = Convert.ToDouble(pokedex[dexnum][7]);
                        this.description = pokedex[dexnum][8];
                        this.category = pokedex[dexnum][9];*/
            //this.image1 = pokedex[dexnum][10];
            //this.image2 = pokedex[dexnum][11];
        }
        public string get_type1()
        {
            //Console.WriteLine("TYPE1: " + this.type1);
            return this.type1;
        }
        public string get_type2()
        {
            //Console.WriteLine("TYPE2: " + this.type2);
            return this.type2;




        }
    }
}
