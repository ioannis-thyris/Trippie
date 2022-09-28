using Trippie.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trippie.Models.ViewModels
{
    public class CreateTripViewModel
    {
        public bool IsRegistered { get; set; }
        public bool IsPremiumUser { get; set; }
        public IEnumerable<PlaceType> PlaceTypes { get; set; }
    }
}