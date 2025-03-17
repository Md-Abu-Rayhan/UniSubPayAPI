using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;

namespace API.Services
{
    public class UserService : IUserService
    {
        // In-memory user data for demo 
        private readonly List<User> _users = new List<User>
        {
            new User
            {
                Id = 1,
                Username = "john_doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                RegistrationDate = DateTime.UtcNow.AddDays(-30)
            },
            new User
            {
                Id = 2,
                Username = "jane_smith",
                Email = "jane@example.com",
                PhoneNumber = "0987654321",
                RegistrationDate = DateTime.UtcNow.AddDays(-15)
            }
        };

        public Task<User> GetUserByIdAsync(int userId)
        {
            var user = _users.FirstOrDefault(u => u.Id == userId);
            
            if (user == null)
            {
                user = new User
                {
                    Id = 0,
                    Username = string.Empty,
                    Email = string.Empty,
                    PhoneNumber = string.Empty,
                    RegistrationDate = DateTime.MinValue
                };
            }
            
            return Task.FromResult(user);
        }

        public Task<List<User>> GetAllUsersAsync()
        {
            return Task.FromResult(_users);
        }
    }
}