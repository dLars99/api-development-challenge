using System;
using APIDevelopmentChallenge.Tests.Mocks;
using APIDevelopmentChallenge.Models;
using Xunit;
using System.Collections.Generic;
using APIDevelopmentChallenge.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace APIDevelopmentChallenge.Tests
{
    public class LabResultControllerTests
    {
        [Fact]
        public void Post_Method_Adds_A_New_Lab_Result()
        {
            var labResultCount = 20;
            var patients = CreateTestPatients(20);
            var labResults = CreateTestLabResults(labResultCount);

            var repo = new InMemoryLabResultRepository(labResults);
            var patientRepo = new InMemoryPatientRepository(patients);
            var controller = new LabResultController(repo, patientRepo);

            var newLabResult = new LabResult()
            {
                TestType = "TestType",
                Result = "Result",
                PatientId = 999,
                Patient = CreateTestPatient(999),
                TimeOfTest = DateTime.Now.AddDays(-2),
                LabName = "LabName",
                OrderedByProvider = "OrderedByProvider",
                Measurement = 1,
                MeasurementUnit = "Unit"
            };

            controller.Post(newLabResult);

            Assert.Equal(labResultCount + 1, repo.InternalData.Count);
        }

        //private LabResultController CreateController(List<LabResult> labResults)
        //{
        //    var repo = new InMemoryLabResultRepository(labResults);
        //    var controller = new LabResultController(repo);
        //    return controller;
        //}

        private List<LabResult> CreateTestLabResults(int count)
        {
            var labResults = new List<LabResult>();
            for (var i = 1; i <= count; i++)
            {
                labResults.Add(new LabResult()
                {
                    Id = i,
                    TestType = $"TestType {i}",
                    Result = $"Result {i}",
                    PatientId = i,
                    TimeOfTest = DateTime.Now.AddDays(-i),
                    LabName = $"LabName {i}",
                    OrderedByProvider = $"OrderedByProvider {i}",
                    Measurement = 1 / i,
                    MeasurementUnit = $"Unit {i}"
                });
            }
            return labResults;
        }

        private List<Patient> CreateTestPatients(int count)
        {
            var patients = new List<Patient>();
            for (var i = 1; i <= count; i++)
            {
                patients.Add(new Patient()
                {
                    Id = i,
                    FirstName = $"FirstName {i}",
                    LastName = $"LastName {i}",
                    SexAtBirth = $"Sex {i}",
                    DateOfBirth = DateTime.Today.AddDays(-i),
                    Height = 50 + i,
                    Weight = 100 + i,
                });
            }
            return patients;
        }
        private Patient CreateTestPatient(int id)
        {
            return new Patient()
            {
                Id = id,
                FirstName = $"FirstName {id}",
                LastName = $"LastName {id}",
                SexAtBirth = $"Sex {id}",
                DateOfBirth = DateTime.Today.AddDays(-id),
                Height = 50,
                Weight = 100,
            };
        }
    }
}
