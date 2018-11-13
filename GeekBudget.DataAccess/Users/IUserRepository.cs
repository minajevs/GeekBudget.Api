using System.Collections.Generic;
using GeekBudget.Domain.Users;

namespace GeekBudget.DataAccess.Users
{
    public interface IUserRepository
    {
        string Add(string username);
        IEnumerable<User> GetAll();
        User Find(string key);
        void Remove(int id);
        void Update(User item);

        bool CheckValidUserKey(string reqkey);
        bool AreContactsEmpty();
    }
}
