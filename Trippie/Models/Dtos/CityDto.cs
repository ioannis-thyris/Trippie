using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trippie.Models.Dtos
{
    public class CityDto
    {
        public int GeonameID { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}