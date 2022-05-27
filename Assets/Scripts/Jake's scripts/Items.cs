using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon

{
    public class Items
    {
        public static List<Dictionary<string, object>> all_Items = new List<Dictionary<string, object>>();
        public static Dictionary<string, Items> items_dict = new Dictionary<string, Items>();
        public string name;
        public Status status_heal;
        public int restore_health;
        public int price;
        public string description;
        public int count;

        Items(string name, Status status_heal, int restore_health, int price, string description, int amount = 1)
        {
            this.name = name;
            this.status_heal = status_heal;
            this.restore_health = restore_health;
            this.price = price;
            this.description = description;
            this.count = amount;
            //items_dict.Add(name, this);
        }

        static public void load_items_to_dict()
        {
            items_dict.Clear();
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
        public static Items getItem(string name)
        {
            return items_dict[name];
        }
        public void addAmount(int amount)
        {
            this.count += amount;
        }
        public void subtractAmount(int amount)
        {
            this.count -= amount;
            if(count < 0) this.count = 0;
        }
        public void updateAmount(int amount)
        {
            this.count = amount;
        }
    }
}
