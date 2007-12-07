namespace SJRAtlas.Site.Controllers
{
	using System;

	using Castle.MonoRail.Framework;
    using System.Collections.Generic;
    using SJRAtlas.Site.Models;
    using System.Text.RegularExpressions;
    using System.Collections.Specialized;

	[Layout("sjratlas"), Rescue("generalerror")]
	public class SearchController : SJRAtlasController
	{
        ////[Cache(System.Web.HttpCacheability.Server, Duration = 300, VaryByParams = "q")]
        //public void Quick(string q)
        //{
        //    if (IsWatershedQuery(q))
        //    {
        //        Logger.Debug("Quick Search is redirecting to the Watershed Search");
        //        q = new Regex(@"\swatershed$|\sbasin$|\scatchment$", RegexOptions.IgnoreCase).Replace(q, "");
        //        RedirectToAction("watershed", "q=" + q);
        //    }
        //    else if (IsMetadataQuery(q))
        //    {
        //        Logger.Debug("Quick Search is redirecting to the Metadata Search");
        //        q = new Regex(@".metadata$", RegexOptions.IgnoreCase).Replace(q, "");
        //        RedirectToAction("metadata", "q=" + q);
        //    }
        //    else
        //    {
        //        Logger.Debug("Quick Search is redirecting to the Gazetteer Search");
        //        RedirectToAction("gazetteer", "q=" + q);
        //    }
        //}

        ////[Cache(System.Web.HttpCacheability.Server, Duration = 300, VaryByParams = "q")]
        //[Rescue("servicedown", typeof(Exception))]
        //public void Gazetteer(string q)
        //{
        //    PropertyBag["query"] = q;
        //    try
        //    {
        //        IPlaceName[] results = PlaceNameLookup.FindByQuery(q);

        //        if (results.Length == 0)
        //        {
        //            // try the metadata search then...
        //            RedirectToAction("metadata", "q=" + q);
        //        }
        //        else if (results.Length == 1)
        //        {
        //            NameValueCollection parameters = new NameValueCollection();
        //            parameters.Add("id", results[0].Id);
        //            Redirect("", "placename", "view", parameters);
        //        }

        //        double maxLatitude = double.NaN;
        //        double minLatitude = double.NaN;
        //        double maxLongitude = double.NaN;
        //        double minLongitude = double.NaN;
        //        foreach (IPlaceName place in results)
        //        {
        //            if (double.IsNaN(maxLatitude) || maxLatitude < place.LatDec)
        //            {
        //                maxLatitude = place.LatDec;
        //            }
        //            if (double.IsNaN(minLatitude) || minLatitude > place.LatDec)
        //            {
        //                minLatitude = place.LatDec;
        //            }
        //            if (double.IsNaN(maxLongitude) || maxLongitude < place.LongDec)
        //            {
        //                maxLongitude = place.LongDec;
        //            }
        //            if (double.IsNaN(minLongitude) || minLongitude > place.LongDec)
        //            {
        //                minLongitude = place.LongDec;
        //            }
        //        }

        //        PropertyBag["results"] = results;
        //        PropertyBag["max_lat"] = maxLatitude;
        //        PropertyBag["min_lat"] = minLatitude;
        //        PropertyBag["max_lon"] = maxLongitude;
        //        PropertyBag["min_lon"] = minLongitude;
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.Error("Error in PlaceNameSearchService", e);
        //        throw new Exception("Place name search is currently unavailable.", e);
        //    }
        //}

        ////[Cache(System.Web.HttpCacheability.Server, Duration = 300, VaryByParams = "q")]
        //[Rescue("servicedown", typeof(Exception))]
        //public void Watershed(string q)
        //{
        //    IWatershed[] results = WatershedLookup.FindAllByProperty("UnitName", q);

        //    if (results.Length == 1)
        //    {
        //        NameValueCollection parameters = new NameValueCollection();
        //        parameters.Add("id", results[0].Id);                
        //        Redirect("", "watershed", "view", parameters);
        //    }

        //    PropertyBag["query"] = q;
        //    PropertyBag["watersheds"] = results;
        //}

        ////[Cache(System.Web.HttpCacheability.Server, Duration = 300, VaryByParams = "q")]
        //[Rescue("servicedown", typeof(Exception))]
        //public void Metadata(string q)
        //{
        //    IMetadata[] results = MetadataLookup.FindByQuery(q);

        //    PropertyBag["query"] = q;
        //    PropertyBag["results"] = results;
        //}

        //public void Index()
        //{
        //    RedirectToAction("advanced");
        //}

        //public void Advanced()
        //{
        //    Agency[] agencies = Agency.FindAll();
        //    List<string> agencyNames = new List<string>(agencies.Length);
        //    foreach(Agency agency in agencies)
        //    {
        //        if(!String.IsNullOrEmpty(agency.Name))
        //            agencyNames.Add(agency.Name);
        //    }
        //    agencyNames.Sort();

        //    PropertyBag["datasets"] = DataType.FindAll();
        //    PropertyBag["agencies"] = agencyNames;
        //    PropertyBag["optionstype"] = typeof(SearchOptions);
        //}

        //public void Tips()
        //{
        //}

        //public void SubmitAdvanced([DataBind("options", Validate=true)] SearchOptions options)
        //{
        //    if (HasValidationError(options))
        //    {
        //        Flash["options"] = options;
        //        Flash["summary"] = GetErrorSummary(options);
        //        RedirectToAction("advanced");
        //        return;
        //    }

        //    RenderSharedView("shared/todo");
        //}

        //private bool IsWatershedQuery(string query)
        //{
        //    Regex watershed = new Regex(@"\swatershed$|\sbasin$|\scatchment$", RegexOptions.IgnoreCase);
        //    return watershed.IsMatch(query.Trim());
        //}

        //private bool IsMetadataQuery(string query)
        //{
        //    Regex metadata = new Regex(@".metadata$", RegexOptions.IgnoreCase);
        //    return metadata.IsMatch(query.Trim());
        //}
	}
}
