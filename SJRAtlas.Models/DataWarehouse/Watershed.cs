using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Castle.ActiveRecord;
using NHibernate.Expression;
using SJRAtlas.Models.Atlas;
using Newtonsoft.Json;

namespace SJRAtlas.Models.DataWarehouse
{
    [ActiveRecord("tblDrainageUnit")]
    public class Watershed : ActiveRecordBase<Watershed>
    {
        public Watershed()
        {
            this.waterbodies = new List<WaterBody>();
            this.level1No = "00";
            this.level2No = "00";
            this.level3No = "00";
            this.level4No = "00";
            this.level5No = "00";
            this.level6No = "00";
            this.drainageCode = String.Format("{0}-{1}-{2}-{3}-{4}-{5}",
                                          Level1No, Level2No, Level3No,
                                          Level4No, Level5No, Level6No);
        }

        public string FlowsInto
        {
            get { return DrainsInto != null ? DrainsInto : TributaryOf; }
        }

        [JsonIgnore]
        public string TributaryOf
        {
            get
            {
                if (Level2No != "00")
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(Level1Name);

                    if (Level3No != "00")
                    {
                        sb.Append(" - ");
                        sb.Append(Level2Name);
                    }
                    if (Level4No != "00")
                    {
                        sb.Append(" - ");
                        sb.Append(Level3Name);
                    }
                    if (Level5No != "00")
                    {
                        sb.Append(" - ");
                        sb.Append(Level4Name);
                    }
                    if (Level6No != "00")
                    {
                        sb.Append(" - ");
                        sb.Append(Level5Name);
                    }

                    return sb.ToString();
                }

                return String.Empty;
            }
        }

        private IList<DataSet> datasets;

        [JsonIgnore]
        public IList<DataSet> DataSets
        {
            get
            {
                if (datasets == null)
                {
                    List<DataSet> collectedDataSets = new List<DataSet>();
                    foreach (WaterBody waterbody in WaterBodies)
                    {
                        foreach (DataSet dataset in waterbody.DataSets)
                        {
                            if (!collectedDataSets.Contains(dataset))
                                collectedDataSets.Add(dataset);
                        }
                    }
                    datasets = collectedDataSets;
                }

                return datasets;
            }
        }

        #region ActiveRecord Properties

        private string drainageCode;

        [PrimaryKey("DrainageCd", Generator = PrimaryKeyType.Assigned)]
        public string DrainageCode
        {
            get { return drainageCode; }
            set { drainageCode = value; }
        }

        private string name;

        [Property("UnitName")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
                
        private Place place;

        [JsonIgnore]
        [BelongsTo("CGNDB_Key", Cascade=CascadeEnum.SaveUpdate)]
        public Place Place
        {
            get { return place; }            
            set { place = value; }
        }

        private IList<WaterBody> waterbodies;

        [JsonIgnore]
        [HasMany(typeof(WaterBody), Cascade = ManyRelationCascadeEnum.SaveUpdate)]
        public IList<WaterBody> WaterBodies
        {
            get { return waterbodies; }
            set { waterbodies = value; }
        }

        private string drainsInto;

        [JsonIgnore]
        [Property]
        public string DrainsInto
        {
            get { return drainsInto; }
            set { drainsInto = value; }
        }

        private string unitType;

        [Property(Length = 4)]
        public string UnitType
        {
            get { return unitType; }
            set { unitType = value; }
        }

        private string borderInd;

        [Property(Length = 1)]
        public string BorderInd
        {
            get { return borderInd; }
            set { borderInd = value; }
        }

        private int streamOrder;

        [Property]
        public int StreamOrder
        {
            get { return streamOrder; }
            set { streamOrder = value; }
        }
        private float areaHA;

        [Property(Column = "Area_ha")]
        public float AreaHA
        {
            get { return areaHA; }
            set { areaHA = value; }
        }

        private float areaPercent;

        [Property(Column = "Area_percent")]
        public float AreaPercent
        {
            get { return areaPercent; }
            set { areaPercent = value; }
        }

        private string level1No;

        [Property(Length = 2)]
        public string Level1No
        {
            get { return level1No; }
            set { level1No = value; }
        }

        private string level1Name;

        [Property(Length = 40)]
        public string Level1Name
        {
            get { return level1Name; }
            set { level1Name = value; }
        }

        private string level2No;

        [Property(Length = 2)]
        public string Level2No
        {
            get { return level2No; }
            set { level2No = value; }
        }

        private string level2Name;

        [Property(Length = 50)]
        public string Level2Name
        {
            get { return level2Name; }
            set { level2Name = value; }
        }

        private string level3No;

        [Property(Length = 2)]
        public string Level3No
        {
            get { return level3No; }
            set { level3No = value; }
        }

        private string level3Name;

        [Property(Length = 50)]
        public string Level3Name
        {
            get { return level3Name; }
            set { level3Name = value; }
        }

        private string level4No;

        [Property(Length = 2)]
        public string Level4No
        {
            get { return level4No; }
            set { level4No = value; }
        }

        private string level4Name;

        [Property(Length = 50)]
        public string Level4Name
        {
            get { return level4Name; }
            set { level4Name = value; }
        }

        private string level5No;

        [Property(Length = 2)]
        public string Level5No
        {
            get { return level5No; }
            set { level5No = value; }
        }

        private string level5Name;

        [Property(Length = 50)]
        public string Level5Name
        {
            get { return level5Name; }
            set { level5Name = value; }
        }

        private string level6No;

        [Property(Length = 2)]
        public string Level6No
        {
            get { return level6No; }
            set { level6No = value; }
        }

        private string level6Name;

        [Property(Length = 50)]
        public string Level6Name
        {
            get { return level6Name; }
            set { level6Name = value; }
        }

        #endregion
                
        public virtual bool IsWithinBasin()
        {
            if (DrainageCode == null)
                return false;

            Regex re = new Regex(@"01-[\d]{2}-[\d]{2}-[\d]{2}-[\d]{2}-[\d]{2}");
            return re.IsMatch(DrainageCode);
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

                    maps.Sort(delegate(InteractiveMap im1, InteractiveMap im2) { 
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

        public static bool ExistsForCgndbKey(string cgndbKey)
        {
            DetachedCriteria criteria = CreateCriteriaForCgndbKey(cgndbKey);
            return Watershed.Exists(criteria);
        }

        public static Watershed FindByCgndbKey(string cgndbKey)
        {
            DetachedCriteria criteria = CreateCriteriaForCgndbKey(cgndbKey);
            return Watershed.FindFirst(criteria);
        }

        private static DetachedCriteria CreateCriteriaForCgndbKey(string cgndbKey)
        {
            Place place = new Place();
            place.CgndbKey = cgndbKey;
            DetachedCriteria criteria = DetachedCriteria.For<Watershed>();
            criteria.Add(Expression.Eq("Place", place));
            return criteria;
        }

        public static IList<Watershed> FindAllByQuery(string query)
        {
            if (query == null)
                throw new ArgumentNullException("query");

            DetachedCriteria criteria = DetachedCriteria.For<Watershed>();
            criteria.Add(Expression.Like("Name", query.Trim(), MatchMode.Start));
            return Watershed.FindAll(criteria, new Order[] { Order.Asc("Name") });
        }
    }
}
