using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trippie.Models.Dtos
{
    public class TripDto
    {
        public CityDto Start { get; set; }
        public CityDto End { get; set; }
        public DateTime CreationDate { get; set; }
        public List<int> ChosenPlaceTypes { get; set; }
        public List<PlaceDtoForCreate> Places { get; set; }
    }
}