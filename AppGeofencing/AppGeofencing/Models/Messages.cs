namespace AppGeofencing.Models
{
    public class BaseMessage
    {
        public string Message { get; set; }
    }

    public class StartServiceMessage : BaseMessage
    {
    }

    public class StopServiceMessage : BaseMessage
    {
    }

    public class LocationMessage : BaseMessage
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
    }

    public class LocationErrorMessage : BaseMessage
    {
    }
}