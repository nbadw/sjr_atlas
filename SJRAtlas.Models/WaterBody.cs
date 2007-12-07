using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;

namespace SJRAtlas.Models
{
    [ActiveRecord("tblWaterBody")]
    public class WaterBody : ActiveRecordBase<WaterBody>, IPlace, ICoordinateAware
    {
        public WaterBody()
            : this(new Place(), new Watershed())
        {

        }

        public WaterBody(Place place, Watershed watershed)
        {
            if (place == null)
                throw new ArgumentNullException("place");
            if (watershed == null)
                throw new ArgumentNullException("watershed");

            this.watershed = watershed;
            this.watershed.Place = place;
        }

        #region ActiveRecord Properties

        private int id;

        [PrimaryKey("WaterBodyID", Generator = PrimaryKeyType.Assigned)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [BelongsTo("CGNDB_Key")]
        public Place Place
        {
            get { return Watershed.Place; }
            set { Watershed.Place = value; }
        }

        private string altCgndbKey;

        [Property("CGNDB_Key_Alt", Length = 10)]
        public string AltCgndbKey
        {
            get { return altCgndbKey; }
            set { altCgndbKey = value; }
        }

        private Watershed watershed;

        [BelongsTo("DrainageCd")]
        public Watershed Watershed
        {
            get { return watershed; }
            set 
            {
                if (value == null)
                    throw new ArgumentNullException("watershed");

                watershed = value; 
            }
        }

        private string type;

        [Property("WaterBodyTypeCd", Length = 4)]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        private string name;

        [Property("WaterBodyName", Length = 55)]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string abbreviation;

        [Property("WaterBodyName_Abrev", Length = 40)]
        public string Abbreviation
        {
            get { return abbreviation; }
            set { abbreviation = value; }
        }

        private string altName;

        [Property("WaterBodyName_Alt", Length = 40)]
        public string AltName
        {
            get { return altName; }
            set { altName = value; }
        }

        private int complexId;

        [Property("WaterBodyComplexID")]
        public int ComplexId
        {
            get { return complexId; }
            set { complexId = value; }
        }

        private string surveyedInd;

        [Property("Surveyed_Ind", Length = 1)]
        public string SurveyedInd
        {
            get { return surveyedInd; }
            set { surveyedInd = value; }
        }

        private int flowsIntoWaterBodyId;

        [Property("FlowsIntoWaterBodyID")]
        public int FlowsIntoWaterBodyId
        {
            get { return flowsIntoWaterBodyId; }
            set { flowsIntoWaterBodyId = value; }
        }

        private string flowsIntoWaterBodyName;

        [Property("FlowsIntoWaterBodyName", Length = 40)]
        public string FlowsIntoWaterBodyName
        {
            get { return flowsIntoWaterBodyName; }
            set { flowsIntoWaterBodyName = value; }
        }

        #endregion

        private DataSet[] datasets;

        public virtual DataSet[] DataSets
        {
            get 
            {
                if (datasets == null)
                {
                    datasets = new DataSet[DataSetList.Count];
                    DataSetList.CopyTo(datasets, 0);
                }

                return datasets;
            }
        }

        private IList<DataSet> datasetList = new List<DataSet>();

        [HasAndBelongsToMany(typeof(DataSet), Table = "auxWaterBody_DataType_XRef",
            ColumnKey = "WaterBodyID", ColumnRef = "DataTypeID")]
        public IList<DataSet> DataSetList
        {
            get { return datasetList; }
            set { datasetList = value; }
        }

        #region IPlace Members

        public string CgndbKey
        {
            get { return Place.CgndbKey; }
            set { Place.CgndbKey = value; }
        }

        public string ConciseTerm
        {
            get { return Place.ConciseTerm; }
            set { Place.ConciseTerm = value; }
        }

        public string ConciseType
        {
            get { return Place.ConciseType; }
            set { Place.ConciseType = value; }
        }

        public string CoordAccM
        {
            get { return Place.CoordAccM; }
            set { Place.CoordAccM = value; }
        }

        public string County
        {
            get { return Place.County; }
            set { Place.County = value; }
        }

        public string Datum
        {
            get { return Place.Datum; }
            set { Place.Datum = value; }
        }

        public string FeatureId
        {
            get { return Place.FeatureId; }
            set { Place.FeatureId = value; }
        }

        public string GenericTerm
        {
            get { return Place.GenericTerm; }
            set { Place.GenericTerm = value; }
        }

        public bool IsWithinBasin()
        {
            return Watershed.IsWithinBasin();
        }

        public double Latitude
        {
            get { return Place.Latitude; }
            set { Place.Latitude = value; }
        }

        public double Longitude
        {
            get { return Place.Longitude; }
            set { Place.Longitude = value; }
        }

        public string NameStatus
        {
            get { return Place.NameStatus; }
            set { Place.NameStatus = value; }
        }

        public string NtsMap
        {
            get { return Place.NtsMap; }
            set { Place.NtsMap = value; }
        }

        public string Region
        {
            get { return Place.Region; }
            set { Place.Region = value; }
        }

        private IList<InteractiveMap> interactiveMaps;

        public IList<InteractiveMap> RelatedInteractiveMaps
        {
            get
            {
                if (interactiveMaps == null)
                {
                    interactiveMaps = InteractiveMap.FindAllByQuery(String.Format("%{0}%", Name));
                }

                return interactiveMaps;
            }
        }

        private IList<IPublication> publications;

        public IList<IPublication> RelatedPublications
        {
            get
            {
                if (publications == null)
                {
                    publications = Publication.FindAllByQuery(String.Format("%{0}%", Name));
                }

                return publications;
            }
        }

        #endregion

        

        #region ICoordinateAware Members

        public LatLngCoord GetCoordinate()
        {
            return Place.GetCoordinate();
        }

        #endregion
    }
}
