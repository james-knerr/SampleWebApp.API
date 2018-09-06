using Microsoft.EntityFrameworkCore;
using SampleWebApp.API.Helpers;
using SampleWebApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SampleWebApp.API
{
    public class SampleRepository : ISampleRepository
    {
        private SampleContext _context;
        public SampleRepository(SampleContext context)
        {
            _context = context;
        }
        public bool SaveAll(ClaimsPrincipal user)
        {
            var email = ""; //UserClaimHelper.Email(user.Identity);
            return SaveAll(email);
        }

        public bool SaveAll(string email)
        {
            // TODO: update changed records to have changed by email
            return _context.SaveChanges() > 0;
        }
        public ICollection<SampleModel> GetSamples(bool includeDeleted = false)
        {
            if (includeDeleted)
            {
                return _context.Samples.ToList();
            }
            else
            {
                return _context.Samples.Where(k => k.IsDeleted == false).ToList();
            }

        }
        public void AddSample(SampleModel newSample)
        {
            _context.Add(newSample);
        }
    }
}