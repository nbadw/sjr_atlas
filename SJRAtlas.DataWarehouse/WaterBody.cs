using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using SJRAtlas.Core;

namespace SJRAtlas.DataWarehouse
{
    [ActiveRecord("tblWaterBody", Mutable=false)]
    public class WaterBody : DataWarehouseARBase<WaterBody>, IWaterBody
    {
        private int id;
        private string cgndbKey;
        private string altCgndbKey;
        private string drainageCode;
        private string type;
        private string name;
        private string abbreviation;
        private string altName;
        private int complexId;
        private string surveyedInd;
        private float flowsIntoWaterBodyId;
        private string flowsIntoWaterBodyName;
        private DateTime created;
        private DateTime modified;
        private IPlaceName placename;
        private IWatershed watershed;

        public WaterBody() : this(null, null)
        {

        }

        public WaterBody(IPlaceName placename, IWatershed watershed)
        {
            this.placename = placename;
            this.watershed = watershed;
            created = DateTime.Now;
            modified = DateTime.Now;
        }

        public IPlaceName Placename
        {
            get { return placename; }
            set { placename = value; }
        }

        public IWatershed Watershed
        {
            get { return watershed; }
            set { watershed = value; }
        }

        #region ActiveRecord Properties
        [PrimaryKey("WaterBodyID")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [Property("CGNDB_Key", Length=10)]
        public string CgndbKey
        {
            get { return cgndbKey; }
            set { cgndbKey = value; }
        }
        
        [Property("CGNDB_Key_Alt", Length=10)]
        public string AltCgndbKey
        {
            get { return altCgndbKey; }
            set { altCgndbKey = value; }
        }
        
        [Property("DrainageCd", Length=17)]
        public string DrainageCode
        {
            get { return drainageCode; }
            set { drainageCode = value; }
        }

        [Property("WaterBodyTypeCd", Length=4)]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        
        [Property("WaterBodyName", Length=55)]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        
        [Property("WaterBodyName_Abrev", Length=40)]
        public string Abbreviation
        {
            get { return abbreviation; }
            set { abbreviation = value; }
        }
        
        [Property("WaterBodyName_Alt", Length=40)]
        public string AltName
        {
            get { return altName; }
            set { altName = value; }
        }
        
        [Property("WaterBodyComplexID")]
        public int ComplexId
        {
            get { return complexId; }
            set { complexId = value; }
        }
        
        [Property("Surveyed_Ind", Length=1)]
        public string SurveryInd
        {
            get { return surveyedInd; }
            set { surveyedInd = value; }
        }
        
        [Property("FlowsIntoWaterBodyID")]
        public float FlowsIntoWaterBodyId
        {
            get { return flowsIntoWaterBodyId; }
            set { flowsIntoWaterBodyId = value; }
        }
        
        [Property("FlowsIntoWaterBodyName", Length=40)]
        public string FlowsIntoWaterBodyName
        {
            get { return flowsIntoWaterBodyName; }
            set { flowsIntoWaterBodyName = value; }
        }
        
        [Property("DateEntered")]
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }
        
        [Property("DateModified")]
        public DateTime Modified
        {
            get { return modified; }
            set { modified = value; }
        }
        #endregion

        #region IPlaceName Members

        public string Region
        {
            get { return (Placename != null ? Placename.Region : "NB"); }
        }

        public string County
        {
            get { return (Placename != null ? Placename.County : null); }
        }

        public string Latitude
        {
            get { return (Placename != null ? Placename.Latitude : null); }
        }

        public string Longitude
        {
            get { return (Placename != null ? Placename.Longitude : null); }
        }

        public double LatDec
        {
            get { return (Placename != null ? Placename.LatDec : double.NaN); }
        }

        public double LongDec
        {
            get { return (Placename != null ? Placename.LongDec : double.NaN); }
        }

        public string StatusTerm
        {
            get { return (Placename != null ? Placename.StatusTerm : null); }
        }

        public string ConciseTerm
        {
            get { return (Placename != null ? Placename.ConciseTerm : null); }
        }

        public string GenericTerm
        {
            get { return (Placename != null ? Placename.GenericTerm : null); }
        }

        #endregion

        #region IWatershed Members

        public string TributaryOf
        {
            get { return (Watershed != null ? Watershed.TributaryOf : null); }
        }

        public string DrainsInto
        {
            get { return (Watershed != null ? Watershed.DrainsInto : FlowsIntoWaterBodyName); }
        }

        #endregion
    }
}
