using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Trippie.Models;
using Trippie.Models.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Trippie.Database
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("Sindesmos", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<Trip> Trips { get; set;} 
        public virtual DbSet<Place> Places { get; set;} 
        public virtual DbSet<City> Cities { get; set;} 
        public virtual DbSet<PlaceType> PlaceTypes { get; set;} 

        
    }
}
