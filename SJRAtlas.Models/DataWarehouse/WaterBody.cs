using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using NHibernate.Expression;
using SJRAtlas.Models.Atlas;
using Newtonsoft.Json;

namespace SJRAtlas.Models.DataWarehouse
{
    [ActiveRecord("tblWaterBody")]
    public class WaterBody : ActiveRecordBase<WaterBody>
    {
        #region ActiveRecord Properties

        private int id;

        [PrimaryKey("WaterBodyID", Generator = PrimaryKeyType.Assigned)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private Place place;

        [JsonIgnore]
        [BelongsTo("CGNDB_Key")]
        public Place Place
        {
            get { return place; }
            set { place = value; }
        }

        private Place altPlace;

        [JsonIgnore]
        [BelongsTo("CGNDB_Key_Alt")]
        public Place AltPlace
        {
            get { return altPlace; }
            set { altPlace = value; }
        }

        private Watershed watershed;

        [JsonIgnore]
        [BelongsTo("DrainageCd")]
        public Watershed Watershed
        {
            get { return watershed; }
            set { watershed = value; }
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

        private IList<DataSet> datasets = new List<DataSet>();

        [JsonIgnore]
        [HasAndBelongsToMany(typeof(DataSet), Table = "web_data_sets_waterbodies",
            ColumnKey = "waterbody_id", ColumnRef = "data_set_id")]
        public IList<DataSet> DataSets
        {
            get { return datasets; }
            set { datasets = value; }
        }     

        #endregion        

        public bool IsWithinBasin()
        {
            return Watershed != null ? Watershed.IsWithinBasin() : false;
        }

        private IList<InteractiveMap> interactiveMaps;

        [JsonIgnore]
        public IList<InteractiveMap> RelatedInteractiveMaps
        {
            get
            {
                if (interactiveMaps == null)
                {
                    List<InteractiveMap> maps = IsWithinBasin() ?
                        new List<InteractiveMap>(InteractiveMap.FindAllBasinMaps()) :
                        new List<InteractiveMap>();

                    foreach (DataSet dataset in DataSets)
                    {
                        if (dataset.InteractiveMap != null && !maps.Contains(dataset.InteractiveMap))
                            maps.Add(dataset.InteractiveMap);
                    }

                    maps.Sort(delegate(InteractiveMap im1, InteractiveMap im2)
                    {
                        return im1.Title.CompareTo(im2.Title);
                    });

                    interactiveMaps = maps;
                }

                return interactiveMaps;
            }
        }

        private IList<Publication> publications;

        [JsonIgnore]
        public IList<Publication> RelatedPublications
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

        #region ICoordinateAware Members

        public LatLngCoord GetCoordinate()
        {
            return Place != null ? Place.GetCoordinate() : null;
        }

        #endregion

        public static WaterBody FindByCgndbKeyOrAltCgndbKey(string cgndbKey)
        {
            DetachedCriteria criteria = CreateCgndbOrAltCgndbKeyCriteria(cgndbKey);
            return WaterBody.FindFirst(criteria);
        }

        public static bool ExistsForCgndbKeyOrAltCgndbKey(string cgndbKey)
        {
            DetachedCriteria criteria = CreateCgndbOrAltCgndbKeyCriteria(cgndbKey);
            return WaterBody.Exists(criteria);
        }

        private static DetachedCriteria CreateCgndbOrAltCgndbKeyCriteria(string cgndbKey)
        {
            Place place = new Place();
            place.CgndbKey = cgndbKey;
            DetachedCriteria criteria = DetachedCriteria.For<WaterBody>();
            criteria.Add(Expression.Or(Expression.Eq("Place", place),
                Expression.Eq("AltPlace", place)));
            return criteria;
        }
    }
}
