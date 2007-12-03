SJRAtlas.CGNS
=============

This assembly acts as an interface to the Canadian Geographical Names Service (CGNS) through the web.  It uses the Geographical Name Search Service (GNSS) API to make queries and process the results. 

* REQUIRES - SJRAtlas.Core.dll, Castle.Core.dll

USAGE
=====

To lookup a placename using it's CGNDB Key:

  string cgndbKey = "BADHP";
  string gnssApiUrl = "http://gnss.nrcan.gc.ca/gnss-srt/api";
  GeographicalNameSearchService gnss = new GeographicalNameSearchService( new GnssWebApi( gnssApiUrl ) );
  IPlaceName result = gnss.FindById( cgndbKey );
  
To search for a placename:

  string nameToSearch = "saint john";
  string gnssApiUrl = "http://gnss.nrcan.gc.ca/gnss-srt/api";
  GeographicalNameSearchService gnss = new GeographicalNameSearchService( new GnssWebApi( gnssApiUrl ) );
  IPlaceName[] results = gnss.FindByQuery( nameToSearch );
