using Trippie.Models.Dtos;
using Trippie.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Razor.Text;

namespace Trippie.Models.Entities
{
    public class Trip
    {
        
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual City Start { get; set; }
        public virtual City End { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Place> Places { get; set; }
        public virtual ICollection<PlaceType> ChosenPlaceTypes { get; set; }

        public Trip()
        {
            this.Places = new HashSet<Place>();
            this.ChosenPlaceTypes = new HashSet<PlaceType>();
        }

        public Trip(TripDto dto)
        {
            this.Places = new HashSet<Place>();
            this.ChosenPlaceTypes = new HashSet<PlaceType>();

            City start = new City(dto.Start);
            Start = start;

            City end = new City(dto.End);
            End = end;

            CreationDate = dto.CreationDate;

            List<Place> places = new List<Place>();
            Place place;

            foreach (var placeDto in dto.Places)
            {
                place = new Place(placeDto);
                places.Add(place);
            }

            Places = places;
        }
    }
}