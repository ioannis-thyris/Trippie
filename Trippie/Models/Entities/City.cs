using Trippie.Models.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading;
using System.Web;

namespace Trippie.Models.Entities
{
    public class City
    {
        public int Id { get; set; }
        public int GeonameID { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public City()
        {
        }

        public City(CityDto dto)
        {
            GeonameID = dto.GeonameID;
            Name = dto.Name;
            Country = dto.Country;
            Latitude = dto.Latitude;
            Longitude = dto.Longitude;
        }
    }


}
