using AppGeofencing.Interfaces;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(AppGeofencing.iOS.Implementations.AppleLiteDB))]

namespace AppGeofencing.iOS.Implementations
{
    public class AppleLiteDB : ILiteDB
    {
        public string CadenaConexion()
        {
            return "";
        }
    }
}