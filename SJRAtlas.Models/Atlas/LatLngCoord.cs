using System;
using System.Collections.Generic;
using System.Text;

namespace SJRAtlas.Models.Atlas
{
    public class LatLngCoord
    {
        public LatLngCoord(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }

        private double latitude;

        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        private double longitude;

        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }
	
    }
}
