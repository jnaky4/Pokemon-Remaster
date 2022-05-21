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

        public static Dictionary<string, List<Route>> route_dictionary = new Dictionary<string, List<Route>>()
        {
            {
                "Pallet Town", new List<Route>(new Route[]{
                    new Route("Pallet Town",        60, "Poliwag",      0.1, -5, -2, 24, "any",     10, "Old Rod"),
                    new Route("Pallet Town",        61, "Poliwhirl",    0.1, -5, -2, 24, "any",     10, "Good Rod"),
                })
            },
            {
                "Route 1", new List<Route>(new Route[]{
                    new Route("Route 1",            16, "Pidgey",       0.4, -8, -5, 17, "any",     10, "Grass"),
                    new Route("Route 1",            17, "Pidgeotto",    0.1, -5, -2, 35, "Ground",  2,  "Grass"),
                    new Route("Route 1",            18, "Pidgeot",      0.1, -2, 0,  50, "Ground",  3,  "Grass"),
                    new Route("Route 1",            19, "Rattata",      0.3, -8, -5, 19, "any",     10, "Grass"),
                    new Route("Route 1",            20, "Raticate",     0.1, -5, -1, 50, "none",    2,  "Grass"),
                    new Route("Route 1",            23, "Ekans",        0.2, -8, -5, 21, "any",     10, "Grass"),
                    new Route("Route 1",            24, "Arbok",        0.1, -5, -2, 50, "none",    2, "Grass"),
                })
            },
            {
                "Route 2", new List<Route>(new Route[]{
                    new Route("Route 2",            16, "Pidgey",       0.4, -8, -5, 17, "any",     10, "Grass"),
                    new Route("Route 2",            17, "Pidgeotto",    0.1, -5, -2, 35, "Ground",  2,  "Grass"),
                    new Route("Route 2",            18, "Pidgeot",      0.1, -2, 0,  50, "Ground",  3,  "Grass"),
                    new Route("Route 2",            19, "Rattata",      0.3, -8, -5, 19, "any",     10, "Grass"),
                    new Route("Route 2",            20, "Raticate",     0.1, -5, -1, 50, "none",    2,  "Grass"),
                    new Route("Route 2",            32, "Nidoran",      0.3, -8, -5, 15, "any",     10, "Grass"),
                    new Route("Route 2",            33, "Nidorino",     0.2, -8, -5, 35, "none",    2,  "Grass"),
                    new Route("Route 2",            34, "Nidoking",     0.1, -2, 0,  50, "none",    3,  "Grass"),
                    new Route("Route 2",            21, "Spearow",      0.3, -8, -5, 19, "any",     10, "Grass"),
                    new Route("Route 2",            22, "Fearow",       0.1, -5, -2, 50, "Ground",  10, "Grass"),
                })
            },
            {
                "Route 22", new List<Route>(new Route[]{
                    new Route("Route 22",           19, "Rattata",      0.4, -7, 0,  19, "any",     10, "Grass"),
                    new Route("Route 22",           20, "Raticate",     0.1, -5, -1, 50, "none",    2,  "Grass"),
                    new Route("Route 22",           21, "Spearow",      0.3, -8, -5, 19, "any",     10, "Grass"),
                    new Route("Route 22",           22, "Fearow",       0.1, -5, -2, 50, "Ground",  10, "Grass"),
                    new Route("Route 22",           29, "Nidoran",      0.3, -8, -5, 15, "any",     10, "Grass"),
                    new Route("Route 22",           30, "Nidorina",     0.2, -8, -5, 35, "none",    2,  "Grass"),
                    new Route("Route 22",           31, "Nidoqueen",    0.1, -2, 0,  50, "none",    3,  "Grass"),
                    new Route("Route 22",           56, "Mankey",       0.3, -7, -5, 27, "Rock",    10,  "Grass"),
                    new Route("Route 22",           57, "Primeape",     0.1, -5, 0,  50, "Rock",    10,  "Grass"),
                })
            },
            {
                "Viridian Forest", new List<Route>(new Route[]{
                    new Route("Viridian Forest",    10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                    new Route("Viridian Forest",    11, "Metapod",      0.4, -3, -1, 9,  "any",     10, "Grass"),
                    new Route("Viridian Forest",    12, "Butterfree",   0.1, 0,  0,  50, "none",    2, "Grass"),
                    new Route("Viridian Forest",    13, "Weedle",       0.4, -8, -0, 6,  "any",     10, "Grass"),
                    new Route("Viridian Forest",    14, "Kakuna",       0.4, -3, -1, 9,  "any",     10, "Grass"),
                    new Route("Viridian Forest",    15, "Beedrill",     0.1, 0,  0,  50, "none",    2,  "Grass"),
                    new Route("Viridian Forest",    25, "Pikachu",      0.1, -7, -5, 50, "any",     10,  "Grass"),
                    new Route("Viridian Forest",    26, "Raichu",       0.1, 0,  0,  50, "Water",   10,  "Grass"),
                    new Route("Viridian Forest",    123,"Scyther",      0.1, -5, 0,  50, "Rock",    10,  "Grass"),
                    new Route("Viridian Forest",    127,"Pinsir",       0.1, -5, 0,  50, "Rock",    10,  "Grass"),
                    new Route("Viridian Forest",    46, "Paras",        0.3, -7, -5, 23, "any",     10,  "Grass"),
                    new Route("Viridian Forest",    47, "Parasect",     0.1, -5, 0,  50, "Rock",    10,  "Grass"),
                    new Route("Viridian Forest",    48, "Venonat",      0.3, -7, -5, 30, "any",     10,  "Grass"),
                    new Route("Viridian Forest",    49, "Venomoth",     0.1, -5, 0,  50, "Rock",    10,  "Grass"),
                })
            },
        };

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

        public static List<Route> get_route(string route)
        {
            return route_dictionary[route];
        }
        public void get_background_path()
        {
            var path = Directory.GetCurrentDirectory();
            /* if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
             {*/
            //Debug.Log("Does something happen here?");
            string[] paths = { path, "Images", "Backgrounds", (this.route_background).ToString() };
            this.route_background = Path.Combine(paths);
                /*this.route_background = path + "/Images/Backgrounds/" + (this.route_background).ToString();*/
            /*}
            else
            {
                this.route_background = path + "\\Images\\Backgrounds\\" + (this.route_background).ToString();

            }*/
            Debug.Log(this.route_background);

        }
    }
}