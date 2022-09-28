using Trippie.Database;
using Trippie.Models.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Trippie.Repository
{
    public class UserRepository
    {
        ApplicationDbContext db;
        UserManager<ApplicationUser> userManager;

        public UserRepository(ApplicationDbContext context)
        {
            db = context;
            var userStore = new UserStore<ApplicationUser>(db);
            userManager = new UserManager<ApplicationUser>(userStore);
        }

        public ApplicationUser GetCurrentUser(IPrincipal user)
        {
            var userId = user.Identity.GetUserId();

            return userManager.FindById(userId);
        }

        public ApplicationUser GetCurrentUserById(string userId) => userManager.FindById(userId);

        public void MakeUserPremium(ApplicationUser user)
        {
            user.IsPremiumUser = true;

            db.SaveChanges();
        }
    }
}