using System;
using Castle.Core.Logging;
using Castle.MonoRail.Framework;
using SJRAtlas.Core;
using SJRAtlas.Site.Helpers;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using SJRAtlas.Site.Models;

namespace SJRAtlas.Site
{
    [Helper(typeof(BreadCrumbHelper), "BreadCrumbs")]
    [Helper(typeof(ResourceHelper), "Resource")]
    public class SJRAtlasController : SmartDispatcherController
    {
        private IPlaceNameLookup placeNameLookup;
        private IWatershedLookup watershedLookup;
        private IMetadataLookup metadataLookup;
        private IEasyMapLookup easymapLookup;
        private IAtlasUtils atlasUtils;
        private IMetadataUtils metadataUtils;

        public SJRAtlasController()
        {
            placeNameLookup = NullPlaceNameLookupService.INSTANCE;
            watershedLookup = NullWatershedLookupService.INSTANCE;
            metadataLookup = NullMetadataLookupService.INSTANCE;
            easymapLookup = NullEasyMapLookupService.INSTANCE;
        }

        public IPlaceNameLookup PlaceNameLookup
        {
            get { return placeNameLookup; }
            set { placeNameLookup = value; }
        }

        public IWatershedLookup WatershedLookup
        {
            get { return watershedLookup; }
            set { watershedLookup = value; }
        }

        public IMetadataLookup MetadataLookup
        {
            get { return metadataLookup; }
            set { metadataLookup = value; }
        }

        public IEasyMapLookup EasyMapLookup
        {
            get { return easymapLookup; }
            set { easymapLookup = value; }
        }

        public IAtlasUtils AtlasUtils
        {
            get { return atlasUtils; }
            set { atlasUtils = value; }
        }

        public IMetadataUtils MetadataUtils
        {
            get { return metadataUtils; }
            set { metadataUtils = value; }
        }

        protected IEasyMap[] GetInteractiveMaps(IMetadata[] metadata)
        {
            return GetInteractiveMaps(metadata, false);
        }

        protected IEasyMap[] GetInteractiveMaps(IMetadata[] metadata, bool inBasin)
        {
            IEasyMap[] allMaps = EasyMapLookup.FindAll();
            List<IEasyMap> targetMaps = new List<IEasyMap>();

            foreach (IEasyMap map in allMaps)
            {
                bool keep = false;

                if (inBasin && map.FullBasinCoverage)
                {
                    keep = true;
                }

                // only check metadata if basin check failed
                if (!keep)
                {
                    foreach (IMetadata item in metadata)
                    {
                        if (item.Title == map.Title)
                        {
                            keep = true;
                            break;
                        }
                    }
                }

                if (keep)
                    targetMaps.Add(map);
            }

            IEasyMap[] maps = targetMaps.ToArray();
            AttachMetadataToMaps(maps, metadata);
            // retrieve any metadata that might not have been fetched the first time
            AttachMetadataToMaps(maps);

            return maps;
        }

        protected void AttachMetadataToMaps(IEasyMap[] maps)
        {
            List<string> titlesOfMapsWithNoMetadata = new List<string>();
            foreach (IEasyMap map in maps)
            {
                if (map.Metadata == null)
                    titlesOfMapsWithNoMetadata.Add(map.Title);
            }
            AttachMetadataToMaps(maps, MetadataLookup.FindByQuery(MetadataUtils
                .BuildGetByTitlesQuery(titlesOfMapsWithNoMetadata.ToArray())));
        }

        protected void AttachMetadataToMaps(IEasyMap[] maps, IMetadata[] metadata)
        {
            foreach (IEasyMap map in maps)
            {
                foreach (IMetadata item in metadata)
                {
                    if (item.Title == map.Title)
                        map.Metadata = item;
                }
            }
        }

        protected IMetadata[] RemoveEasyMaps(IMetadata[] metadata, IEasyMap[] maps)
        {
            List<IMetadata> newList = new List<IMetadata>(metadata);
            foreach (IMetadata item in metadata)
            {
                foreach (IEasyMap map in maps)
                {
                    if (item.Title == map.Title)
                    {
                        newList.Remove(item);
                    }
                }
            }
            return newList.ToArray();
        }

        protected IMetadata[] MergeMetadata(IMetadata[] setOne, IMetadata[] setTwo)
        {
            Dictionary<int, IMetadata> uniqueMetadata = new Dictionary<int, IMetadata>();
            foreach (IMetadata item in setOne)
            {
                uniqueMetadata[item.Id] = item;
            }
            foreach (IMetadata item in setTwo)
            {
                uniqueMetadata[item.Id] = item;
            }
            IMetadata[] metadata = new IMetadata[uniqueMetadata.Count];
            uniqueMetadata.Values.CopyTo(metadata, 0);
            return metadata;
        }

        protected override void InvokeMethod(MethodInfo method, IRequest request, IDictionary actionArgs)
        {
            bool isAjaxAction = method.GetCustomAttributes(typeof(AjaxActionAttribute), false).Length > 0;
            Logger.Info("Ajax Action Requested");

            try
            {
                ParameterInfo[] parameters = method.GetParameters();                

                object[] methodArgs = BuildMethodArguments(parameters, request, actionArgs);  

                object result = method.Invoke(this, methodArgs);

                if (result != null)
                {
                    if (isAjaxAction)
                    {
                        Response.ContentType = "text/plain";
                        if (typeof(bool).IsAssignableFrom(result.GetType()))
                        {
                            RenderText(
                                (bool)result
                                    ? Newtonsoft.Json.JavaScriptConvert.True
                                    : Newtonsoft.Json.JavaScriptConvert.False
                            );
                        }
                        else
                        {
                            string json = Newtonsoft.Json.JavaScriptConvert.SerializeObject(result);
                            if (Logger.IsDebugEnabled)
                                Logger.Debug(json);
                            RenderText(json);
                        }
                    }
                    else
                    {
                        RenderText(result.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Error error = CreateError(e, null);
                Exception inner = e.InnerException;

                while (inner != null)
                {
                    error = CreateError(inner, error);
                    inner = inner.InnerException;
                }

                if (Logger.IsErrorEnabled)
                    Logger.Error("Controller Error", e);

                throw e;
            }
        }

        private Error CreateError(Exception e, Error parent)
        {
            Error error = new Error();
            error.Message = e.Message;
            error.StackTrace = e.StackTrace;
            error.Source = e.Source;

            if (parent != null)
                error.ParentId = parent.Id;

            error.SaveAndFlush();
            return error;
        }
    }
}
