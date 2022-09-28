using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trippie.Models.Dtos
{
    public class PlaceDtoForCreate
    {
        public string Xid { get; set; }
        public string Name { get; set; }
        public string Rate { get; set; }
        public string ImageUrl { get; set; }

        public string Info { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

    }
}