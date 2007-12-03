using System;

namespace SJRAtlas.CGNS
{
    public interface IGnssWebApi
    {
        IGnssWebApiRequestBuilder CreateRequestBuilder();
        GnssPlaceName[] FindByRequest(Uri request);
    }
}
