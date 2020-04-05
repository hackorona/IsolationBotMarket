using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    public enum UserState
    {
        RequireFirstName,
        RequireLastName,
        RequireCity,
        RequireAddress,
        Complete
    }
    public class User
    {
        private long _id;
        private string _firstName;
        private string _lastName;
        private string _city;
        private string _address;
        private int _points;
        public UserState state;
        public User(long id)
        {
            _id = id;
            state = UserState.RequireFirstName;
            _points = 3;
        }

        public void UpdateFirstName(string name)
        {
            _firstName = name;
            state = UserState.RequireLastName;
        }
        public void UpdateLastName(string name)
        {
            _lastName = name;
            state = UserState.RequireCity;

        }
        public void UpdateCity(string name)
        {
            _city = name;
            state = UserState.RequireAddress;

        }
        public void UpdateAddress(string name)
        {
            _address = name;
            state = UserState.Complete;
        }
        public string UserData()
        {
            return $"{_firstName} {_id}";
        }

        public int GetPoints()
        {
            return _points;
        }
        public void AddPoint()
        {
            _points++;
        }
        public void SubstractPoint()
        {
            _points--;
        }
    }
}
