using System;
using Castle.ActiveRecord.Queries;

namespace SJRAtlas.DataWarehouse
{
    public partial class Agency
    {
        /**
         * SELECT tblAquaticActivity.AgencyCd, cdAgency.Agency FROM cdAgency 
         * INNER JOIN ((tblWaterBody RIGHT JOIN tblAquaticSite ON 
         * tblWaterBody.WaterBodyID = tblAquaticSite.WaterBodyID) 
         * INNER JOIN tblAquaticActivity ON 
         * tblAquaticSite.AquaticSiteID = tblAquaticActivity.AquaticSiteID) ON 
         * cdAgency.AgencyCd = tblAquaticActivity.AgencyCd 
         * GROUP BY tblAquaticActivity.AgencyCd, cdAgency.Agency
         **/

        public static Agency[] FindAllByDrainageCode(string drainageCode)
        {
            string where = "tblWaterBody.DrainageCd LIKE ?";            
            SimpleQuery<Agency> q = new SimpleQuery<Agency>(AgencyShortListQuery(where), drainageCode);
            return q.Execute();
        }

        public static Agency[] FindAllByWaterBodyId(string waterbodyId)
        {
            string where = "tblWaterBody.WaterBodyID = ?";
            SimpleQuery<Agency> q = new SimpleQuery<Agency>(AgencyShortListQuery(where), waterbodyId);
            return q.Execute();
        }

        public static Agency[] FindAllByAquaticSiteId(string aquaticSiteId)
        {
            string where = "tblAquaticActivity.AquaticSiteID = ?";
            SimpleQuery<Agency> q = new SimpleQuery<Agency>(AgencyShortListQuery(where), aquaticSiteId);
            return q.Execute();
        }

        private static string AgencyShortListQuery(string where)
        {
            return String.Format(shortListQuery, where);
        }

        public static readonly string shortListQuery = 
        @"SELECT tblAquaticActivity.AgencyCd, cdAgency.Agency
          FROM cdAgency INNER JOIN
                tblWaterBody RIGHT OUTER JOIN
                tblAquaticSite ON tblWaterBody.WaterBodyID = tblAquaticSite.WaterBodyID INNER JOIN
                tblAquaticActivity ON tblAquaticSite.AquaticSiteID = tblAquaticActivity.AquaticSiteID ON cdAgency.AgencyCd = tblAquaticActivity.AgencyCd
          WHERE ({0})
          GROUP BY tblAquaticActivity.AgencyCd, cdAgency.Agency";
    }
}
