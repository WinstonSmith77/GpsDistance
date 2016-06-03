using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpsDistance
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"C:\Users\mhenning\Desktop\track.csv";
            var lines = File.ReadAllLines(path);

            var points = lines
                .Select(line => line.Split(','))
                .Where(item => item.Length == 4).Select(parts =>
                Tuple.Create(
                    double.Parse(parts[1], CultureInfo.InvariantCulture),
                    double.Parse(parts[2], CultureInfo.InvariantCulture)
                    )).ToList();

            var sum = 0.0;
            for (var i = 0; i < points.Count - 1; i++)
            {
                var a = points[i];
                var b = points[i + 1];
                sum += HaversineInKM(a.Item1, a.Item2, b.Item1, b.Item2);
            }

            Console.WriteLine("Dist " + sum);
            Console.ReadLine();
        }

        const double _eQuatorialEarthRadius = 6378.1370D;
        const double _d2r = (Math.PI / 180D);

        private static double HaversineInKM(double lat1, double long1, double lat2, double long2)
        {
            double dlong = (long2 - long1) * _d2r;
            double dlat = (lat2 - lat1) * _d2r;
            double a = Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(lat1 * _d2r) * Math.Cos(lat2 * _d2r) * Math.Pow(Math.Sin(dlong / 2D), 2D);
            double c = 2D * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1D - a));
            double d = _eQuatorialEarthRadius * c;

            return d;
        }
    }
}
