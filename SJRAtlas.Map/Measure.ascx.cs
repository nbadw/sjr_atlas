using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ESRI.ArcGIS.ADF.Web;
using ESRI.ArcGIS.ADF.Web.UI.WebControls;
using ESRI.ArcGIS.ADF.Web.DataSources;
using ESRI.ArcGIS.ADF.Web.Geometry;

public enum MapUnit
{
    Resource_Default,Degrees, Feet, Meters
}

public enum MeasureUnit
{
    Feet, Kilometers, Meters, Miles
}

public enum AreaUnit
{
    Acres, Sq_Feet, Sq_Kilometers, Sq_Meters, Sq_Miles
}

public partial class Measure : System.Web.UI.UserControl, ICallbackEventHandler
{
    MapResourceManager m_resourceManger;
    IMapFunctionality m_mapFunctionality;
    private Page m_page;
    public string m_callbackInvocation = "";
    private string m_mapBuddyId = "Map1";
    public string m_id;
    private Map m_map;
    private MapUnit m_FallbackMapUnit = MapUnit.Degrees; // fallback value used if resource value is not available
    private MapUnit m_mapUnits;
    private MapUnit m_startMapUnits = MapUnit.Degrees;
    public MeasureUnit m_measureUnits = MeasureUnit.Miles;
    public AreaUnit m_areaUnits = AreaUnit.Sq_Miles;
    private double m_numberDecimals = 4;

    protected void Page_Load(object sender, EventArgs e)
    {
        m_id = this.ClientID;
        m_page = this.Page;
        // find the map control
        if (m_mapBuddyId == null || m_mapBuddyId.Length == 0) m_mapBuddyId = "Map1";
        m_map = m_page.FindControl(m_mapBuddyId) as Map;
        // find the map resource manager
        m_resourceManger = m_page.FindControl(m_map.MapResourceManager) as MapResourceManager;
        m_callbackInvocation = m_page.ClientScript.GetCallbackEventReference(this, "argument", "processCallbackResult", "context", true);
    
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        GetMeasureResource();
    }


    #region Action Methods

    private void GetMeasureResource()
    {
        // use the primary resouce, if defined
        string primeResource = m_map.PrimaryMapResource;
        IEnumerable mapResources = m_resourceManger.GetResources();
        IEnumerator resEnum = mapResources.GetEnumerator();
        resEnum.MoveNext();
        IGISResource resource = (primeResource != null && primeResource.Length > 0) ? m_resourceManger.GetResource(primeResource) : resEnum.Current as IGISResource;
        if (resource != null) m_mapFunctionality = (IMapFunctionality)resource.CreateFunctionality(typeof(IMapFunctionality), "mapFunctionality");
    }

