using GeekBudget.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBudget.Middleware
{
    public interface IUserRepository
    {
        string Add(string username);
        IEnumerable<User> GetAll();
        User Find(string key);
        void Remove(int id);
        void Update(User item);

        bool CheckValidUserKey(string reqkey);
    }
}
