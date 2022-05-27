using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Pokemon
{
    public class RouteSpawn
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

        public static Dictionary<string, List<RouteSpawn>> route_dictionary = new Dictionary<string, List<RouteSpawn>>()
        {
            {
                "Pallet Town", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Pallet Town",        60, "Poliwag",      0.1, -5, -2, 24, "any",     10, "Old Rod"),
                    new RouteSpawn("Pallet Town",        61, "Poliwhirl",    0.1, -5, -2, 24, "any",     10, "Good Rod"),
                })
            },
            {
                "Route 1", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Route 1",            16, "Pidgey",       0.4, -8, -5, 17, "any",     10, "Grass"),
                    new RouteSpawn("Route 1",            17, "Pidgeotto",    0.1, -5, -2, 35, "Ground",  2,  "Grass"),
                    new RouteSpawn("Route 1",            18, "Pidgeot",      0.1, -2, 0,  50, "Ground",  3,  "Grass"),
                    new RouteSpawn("Route 1",            19, "Rattata",      0.3, -8, -5, 19, "any",     10, "Grass"),
                    new RouteSpawn("Route 1",            20, "Raticate",     0.1, -5, -1, 50, "none",    2,  "Grass"),
                    new RouteSpawn("Route 1",            23, "Ekans",        0.2, -8, -5, 21, "any",     10, "Grass"),
                    new RouteSpawn("Route 1",            24, "Arbok",        0.1, -5, -2, 50, "none",    2, "Grass"),
                })
            },
            {
                "Route 2", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Route 2",            16, "Pidgey",       0.4, -8, -5, 17, "any",     10, "Grass"),
                    new RouteSpawn("Route 2",            17, "Pidgeotto",    0.1, -5, -2, 35, "Ground",  2,  "Grass"),
                    new RouteSpawn("Route 2",            18, "Pidgeot",      0.1, -2, 0,  50, "Ground",  3,  "Grass"),
                    new RouteSpawn("Route 2",            19, "Rattata",      0.3, -8, -5, 19, "any",     10, "Grass"),
                    new RouteSpawn("Route 2",            20, "Raticate",     0.1, -5, -1, 50, "none",    2,  "Grass"),
                    new RouteSpawn("Route 2",            32, "Nidoran",      0.3, -8, -5, 15, "any",     10, "Grass"),
                    new RouteSpawn("Route 2",            33, "Nidorino",     0.2, -8, -5, 35, "none",    2,  "Grass"),
                    new RouteSpawn("Route 2",            34, "Nidoking",     0.1, -2, 0,  50, "none",    3,  "Grass"),
                    new RouteSpawn("Route 2",            21, "Spearow",      0.3, -8, -5, 19, "any",     10, "Grass"),
                    new RouteSpawn("Route 2",            22, "Fearow",       0.1, -5, -2, 50, "Ground",  10, "Grass"),
                })
            },
            {
                "Route 22", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Route 22",           19, "Rattata",      0.4, -7, 0,  19, "any",     10, "Grass"),
                    new RouteSpawn("Route 22",           20, "Raticate",     0.1, -5, -1, 50, "none",    2,  "Grass"),
                    new RouteSpawn("Route 22",           21, "Spearow",      0.3, -8, -5, 19, "any",     10, "Grass"),
                    new RouteSpawn("Route 22",           22, "Fearow",       0.1, -5, -2, 50, "Ground",  10, "Grass"),
                    new RouteSpawn("Route 22",           29, "Nidoran",      0.3, -8, -5, 15, "any",     10, "Grass"),
                    new RouteSpawn("Route 22",           30, "Nidorina",     0.2, -8, -5, 35, "none",    2,  "Grass"),
                    new RouteSpawn("Route 22",           31, "Nidoqueen",    0.1, -2, 0,  50, "none",    3,  "Grass"),
                    new RouteSpawn("Route 22",           56, "Mankey",       0.3, -7, -5, 27, "Rock",    10,  "Grass"),
                    new RouteSpawn("Route 22",           57, "Primeape",     0.1, -5, 0,  50, "Rock",    10,  "Grass"),
                })
            },
            {
                "Viridian Forest", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Viridian Forest",    10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                    new RouteSpawn("Viridian Forest",    11, "Metapod",      0.4, -3, -1, 9,  "any",     10, "Grass"),
                    new RouteSpawn("Viridian Forest",    12, "Butterfree",   0.1, 0,  0,  50, "none",    2, "Grass"),
                    new RouteSpawn("Viridian Forest",    13, "Weedle",       0.4, -8, -0, 6,  "any",     10, "Grass"),
                    new RouteSpawn("Viridian Forest",    14, "Kakuna",       0.4, -3, -1, 9,  "any",     10, "Grass"),
                    new RouteSpawn("Viridian Forest",    15, "Beedrill",     0.1, 0,  0,  50, "none",    2,  "Grass"),
                    new RouteSpawn("Viridian Forest",    25, "Pikachu",      0.1, -7, -5, 50, "any",     10,  "Grass"),
                    new RouteSpawn("Viridian Forest",    26, "Raichu",       0.1, 0,  0,  50, "Water",   10,  "Grass"),
                    new RouteSpawn("Viridian Forest",    46, "Paras",        0.3, -7, -5, 23, "any",     10,  "Grass"),
                    new RouteSpawn("Viridian Forest",    47, "Parasect",     0.1, -5, 0,  50, "Rock",    10,  "Grass"),
                    new RouteSpawn("Viridian Forest",    48, "Venonat",      0.3, -7, -5, 30, "any",     10,  "Grass"),
                    new RouteSpawn("Viridian Forest",    49, "Venomoth",     0.1, -5, 0,  50, "Rock",    10,  "Grass"),
                    new RouteSpawn("Viridian Forest",    123,"Scyther",      0.1, -5, 0,  50, "Rock",    10,  "Grass"),
                    new RouteSpawn("Viridian Forest",    127,"Pinsir",       0.1, -5, 0,  50, "Rock",    10,  "Grass"),

                })
            },
            {
                "Pewter City", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Pewter City",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Route 3", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Route 3",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Mt Moon 1", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Mt Moon 1",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Mt Moon 2", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Mt Moon 2",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Mt Moon 3", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Mt Moon 3",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Route 4", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Route 4",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Cerulean City", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Cerulean City",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Route 24", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Route 24",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Route 25", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Route 25",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Route 5", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Route 5",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Route 6", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Route 6",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Vermillion City", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Vermillion City",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Route 11", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Route 11",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Diglett Cave", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Diglett Cave",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Route 9", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Route 9",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Route 10", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Route 10",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Rock Tunnel 1", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Rock Tunnel 1",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Rock Tunnel 2", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Rock Tunnel 2",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Lavender Town", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Lavender Town",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Route 8", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Route 8",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Route 7", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Route 7",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Celadon City", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Celadon City",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Pokemon Tower", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Pokemon Tower",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Saffron City", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Saffron City",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Route 13", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Route 13",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Route 14", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Route 14",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Route 15", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Route 15",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Fuscia City", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Fuscia City",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Safari Zone Entrance", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Safari Zone Entrance",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Safari Zone 1", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Safari Zone 1",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Safari Zone 2", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Safari Zone 2",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Safari Zone 3", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Safari Zone 3",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Route 16", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Route 16",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Route 17", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Route 17",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Route 18", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Route 18",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Power Plant", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Power Plant",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Route 19", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Route 19",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Route 20", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Route 20",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Seafoam Island 1", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Seafoam Island 1",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Seafoam Island 2", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Seafoam Island 2",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Seafoam Island 3", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Seafoam Island 3",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Seafoam Island 4", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Seafoam Island 4",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Cinnabar Island", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Cinnabar Island",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Pokemon Mansion 1", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Pokemon Mansion 1",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Pokemon Mansion 2", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Pokemon Mansion 2",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Pokemon Mansion 3", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Pokemon Mansion 3",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Pokemon Mansion Basement", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Pokemon Mansion Basement",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Route 21", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Route 21",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Route 23", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Route 23",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Victory Road 1", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Victory Road 1",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Victory Road 2", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Victory Road 2",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Victory Road 3", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Victory Road 3",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Unknown Dungeon 1", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Unknown Dungeon 1",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Unknown Dungeon 2", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Unknown Dungeon 2",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
            {
                "Unknown Dungeon Basement", new List<RouteSpawn>(new RouteSpawn[]{
                    new RouteSpawn("Unknown Dungeon Basement",            10, "Caterpie",     0.4, -8, 0,  6,  "any",     10, "Grass"),
                })
            },
        };

        public RouteSpawn(string name, int dexnum, string pokemon_name, double spawn_chance,
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

        public static List<RouteSpawn> get_route(string route) { return route_dictionary[route]; }
      
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