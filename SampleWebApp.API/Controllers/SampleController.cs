using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleWebApp.API.Helpers;
using SampleWebApp.API.Models;
using SampleWebApp.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;

namespace SampleWebApp.API.Controllers
{
    [Route("samples")]
    public class SampleController : Controller
    {
        private ISampleRepository _repository;
        private ILogger<SampleController> _logger;
        private TelemetryClient _telemetry;
        public SampleController(ISampleRepository repository, TelemetryClient telemetry, ILogger<SampleController> logger)
        {
            _repository = repository;
            _logger = logger;
            _telemetry = telemetry;
            _telemetry.InstrumentationKey = Startup.Configuration["ApplicationInsights:InstrumentationKey"];
        }

        [HttpGet("")]
        public JsonResult GetSamples()
        {
            Guid analyticsEventGuid = Guid.NewGuid();
            Dictionary<string, string> analyticsProperties = new Dictionary<string, string>();
            analyticsProperties.Add("Source", "API");
            analyticsProperties.Add("Method", "GetSamples()");
            analyticsProperties.Add("ID", GuidMappings.Map(analyticsEventGuid));
            try
            {
                _telemetry.TrackEvent("Start", analyticsProperties);
                ICollection<SampleModel> samples = new List<SampleModel>();
                samples = _repository.GetSamples();
                ICollection<SampleViewModel> vms = new List<SampleViewModel>(samples.Count);
                foreach (var sample in samples)
                {
                    vms.Add(sample.ToViewModel());
                }
                Response.StatusCode = (int)HttpStatusCode.OK;
                _telemetry.TrackEvent("End", analyticsProperties);
                return Json(vms);

            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to retrieve samples.", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                _telemetry.TrackEvent("Exception", analyticsProperties);
                return Json(new Message(ex));
            }
        }
        [HttpPost("")]
        public JsonResult AddSample([FromBody] SampleViewModel vm)
        {
            Guid analyticsEventGuid = Guid.NewGuid();
            Dictionary<string, string> analyticsProperties = new Dictionary<string, string>();
            analyticsProperties.Add("Source", "API");
            analyticsProperties.Add("Method", "AddSample()");
            analyticsProperties.Add("ID", GuidMappings.Map(analyticsEventGuid));
            try
            {
                _telemetry.TrackEvent("Start", analyticsProperties);
                if (ModelState.IsValid)
                {
                    SampleModel newSample = vm.ToModel();
                    _repository.AddSample(newSample);
                    SampleViewModel newVm = newSample.ToViewModel();
                    if (_repository.SaveAll(User))
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        _telemetry.TrackEvent("End", analyticsProperties);
                        return Json(newVm);
                    }
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _telemetry.TrackEvent("Exception", analyticsProperties);
                    return Json(new Message(MessageType.Error, "Unable to save new sample"));
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _telemetry.TrackEvent("Exception", analyticsProperties);
                    return Json(new Message(ModelState));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to add sample.", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new Message(ex));
            }
        }
    }
}
