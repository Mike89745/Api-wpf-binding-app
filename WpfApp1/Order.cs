using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Order
    {
        public string ID { get; set; }
        public string userID { get; set; }
        public List<Item> items { get; set; }
    }
}
