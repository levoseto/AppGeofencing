using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Content;
using Xamarin.Forms;
using AppGeofencing.Models;
using AppGeofencing.Droid.Services;

namespace AppGeofencing.Droid
{
    [Activity(Label = "AppGeofencing", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private Intent serviceIntent;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            serviceIntent = new Intent(this, typeof(AndroidLocationService));
            SetServiceMethods();

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void SetServiceMethods()
        {
            MessagingCenter.Subscribe<StartServiceMessage>(this, "ServiceStarted",
                message =>
                {
                    if (!IsServiceRunning(typeof(AndroidLocationService)))
                    {
                        if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                        {
                            StartForegroundService(serviceIntent);
                        }
                        else
                        {
                            StartService(serviceIntent);
                        }
                    }
                });

            MessagingCenter.Subscribe<StopServiceMessage>(this, "ServiceStopped",
                message =>
                {
                    if (IsServiceRunning(typeof(AndroidLocationService)))
                    {
                        StopService(serviceIntent);
                    }
                });
        }

        private bool IsServiceRunning(Type cls)
        {
            ActivityManager manager =
                (ActivityManager)GetSystemService(Context.ActivityService);

            foreach (var service in manager.GetRunningServices(int.MaxValue))
            {
                if (service.Service.ClassName.Equals(Java.Lang.Class.FromType(cls)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}