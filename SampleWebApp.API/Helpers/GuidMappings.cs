using System;

namespace SampleWebApp.API.Helpers
{
    public static class GuidMappings
    {
        public static Guid Map(string guid)
        {
            var gGuid = Guid.Empty;
            Guid.TryParse(guid, out gGuid);
            return gGuid;
        }

        public static string Map(Guid guid)
        {
            if (guid == null)
                return Guid.Empty.ToString();

            return guid.ToString();
        }
    }
}
