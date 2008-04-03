using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
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
using ESRI.ArcGIS.ADF.Web.DataSources.ArcGISServer;
using ESRI.ArcGIS.ADF.Connection.AGS;
using ESRI.ArcGIS.Server;
using SJRAtlas.Models.Atlas;
using Castle.Core.Logging;


public partial class WebMapApplication : System.Web.UI.Page, ICallbackEventHandler
{
    MapIdentify identify;

    public string m_newLoad = "false";
    public string m_closeOutCallback = "";
    public string m_copyrightCallback = "";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsCallback && !Page.IsPostBack)
        {
            Logger = GlobalApplication.CreateLogger(typeof(WebMapApplication));

            if (Map1.MapResourceManager == null)
                throw new Exception("No MapResourceManager defined for the map.");
            
            InteractiveMap interactiveMap = InteractiveMap.Find(
                int.Parse(HttpContext.Current.Request.Params["id"])
            );
            HeaderTitle.Text = "Saint John River Atlas - " + interactiveMap.Title;
            PageTitle.Text = interactiveMap.Title;

            new MapInitializer(Map1, interactiveMap).Init();

            if (MapResourceManager1.ResourceItems.Count == 0)
                throw new Exception("The MapResourceManager does not have a valid ResouceItem Definition.");

            m_newLoad = "true";
        }      
        //m_closeOutCallback = Page.ClientScript.GetCallbackEventReference(Page, "argument", "CloseOutResponse", "context", true);
        //m_copyrightCallback = Page.ClientScript.GetCallbackEventReference(Page, "argument", "processCallbackResult", "context", true);

        //// initiate identify class and set link to TaskResults1 for response
        //identify = new MapIdentify(Map1);
        //identify.ResultsDisplay = TaskResults1;
        //identify.NumberDecimals = 4;
    }

    private ILogger logger;

    public ILogger Logger
    {
        get { return logger; }
        set { logger = value; }
    }
    
    /// <summary>
    /// Handles unhandled exceptions in the page.
    /// </summary>
    protected void Page_Error(object sender, System.EventArgs e)
    {        
        Exception exception = Server.GetLastError();
        Server.ClearError();
        callErrorPage("Page_Error", exception);
    }


    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        // If no tasks have been defined, hide the tasks panel
        if (TaskMenu.Items.Count == 0)
            Tasks_Menu_Panel.Visible = false;
        // check to see if any of the resource items are non-pooled
        if (!Page.IsCallback || !Page.IsPostBack)
        {
            CloseHyperLink.Visible = HasNonPooledResources();
        }
        CopyrightTextHolder.Visible = HasCopyrightText();

    }

    protected void TitleMenu_DataBound(object sender, EventArgs e)
    {
        Menu menu = sender as Menu;
        if (menu != null)
        {
            for (int i = 0; i < menu.Items.Count - 1;i++ )
            {
                menu.Items[i].SeparatorImageUrl = "~/images/separator.gif";
            }
        }
    }

    /// <summary>
    /// Displays the error page.
    /// </summary>
    private void callErrorPage(string errorMessage, Exception exception)
    {
        Logger.Error(errorMessage, exception);
        Session["ErrorMessage"] = errorMessage;
        Session["Error"] = exception;
        Page.Response.Redirect("ErrorPage.aspx", true);
    }

    /// <summary>
    ///  Checks to see if any resources used by the app are local non-pooled
    /// </summary>
    /// <returns>True if any resources are local non-pooled</returns>
    private bool HasNonPooledResources()
    {
        // define a boolean and set it to false by default... no non-pooled resourceitems
        bool hasNonPooledResource = false;
        // Now go through all resources and find any non-pooled local resources
        MapResourceLocal mapResource = null;
        GISDataSourceLocal localDataSource = null;
        // First, check the map resourceitems
        foreach (MapResourceItem mri in MapResourceManager1.ResourceItems)
        {
            if (mri != null)
            {
                mapResource = mri.Resource as MapResourceLocal;
                if (mapResource != null)
                {
                    MapResourceLocal localRes = mapResource as MapResourceLocal;
                    localDataSource = mapResource.DataSource as GISDataSourceLocal;
                    if (!localDataSource.Connection.IsServerObjectPooled(mapResource.ServerContextInfo.ServerObjectName, "MapServer")) hasNonPooledResource = true;

                }
            }
        }

        return hasNonPooledResource;
    }

    protected void ResourceManager_ResourcesInit(object sender, EventArgs e)
    {
        if (DesignMode)
            return;
        ResourceManager manager = sender as ResourceManager;
        if (!manager.FailureOnInitialize)
            return;
        if (manager is MapResourceManager)
        {
            MapResourceManager mapManager = manager as MapResourceManager;
            for (int i = 0; i < mapManager.ResourceItems.Count; i++)
            {
                MapResourceItem item = mapManager.ResourceItems[i];
                if (item != null && item.FailedToInitialize)
                {
                    mapManager.ResourceItems[i] = null;
                }
            }
        }
        else if (manager is GeocodeResourceManager)
        {
            GeocodeResourceManager gcManager = manager as GeocodeResourceManager;
            for (int i = 0; i < gcManager.ResourceItems.Count; i++)
            {
                GeocodeResourceItem item = gcManager.ResourceItems[i];
                if (item != null && item.FailedToInitialize)
                {
                    gcManager.ResourceItems[i] = null;
                }
            }
        }
        else if (manager is GeoprocessingResourceManager)
        {
            GeoprocessingResourceManager gpManager = manager as GeoprocessingResourceManager;
            for (int i = 0; i < gpManager.ResourceItems.Count; i++)
            {
                GeoprocessingResourceItem item = gpManager.ResourceItems[i];
                if (item != null && item.FailedToInitialize)
                {
                    gpManager.ResourceItems[i] = null;
                }
            }
        }
    }

    private bool HasCopyrightText()
    {
        return (GetCopyrightText().Length > 0);
    }

    private string GetCopyrightText()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        string key = "";
        string value = "";
        string dataframeValue = "";
        int layerId;
        foreach (IMapFunctionality mapFunc in Map1.GetFunctionalities())
        {
            if (mapFunc.Supports("GetCopyrightText"))
            {
                Dictionary<string, string> crDictionary = mapFunc.GetCopyrightText();
                System.Text.StringBuilder sb2 = new System.Text.StringBuilder();
                string[] layerIds = null;
                string[] layerNames = null;
                mapFunc.GetLayers(out layerIds, out layerNames);
                foreach (KeyValuePair<string, string> kvPair in crDictionary)
                {
                    key = kvPair.Key;
                    value = kvPair.Value;
                    if (value != null && value.Length > 0)
                    {
                        if (key != null && key.Length > 0)
                        {
                            layerId = Convert.ToInt32(key);
                            sb2.Append(layerNames[layerId] + ": ");
                            sb2.Append(value + "<br/>");
                        }
                        else
                            dataframeValue = value;
                    }
                }
                //if (sb2.Length > 0) sb.AppendFormat("<div style='font-weight: bold'>{0}:</div><div style='padding: 0px 5px 5px 5px;font-weight: normal;'>{2}<br/>{1}</div>", mapFunc.Resource.Name, sb2.ToString(), dataframeValue);
                string layerCopyrights = string.Empty;
                if (sb2.Length > 0)
                    layerCopyrights = sb2.ToString();
                if (!string.IsNullOrEmpty(dataframeValue) || !string.IsNullOrEmpty(layerCopyrights))
                    sb.AppendFormat("<div style='font-weight: bold'>{0}:</div><div style='padding: 0px 5px 5px 5px;font-weight: normal;'>{2}<br/>{1}</div>", mapFunc.Resource.Name, layerCopyrights, dataframeValue);
                sb2.Length = 0;
                dataframeValue = "";

                sb2.Length = 0;
                dataframeValue = "";
            }
        }
        return sb.ToString();
    }

    #region ICallbackEventHandler Members
    private string _callbackArg;

    //public string GetCallbackResult()
    //{
    //    throw new Exception("The method or operation is not implemented.");
    //}

    void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
    {
        _callbackArg = eventArgument;
    }

    string ICallbackEventHandler.GetCallbackResult()
    {
        return RaiseCallbackEvent(_callbackArg);
    }

    #endregion

    #region ICallbackHandler

    public virtual string RaiseCallbackEvent(string responseString)
    {
        // break out the responseString into a querystring
        Array keyValuePairs = responseString.Split("&".ToCharArray());
        NameValueCollection m_queryString = new NameValueCollection();
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
            keyValue = responseString.Split("=".ToCharArray());
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
                if (eventArg == "MapIdentify")
                {
                    // Identify
                    if (identify != null)
                    {
                        identify.Map = Map1; // make sure it is current
                        response = identify.Identify(m_queryString);
                    }
                }
                else if (eventArg == "CloseOutApplication")
                {
                    // Close out session and quit application
                    IServerContext context;
                    for (int i = 0; i < Session.Count; i++)
                    {
                        context = Session[i] as IServerContext;
                        if (context != null)
                        {
                            context.RemoveAll();
                            context.ReleaseContext();
                        }
                    }
                    response = "SessionClosed";
                    Session.RemoveAll();
                    response = ConfigurationManager.AppSettings["CloseOutUrl"];
                    if (response == null || response.Length == 0) response = "ApplicationClosed.aspx";
                }
                else if (eventArg == "GetCopyrightText")
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendFormat("///:::{0}:::innercontent:::","CopyrightTextContents");
                    int sbLength = sb.Length;
                    sb.Append(GetCopyrightText());
                    if (sb.Length==sbLength) sb.Append("No Copyright information available.");
                    response = sb.ToString();
                }

                break;
            default:
                //
                break;
        }
        return response;
    }

    #endregion


}
