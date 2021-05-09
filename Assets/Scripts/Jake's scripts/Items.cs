using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon

{
    class Items
    {
        public static List<Dictionary<string, object>> all_Items = new List<Dictionary<string, object>>();
        public static Dictionary<string, Items> items_dict = new Dictionary<string, Items>();
        string name;
        Status status_heal;
        int restore_health;
        int price;
        string description;

        Items(string name, Status status_heal, int restore_health, int price, string description)
        {
            this.name = name;
            this.status_heal = status_heal;
            this.restore_health = restore_health;
            this.price = price;
            this.description = description;
            //items_dict.Add(name, this);
        }

        static public void load_items_to_dict()
        {

            for (var i = 0; i < Items.all_Items.Count; i++)
            {
                string name = Items.all_Items[i]["Name"].ToString();
                Status status_heal = Status.get_status(Items.all_Items[i]["Status"].ToString()); 
                int restore_health = int.Parse(Items.all_Items[i]["Health"].ToString());
                int price = int.Parse(Items.all_Items[i]["Price"].ToString());
                string description = Items.all_Items[i]["Description"].ToString();
                Items temp_item = new Items(name, status_heal, restore_health, price, description);
                items_dict.Add(name, temp_item);
            }






     
        }
    }
}
