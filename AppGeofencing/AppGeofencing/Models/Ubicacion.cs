using System;
using LiteDB;

namespace AppGeofencing.Models
{
    public class Ubicacion
    {
        [BsonId]
        public int _id { get; set; }

        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public double Altitud { get; set; }
        public DateTime DbTimeStamp { get; set; }
    }
}