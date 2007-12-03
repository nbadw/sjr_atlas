using System;

namespace SJRAtlas.CGNS
{
    public interface IGnssWebApiRequestBuilder
    {
        void ParseQuery(string query);
        void AddCgndbKey(string id);
        Uri Build();
    }
}
