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
                Patient = CreateTestPatient(999),
                PatientId = 999,
                TimeOfTest = DateTime.Now.AddDays(-2),
                LabName = "LabName",
                OrderedByProvider = "OrderedByProvider",
                Measurement = 1,
                MeasurementUnit = "Unit"
            };

            controller.Post(newLabResult);

            Assert.Equal(labResultCount + 1, repo.InternalData.Count);
        }

        [Fact]
        public void Get_By_Id_Returns_LabResult_Record()
        {
            var labResultId = 99;
            var labResultCount = 20;
            var patients = CreateTestPatients(20);
            var labResults = CreateTestLabResults(labResultCount);
            labResults[0].Id = labResultId;

            var controller = CreateController(labResults, patients);

            var result = controller.Get(labResultId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualLabResult = Assert.IsType<LabResult>(okResult.Value);

            Assert.Equal(labResultId, actualLabResult.Id);
        }

        [Fact]
        public void Get_By_Patient_Id_Returns_Matching_LabResults()
        {
            var patientId = 99;
            var labResultCount = 20;
            var patients = CreateTestPatients(20);
            patients[0].Id = patientId;
            var labResults = CreateTestLabResults(labResultCount);
            labResults[0].PatientId = patientId;

            var repo = new InMemoryLabResultRepository(labResults);
            var patientRepo = new InMemoryPatientRepository(patients);
            var controller = new LabResultController(repo, patientRepo);

            var result = controller.GetByPatient(patientId);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualLabResult = Assert.IsType<List<LabResult>>(okResult.Value);

            Assert.All(actualLabResult, labResult => Assert.Equal(labResult.PatientId, patientId));
        }

        [Fact]
        public void Put_Method_Returns_BadRequest_When_Ids_Do_Not_Match()
        {
            var labResultId = 99;
            var patients = CreateTestPatients(5);
            var labResults = CreateTestLabResults(5);
            labResults[0].Id = labResultId;

            var controller = CreateController(labResults, patients);

            var labResultToUpdate = new LabResult()
            {
                Id = labResultId,
                TestType = "TestType",
                Result = "Result",
                Patient = CreateTestPatient(999),
                PatientId = 999,
                TimeOfTest = DateTime.Now.AddDays(-2),
                LabName = "LabName",
                OrderedByProvider = "OrderedByProvider",
                Measurement = 1,
                MeasurementUnit = "Unit"
            };
            var someOtherLabResultId = labResultId + 1;

            var result = controller.Put(someOtherLabResultId, labResultToUpdate);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Put_Method_Updates_A_Patient()
        {
            var labResultId = 99;
            var patients = CreateTestPatients(5);
            var labResults = CreateTestLabResults(5);
            patients[0].Id = labResultId;

            var repo = new InMemoryLabResultRepository(labResults);
            var patientRepo = new InMemoryPatientRepository(patients);
            var controller = new LabResultController(repo, patientRepo);

            var labResultToUpdate = new LabResult()
            {
                Id = labResultId,
                TestType = "TestType",
                Result = "Result",
                Patient = CreateTestPatient(999),
                PatientId = 999,
                TimeOfTest = DateTime.Now.AddDays(-2),
                LabName = "LabName",
                OrderedByProvider = "OrderedByProvider",
                Measurement = 1,
                MeasurementUnit = "Unit"
            };

            controller.Put(labResultId, labResultToUpdate);

            var labResultFromDb = repo.InternalData.FirstOrDefault(lr => lr.Id == labResultId);
            Assert.NotNull(labResultFromDb);

            Assert.Equal(labResultToUpdate.TestType, labResultFromDb.TestType);
            Assert.Equal(labResultToUpdate.Result, labResultFromDb.Result);
            Assert.Equal(labResultToUpdate.PatientId, labResultFromDb.PatientId);
            Assert.Equal(labResultToUpdate.TimeOfTest, labResultFromDb.TimeOfTest);
            Assert.Equal(labResultToUpdate.EnteredTime, labResultFromDb.EnteredTime);
            Assert.Equal(labResultToUpdate.LabName, labResultFromDb.LabName);
            Assert.Equal(labResultToUpdate.OrderedByProvider, labResultFromDb.OrderedByProvider);
            Assert.Equal(labResultToUpdate.Measurement, labResultFromDb.Measurement);
            Assert.Equal(labResultToUpdate.MeasurementUnit, labResultFromDb.MeasurementUnit);
        }

        private LabResultController CreateController(List<LabResult> labResults, List<Patient> patients)
        {
            var repo = new InMemoryLabResultRepository(labResults);
            var patientRepo = new InMemoryPatientRepository(patients);
            var controller = new LabResultController(repo, patientRepo);
            return controller;
        }

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
                    Patient = CreateTestPatient(i),
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
