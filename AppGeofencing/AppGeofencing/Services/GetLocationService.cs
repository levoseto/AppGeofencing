using AppGeofencing.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppGeofencing.Services
{
    public class GetLocationService
    {
        private readonly bool stopping = false;

        public async Task Run(CancellationToken token)
        {
            await Task.Run(async () =>
            {
                while (!stopping)
                {
                    token.ThrowIfCancellationRequested();
                    try
                    {
                        await Task.Delay(10000);

                        var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));
                        var location = await Geolocation.GetLocationAsync(request).ConfigureAwait(false);
                        if (location != null)
                        {
                            var message = new LocationMessage
                            {
                                Message = $"Location OK - {DateTime.Now}",
                                Latitude = location.Latitude,
                                Longitude = location.Longitude,
                                Altitude = location.Altitude ?? 0
                            };

                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                MessagingCenter.Send(message, "Location");
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            var errormessage = new LocationErrorMessage
                            {
                                Message = ex.Message
                            };

                            MessagingCenter.Send(errormessage, "LocationError");
                        });
                    }
                }
                return;
            }, token);
        }
    }
}