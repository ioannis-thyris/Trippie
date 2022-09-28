using Trippie.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Trippie.Models.Entities
{
    public class PlaceType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsPremium { get; set; }
        public virtual ICollection<Trip> Trips { get; set; }

        public PlaceType(PlaceTypeEnum placeTypeEnum, bool isPremium)
        {
            Id = (int)placeTypeEnum;
            Name = Enum.GetName(typeof(PlaceTypeEnum), placeTypeEnum);
            IsPremium = isPremium;

            this.Trips = new HashSet<Trip>();
        }

        public PlaceType()
        {
        }
    }
}