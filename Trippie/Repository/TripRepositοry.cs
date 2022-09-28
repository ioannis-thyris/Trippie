using Trippie.Database;
using Trippie.Models;
using Trippie.Models.Entities;
using Trippie.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;


namespace Trippie.Repository
{
    public class TripRepositοry
    {
        ApplicationDbContext db;

        public TripRepositοry(ApplicationDbContext context)
        {
            db = context;
        }


        public IEnumerable<PlaceType> GetPlaceTypes() => db.PlaceTypes;

        public void CreateTrip(Trip trip)
        {
            db.Trips.Add(trip);
            db.SaveChanges();
        }

        public void AddPlaceTypesToTrip(Trip trip, List<int> placeTypeValues)
        {
            List<PlaceType> placeTypes = new List<PlaceType>();

            PlaceType placeType;

            foreach (var value in placeTypeValues)
            {
                placeType = db.PlaceTypes.Find(value);

                placeTypes.Add(placeType);
            }

            trip.ChosenPlaceTypes = placeTypes;

            db.SaveChanges();
        }

        public IEnumerable<Trip> GetAllTrips() => db.Trips;

        public Trip GetTripById(int tripId) => db.Trips.Find(tripId);

        public void DeleteTrip(int tripId)
        {
            var tripInDB = db.Trips.Find(tripId);

            db.Trips.Remove(tripInDB);
            db.SaveChanges();
        }

        public void DeleteTrip(Trip trip)
        {
            db.Trips.Remove(trip);
            db.SaveChanges();
        }


        #region ReportMethods
        public int GeNumberOfUsers() => db.Users.Count();

        public int GetNumberOfTrips() => db.Trips.Count();

        public int GetNumberOfPlaces() => db.Places.Count();

        public int GetNumberOfCities()
        {
             return db.Cities.GroupBy(c => c.GeonameID)
                             .Count();
        }

        public double GetAveragePlacesInTrip()
        {
            var average = db.Places.Count() == 0 ? 0 : db.Trips.Average(t => t.Places.Count);

            return Math.Round(average, 1);
        }

        public IEnumerable<AdminViewModelPlaceType> GetMostSelectedPlaceTypes()
        {
            return db.PlaceTypes.Select(t => new AdminViewModelPlaceType { Name = t.Name, Count = t.Trips.Count })
                                      .OrderByDescending(g => g.Count);
        }

        public IEnumerable<AdminViewModelCity> GetMostSelectedCitiesOverall(int number)
        {
            var startCities = db.Trips.Select(t => t.Start);
            var endCities = db.Trips.Select(t => t.End);

            return startCities.Union(endCities)
                              .GroupBy(c => c.GeonameID, (id, cities) => new AdminViewModelCity { City = cities.FirstOrDefault(), Count = cities.Count() })
                              .OrderByDescending(g => g.Count)
                              .ThenBy(g => g.City.Name)
                              .Take(number);
        }

        public IEnumerable<AdminViewModelCity> GetMostSelectedCitiesStart(int number)
        {

            return db.Trips.Select(t => t.Start)
                                 .GroupBy(c => c.GeonameID, (id, cities) => new AdminViewModelCity { City = cities.FirstOrDefault(), Count = cities.Count() })
                                 .OrderByDescending(g => g.Count)
                                 .ThenBy(g => g.City.Name)
                                 .Take(number);
        }

        public IEnumerable<AdminViewModelCity> GetMostSelectedCitiesEnd(int number)
        {
            return db.Trips.Select(t => t.End)
                                 .GroupBy(c => c.GeonameID, (id, cities) => new AdminViewModelCity { City = cities.FirstOrDefault(), Count = cities.Count() })
                                 .OrderByDescending(g => g.Count)
                                 .ThenBy(g => g.City.Name)
                                 .Take(number);
        }

        public IEnumerable<AdminViewModelCountry> GetMostActiveCountries(int number)
        {
            return db.Cities.GroupBy(c => c.Country, (country, cities) => new AdminViewModelCountry { Name = country, Count = cities.Count() })
                                  .OrderBy(g => g.Count)
                                  .OrderByDescending(g => g.Count)
                                  .Take(number);
        }
        #endregion


    }
}