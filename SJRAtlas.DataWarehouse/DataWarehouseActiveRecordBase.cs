using Castle.ActiveRecord;
using Castle.Core.Logging;

namespace SJRAtlas.DataWarehouse
{
    [ActiveRecord]
    public abstract class DataWarehouseARBase<T> : ActiveRecordBase<T>
    {
        //private static ILogger logger;

        //public static ILogger Logger
        //{
        //    get { return logger; }
        //    set { logger = value; }
        //}
    }
}
