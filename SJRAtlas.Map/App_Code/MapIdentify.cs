
using System;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
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
using ESRI.ArcGIS.ADF.Web.Display.Graphics;
using System.Collections.Generic;

/// <summary>
/// Summary description for Identify
/// </summary>
public class MapIdentify 
{
    private Page m_page;
    private Map m_map;
    private DataSet m_dataset;
    private IdentifyOption m_idOption = IdentifyOption.VisibleLayers;
    private string m_callbackInvocation = "";
    private string m_filePath = "";
    private TaskResults m_resultsDisplay = null;
    private int m_numberDecimals = 3;

    public int m_IdentifyTolerance = 5; // tolerance used in identify request... may need to be adjusted to a specific resource type


    #region Identify Constructors

    public MapIdentify()
    {
    }

    public MapIdentify(Map map)
    {
        if (map != null)
        {
            m_map = map;
            SetupIdentify();
        }
    }

    public MapIdentify(Map map, string filePath)
    {
        m_map = map;
        m_filePath = filePath;
        SetupIdentify();
        
    }
    #endregion

    #region Methods

    public void SetupIdentify()
    {
        m_page = m_map.Page;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        m_callbackInvocation = m_page.ClientScript.GetCallbackEventReference(m_page, "message", "processCallbackResult", "context", true);
        sb.Append("\n<script language=\"javascript\" type=\"text/javascript\" src=\"" + m_filePath + "JavaScript/display_mapidentify.js\" ></script>\n");
        //sb.Append("<link rel=\"Stylesheet\" href=\"" + m_filePath + "styles/IdentifyWindowStyleSheet.css\" type=\"text/css\" />\n");
        sb.Append("<script language=\"javascript\" type=\"text/javascript\">var identifyCallbackFunctionString = \"" + m_callbackInvocation + "\";</script>\n");
        if (!m_page.ClientScript.IsClientScriptBlockRegistered("IdentifyScript"))
            m_page.ClientScript.RegisterClientScriptBlock(m_page.GetType(), "IdentifyScript", sb.ToString());
    }

    public string Identify(NameValueCollection queryString)
    {
        string xString = queryString["minx"];
        string yString = queryString["miny"];
        string locXString = "";
        string locYString = "";
        int x = Convert.ToInt32(xString);
        int y = Convert.ToInt32(yString);
        IGISResource resource;
        IQueryFunctionality query;
        ESRI.ArcGIS.ADF.Web.Geometry.Point mapPoint = ESRI.ArcGIS.ADF.Web.Geometry.Point.ToMapPoint(x, y, 
            m_map.GetTransformationParams(TransformationDirection.ToMap));
        List<DataSet> gdsList = new List<DataSet>();
        foreach (IMapFunctionality mapFunc in m_map.GetFunctionalities())

        {
            if (mapFunc is ESRI.ArcGIS.ADF.Web.DataSources.Graphics.MapFunctionality) continue;
            resource = mapFunc.Resource;
            query = resource.CreateFunctionality(typeof(ESRI.ArcGIS.ADF.Web.DataSources.IQueryFunctionality), "identify_") as IQueryFunctionality;

            string[] layerIds;
            string[] layerNames;
            query.GetQueryableLayers(null, out layerIds, out layerNames);
            string resourceType = resource.DataSource.GetType().ToString();
            double roundFactor = Math.Pow(10, m_numberDecimals);
            string pointXString = Convert.ToString(Math.Round(mapPoint.X * roundFactor) / roundFactor);
            string pointYString = Convert.ToString(Math.Round(mapPoint.Y * roundFactor) / roundFactor);
            locXString = pointXString;
            locYString = pointYString;
            DataTable[] ds = null;
            try
            {
                ds = query.Identify(mapFunc.Name, mapPoint, m_IdentifyTolerance, m_idOption, null);
            }
            catch (Exception e)
            {
                DataTable table = new DataTable();
                table.TableName = "Identify Error: " + e.Message;
                ds = new DataTable[] { table };
            }
            if (ds != null && ds.Length > 0)
            {
                DataSet gds = new DataSet();
                DataTable table;
                for (int j=ds.Length-1; j>=0;j--)
                {
                    table = ds[j];
                    // Remove empty tables.
                    if (table.Rows.Count == 0 && table.TableName.IndexOf("Error") < 0)
                        continue;
                    GraphicsLayer layer = ESRI.ArcGIS.ADF.Web.Converter.ToGraphicsLayer(table, System.Drawing.Color.Empty, System.Drawing.Color.Aqua);
                    if (layer != null)
                        gds.Tables.Add(layer);
                    else
                        gds.Tables.Add(table);
                }
                if (gds.Tables.Count == 0)
                    continue;

                gds.DataSetName = resource.Name + " (" + pointXString + ", " + pointYString + ")";
                gdsList.Add(gds);
            }
        }

        for (int i = gdsList.Count-1; i >=0; i--)
        {
            m_resultsDisplay.DisplayResults(null, null, null, gdsList[i]);
        }
        if (gdsList.Count == 0)
        {
            string heading = "Location (" + locXString + ", " + locYString + ") No results found";
            string detail = "No results found";
            SimpleTaskResult str = new SimpleTaskResult(heading, detail);
            m_resultsDisplay.DisplayResults(null, null, null, str);
        }
        return m_resultsDisplay.CallbackResults.ToString();

    }



    #endregion


    #region Properties
    public Map Map
    {
        get { return m_map; }
        set { m_map = value; }
    }

    public Page Page
    {
        get { return m_page; }
        set { m_page = value; }
    }

    public DataSet DataSet
    {
        get { return m_dataset; }
        set { m_dataset = value; }
    }

    public IdentifyOption IdentifyOption
    {
        get { return m_idOption; }
        set { m_idOption = value; }
    }

    public string ClientCallbackInvocation
    {
        get { return m_callbackInvocation; }
        set { m_callbackInvocation = value; }
    }

    public string FilePath
    {
        get { return m_filePath; }
        set { m_filePath = value; }
    }

    public TaskResults ResultsDisplay
    {
        get { return m_resultsDisplay; }
        set { m_resultsDisplay = value; }
    }

    public int NumberDecimals
    {
        get { return m_numberDecimals; }
        set { m_numberDecimals = value; }
    }

#endregion


}
