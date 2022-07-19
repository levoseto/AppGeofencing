using AppGeofencing.Models;
using AppGeofencing.Services;
using Plugin.Geolocator;
using System;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppGeofencing
{
    public partial class MainPage : ContentPage
    {
        private static readonly LiteDBService _LiteDBService = new LiteDBService();
        private static Ubicacion ubicacion;

        public MainPage()
        {
            InitializeComponent();
            InicializaMensajes();
        }

        private void InicializaMensajes()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                MessagingCenter.Subscribe<LocationMessage>(this, "Location",
                    message =>
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            ubicacion = new Ubicacion
                            {
                                DbTimeStamp = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                                Altitud = message.Altitude,
                                Latitud = message.Latitude,
                                Longitud = message.Longitude,
                            };

                            _LiteDBService.Inserta(ubicacion);

                            slContenedor.IsVisible = true;
                            slCarga.IsVisible = false;
                            aiCarga.IsRunning = false;

                            locationLabel.Text += $"{Environment.NewLine}" +
                            $"Lat: {ubicacion.Latitud}, Long: {ubicacion.Longitud}, Alt:{ubicacion.Altitud}" +
                            $" - {DateTime.Now.ToLongTimeString()}";

                            Console.WriteLine($"Lat: {ubicacion.Latitud}, Long: {ubicacion.Longitud}," +
                            $", Alt:{ubicacion.Altitud} - {ubicacion.DbTimeStamp.ToLongTimeString()}");
                        });
                    });

                MessagingCenter.Subscribe<StopServiceMessage>(this, "ServiceStopped",
                    message =>
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            locationLabel.Text = "Location service has been stoped";
                        });
                    });

                MessagingCenter.Subscribe<LocationErrorMessage>(this, "LocationError",
                    message =>
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            locationLabel.Text = "There was an error updating location!!";
                        });
                    });

                if (Preferences.Get("LocationServiceRunning", false) == true)
                {
                    StartService();
                }
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var permission = await
                Permissions.RequestAsync<Permissions.LocationAlways>();

            if (permission == PermissionStatus.Denied)
            {
                return;
            }

            var memoryPermission = await Permissions.RequestAsync<Permissions.StorageWrite>();

            if (memoryPermission == PermissionStatus.Denied)
            {
                return;
            }

            if (Device.RuntimePlatform == Device.iOS)
            {
                if (CrossGeolocator.Current.IsListening)
                {
                    await CrossGeolocator.Current.StopListeningAsync();
                    CrossGeolocator.Current.PositionChanged -= Current_PositionChanged;

                    return;
                }

                await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(1), 10, false, new Plugin.Geolocator.Abstractions.ListenerSettings
                {
                    ActivityType = Plugin.Geolocator.Abstractions.ActivityType.AutomotiveNavigation,
                    AllowBackgroundUpdates = true,
                    DeferLocationUpdates = true,
                    DeferralDistanceMeters = 10,
                    DeferralTime = TimeSpan.FromSeconds(5),
                    ListenForSignificantChanges = true,
                    PauseLocationUpdatesAutomatically = true
                });

                CrossGeolocator.Current.PositionChanged += Current_PositionChanged;
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                if (Preferences.Get("LocationServiceRunning", false) == false)
                {
                    StartService();
                    await this.ShowToastAsync("Servicio corriendo actualmente", Models.Enums.MessageType.Success);
                }
                else
                {
                    StopService();
                    await this.ShowToastAsync("Servicio detenido", Models.Enums.MessageType.Warning);
                }
            }
        }

        private void StartService()
        {
            var startServiceMessage = new StartServiceMessage();
            MessagingCenter.Send(startServiceMessage, "ServiceStarted");
            Preferences.Set("LocationServiceRunning", true);
            locationLabel.Text = $"Location Service has been started!";
            locationLabel2.Text = $"Location Service has been started!";
            if (ubicacion is null)
            {
                slContenedor.IsVisible = false;
                slCarga.IsVisible = true;
                aiCarga.IsRunning = true;
            }
        }

        private void StopService()
        {
            var stopServiceMessage = new StopServiceMessage();
            MessagingCenter.Send(stopServiceMessage, "ServiceStopped");
            Preferences.Set("LocationServiceRunning", false);
        }

        private void Current_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            locationLabel.Text = $"{e.Position.Latitude}, {e.Position.Longitude}," +
                $"{e.Position.Timestamp.TimeOfDay}{Environment.NewLine}";

            Console.WriteLine($"{e.Position.Latitude}, {e.Position.Longitude}," +
                $"{e.Position.Timestamp.TimeOfDay}");
        }
    }
}