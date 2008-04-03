using System;
using System.Collections.Generic;
using ESRI.ArcGIS.Server;
using ESRI.ArcGIS.Geometry;
using System.Configuration;
using ESRI.ArcGIS.ADF.Connection.AGS;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.ADF.Web;
using ESRI.ArcGIS.ADF.Web.DataSources;
using ESRI.ArcGIS.ADF.Web.DataSources.Graphics;
using ESRI.ArcGIS.ADF.Web.Display.Graphics;
using ESRI.ArcGIS.ADF.Web.Display.Symbol;
using ESRI.ArcGIS.ADF.Web.Geometry;
using ESRI.ArcGIS.ADF.Web.UI.WebControls;
using ESRI.ArcGIS.ADF.Web.DataSources.ArcGISServer;
using SJRAtlas.Models.Atlas;
using Castle.Core.Logging;
using Castle.Windsor;
using Envelope = ESRI.ArcGIS.ADF.Web.Geometry.Envelope;
using Point = ESRI.ArcGIS.ADF.Web.Geometry.Point;
using Map = ESRI.ArcGIS.ADF.Web.UI.WebControls.Map;
using System.Data;
using System.Web;

/// <summary>
/// A MapController acts as the manager of a page displaying a map.
/// </summary>
public class MapInitializer
{
    private Map _map;

    private readonly InitializerProperties properties;
    private readonly ILogger logger;

    /// <summary>
    /// Basic constructor
    /// </summary>
    public MapInitializer(Map map, InteractiveMap interactiveMap)
    {
        if (map == null)
            throw new ArgumentNullException("map");

        _map = map;
        _interactiveMap = interactiveMap;

        properties = GlobalApplication.ContainerAccessor.Container
            .Resolve<InitializerProperties>("init.properties");

        logger = GlobalApplication
            .CreateLogger(typeof(MapInitializer));

    }

    /// <summary>
    /// Gets a reference to the MapResourceManager control in the view.
    /// </summary>
    public MapResourceManager MapResourceManager
    {
        get { return _map.MapResourceManagerInstance; }
    }

    /// <summary>
    /// Gets a reference to the Map control in the view.
    /// </summary>
    public Map Map
    {
        get { return _map; }
    }

    private InteractiveMap _interactiveMap;

    public InteractiveMap InteractiveMap
    {
        get { return _interactiveMap; }
    }

    public InitializerProperties Properties
    {
        get { return properties; }
    }

    public ILogger Logger
    {
        get { return logger; }
    }

    /// <summary>
    /// Usually called from a Page_Load event. Defines which resource(s) will be displayed on the map control.
    /// </summary>
    public void Init()
    {
        AddMapResources();
        //DrawMarkers();
        RefreshResources();
    }

    private void AddMapResources()
    {
        try
        {
            AddGraphicsLayer();

            Logger.Debug("Initializing Map Resources");
            Logger.Debug("ArcGIS Host: " + Properties.Host);
            Logger.Debug("Default DataSourceType: " + Properties.DataSourceType);
            Logger.Debug("Default DataSourceDefinition: " + Properties.DataSourceDefinition);
            Logger.Debug("Using Identity: " + Properties.Identity);

            foreach (MapService mapService in InteractiveMap.MapServices)
            {
                MapResourceItem resourceItem = CreateMapResourceItem(mapService);
                MapResourceManager.ResourceItems.Add(resourceItem);
                MapResourceManager.CreateResource(resourceItem);

                if (resourceItem.FailedToInitialize)
                    throw new Exception(String.Format("Map service {0} failed to initialize"));
            }

            MapResourceManager.Refresh();
        }
        catch (Exception e)
        {
            Logger.Debug("Interactive map could not be built", e);
            throw e;
        }
    }

    private MapResourceItem CreateMapResourceItem(MapService mapService)
    {
        Logger.Debug("Creating map resource item for service=" + mapService.ServiceName);
        GISResourceItemDefinition definition = new GISResourceItemDefinition();
        definition.DataSourceType = Properties.DataSourceType;
        definition.DataSourceDefinition = Properties.DataSourceDefinition;
        definition.Identity = Properties.Identity;
        definition.ResourceDefinition = "(default)@" + mapService.ServiceName;
        definition.DataSourceShared = true;

        MapResourceItem resourceItem = new MapResourceItem();
        resourceItem.Definition = definition;
        resourceItem.Name = mapService.DisplayName;
        resourceItem.DisplaySettings = new DisplaySettings();
        resourceItem.DisplaySettings.Visible = mapService.Visible;
        resourceItem.Parent = MapResourceManager;

        return resourceItem;
    }