    public string ProcessMeasureRequest(NameValueCollection queryString)
    {
        if (m_mapFunctionality == null) GetMeasureResource();
        object o = Session["MeasureMapUnits"];
        if (o != null)
            m_mapUnits = (MapUnit)Enum.Parse(typeof(MapUnit), o.ToString());
        else if (m_startMapUnits == MapUnit.Resource_Default)
            m_mapUnits = GetResourceDefaultMapUnit();
        else
            m_mapUnits = m_startMapUnits;
        
        string eventArg = queryString["EventArg"].ToLower();
        string vectorAction = queryString["VectorAction"].ToLower();
        string[] coordPairs, xys;
        string coordString = queryString["coords"];
        if (coordString == null && coordString.Length == 0)
            coordString = "";
        coordPairs = coordString.Split(char.Parse("|"));
        string mapUnitString = queryString["MapUnits"];
        if (mapUnitString != null && mapUnitString.Length > 0)
            m_mapUnits = (MapUnit)Enum.Parse(typeof(MapUnit), mapUnitString);
        Session["MeasureMapUnits"] = m_mapUnits;
        string measureUnitString = queryString["MeasureUnits"];
        if (measureUnitString != null && measureUnitString.Length > 0)
            m_measureUnits = (MeasureUnit)Enum.Parse(typeof(MeasureUnit),measureUnitString);
        string areaUnitstring = queryString["AreaUnits"];
        if (areaUnitstring != null && areaUnitstring.Length > 0)
            m_areaUnits = (AreaUnit)Enum.Parse(typeof(AreaUnit),areaUnitstring);
        string response = "";
        PointCollection points = new PointCollection();
        PointCollection dPoints = new PointCollection();
        ArrayList distances = new ArrayList();
        double totalDistance = 0;
        double segmentDistance = 0;
        double area = 0;
        double perimeter = 0;
        double roundFactor = Math.Pow(10, m_numberDecimals);
        double xD, yD, tempDist, tempDist2, tempArea, x1, x2, y1, y2;
        TransformationParams transformationParameters = m_map.GetTransformationParams(TransformationDirection.ToMap);
        if (vectorAction == "addpoint")
        {
            if (coordPairs != null && coordPairs.Length > 1)
            {
                for (int i = 0; i < coordPairs.Length; i++)
                {
                    xys = coordPairs[i].Split(char.Parse(":"));
                    points.Add(Point.ToMapPoint(Convert.ToInt32(xys[0]), Convert.ToInt32(xys[1]), transformationParameters));
                    if (i > 0)
                    {
                        if (m_mapUnits == MapUnit.Degrees)
                        {
                            // use great circle formula
                            tempDist = DegreeToFeetDistance(points[i - 1].X, points[i - 1].Y, points[i].X, points[i].Y);
                            y1 = DegreeToFeetDistance(points[i].X, points[i].Y, points[i].X, 0);
                            x1 = DegreeToFeetDistance(points[i].X, points[i].Y, 0, points[i].Y);
                            dPoints.Add(new Point(x1, y1));
                            segmentDistance = ConvertUnits(tempDist, MapUnit.Feet, m_measureUnits);
                        }
                        else
                        {
                            // get third side of triangle for distance
                            xD = Math.Abs(points[i].X - points[i - 1].X);
                            yD = Math.Abs(points[i].Y - points[i - 1].Y);
                            tempDist = Math.Sqrt(Math.Pow(xD, 2) + Math.Pow(yD, 2));
                            segmentDistance = ConvertUnits(tempDist, m_mapUnits, m_measureUnits);

                        }

                        distances.Add(segmentDistance);
                        totalDistance += segmentDistance;
                        segmentDistance = Math.Round(segmentDistance * roundFactor) / roundFactor;
                        totalDistance = Math.Round(totalDistance * roundFactor) / roundFactor;
                    }
                    else
                    {
                        if (m_mapUnits == MapUnit.Degrees)
                        {
                            y1 = DegreeToFeetDistance(points[i].X, points[i].Y, points[i].X, 0);
                            x1 = DegreeToFeetDistance(points[i].X, points[i].Y, 0, points[i].Y);
                            dPoints.Add(new Point(x1, y1));
                        }
                    }
                }
            }
            if (eventArg == "polygon")
            {
                if (points.Count > 2)
                {
                    if (m_mapUnits == MapUnit.Degrees)
                    {
                        tempDist = DegreeToFeetDistance(points[points.Count - 1].X, points[points.Count - 1].Y, points[0].X, points[0].Y);
                        tempDist2 = ConvertUnits(tempDist, MapUnit.Feet, m_measureUnits);
                        distances.Add(tempDist2);
                        dPoints.Add(dPoints[0]);
                    }
                    else
                    {
                        xD = Math.Abs(points[points.Count - 1].X - points[0].X);
                        yD = Math.Abs(points[points.Count - 1].Y - points[0].Y);
                        tempDist = Math.Sqrt(Math.Pow(xD, 2) + Math.Pow(yD, 2));
                        tempDist2 = ConvertUnits(tempDist, m_mapUnits, m_measureUnits);
                        distances.Add(tempDist2);
                    }
                    points.Add(points[0]);
                    perimeter = totalDistance + tempDist2;
                    // add area calculation
                    tempArea = 0;
                    MapUnit mUnits = m_mapUnits;
                    for (int j = 0; j < points.Count - 1; j++)
                    {
                        if (m_mapUnits == MapUnit.Degrees)
                        {
                            x1 = Convert.ToDouble(dPoints[j].X);
                            x2 = Convert.ToDouble(dPoints[j + 1].X);
                            y1 = Convert.ToDouble(dPoints[j].Y);
                            y2 = Convert.ToDouble(dPoints[j + 1].Y);
                            mUnits = MapUnit.Feet;
                        }
                        else
                        {
                            x1 = Convert.ToDouble(points[j].X);
                            x2 = Convert.ToDouble(points[j + 1].X);
                            y1 = Convert.ToDouble(points[j].Y);
                            y2 = Convert.ToDouble(points[j + 1].Y);
                        }
                        //tempArea += tempArea + (x1 + x2) * (y1 - y2);
                        double xDiff = x2 - x1;
                        double yDiff = y2 - y1;
                        tempArea += x1 * yDiff - y1 * xDiff;
                    }
                    tempArea = Math.Abs(tempArea) / 2;
                    area = ConvertAreaUnits(tempArea, mUnits, m_areaUnits);
                    perimeter = Math.Round(perimeter * roundFactor) / roundFactor;
                    area = Math.Round(area * roundFactor) / roundFactor;
                    response = String.Format("<table cellspacing='0'  ><tr><td>Perimeter: </td><td align='right'>{0}</td><td>{1}</td></tr><tr><td>Area:</td><td  align='right'>{2}</td><td>{3}</td></tr></table>", perimeter, WriteMeasureUnitDropdown(), area, WriteAreaUnitDropdown());

                }
                else
                    response = String.Format("<table cellspacing='0' ><tr><td>Perimeter: </td><td align='right'> 0</td><td>{0}</td></tr><tr><td>Area:</td><td align='right'>0 </td><td>{1}</td></tr></table>", WriteMeasureUnitDropdown(), WriteAreaUnitDropdown());
            }
            else
                response = String.Format("<table cellspacing='0' ><tr><td>Segment: </td><td align='right'>{0} </td><td>{1}</td></tr><tr><td>Total Length:</td><td align='right'>{2} </td><td>{3}</td></tr></table>", segmentDistance, m_measureUnits.ToString(), totalDistance, WriteMeasureUnitDropdown());
        }
        else if (vectorAction == "coordinates")
        {
            xys = coordPairs[0].Split(char.Parse(":"));
            Point coordPoint = Point.ToMapPoint(Convert.ToInt32(xys[0]), Convert.ToInt32(xys[1]), transformationParameters);

            response = String.Format("<table cellspacing='0' ><tr><td>X Coordinate:</td><td align='right'>{0}</td></tr><tr><td>Y Coordinate:</td><td align='right'>{1}</td></tr></table>", (Math.Round(coordPoint.X * roundFactor) / roundFactor).ToString(), (Math.Round(coordPoint.Y * roundFactor) / roundFactor).ToString());
        }
        else if (vectorAction == "finish")
        {
            response = "Shape complete";
        }
        return String.Format("measure:::{0}:::{1}:::{2}", m_id, vectorAction, response);
    }

