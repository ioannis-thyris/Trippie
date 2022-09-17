﻿namespace GroupProject.Migrations
{
    using GroupProject.Models;
    using GroupProject.Models.Enums;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GroupProject.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(GroupProject.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole() { Name = "Admin" };
                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "admin@admin.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);

                var PasswordHash = new PasswordHasher();
                var admin = new ApplicationUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    PasswordHash = PasswordHash.HashPassword("Admin!123")
                };

                manager.Create(admin);
                manager.AddToRole(admin.Id, "Admin");

            }


            var placeTypes = GetPlaceTypes();

            context.PlaceTypes.AddRange(placeTypes);



        }

        private List<PlaceType> GetPlaceTypes()
        {
            PlaceType natural = new PlaceType(PlaceTypeEnum.Natural, false);
            PlaceType religion = new PlaceType(PlaceTypeEnum.Religion, false);
            PlaceType sport = new PlaceType(PlaceTypeEnum.Sport, false);
            PlaceType amusements = new PlaceType(PlaceTypeEnum.Amusements, false);
            PlaceType historic = new PlaceType(PlaceTypeEnum.Historic, false);
            PlaceType cultural = new PlaceType(PlaceTypeEnum.Cultural, false);

            return new List<PlaceType>
            {
                natural, religion, sport, amusements, historic, cultural
            };
        }

    }
}
