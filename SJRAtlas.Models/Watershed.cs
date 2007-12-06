using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Castle.ActiveRecord;
using SJRAtlas.Models.Finders;

namespace SJRAtlas.Models
{
    [ActiveRecord("tblDrainageUnit")]
    public class Watershed : ActiveRecordBase<Watershed>, IPlace, IWatershed, ICoordinateAware
    {
        public Watershed() : this(new NullAtlasRepository(), new Place())
        {
        }

        public Watershed(IAtlasRepository repository) : this(repository, new Place())
        {
        }

        public Watershed(IAtlasRepository repository, Place place)
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            if (place == null)
                throw new ArgumentNullException("place");

            this.place = place;
            this.place.Repository = repository;
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

        public IAtlasRepository Repository
        {
            get { return Place.Repository; }
            set { Place.Repository = value; }
        }

        public string FlowsInto
        {
            get
            {
                return DrainsInto != null ? DrainsInto : TributaryOf;
            }
        }
        
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

        private IList<WaterBody> waterbodies;

        [HasMany(typeof(WaterBody), Cascade = ManyRelationCascadeEnum.SaveUpdate)]
        public IList<WaterBody> WaterBodies
        {
            get { return waterbodies; }
            set { waterbodies = value; }
        }

        private DataSet[] datasets;

        public DataSet[] DataSets
        {
            get
            {
                if (datasets == null)
                {
                    List<DataSet> collectedDataSets = new List<DataSet>();
                    foreach (WaterBody waterbody in WaterBodies)
                    {
                        collectedDataSets.AddRange(waterbody.DataSets);
                    }
                    datasets = collectedDataSets.ToArray();
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

        [BelongsTo("CGNDB_Key", Cascade=CascadeEnum.SaveUpdate)]
        public Place Place
        {
            get { return place; }            
            set             
            { 
                if(value == null)
                    throw new ArgumentNullException("place");

                place = value; 
            }
        }

        private string drainsInto;

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

        public virtual bool IsWithinBasin()
        {
            if (DrainageCode == null)
                return false;

            Regex re = new Regex(@"01-[\d]{2}-[\d]{2}-[\d]{2}-[\d]{2}-[\d]{2}");
            return re.IsMatch(DrainageCode);
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

        private InteractiveMap[] interactiveMaps;

        public InteractiveMap[] RelatedInteractiveMaps
        {
            get
            {
                if (interactiveMaps == null)
                {
                    string query = String.Format("%{0}%", Name);
                    InteractiveMapFinder finder = Repository.GetFinder<InteractiveMapFinder>();
                    interactiveMaps = finder.FindAllByQuery(query);
                }

                if (interactiveMaps == null)
                    interactiveMaps = new InteractiveMap[0];

                return interactiveMaps;
            }
        }

        private IPublication[] publications;

        public IPublication[] RelatedPublications
        {
            get
            {
                if (publications == null)
                {
                    string query = String.Format("%{0}%", Name);
                    IPublicationFinder finder = Repository.GetFinder<IPublicationFinder>();
                    publications = finder.FindAllByQuery(query);
                }

                if (publications == null)
                    publications = new IPublication[0];

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
