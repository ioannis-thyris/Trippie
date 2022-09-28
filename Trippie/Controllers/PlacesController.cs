using Trippie.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Trippie.Helpers;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Collections.Concurrent;
using System.Web.Http.Cors;
using Trippie.Models.Dtos;
using Trippie.Models.Helper;

namespace Trippie.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")] // tune to your needs
    [RoutePrefix("")]
    public class PlacesController : ApiController
    {
        private static readonly HttpClient _client = new HttpClient();
        private readonly string baseUri = "https://api.opentripmap.com/0.1/en/places/radius?";
        private readonly string apiKey = "5ae2e3f221c38a28845f05b6068096737a6bd1b9a215367ca1d1bd33";


        [HttpPost]
        public async Task<IEnumerable<PlaceDtoForOpenTripMap>> GetPlacesAlongPath(SearchAlongPathDto dto)
        {
            SetDecimalSeparator();

            int points = dto.PointsAlongPath ?? 8;
            int radius = dto.Radius ?? 7000;

            var reducedPath = PointReductionHelper.ReducePoints(dto.PathOverview, points);

            var places = await GetPlaces(reducedPath, dto.PlaceTypes, radius);

            return places.GroupBy(p => p.xid)
                         .Select(g => g.FirstOrDefault());
        }


        [NonAction]
        public async Task<List<PlaceDtoForOpenTripMap>> GetPlaces(Coordinates[] reducedPath, PlaceTypeEnum[] placeTypes, int radius)
        {
            ConcurrentBag<PlaceDtoForOpenTripMap> placesConcur = new ConcurrentBag<PlaceDtoForOpenTripMap>();

            var tasks = new List<Task<PlaceDtoForOpenTripMap[]>>();

            int batchSize = 10;
            int numberOfBatches = (int)Math.Ceiling((double)reducedPath.Length / batchSize);

            Uri placesRadiusAPI;

            for (int i = 0; i < numberOfBatches; i++)
            {
                var currentCoordinates = reducedPath.Skip(i * batchSize)
                                                    .Take(batchSize)
                                                    .ToList();

                for (int j = 0; j < currentCoordinates.Count; j++)
                {
                    placesRadiusAPI = RadiusEndpoint(radius, currentCoordinates[j].Longitude, currentCoordinates[j].Latitude, placeTypes, 3);
                    tasks.Add(_client.GetFromJsonAsync<PlaceDtoForOpenTripMap[]>(placesRadiusAPI));
                }

                var currentPlaces = await Task.WhenAll(tasks);

                var flattenedArray = currentPlaces.SelectMany(placeArray => placeArray);

                foreach (var item in flattenedArray)
                {
                    placesConcur.Add(item);
                }

                if (i < numberOfBatches - 1)
                    await Task.Delay(2000);

                tasks.Clear();
            }

            return placesConcur.ToList();
        }

        [NonAction]
        public Uri RadiusEndpoint(int radius, double lon, double lat, PlaceTypeEnum[] placeTypes, int rate = 2, int limit = 50, string format = "json")
        {
            var typesParsed = placeTypes.Select(value => Enum.GetName(typeof(PlaceTypeEnum), value))
                                 .Select(name => char.ToLower(name[0]).ToString() + name.Substring(1))
                                 .ToArray();

            var kinds = String.Join(",", typesParsed);

            StringBuilder sb = new StringBuilder();

            sb.Append(baseUri)
              .Append($"radius={radius}")
              .Append($"&lon={lon}")
              .Append($"&lat={lat}")
              .Append($"&kinds={kinds}")
              .Append($"&rate={rate}")
              .Append($"&limit={limit}")
              .Append($"&format={format}")
              .Append($"&apikey={apiKey}");

            return new Uri(sb.ToString());
        }



        [NonAction]
        public void SetDecimalSeparator()
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
        }

    }
}
