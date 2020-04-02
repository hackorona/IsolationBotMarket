using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    class ProductsManagement
    {
        private Dictionary<string, List<long>> _productsInStore;
        private Dictionary<string,Product> _productsList;

        public ProductsManagement()
        {
            _productsInStore = new Dictionary<string, List<long>>();
            _productsList = new Dictionary<string, Product>();
        }

        public bool IsProductExist(string product)
        {
            return _productsList.ContainsKey(product);
        }

        public void AddProduct(string nameOfTheProduct, long user)
        {
            if (!IsProductExist(nameOfTheProduct))
            {
                _productsInStore.Add(nameOfTheProduct, new List<long> { user });
                _productsList.Add(nameOfTheProduct, new Product(nameOfTheProduct));
                _productsList[nameOfTheProduct].UpdateAvailable(true);
            }
            else
            {
                _productsInStore[nameOfTheProduct].Add(user);
                _productsList[nameOfTheProduct].UpdateAvailable(true);
            }
        }

        public void UpdateCategory(string product)
        {
            if (_productsList.ContainsKey(product))
            {
                _productsList[product].UpdateCategory(product);
            }
        }
            
        public void UpdateAvailable(string product ,bool status)
        {
            if(_productsList.ContainsKey(product))
            {
                _productsList[product].UpdateAvailable(status);
            }
        }

        public long GetProduct(string product)
        {
            if(_productsList.ContainsKey(product) && _productsList[product].isAvailable())
            {
                //Distance..?
                List <long> _usersWithThisProduct = _productsInStore[product];
                long _userWithProduct = _usersWithThisProduct[0]; // The first of the list?
                _productsInStore[product].RemoveAt(0);
                if(_productsInStore[product].Count() == 0)
                {
                    _productsList[product].UpdateAvailable(false);
                }
                return _userWithProduct;
            }
            return 0;
        }
    }

}