    /// <summary>
    /// Creates a GraphicsResource in the MapResourceManager. 
    /// This must be the first layer to be created on the web-tier.
    /// </summary>
    public void AddGraphicsLayer()
    {
        GISResourceItemDefinition graphicsDefinition = new GISResourceItemDefinition();
        graphicsDefinition.ResourceDefinition = "GraphicsResource";
        graphicsDefinition.DataSourceDefinition = "In Memory";
        graphicsDefinition.DataSourceType = "GraphicsLayer";
        graphicsDefinition.DataSourceShared = true;

        MapResourceItem graphicsResourceItem = new MapResourceItem();
        graphicsResourceItem.Definition = graphicsDefinition;
        graphicsResourceItem.Name = Properties.GraphicsResourceName;
        graphicsResourceItem.DisplaySettings = new DisplaySettings();
        graphicsResourceItem.DisplaySettings.Visible = true;
        graphicsResourceItem.DisplaySettings.DisplayInTableOfContents = false;
        graphicsResourceItem.Parent = MapResourceManager;

        MapResourceManager.ResourceItems.Add(graphicsResourceItem);
        MapResourceManager.CreateResource(graphicsResourceItem);
        MapResourceManager.Refresh();
    }

    /// <summary>
    /// Apply to the graphics layer any markers defined by the user
    /// </summary>
    private void DrawMarkers()
    {
        // Fill mapMarkers collection with markers from QueryString or Form data.
        IList<GraphicElement> mapMarkers = ReadMarkers();

        if (mapMarkers.Count == 0)
            return;

        // Get a reference to the Graphics layer
        MapResource mapResource = null;
        foreach (IGISFunctionality gisFunctionality in Map.GetFunctionalities())
        {
            if (gisFunctionality != null && gisFunctionality.Resource.Name == Properties.GraphicsResourceName)
            {
                mapResource = (MapResource)gisFunctionality.Resource;
                break;
            }
        }
        if (mapResource == null)
            return;

        ElementGraphicsLayer graphicsLayer = null;
        foreach (DataTable table in mapResource.Graphics.Tables)
        {
            if (table is ElementGraphicsLayer)
            {
                graphicsLayer = (ElementGraphicsLayer)table;
                break;
            }
        }

        // Graphics layer not found, create one
        if (graphicsLayer == null)
        {
            graphicsLayer = new ElementGraphicsLayer("Pin 1");
            mapResource.Graphics.Tables.Add(graphicsLayer);
        }

        // Draw markers
        graphicsLayer.Clear();
        foreach (GraphicElement marker in mapMarkers)
        {
            graphicsLayer.Add(marker);
        }

        // Change zoom to fit all points in map
        if (mapMarkers.Count == 1)
        {
            //simply center the map in the point
            //ESRI.ArcGIS.ADF.Web.Geometry.Point point = (point)System.Drawing.Point();
            Map.CenterAt(mapMarkers[0].Geometry as Point);
        }
        else
        {
            // Find the envelope that will enclose all points
            PointCollection points = new PointCollection();
            foreach (GraphicElement element in mapMarkers)
            {
                points.Add(element.Geometry as Point);
            }

            Envelope newExtent = Geometry.GetMinimumEnclosingEnvelope(points);
            newExtent = newExtent.Expand(10);

            // display all markers on the map, adding some margin
            Map.Extent = newExtent;
        }
    }

    /// <summary>
    /// Find map marker points in QueryString parameters or Form data and store them in mapMarkers collection.
    /// </summary>
    /// <example>Saint John: ?markerPoint[0]=2500000,7350000&markerPoint[1]=2550000,7300000</example>
    private IList<GraphicElement> ReadMarkers()
    {
        // clear collection
        List<GraphicElement> mapMarkers = new List<GraphicElement>();

        try
        {
            using (AGSServerConnection connection = new AGSServerConnection())
            {
                connection.Host = Properties.Host;
                connection.Connect();

                string firstService = null;

                IServerContext context = connection
                    .ServerObjectManager
                    .CreateServerContext(Map.PrimaryMapResource, "MapServer");

                // read the marker points from the query string
                IList<Point> markerPoints = new List<Point>();
                //    new MarkerPointReader(HttpContext
                //    .Current
                //    .Request
                //    .QueryString
                //).Read(context, firstService);

                foreach (Point markerPoint in markerPoints)
                {
                    mapMarkers.Add(CreateMarker(markerPoint));
                }

                context.ReleaseContext();
            }
        }
        catch (Exception e)
        {
            Logger.Error("Error while reading markers from query string", e);
        }

        return mapMarkers;
    }

    /// <summary>
    /// Create a Marker using default values
    /// </summary>
    /// <param name="point">The Point in map where the marker will be drawn.</param>
    /// <returns>A GraphicElement that can be drawn in a GraphicsLayer.</returns>
    private GraphicElement CreateMarker(Point point)
    {
        GraphicElement marker = new GraphicElement();
        RasterMarkerSymbol icon = new RasterMarkerSymbol(HttpContext.Current.Request.MapPath("~/images/identify-map-icon.png"));

        marker.Symbol = icon;
        marker.Geometry = point;
        return marker;
    }

    /// <summary>
    /// Ensures that the Map resources are displayed by initializing and refreshing them.
    /// </summary>
    public void RefreshResources()
    {
        Map.InitializeFunctionalities();
        Map.Extent = new ESRI.ArcGIS.ADF.Web.Geometry.Envelope(2286186, 7257990.32869855, 2737164, 7694705.09733051);
        Map.Visible = true;
        Map.UseDefaultWebResources = true;
        Map.Refresh();
    }
}
