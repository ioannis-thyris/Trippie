using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trippie.Models.Helper
{
    public class CoordinateForReduction
    {
        public int Id { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double DistanceFromPrevious { get; set; }
        public double DistanceFromStart { get; set; }
    }
}