    public string CheckFormMeasureUnits(string unit)
    {
        string response = "";
        if (unit == m_measureUnits.ToString())
            response = "selected=\"selected\"";
        return response;
    }

    public string CheckFormAreaUnits(string unit)
    {
        string response = "";
        if (unit == m_areaUnits.ToString())
            response = "selected=\"selected\"";
        return response;
    }

    public string WriteMeasureUnitDropdown()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<select id=\"MeasureUnits2\" onchange=\"changeMeasureUnits()\" style=\"font: normal 7pt Verdana; width: 100px;\">");
        Array mArray = Enum.GetValues(typeof(MeasureUnit));
        foreach (MeasureUnit mu in mArray)
        {
            sb.AppendFormat("<option value=\"{0}\" {1}>{0}</option>", mu.ToString(), CheckFormMeasureUnits(mu.ToString()));
            
        }
        sb.Append("</select>");

        return sb.ToString();
    }

    public string WriteAreaUnitDropdown()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<select id=\"AreaUnits2\" onchange=\"changeAreaUnits()\" style=\"font: normal 7pt Verdana; width: 100px;\">");
        Array aArray = Enum.GetValues(typeof(AreaUnit));
        foreach (AreaUnit au in aArray)
        {
            sb.AppendFormat("<option value=\"{0}\" {1}>{0}</option>", au.ToString(), CheckFormAreaUnits(au.ToString()));

        }
        sb.Append("</select>");

        return sb.ToString();
    }
    
