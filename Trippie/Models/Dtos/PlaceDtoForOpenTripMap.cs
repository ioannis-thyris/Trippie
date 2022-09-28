using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Web;

namespace Trippie.Models.Dtos
{
    public class PlaceDtoForOpenTripMap
    {
        public string name { get; set; }
        public string xid { get; set; }
        public Point point { get; set; }
    }

    public struct Point
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }
}