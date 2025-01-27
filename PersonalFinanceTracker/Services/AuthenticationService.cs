using System;
using Microsoft.AspNetCore.Components;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Services
{
    public class AuthenticationService
    {
        private User currentUser;

        public User GetAuthenticatedUser()
        {
            return currentUser;
        }

        public void SetAuthenticatedUser(User user)
        {
            currentUser = user;
        }

        public void Logout()
        {
            currentUser = null;
        }
    }
}
