using System;
using System.Collections.Generic;
using System.Text;
using SJRAtlas.Core;
using System.Text.RegularExpressions;
using Castle.ActiveRecord.Queries;

namespace SJRAtlas.DataWarehouse
{
    public class DWAtlasUtils : IAtlasUtils
    {
        #region IAtlasUtils Members

        public bool IsWithinBasin(IPlaceName placename)
        {
            CgnsWatershedCrossReference[] xref = CgnsWatershedCrossReference.FindAllByProperty("CgndbKey", placename.Id);
            return (xref.Length == 1 && IsDrainageCodeWithinBasin(xref[0].DrainageCode));
        }

        public bool IsWithinBasin(IWatershed watershed)
        {
            return IsDrainageCodeWithinBasin(watershed.DrainageCode);
        }

        protected bool IsDrainageCodeWithinBasin(string drainageCode)
        {
            Regex re = new Regex(@"01-[\d]{2}-[\d]{2}-[\d]{2}-[\d]{2}-[\d]{2}");
            return re.IsMatch(drainageCode);
        }

        public bool IsWatershed(IPlaceName placename)
        {
            throw new Exception("Don't Use This!!!");
        }

        public string FindDrainageCode(IPlaceName placename)
        {
            throw new Exception("Don't Use This!!!");
        }

        public bool IsWaterBody(IPlaceName placename)
        {
            if (placename.ConciseTerm == "Lake" || placename.ConciseTerm == "River")
            {
                string property = "CgndbKey";
                if (placename.StatusTerm == "Rescinded")
                {
                    property = "AltCgndbKey";
                }
                
                WaterBody[] waterbodies = WaterBody.FindAllByProperty(property, placename.Id);
                return (waterbodies != null && waterbodies.Length == 1);
            }

            return false;
        }

        public IWaterBody CreateWaterBodyFromPlaceName(IPlaceName placename)
        {
            if (placename == null)
                return null;

            string property = "CgndbKey";
            if (placename.StatusTerm == "Rescinded")
            {
                property = "AltCgndbKey";
            }

            WaterBody[] waterbodies = WaterBody.FindAllByProperty(property, placename.Id);
            if (waterbodies != null && waterbodies.Length == 1)
            {
                WaterBody waterbody = waterbodies[0];
                waterbody.Placename = placename;

                waterbody.Watershed = DrainageUnit.Find(waterbody.DrainageCode);

                return waterbody;
            }

            return null;
        }

        public string[] DataSetTitles(IWaterBody waterbody)
        {
            WaterBodyDataTypeXRef[] xrefs = WaterBodyDataTypeXRef.FindAllByProperty("WaterBodyId", waterbody.Id);
            string[] titles = new string[xrefs.Length];
            for(int i=0; i < xrefs.Length; i++)
            {
                titles[i] = xrefs[i].DataName;
            }
            return titles;
        }

        public string[] DataSetTitles(IWatershed watershed)
        {
            string drainageCodeMatch = watershed.DrainageCode.Substring(0, 
                watershed.DrainageCode.IndexOf("00")) + "%";
            SimpleQuery<WaterBodyDataTypeXRef> query = new SimpleQuery<WaterBodyDataTypeXRef>("from WaterBodyDataTypeXRef as xref " +
                @"where xref.DrainageCd like ?", drainageCodeMatch);
            WaterBodyDataTypeXRef[] xrefs = query.Execute();

            List<string> titles = new List<string>();
            foreach (WaterBodyDataTypeXRef xref in xrefs)
            {
                if (!titles.Contains(xref.DataName))
                    titles.Add(xref.DataName);
            }
            return titles.ToArray();
        }

        #endregion
    }
}
