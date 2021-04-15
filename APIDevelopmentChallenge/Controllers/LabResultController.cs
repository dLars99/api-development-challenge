using APIDevelopmentChallenge.Models;
using APIDevelopmentChallenge.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;

namespace APIDevelopmentChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabResultController : ControllerBase
    {
        private readonly ILabResultRepository _labResultRepository;
        private readonly IPatientRepository _patientRepository;

        public LabResultController(ILabResultRepository labResultRepository,
                                   IPatientRepository patientRepository)
        {
            _labResultRepository = labResultRepository;
            _patientRepository = patientRepository;
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
                return CreatedAtAction("Get", new { id = labResult.Id }, labResult);
            }
            catch
            {
                return StatusCode(500, "There was a problem saving this lab result.");
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var labResult = _labResultRepository.GetById(id);
                if (labResult == null)
                {
                    return NotFound();
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
                return Ok(_labResultRepository.GetByPatientId(patientId));
            }
            catch
            {
                return StatusCode(500, "Internal error - cannot retrieve lab results");
            }
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
