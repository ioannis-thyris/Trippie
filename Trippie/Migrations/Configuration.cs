namespace Trippie.Migrations
{
    using Trippie.Database;
    using Trippie.Models;
    using Trippie.Models.Entities;
    using Trippie.Models.Enums;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            CreateAdminRole(context);

            CreateAdminUser(context);

            CreateClientRole(context);

            var placeTypes = GetPlaceTypes();

            context.PlaceTypes.AddRange(placeTypes);



        }

        private void CreateAdminRole(ApplicationDbContext context)
        {
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole() { Name = "Admin" };
                manager.Create(role);
            }
        }

        private void CreateClientRole(ApplicationDbContext context)
        {
            if (!context.Roles.Any(r => r.Name == "Client"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole() { Name = "Client" };
                manager.Create(role);
            }
        }

        private void CreateAdminUser(ApplicationDbContext context)
        {
            if (!context.Users.Any(u => u.UserName == "admin@admin.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);

                var PasswordHash = new PasswordHasher();
                var admin = new ApplicationUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    PasswordHash = PasswordHash.HashPassword("Admin!123"),
                    IsPremiumUser = true
                };

                manager.Create(admin);
                manager.AddToRole(admin.Id, "Admin");
            }
        }

        private List<PlaceType> GetPlaceTypes()
        {
            PlaceType natural = new PlaceType(PlaceTypeEnum.Natural, false);
            PlaceType religion = new PlaceType(PlaceTypeEnum.Religion, false);
            PlaceType sport = new PlaceType(PlaceTypeEnum.Sport, false);
            PlaceType amusements = new PlaceType(PlaceTypeEnum.Amusements, false);
            PlaceType historic = new PlaceType(PlaceTypeEnum.Historic, false);
            PlaceType cultural = new PlaceType(PlaceTypeEnum.Cultural, false);

            PlaceType accomodations = new PlaceType(PlaceTypeEnum.Accomodations, true);
            PlaceType adult = new PlaceType(PlaceTypeEnum.Adult, true);
            PlaceType foods = new PlaceType(PlaceTypeEnum.Foods, true);

            return new List<PlaceType>
            {
                natural, 
                religion, 
                sport, 
                amusements, 
                historic, 
                cultural, 
                accomodations, 
                adult,
                foods
            };
        }

    }
}
