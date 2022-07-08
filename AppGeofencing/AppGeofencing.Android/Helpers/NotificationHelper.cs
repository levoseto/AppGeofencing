using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using Xamarin.Essentials;

namespace AppGeofencing.Droid.Helpers
{
    public class NotificationHelper
    {
        private const string _ForegroundChannelId = "9001";
        private static readonly Context _Context = Platform.CurrentActivity.ApplicationContext;

        public Notification GetServiceStartedNotification()
        {
            var intent = new Intent(_Context, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.SingleTop);
            intent.PutExtra("Title", "Message");

            var pendingIntent = PendingIntent.GetActivity(_Context, 0, intent, PendingIntentFlags.UpdateCurrent);

            var notificationBuilder = new NotificationCompat.Builder(_Context, _ForegroundChannelId)
                .SetContentTitle("Background Tracking Example")
                .SetContentText("Your location is being tracked")
                .SetSmallIcon(Resource.Drawable.notification)
                .SetOngoing(true)
                .SetContentIntent(pendingIntent);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                NotificationChannel notificationChannel = new NotificationChannel(_ForegroundChannelId, "Title", NotificationImportance.High)
                {
                    Importance = NotificationImportance.High
                };
                notificationChannel.EnableLights(true);
                notificationChannel.EnableVibration(true);
                notificationChannel.SetShowBadge(true);
                notificationChannel.SetVibrationPattern(new long[] { 100, 200, 300 });

                var notificationManager = _Context.GetSystemService(Context.NotificationService) as NotificationManager;
                if (notificationManager != null)
                {
                    notificationBuilder.SetChannelId(_ForegroundChannelId);
                    notificationManager.CreateNotificationChannel(notificationChannel);
                }
            }

            return notificationBuilder.Build();
        }
    }
}