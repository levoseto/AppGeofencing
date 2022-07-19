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

        public static async Task ShowToastAsync(this ContentPage contentPage, string message, MessageType? messageType, int durationInSeconds = 4)
        {
            if (MainThread.IsMainThread)
            {
                await CreateToastAsync(contentPage, message, messageType, durationInSeconds).ConfigureAwait(false);
            }
            else
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await CreateToastAsync(contentPage, message, messageType, durationInSeconds).ConfigureAwait(false);
                });
            }
        }

        private static async Task CreateToastAsync(ContentPage contentPage, string message, MessageType? messageType, int durationInSeconds = 4)
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
                    toastOptions.BackgroundColor = Color.Green;
                    break;

                case MessageType.Info:
                    messageOptions.Foreground = Color.White;
                    toastOptions.BackgroundColor = Color.LightBlue;
                    break;

                case MessageType.Warning:
                    messageOptions.Foreground = Color.Black;
                    toastOptions.BackgroundColor = Color.Yellow;
                    break;

                case MessageType.Error:
                    messageOptions.Foreground = Color.Black;
                    toastOptions.BackgroundColor = Color.Red;
                    break;

                default:

                    break;
            }

            toastOptions.MessageOptions = messageOptions;
            await contentPage.DisplayToastAsync(toastOptions).ConfigureAwait(false);
        }

        #endregion Toast
    }
}