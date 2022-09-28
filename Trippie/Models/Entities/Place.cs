using Trippie.Models.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;

namespace Trippie.Models.Entities
{
    public class Place
    {
        public int Id { get; set; }
        public string Xid { get; set; }
        public string Name { get; set; }
        public string Rate { get; set; }
        public string ImageUrl { get; set; }

        public string Info { get; set; }
        
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public virtual ICollection<Trip> Trip { get; set; }

        public Place()
        {
            this.Trip = new HashSet<Trip>();
        }

        public Place(PlaceDtoForCreate dto)
        {
            Xid = dto.Xid;
            Name = dto.Name;
            Rate = dto.Rate;
            ImageUrl = dto.ImageUrl;
            Info = dto.Info;
            Latitude = dto.Latitude;
            Longitude = dto.Longitude;
        }
    }
}
