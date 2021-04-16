using APIDevelopmentChallenge.Models;
using APIDevelopmentChallenge.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIDevelopmentChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMemoryCache _cache;

        public PatientController(IPatientRepository patientRepository,
                                 IMemoryCache memoryCache)
        {
            _patientRepository = patientRepository;
            _cache = memoryCache;
        }

        [HttpPost]
        public IActionResult Post(Patient patient)
        {
            try
            {
                _patientRepository.Add(patient);
                var cacheKey = "Patient" + patient.Id;
                _cache.Set(cacheKey, patient);
                return CreatedAtAction("Get", new { id = patient.Id }, patient);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Cannot save this patient -- Internal error: {e}");
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            Console.WriteLine(DateTime.Now);
            try
            {
                var patients = _patientRepository.GetAll();
                foreach (var patient in patients)
                {
                    var cacheKey = "Patient" + patient.Id;
                    _cache.Set(cacheKey, patient);
                }
                return Ok(patients);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Unable to retrieve patients -- internal error: {e}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var cacheKey = "Patient" + id;
                if (!_cache.TryGetValue(cacheKey, out Patient patient))
                {
                    patient = _patientRepository.GetById(id);
                }
                if (patient == null)
                {
                    return NotFound();
                }
                _cache.Set(cacheKey, patient);

                return Ok(patient);
            }
            catch (Exception e)
            { 
                return StatusCode(500, $"Cannot retrieve patient -- internal error: {e}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Patient patient)
        {
            if (id != patient.Id)
            {
                return BadRequest("Patient Id does not match route");
            }

            try
            {
                _patientRepository.Update(patient);
                var cacheKey = "Patient" + id;
                _cache.Set(cacheKey, patient);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Unable to update patient -- Internal error {e}");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _patientRepository.Delete(id);
                var cacheKey = "Patient" + id;
                _cache.Remove(cacheKey);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Unable to update patient -- Internal error {e}");
            }

            return NoContent();
        }

        [HttpGet("labs")]
        public IActionResult GetByLabs(string query, DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("Invalid date range");
            }
            try
            {
                var cacheKey = "Patient" + query + startDate + endDate;
                if (!_cache.TryGetValue(cacheKey, out List<Patient> patients))
                {
                    patients = _patientRepository.GetByLabs(query, startDate, endDate);
                    _cache.Set(cacheKey, patients);
                }

                return Ok(patients);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Unable to retrieve patients -- Internal error: {e}");
            }
        }
    }
}
