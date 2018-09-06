using SampleWebApp.API.Models;

namespace SampleWebApp.API.ViewModels
{
    public class SampleViewModel
    {
        public string Id { get; set; }
        public string SampleText { get; set; }
        public bool IsDeleted { get; set; }

        public SampleModel ToModel()
        {
            SampleModel model = new SampleModel()
            {
                SampleText = this.SampleText,
                IsDeleted = this.IsDeleted
            };
            return model;
        }
    }
}
