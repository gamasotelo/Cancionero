using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace Alabanza.utils
{
    public class DisplayInfoTest
    {
        public DisplayInfoTest()
        {
            // Subscribe to changes of screen metrics
            DeviceDisplay.MainDisplayInfoChanged += OnMainDisplayInfoChanged;
        }

        void OnMainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
        {
            // Process changes
            var displayInfo = e.DisplayInfo;
        }
    }
}
