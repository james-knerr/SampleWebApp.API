﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleWebApp.API.Models;
using SampleWebApp.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;

namespace SampleWebApp.API.Controllers
{
    [Route("sample")]
    public class SampleController : Controller
    {
        private ISampleRepository _repository;
        private ILogger<SampleController> _logger;
        public SampleController(ISampleRepository repository, ILogger<SampleController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("")]
        public JsonResult GetSamples()
        {
            try
            {
                ICollection<SampleModel> samples = new List<SampleModel>();
                samples = _repository.GetSamples();
                ICollection<SampleViewModel> vms = new List<SampleViewModel>(samples.Count);
                foreach (var sample in samples)
                {
                    vms.Add(sample.ToViewModel());
                }
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(vms);

            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to retrieve samples.", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new Message(ex));
            }
        }
        [HttpPost("")]
        public JsonResult AddSample([FromBody] SampleViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SampleModel newSample = vm.ToModel();
                    _repository.AddSample(newSample);
                    SampleViewModel newVm = newSample.ToViewModel();
                    if (_repository.SaveAll(User))
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json(newVm);
                    }
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new Message(MessageType.Error, "Unable to save new sample"));
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
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