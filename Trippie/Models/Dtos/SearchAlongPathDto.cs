using Trippie.Models.Enums;
using Trippie.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trippie.Models.Dtos
{
    public class SearchAlongPathDto
    {
        public List<Coordinates> PathOverview { get; set; }
        public int? Radius { get; set; }
        public int? PointsAlongPath { get; set; }
        public PlaceTypeEnum[] PlaceTypes { get; set; }
    }
}