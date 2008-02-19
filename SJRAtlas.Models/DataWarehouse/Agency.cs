using System;
using Castle.ActiveRecord;
using Newtonsoft.Json;
using System.Collections.Generic;
using NHibernate;
using Castle.ActiveRecord.Queries;

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

        [JsonIgnore]
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
            SimpleQuery<Agency> q = new SimpleQuery<Agency>(BuildAgencyShortListQuery(where), drainageCode);
            return q.Execute();
        }

        public static Agency[] FindAllByWaterBodyId(int waterbodyId)
        {
            string where = "tblWaterBody.WaterBodyID = ?";
            SimpleQuery<Agency> q = new SimpleQuery<Agency>(BuildAgencyShortListQuery(where), waterbodyId);
            return q.Execute();
        }

        public static Agency[] FindAllByAquaticSiteId(int aquaticSiteId)
        {
            string where = "tblAquaticActivity.AquaticSiteID = ?";
            SimpleQuery<Agency> q = new SimpleQuery<Agency>(BuildAgencyShortListQuery(where), aquaticSiteId);
            return q.Execute();
        }

        public static IList<Agency> FindAllOrByShortListQuery(string drainageCode, int waterbodyId, int aquaticSiteId)
        {
            Dictionary<object, string> criteria = new Dictionary<object, string>(3);
            if (!String.IsNullOrEmpty(drainageCode))
                criteria.Add(drainageCode, "tblWaterBody.DrainageCd LIKE ?");
            
            if (waterbodyId > 0)
                criteria.Add(waterbodyId, "tblWaterBody.WaterBodyID = ?");
            
            if (aquaticSiteId > 0)
                criteria.Add(aquaticSiteId, "tblAquaticActivity.AquaticSiteID = ?");
            
            if (criteria.Count == 0)
                return Agency.FindAll();

            int pos = 0;
            string where = "";
            List<object> parameters = new List<object>();
            foreach(KeyValuePair<object, string> entry in  criteria)
            {
                parameters.Add(entry.Key);
                where += entry.Value;
                if (pos < criteria.Count - 1)
                    where += " OR ";
                pos++;
            }

            ISession session = ActiveRecordMediator.GetSessionFactoryHolder().CreateSession(typeof(Agency));
            try
            {
                ISQLQuery query = session.CreateSQLQuery(BuildAgencyShortListQuery(where)).AddEntity(typeof(Agency));
                for (int i = 0; i < parameters.Count; i++)
                {
                    query.SetParameter(i, parameters[i]);
                }
                return query.List<Agency>();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ActiveRecordMediator.GetSessionFactoryHolder().ReleaseSession(session);
            }
        }

        private static string BuildAgencyShortListQuery(string where)
        {
            return String.Format(shortListQuery, where);
        }

        public static readonly string shortListQuery =
        @"SELECT DISTINCT cdAgency.*
          FROM cdAgency INNER JOIN
                tblWaterBody RIGHT OUTER JOIN
                tblAquaticSite ON tblWaterBody.WaterBodyID = tblAquaticSite.WaterBodyID INNER JOIN
                tblAquaticActivity ON tblAquaticSite.AquaticSiteID = tblAquaticActivity.AquaticSiteID ON cdAgency.AgencyCd = tblAquaticActivity.AgencyCd
          WHERE ({0})";
    }
}
