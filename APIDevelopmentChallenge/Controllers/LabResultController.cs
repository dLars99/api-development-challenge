using APIDevelopmentChallenge.Models;
using APIDevelopmentChallenge.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace APIDevelopmentChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabResultController : ControllerBase
    {
        private readonly ILabResultRepository _labResultRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IMemoryCache _cache;

        public LabResultController(ILabResultRepository labResultRepository,
                                   IPatientRepository patientRepository,
                                   IMemoryCache memoryCache)
        {
            _labResultRepository = labResultRepository;
            _patientRepository = patientRepository;
            _cache = memoryCache;
        }

        [HttpPost]
        public IActionResult Post(LabResult labResult)
        {
            var patient = _patientRepository.GetById(labResult.PatientId);
            var errorMessage = ValidateData(labResult, patient);
            if (!String.IsNullOrEmpty(errorMessage))
            {
                return BadRequest(errorMessage);
            }

            try
            {
                _labResultRepository.Add(labResult);
                var cacheKey = "LabResult" + labResult.Id;
                return CreatedAtAction("Get", new { id = labResult.Id }, labResult);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Cannot save this lab report -- internal error {e}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var cacheKey = "LabResult" + id;
                if (!_cache.TryGetValue(cacheKey, out LabResult labResult))
                {
                    labResult = _labResultRepository.GetById(id);
                    if (labResult == null)
                    {
                        return NotFound();
                    }
                    _cache.Set(cacheKey, labResult);
                }
                return Ok(labResult);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Cannot return lab result. Internal error: {e}");
            }
        }

        [HttpGet]
        public IActionResult GetByPatient(int patientId)
        {
            try
            {
                var cacheKey = "LabResultPatient" + patientId;
                if (!_cache.TryGetValue(cacheKey, out List<LabResult> labResults))
                {

                    labResults = _labResultRepository.GetByPatientId(patientId);
                    _cache.Set(cacheKey, labResults);
                }
                 
                return Ok(labResults);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Unable to retrieve lab reports -- Internal Error {e}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, LabResult labResult)
        {
            if (id != labResult.Id)
            {
                return BadRequest("LabResult Id does not match route");
            }
            try
            {
                var cacheKey = "LabResult" + labResult.Id;
                var patientCacheKey = "Patient" + labResult.PatientId;
                if (!_cache.TryGetValue(patientCacheKey, out Patient patient))
                {
                    patient = _patientRepository.GetById(labResult.PatientId);
                    if (patient != null)
                    {
                        _cache.Set(patientCacheKey, patient);
                    }
                }
                var errorMessage = ValidateData(labResult, patient);
                if (!String.IsNullOrEmpty(errorMessage))
                {
                    return BadRequest(errorMessage);
                }

                _labResultRepository.Update(labResult);
                _cache.Set(cacheKey, labResult);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Unable to update patient -- Internal error: {e}");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var cacheKey = "LabResult" + id;
                _labResultRepository.Delete(id);
                _cache.Remove(cacheKey);
            }
            catch
            {
                return StatusCode(500, "Internal error -- Unable to delete patient");
            }

            return NoContent();
        }


        private static string ValidateData(LabResult labResult, Patient patient)
        {
            if (patient == null)
            {
                return "Invalid patient";
            }
            if (labResult.TimeOfTest > DateTime.Now)
            {
                return "Time out of range";
            }
            if (labResult.Measurement < 0)
            {
                return "Measurement cannot be negative";
            }

            return "";
        }

    }
}
