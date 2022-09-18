﻿using GroupProject.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class SearchAlongPathDto
    {
        public List<Coordinates> PathOverview { get; set; }
        public int? Radius { get; set; }
        public int? PointsAlongPath { get; set; }
        public PlaceTypeEnum[] PlaceTypes { get; set; }
    }
}