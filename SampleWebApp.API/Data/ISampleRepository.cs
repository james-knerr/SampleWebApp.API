using SampleWebApp.API.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace SampleWebApp.API
{
    public interface ISampleRepository
    {
        bool SaveAll(ClaimsPrincipal userName);
        bool SaveAll(string email);
        ICollection<SampleModel> GetSamples(bool includeDeleted = false);
        void AddSample(SampleModel newSample);
    }
}
