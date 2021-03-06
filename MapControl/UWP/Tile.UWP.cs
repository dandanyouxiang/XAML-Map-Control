﻿// XAML Map Control - https://github.com/ClemensFischer/XAML-Map-Control
// © 2018 Clemens Fischer
// Licensed under the Microsoft Public License (Ms-PL)

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace MapControl
{
    public partial class Tile
    {
        public void SetImage(ImageSource imageSource, bool fadeIn = true)
        {
            Pending = false;

            if (fadeIn && FadeDuration > TimeSpan.Zero)
            {
                var bitmapImage = imageSource as BitmapImage;

                if (bitmapImage != null && bitmapImage.UriSource != null)
                {
                    bitmapImage.ImageOpened += BitmapImageOpened;
                    bitmapImage.ImageFailed += BitmapImageFailed;
                }
                else
                {
                    Image.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation { To = 1d, Duration = FadeDuration });
                }
            }
            else
            {
                Image.Opacity = 1d;
            }

            Image.Source = imageSource;
        }

        private void BitmapImageOpened(object sender, RoutedEventArgs e)
        {
            var bitmapImage = (BitmapImage)sender;

            bitmapImage.ImageOpened -= BitmapImageOpened;
            bitmapImage.ImageFailed -= BitmapImageFailed;

            Image.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation { To = 1d, Duration = FadeDuration });
        }

        private void BitmapImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            var bitmapImage = (BitmapImage)sender;

            bitmapImage.ImageOpened -= BitmapImageOpened;
            bitmapImage.ImageFailed -= BitmapImageFailed;

            Image.Source = null;
        }
    }
}
