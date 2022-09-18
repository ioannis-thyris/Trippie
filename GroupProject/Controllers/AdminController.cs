﻿using GroupProject.Database;
using GroupProject.Models.ViewModels;
using GroupProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Web;
using System.Web.Mvc;

namespace GroupProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        private TripRepositοry tripRepo;
        private UserRepository userRepo;

        public AdminController()
        {
            _context = new ApplicationDbContext();

            tripRepo = new TripRepositοry(_context);
            userRepo = new UserRepository(_context);
        }

        // GET: Admin
        public ActionResult Index()
        {
            var numberOfUsers = tripRepo.GeNumberOfUsers();
            var numberOfTrips = tripRepo.GetNumberOfTrips();
            var numberOfPlaces = tripRepo.GetNumberOfPlaces();
            var numberOfCities = tripRepo.GetNumberOfCities();
            var averagePlacesInTrip = tripRepo.GetAveragePlacesInTrip();

            var mostSelectedCitiesOverall = tripRepo.GetMostSelectedCitiesOverall(5).ToList();
            var mostSelectedCitiesStart = tripRepo.GetMostSelectedCitiesStart(5).ToList();
            var mostSelectedCitiesEnd = tripRepo.GetMostSelectedCitiesEnd(5).ToList();
            var mostActiveCountries = tripRepo.GetMostActiveCountries(5).ToList();
            var mostSelectedPlaceTypes = tripRepo.GetMostSelectedPlaceTypes().ToList();

            var viewModel = new AdminViewModel
            {
                NumberOfUsers = numberOfUsers,
                NumberOfTrips = numberOfTrips,
                NumberOfPlaces = numberOfPlaces,
                NumberOfCities = numberOfCities,
                AveragePlacesInTrip = averagePlacesInTrip,
                MostSelectedCitiesOverall = mostSelectedCitiesOverall,
                MostSelectedCitiesStart = mostSelectedCitiesStart,
                MostSelectedCitiesEnd = mostSelectedCitiesEnd,
                MostActiveCountries = mostActiveCountries,
                MostSelectedPlaceTypes = mostSelectedPlaceTypes
            };

            return View(viewModel);
        }
    }
}