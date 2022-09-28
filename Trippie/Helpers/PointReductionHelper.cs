using Trippie.Models;
using Trippie.Models.Helper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Trippie.Helpers
{
    public static class PointReductionHelper
    {
        private static List<CoordinateForReduction> GetFullListOfPoints(List<Coordinates> pointListInitial)
        {
            List<CoordinateForReduction> pointList = new List<CoordinateForReduction>() { new CoordinateForReduction { Id = 1, Longitude = pointListInitial[0].Longitude, Latitude = pointListInitial[0].Latitude } };


            for (int i = 1; i < pointListInitial.Count; i++)
            {
                int Id = i + 1;
                double Longitude = pointListInitial[i].Longitude;
                double Latitude = pointListInitial[i].Latitude;
                double DistanceFromPrevious = PointReductionHelper.Distance(
                                                            Longitude,
                                                            pointList[i - 1].Longitude,
                                                            Latitude,
                                                            pointList[i - 1].Latitude
                                                            );
                double DistanceFromStart = pointList[i - 1].DistanceFromStart + DistanceFromPrevious;
                pointList.Add(new CoordinateForReduction { Id = Id, Longitude = Longitude, Latitude = Latitude, DistanceFromPrevious = DistanceFromPrevious, DistanceFromStart = DistanceFromStart });

            }
            return pointList;
        }

        private static double Distance(double lon1, double lon2, double lat1, double lat2)
        {
            // The math module contains
            // a function named toRadians
            // which converts from degrees
            // to radians.
            lon1 = ToRadians(lon1);
            lon2 = ToRadians(lon2);
            lat1 = ToRadians(lat1);
            lat2 = ToRadians(lat2);

            // Haversine formula
            double dlon = lon2 - lon1;
            double dlat = lat2 - lat1;
            double a = (Math.Pow(Math.Sin(dlat / 2), 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Pow(Math.Sin(dlon / 2), 2));

            double c = (2 * Math.Asin(Math.Sqrt(a)));

            // Radius of earth in
            // kilometers. Use 3956
            // for miles
            double r = 6371;

            // calculate the result
            return (c * r);
        }

        private static double ToRadians(double angleIn10thofaDegree)
        {
            // Angle in 10th
            // of a degree
            return (angleIn10thofaDegree * Math.PI) / 180;
        }

        public static Coordinates[] ReducePoints(List<Coordinates> pathOverview, int noStations)
        {
            var pointList = GetFullListOfPoints(pathOverview);

            List<CoordinateForReduction> reducedPoints = new List<CoordinateForReduction>();
            var spacing = pointList[pointList.Count - 1].DistanceFromStart / (noStations + 1);
            reducedPoints.Add(pointList[0]);

            var i1 = 1;
            for (var j = 1; j <= noStations; j++)
            {
                for (var i = i1; i <= pointList.Count; i++)
                {
                    double interPoint = spacing * j;
                    if (pointList[i].DistanceFromStart < interPoint && pointList[i + 1].DistanceFromStart > interPoint)
                    {
                        CoordinateForReduction interPointActual = InterPoint(pointList[i], interPoint, pointList[i + 1]);
                        reducedPoints.Add(interPointActual);
                        i1 = i + 1;
                        break;
                    }
                }
            }

            reducedPoints.Add(pointList[pointList.Count - 1]);

            return reducedPoints.Select(p => new Coordinates() { Latitude = p.Latitude, Longitude = p.Longitude })
                                 .ToArray();
        }

        private static CoordinateForReduction InterPoint(CoordinateForReduction p1, double interPoint, CoordinateForReduction p3)
        {
            var diff1 = interPoint - p1.DistanceFromStart;
            var diff2 = p3.DistanceFromStart - interPoint;
            var intPoint = diff1 <= diff2 ? p1 : p3;
            return intPoint;
        }
    }
}