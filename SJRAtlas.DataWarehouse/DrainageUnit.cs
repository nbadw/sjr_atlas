using System;
using System.Collections.Generic;
using System.Text;
using SJRAtlas.Core;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using Castle.ActiveRecord.Queries;

namespace SJRAtlas.DataWarehouse
{
    [ActiveRecord("tblDrainageUnit", Mutable=false)]
    public class DrainageUnit : DataWarehouseARBase<DrainageUnit>, IWatershed
    {
        private string drainageCode;
        private string level1No;
        private string level1Name;
        private string level2No;
        private string level2Name;
        private string level3No;
        private string level3Name;
        private string level4No;
        private string level4Name;
        private string level5No;
        private string level5Name;
        private string level6No;
        private string level6Name;
        private string unitName;
        private string unitType;
        private string borderInd;
        private int streamOrder;
        private float areaHA;
        private float areaPercent;
        private string cgndbKey;
        private string drainsInto;

        public DrainageUnit()
        {
            level1No = "00";
            level2No = "00";
            level3No = "00";
            level4No = "00";
            level5No = "00";
            level6No = "00";
            drainageCode = String.Format("{0}-{1}-{2}-{3}-{4}-{5}",
                                          Level1No, Level2No, Level3No, 
                                          Level4No, Level5No, Level6No);
        }

        #region ActiveRecord Properties

        [Property(Length = 2)]
        public string Level1No
        {
            get { return level1No; }
            set { level1No = value; }
        }

        [Property(Length = 40)]
        public string Level1Name
        {
            get { return level1Name; }
            set { level1Name = value; }
        }

        [Property(Length = 2)]
        public string Level2No
        {
            get { return level2No; }
            set { level2No = value; }
        }

        [Property(Length = 50)]
        public string Level2Name
        {
            get { return level2Name; }
            set { level2Name = value; }
        }

        [Property(Length = 2)]
        public string Level3No
        {
            get { return level3No; }
            set { level3No = value; }
        }

        [Property(Length = 50)]
        public string Level3Name
        {
            get { return level3Name; }
            set { level3Name = value; }
        }

        [Property(Length = 2)]
        [ValidateRegExpAttribute(@"[\d]{2}")]
        public string Level4No
        {
            get { return level4No; }
            set { level4No = value; }
        }

        [Property(Length = 50)]
        public string Level4Name
        {
            get { return level4Name; }
            set { level4Name = value; }
        }

        [Property(Length = 2)]
        public string Level5No
        {
            get { return level5No; }
            set { level5No = value; }
        }

        [Property(Length = 50)]
        public string Level5Name
        {
            get { return level5Name; }
            set { level5Name = value; }
        }

        [Property(Length = 2)]
        public string Level6No
        {
            get { return level6No; }
            set { level6No = value; }
        }

        [Property(Length = 50)]
        public string Level6Name
        {
            get { return level6Name; }
            set { level6Name = value; }
        }

        [Property(Length = 55)]
        public string UnitName
        {
            get { return unitName; }
            set { unitName = value; }
        }

        [Property(Length = 4)]
        public string UnitType
        {
            get { return unitType; }
            set { unitType = value; }
        }

        [Property(Length = 1)]
        public string BorderInd
        {
            get { return borderInd; }
            set { borderInd = value; }
        }

        [Property]
        public int StreamOrder
        {
            get { return streamOrder; }
            set { streamOrder = value; }
        }

        [Property(Column = "Area_ha")]
        public float AreaHA
        {
            get { return areaHA; }
            set { areaHA = value; }
        }

        [Property(Column = "Area_percent")]
        public float AreaPercent
        {
            get { return areaPercent; }
            set { areaPercent = value; }
        }

        [Property("CGNDB_Key", Length=10)]
        public string CgndbKey
        {
            get { return cgndbKey; }
            set { cgndbKey = value; }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, Column = "DrainageCd")]
        public string DrainageCode
        {
            get
            {
                return drainageCode;
            }

            set
            {
                drainageCode = value;
            }
        }

        [Property]
        public string DrainsInto
        {
            get { return drainsInto; }
            set { drainsInto = value; }
        }

        #endregion

        public static DrainageUnit[] FindAllByUnitNameSearch(string name)
        {
            SimpleQuery<DrainageUnit> q = new SimpleQuery<DrainageUnit>("from DrainageUnit du where du.UnitName like ?", "%" + name + "%");
            return q.Execute();
        }

        public static DrainageUnit[] FindAllByDrainageCode(string code)
        {
            string codeWithWildcard = code.Replace('*', '%');
            SimpleQuery<DrainageUnit> q = new SimpleQuery<DrainageUnit>("from DrainageUnit du where du.DrainageCode like ?", codeWithWildcard);
            return q.Execute();
        }

        #region IWatershed Members

        public string Id
        {
            get { return this.DrainageCode; }
        }

        private IPlaceName placename; 

        public IPlaceName PlaceName
        {
            get { return placename; }
            set { placename = value; }
        }

        public string Name
        {
            get { return this.UnitName; }
        }

        public string TributaryOf
        {
            get 
            {
                if (DrainsInto != null)
                    return String.Empty;

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

        #endregion
    }
}
