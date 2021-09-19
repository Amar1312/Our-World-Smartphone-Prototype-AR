using System.Globalization;

namespace OurWorld.Scripts.DataModels.Geolocation
{
    public class Geolocation
    {
        public double Longitude { get; set; } // In Degrees
        public double Latitude { get; set; } // In Degrees

        public Geolocation()
        {

        }
        public Geolocation(double longtitude, double latitude)
        {
            Longitude = latitude;
            Latitude = latitude;
        }

          public override string ToString()
          {
               return $"{Longitude.ToString(CultureInfo.InvariantCulture)},{Latitude.ToString(CultureInfo.InvariantCulture)}";
          }
     }
}
