using System;
using Castle.Components.Validator;

namespace SJRAtlas.Site.Models
{
    public class SearchOptions
    {
        private string dataSet;

        private string watershed;
        private int waterBodyID;
        private string agency;
        private DateTime startDate;
        private DateTime endDate;

        [ValidateNonEmpty("Data Set is a required field")]
        public string DataSet
        {
            get { return dataSet; }
            set { dataSet = value; }
        }

        public string Watershed
        {
            get { return watershed; }
            set { watershed = value; }
        }

        [ValidateInteger("Water Body ID must be an integer")]
        public int WaterBodyID
        {
            get { return waterBodyID; }
            set { waterBodyID = value; }
        }

        public string Agency
        {
            get { return agency; }
            set { agency = value; }
        }

        [ValidateDateTime("The start date must be a year")]
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }
	
    }
}
