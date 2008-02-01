using System;
using Castle.ActiveRecord.Queries;
using Castle.ActiveRecord;

namespace SJRAtlas.Models.DataWarehouse
{
    [ActiveRecord("cdAgency", Schema = "dbo")]
    public class Agency : ActiveRecordBase<Agency>
    {
        #region Active Record Properties

        private string agencyCode;

        [PrimaryKey(PrimaryKeyType.Assigned, "AgencyCd")]
        public virtual string AgencyCode
        {
            get { return this.agencyCode; }
            set { this.agencyCode = value; }
        }

        private string name;

        [Property("Agency")]
        public virtual string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        private string type;

        [Property("AgencyType")]
        public virtual string Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        private string dataRulesInd;

        [Property("DataRulesInd")]
        public virtual string DataRulesInd
        {
            get { return this.dataRulesInd; }
            set { this.dataRulesInd = value; }
        }

        #endregion

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
