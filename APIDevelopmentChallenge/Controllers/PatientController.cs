using APIDevelopmentChallenge.Models;
using APIDevelopmentChallenge.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;

namespace APIDevelopmentChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;

        public PatientController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        [HttpPost]
        public IActionResult Post(Patient patient)
        {
            if (patient.DateOfBirth > new DateTime())
            {
                return BadRequest("Invalid date of birth");
            }
            try
            {
                _patientRepository.Add(patient);
                return CreatedAtAction("Get", new { id = patient.Id }, patient);
            }
            catch
            {
                return StatusCode(500, "There was a problem saving this patient.");
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_patientRepository.GetAll());
            }
            catch
            {
                return StatusCode(500, "Unable to retrieve patients");
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var patient = _patientRepository.GetById(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
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
            }
            catch
            {
                return StatusCode(500, "Internal error -- Unable to update patient");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _patientRepository.Delete(id);
            }
            catch
            {
                return StatusCode(500, "Internal error -- Unable to delete patient");
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

            return Ok(_patientRepository.GetByLabs(query, startDate, endDate));
        }
    }
}
