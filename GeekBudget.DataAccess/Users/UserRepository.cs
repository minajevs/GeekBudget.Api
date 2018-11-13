using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using GeekBudget.Domain.Users;

namespace GeekBudget.DataAccess.Users
{
    public class UserRepository : IUserRepository
    {
        private GeekBudgetContext _context;

        public UserRepository(GeekBudgetContext context)
        {
            this._context = context;
        }

        public string Add(string username)
        {
            if(this._context.Users.Any(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("This username already exists!");

            var key = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
                generator.GetBytes(key);
            var apiKey = Convert.ToBase64String(key);

            var newUser = new User()
            {
                Key = apiKey,
                Username = username
            };

            this._context.Users.Add(newUser);
            this._context.SaveChanges();
            return apiKey;
        }

        public bool AreContactsEmpty()
        {
            return !this._context.Users.Any();
        }

        public bool CheckValidUserKey(string key)
        {
            return this.Find(key) != null;
        }

        public User Find(string key)
        {
            return this._context.Users.SingleOrDefault(x => x.Key == key);
        }

        public IEnumerable<User> GetAll()
        {
            return this._context.Users.ToList();
        }

        public void Remove(int id)
        {
            var itemToRemove = this._context.Users.SingleOrDefault(x => x.Id == id);
            this._context.Users.Remove(itemToRemove);
            this._context.SaveChanges();
        }

        public void Update(User item)
        {
            throw new NotImplementedException();
        }
    }
}
