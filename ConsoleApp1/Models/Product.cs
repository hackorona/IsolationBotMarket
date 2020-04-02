using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
   public class Product
   {

        private string _name;
        private string _category;
        private bool _available;

        public Product(string name)
        {
            _name = name;
        }

        public void UpdateCategory(string category)
        {
            _category = category;
        }

        public void UpdateAvailable(bool available)
        {
            _available = available;
        }

        public bool isAvailable()
        {
            return _available;
        }
    }
}
