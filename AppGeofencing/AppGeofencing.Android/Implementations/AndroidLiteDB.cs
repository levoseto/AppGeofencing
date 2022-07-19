using Android.App;
using AppGeofencing.Interfaces;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(AppGeofencing.Droid.Implementations.AndroidLiteDB))]

namespace AppGeofencing.Droid.Implementations
{
    public class AndroidLiteDB : ILiteDB
    {
        public string CadenaConexion()
        {
            return $"{Path.Combine(Application.Context.GetExternalFilesDir("").ToString(), "UbicacionTrack.db")}";
        }
    }
}