using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Pokemon
{


    public class Route
    {

        public string route_name;
        public int dexnum;
        public string pokemon_name;
        public double spawn_chance;
        public int level_min;
        public int level_max;
        public int cap;
        public string gym_available;
        public int required_badges;
        public string terrain;
        public string route_background;

        public static List<Dictionary<string, object>> all_routes = new List<Dictionary<string, object>>();

        //public Dictionary<string, Route> spawns = new Dictionary<string, Route>();


        public Route(string name, int dexnum, string pokemon_name, double spawn_chance,
            int level_min, int level_max, int cap, string gym_available, int badges, string terrain)
        {
            this.route_name = name;
            this.dexnum = dexnum;
            this.pokemon_name = pokemon_name;
            this.spawn_chance = spawn_chance;
            this.level_min = level_min;
            this.level_max = level_max;
            this.cap = cap;
            this.gym_available = gym_available;
            this.required_badges = badges;
            this.terrain = terrain;
            this.route_background = name + ".png";

        }



        public static Dictionary<string, Route> get_route(string route)
        {
            Dictionary<string, Route> spawns = new Dictionary<string, Route>();
            for (var i = 0; i < Route.all_routes.Count; i++)
            {
                if (Route.all_routes[i]["Area"].ToString() == route)
                {
                    Route temp = new Route(
                    all_routes[i]["Area"].ToString(),
                    int.Parse(all_routes[i]["Dexnum"].ToString()),
                    all_routes[i]["Name"].ToString(),
                    double.Parse(all_routes[i]["Chance"].ToString()),
                    int.Parse(all_routes[i]["Level Min"].ToString()),
                    int.Parse(all_routes[i]["Level Max"].ToString()),
                    int.Parse(all_routes[i]["Cap"].ToString()),
                    all_routes[i]["Gym Available"].ToString(),
                    int.Parse(all_routes[i]["Badges"].ToString()),
                    all_routes[i]["Terrain"].ToString()
                    );
                    spawns.Add(temp.dexnum.ToString(), temp);
                }
            }

            return spawns;
        }

        public void get_background_path()
        {
            var path = Directory.GetCurrentDirectory();
           /* if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
            {*/
                //Debug.Log("Does something happen here?");
                this.route_background = path + "/Images/Backgrounds/" + (this.route_background).ToString();
            /*}
            else
            {
                this.route_background = path + "\\Images\\Backgrounds\\" + (this.route_background).ToString();

            }*/
            Debug.Log(this.route_background);

        }
    }
}