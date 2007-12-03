using System;
using System.Collections.Generic;
using System.Text;

namespace SJRAtlas.Core
{
    public enum GraphScope
    {
        Stream,
        Site
    }

    public enum GeoReference
    {
        Line,
        Polygon,
        Point,
        MultiplePoints,
        PointAndPolygon
    }

    public interface IDataSet
    {
        object Id { get; }
        string DataType { get; }
        GeoReference GeoReference { get; }
        GraphScope GraphedBy { get; }
        bool SummaryTable { get; }
    }
}
