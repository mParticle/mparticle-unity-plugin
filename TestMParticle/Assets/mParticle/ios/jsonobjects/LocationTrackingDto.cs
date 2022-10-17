using System;
using System.Collections.Generic;

namespace mParticle.ios
{
    [Serializable]
    public class LocationTrackingDto : Dictionary<string, object>
    {
        private const string Enabled = "Enabled";
        private const string Provider = "Provider";
        private const string MinTime = "MinTime";
        private const string MinDistance = "MinDistance";
        private const string MinAccuracy = "MinAccuracy";

        public LocationTrackingDto(LocationTracking locationTracking)
        {
            Add(Enabled, locationTracking.Enabled);
            if (String.IsNullOrEmpty(locationTracking.Provider))
            {
                Add(Provider, locationTracking.Provider);
            }
            if (locationTracking.MinTime.HasValue)
            {
                Add(MinTime, locationTracking.MinTime.Value);
            }
            if (locationTracking.MinDistance.HasValue)
            {
                Add(MinDistance, locationTracking.MinDistance.Value);
            }
            if (locationTracking.MinAccuracy.HasValue)
            {
                Add(MinAccuracy, locationTracking.MinAccuracy.Value);
            }
        }
    }
}

