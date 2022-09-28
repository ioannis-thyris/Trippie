using Trippie.Database;
using Trippie.Models.Dtos;
using Trippie.Models.Entities;
using Trippie.Models.ViewModels;
using Trippie.Repository;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Trippie.Controllers
{
    public class TripController : Controller
    {
        private readonly ApplicationDbContext _context;

        private TripRepositοry tripRepo;
        private UserRepository userRepo;

        public TripController()
        {
            _context = new ApplicationDbContext();

            tripRepo = new TripRepositοry(_context);
            userRepo = new UserRepository(_context);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            var user = userRepo.GetCurrentUser(User);

            var trips = user.Trips;

            var tripsViewModel = new ListViewModel<Trip>(trips);

            return View(tripsViewModel);
        }


        // GET: Trip
        public ActionResult CreateTrip()
        {
            var user = userRepo.GetCurrentUser(User);

            bool isRegistered = user != null;
            bool isPremium = user == null ? false : user.IsPremiumUser;

            var placeTypes = tripRepo.GetPlaceTypes();

            CreateTripViewModel tripViewModel = new CreateTripViewModel
            {
                IsRegistered = isRegistered,
                IsPremiumUser = isPremium,
                PlaceTypes = placeTypes
            };

            return View(tripViewModel);
        }


        [Authorize]
        [HttpPost]
        public ActionResult CreateTrip(TripDto dto)
        {
            Trip trip = new Trip(dto);

            var user = userRepo.GetCurrentUser(User);

            trip.ApplicationUser = user;

            tripRepo.CreateTrip(trip);

            tripRepo.AddPlaceTypesToTrip(trip, dto.ChosenPlaceTypes);


            return Json(new { redirectToUrl = Url.Action("Index", "Trip") });
        }


        [Authorize]
        [HttpPost]
        public ActionResult DeleteTrip(int tripId)
        {
            var tripInDB = tripRepo.GetTripById(tripId);

            if (tripInDB == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            tripRepo.DeleteTrip(tripInDB);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RedirectToGoogleMapsUnregistered(TripDto trip)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            string startQueryParam = $"{trip.Start.Latitude}%2c{trip.Start.Longitude}";
            string destinationQueryParam = $"{trip.End.Latitude}%2c{trip.End.Longitude}";

            var waypointQueryParams = trip.Places.Select(p => $"{p.Latitude}%2c{p.Longitude}");
            var waypoints = String.Join("%7C", waypointQueryParams);

            string url = $"https://www.google.com/maps/dir/?api=1&origin={startQueryParam}&destination={destinationQueryParam}&travelmode=driving&waypoints={waypoints}";

            return Json(new { googleMapsUrl = url });
        }

        [HttpPost]
        public ActionResult RedirectToGoogleMaps(TripDtoForRedirect trip)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;


            string startQueryParam = $"{trip.StartLat}%2c{trip.StartLon}";
            string destinationQueryParam = $"{trip.EndLat}%2c{trip.EndLon}";

            var waypointQueryParams = trip.Waypoints.Select(p => $"{p.Lat}%2c{p.Lon}");
            var waypoints = String.Join("%7C", waypointQueryParams);

            string url = $"https://www.google.com/maps/dir/?api=1&origin={startQueryParam}&destination={destinationQueryParam}&travelmode=driving&waypoints={waypoints}";

            return Json(new { googleMapsUrl = url});
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}