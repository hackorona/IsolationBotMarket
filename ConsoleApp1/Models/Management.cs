using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    public class Management
    {
        private Dictionary<long, User> _users;

        public Management()
        {
            _users= new Dictionary<long, User>();
        }

        public bool IsUserExist(long id)
        {
            return _users.ContainsKey(id);
        }

        public void AddUser(long userId)
        {
            if (!_users.ContainsKey(userId))
            {
                _users.Add(userId, new User(userId));
            }
        }

        public User GetUserById(long id)
        {
            return _users[id];
        }
    }
}
