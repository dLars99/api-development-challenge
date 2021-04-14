using APIDevelopmentChallenge.Models;
using APIDevelopmentChallenge.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
