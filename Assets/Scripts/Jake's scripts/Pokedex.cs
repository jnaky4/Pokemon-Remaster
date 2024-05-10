using System.Collections.Generic;
//using Microsoft.VisualBasic.FileIO;

namespace Pokemon
{
    public class Pokedex
    {
        /*public static string[][] pokedex = new string[152][];*/
        /*public static Dictionary<int, Pokedex> pokedex_dictionary = new Dictionary<int, Pokedex>();*/
        public static List<Dictionary<string, object>> all_pokedex = new();

        public int dexnum;
        public string name;
        public string type1;
        public string type2;
        public string stage;
        public int evolve_level;
        public string evolve_item;
        public string gender_ratio;
        public double height;
        public double weight;
        public string description;
        public string category;
        public string image1;
        public string image2;
        public double leveling_speed;
        public int base_exp;
        public int catch_rate;

        public Pokedex(int dexnum)
        {
            this.dexnum = dexnum;

            //load_pokemon_into_pokedex();
            get_pokemon_info();
        }



        public void get_pokemon_info()
        {

            //int.Parse(Pokedex.all_pokedex[this.dexnum - 1]["HP"].ToString());

            name = Pokedex.all_pokedex[dexnum - 1]["Pokemon_Name"].ToString();
            type1 = Pokedex.all_pokedex[dexnum - 1]["Type1"].ToString();
            type2 = Pokedex.all_pokedex[dexnum - 1]["Type2"].ToString();
            stage = Pokedex.all_pokedex[dexnum - 1]["Stage"].ToString();
            evolve_level = int.Parse(Pokedex.all_pokedex[dexnum - 1]["Evolve_Level"].ToString());
            evolve_item = Pokedex.all_pokedex[dexnum - 1]["Evolve_Item"].ToString();
            gender_ratio = Pokedex.all_pokedex[dexnum - 1]["Gender_Ratio"].ToString();
            height = double.Parse(Pokedex.all_pokedex[dexnum - 1]["Height"].ToString());
            weight = double.Parse(Pokedex.all_pokedex[dexnum - 1]["Weight"].ToString());
            description = Pokedex.all_pokedex[dexnum - 1]["Description"].ToString();
            category = Pokedex.all_pokedex[dexnum - 1]["Category"].ToString();
            leveling_speed = double.Parse(Pokedex.all_pokedex[dexnum - 1]["Leveling_Speed"].ToString());
            base_exp = int.Parse(Pokedex.all_pokedex[dexnum - 1]["Base_Exp"].ToString());
            catch_rate = int.Parse(Pokedex.all_pokedex[dexnum - 1]["Catch_Rate"].ToString());

           

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
            return type1;
        }
        public string get_type2()
        {
            //Console.WriteLine("TYPE2: " + this.type2);
            return type2;




        }
    }
}
