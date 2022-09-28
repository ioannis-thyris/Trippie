using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trippie.Models.Dtos
{
    public class TripDtoForRedirect
    {
        public double StartLat { get; set; }
        public double StartLon { get; set; }
        public double EndLat { get; set; }
        public double EndLon { get; set; }
        public List<CoordinatesDto> Waypoints { get; set; }
    }

    public class CoordinatesDto
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}