using SampleWebApp.API.Helpers;
using SampleWebApp.API.ViewModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace SampleWebApp.API.Models
{
    public class SampleModel
    {
        [Key]
        public Guid Id { get; set; }
        public string SampleText { get; set; }
        public bool IsDeleted { get; set; }
        public SampleViewModel ToViewModel()
        {
            SampleViewModel vm = new SampleViewModel()
            {
                Id = GuidMappings.Map(this.Id),
                SampleText = this.SampleText,
                IsDeleted = this.IsDeleted
            };
            return vm;
        }
    }
}