#endregion

    #region Conversion Methods

    public double ConvertUnits(double distance, MapUnit fromUnits, MeasureUnit toUnits)
    {
        double mDistance = distance;
        if (fromUnits == MapUnit.Feet)
        {
            if (toUnits == MeasureUnit.Miles)
            {
                mDistance = distance / 5280;
            }
            else if (toUnits == MeasureUnit.Meters)
            {
                mDistance = distance * 0.304800609601;
            }
            else if (toUnits == MeasureUnit.Kilometers)
            {
                mDistance = distance * 0.0003048;
            }
        }
        else
        {
            if (toUnits == MeasureUnit.Miles)
            {
                mDistance = distance * 0.0006213700922;
            }
            else if (toUnits == MeasureUnit.Feet)
            {
                mDistance = distance * 3.280839895;
            }
            else if (toUnits == MeasureUnit.Kilometers)
            {
                mDistance = distance / 1000;
            }
        }
        return mDistance;
    }

    private double ConvertAreaUnits(double area, MapUnit baseUnits, AreaUnit toUnits)
    {
        double mArea = area;
        if (baseUnits == MapUnit.Feet)
        {
            if (toUnits == AreaUnit.Acres)
                mArea = area * 0.000022956;
            else if (toUnits == AreaUnit.Sq_Meters)
                mArea = area * 0.09290304;
            else if (toUnits == AreaUnit.Sq_Miles)
                mArea = area * 0.00000003587;
            else if (toUnits == AreaUnit.Sq_Kilometers)
                mArea = area * 0.09290304 / 1000000;
        }
        else if (baseUnits == MapUnit.Meters)
        {
            if (toUnits == AreaUnit.Acres)
                mArea = area * 0.0002471054;
            else if (toUnits == AreaUnit.Sq_Miles)
                mArea = area * 0.0000003861003;
            else if (toUnits == AreaUnit.Sq_Kilometers)
                mArea = area * 1.0e-6;
            else if (toUnits == AreaUnit.Sq_Feet)
                mArea = area * 10.76391042;
        }

        return mArea;
    }

    private double DegreeToFeetDistance(double x1, double y1, double x2, double y2)
    {
        // use great circle formula
        double Lat1 = DegToRad(y1);
        double Lat2 = DegToRad(y2);
        double Lon1 = DegToRad(x1);
        double Lon2 = DegToRad(x2);
        double LonDist = Lon1 - Lon2;
        double LatDist = Lat1 - Lat2;
        double x = Math.Pow(Math.Sin(LatDist / 2), 2) + Math.Cos(Lat1) * Math.Cos(Lat2) * Math.Pow(Math.Sin(LonDist / 2), 2);
        x = 2 * Math.Asin(Math.Min(1, Math.Sqrt(x)));
        x = (3963 - 13 * Math.Sin((Lat1 + Lat2) / 2)) * x;
        // in miles... convert to feet and use that as base
        return (x * 5280);
    }

    private double DegToRad(double degrees)
    {
        return Convert.ToDouble(degrees * Math.PI / 180);
    }

    private MapUnit GetResourceDefaultMapUnit()
    {
        MapUnit mUnit = MapUnit.Degrees;
        try
        {
            Units mu = m_mapFunctionality.Units;
            if (mu == Units.DecimalDegrees)
                mUnit = MapUnit.Degrees;
            else if (mu == Units.Feet)
                mUnit = MapUnit.Feet;
            else if (mu == Units.Meters)
                mUnit = MapUnit.Meters;
        }
        catch
        {
            // cannot get units from resource... default to fallback value set in declaration
            mUnit = m_FallbackMapUnit;
        }
        return mUnit;

    }
    #endregion

    #region ICallbackEventHandler Members

    private string _callbackArg;

    //public string GetCallbackResult()
    //{
    //    //;
    //}


    #endregion

    #region ICallbackEventHandler Members

    string ICallbackEventHandler.GetCallbackResult()
    {
        return RaiseCallbackEvent(_callbackArg);
    }

    void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
    {
        _callbackArg = eventArgument;
    }

    public virtual string RaiseCallbackEvent(string requestString)
    {
        // break out the responseString into a querystring
        Array keyValuePairs = requestString.Split("&".ToCharArray());
        NameValueCollection m_queryString = new NameValueCollection();
        m_page = this.Page;
        Map map = m_page.FindControl(this.m_mapBuddyId) as Map;
        string[] keyValue;
        string response = "";
        if (keyValuePairs.Length > 0)
        {
            for (int i = 0; i < keyValuePairs.Length; i++)
            {
                keyValue = keyValuePairs.GetValue(i).ToString().Split("=".ToCharArray());
                m_queryString.Add(keyValue[0], keyValue[1]);
            }
        }
        else
        {
            keyValue = requestString.Split("=".ToCharArray());
            if (keyValue.Length > 0)
                m_queryString.Add(keyValue[0], keyValue[1]);
        }
        // isolate control type and mode
        string controlType = m_queryString["ControlType"];
        string eventArg = m_queryString["EventArg"];
        if (controlType == null) controlType = "Map";


        switch (controlType)
        {
            case "Map":
                // request is for the map control
                string vectorMode = m_queryString["VectorMode"];
                if (vectorMode != null && vectorMode.ToLower() == "measure")
                {
                    response = ProcessMeasureRequest(m_queryString);
                }
                break;
            default:
                //
                break;
        }
        return response;
    }

    #endregion


    #region Properties

    public string Id
    {
        get { return m_id; }
        set { m_id = value; }
    }

    private string ClientCallbackInvocation
    {
        get { return m_callbackInvocation; }
        set { m_callbackInvocation = value; }
    }

    private MapResourceManager MapResourceManager
    {
        get { return m_resourceManger; }
        set { m_resourceManger = value; }
    }

    /// <summary>
    /// Id of Buddy MapControl
    /// </summary>
    public string MapBuddyId
    {
        get { return m_mapBuddyId; }
        set { m_mapBuddyId = value; }
    }


    /// <summary>
    /// Unit used resource. Resource_Default will return value from resource, if available. Other values will force calculations to use that unit.
    /// </summary>
    public MapUnit MapUnits
    {
        get { return m_startMapUnits; }
        set { m_startMapUnits = value; }
    }

    /// <summary>
    ///  Unit used in display of linear measurements.
    /// </summary>
    public MeasureUnit MeasureUnits
    {
        get { return m_measureUnits; }
        set { m_measureUnits = value; }
    }

    /// <summary>
    ///  Area Units - Unit used in display of area measurements.
    /// </summary>
    public AreaUnit AreaUnits
    {
        get { return m_areaUnits; }
        set { m_areaUnits = value; }
    }

    // Number of Decimals - Number of decimal digits displayed in measurements.
    public double NumberDecimals
    {
        get { return m_numberDecimals; }
        set { m_numberDecimals = value; }
    }


    public override bool Visible
    {
        get { return base.Visible; }
    }

    public override bool EnableTheming
    {
        get
        {
            return base.EnableTheming;
        }
    }

    public override bool EnableViewState
    {
        get
        {
            return base.EnableViewState;
        }
    }

    #endregion

}
