using AppGeofencing.Models.Enums;
using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppGeofencing.Services
{
    public static class MessageService
    {
        #region Toast

        public static async Task ShowToastAsync(string message, MessageType? messageType, int durationInSeconds = 4)
        {
            if (MainThread.IsMainThread)
            {
                await CreateToastAsync(message, messageType, durationInSeconds).ConfigureAwait(false);
            }
            else
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await CreateToastAsync(message, messageType, durationInSeconds).ConfigureAwait(false);
                });
            }
        }

        private static async Task CreateToastAsync(string message, MessageType? messageType, int durationInSeconds = 4)
        {
            var messageOptions = new MessageOptions
            {
                Message = message
            };

            var toastOptions = new ToastOptions
            {
                Duration = TimeSpan.FromSeconds(durationInSeconds),
            };

            switch (messageType)
            {
                case MessageType.Success:
                    messageOptions.Foreground = Color.White;
                    toastOptions.BackgroundColor = Color.FromHex("#6651A121");
                    break;

                case MessageType.Info:
                    messageOptions.Foreground = Color.Black;
                    toastOptions.BackgroundColor = Color.FromHex("#660DCAF0");
                    break;

                case MessageType.Warning:
                    messageOptions.Foreground = Color.Black;
                    toastOptions.BackgroundColor = Color.FromHex("#66ffc107");
                    break;

                case MessageType.Error:
                    messageOptions.Foreground = Color.White;
                    toastOptions.BackgroundColor = Color.FromHex("#66DC3545");
                    break;

                default:

                    break;
            }

            toastOptions.MessageOptions = messageOptions;
            await App.Current.MainPage.DisplayToastAsync(toastOptions).ConfigureAwait(false);
        }

        #endregion Toast
    }
}