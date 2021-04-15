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
    public class PatientControllerTests
    {
        [Fact]
        public void Post_Method_Adds_A_New_Patient()
        {
            var patientCount = 20;
            var patients = CreateTestPatients(patientCount);

            var repo = new InMemoryPatientRepository(patients);
            var controller = new PatientController(repo);

            var newPatient = new Patient()
            {

            };

            controller.Post(newPatient);

            Assert.Equal(patientCount + 1, repo.InternalData.Count);
        }

        [Fact]
        public void Get_Returns_All_Patients()
        {  
            var patientCount = 20;
            var patients = CreateTestPatients(patientCount);

            var controller = CreateController(patients);

            var result = controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualPatients = Assert.IsType<List<Patient>>(okResult.Value);

            Assert.Equal(patientCount, actualPatients.Count);
            Assert.Equal(patients, actualPatients);
        }

        [Fact]
        public void Get_By_Id_Returns_Patient_Record()
        {
            var patientId = 99;
            var patientCount = 20;
            var patients = CreateTestPatients(patientCount);
            patients[0].Id = patientId;

            var controller = CreateController(patients);

            var result = controller.Get(patientId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualPatient = Assert.IsType<Patient>(okResult.Value);

            Assert.Equal(patientId, actualPatient.Id);
        }

        [Fact]
        public void Get_By_Id_Returns_Not_Found_If_Id_Not_Found()
        {
            var patientId = 99;
            var patientCount = 10;
            var patients = CreateTestPatients(patientCount);

            var controller = CreateController(patients);

            var result = controller.Get(patientId);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Put_Method_Returns_BadRequest_When_Ids_Do_Not_Match()
        {
            var patientId = 99;
            var patients = CreateTestPatients(5);
            patients[0].Id = patientId;

            var controller = CreateController(patients);

            var patientToUpdate = new Patient()
            {
                Id = patientId,
                FirstName = "Updated!",
                MiddleName = "Updated!",
                LastName = "Updated!",
                SexAtBirth = "Updated!",
                DateOfBirth = DateTime.Today,
                Height = 50,
                Weight = 99,
            };
            var someOtherPatientId = patientId + 1;

            var result = controller.Put(someOtherPatientId, patientToUpdate);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Put_Method_Updates_A_Patient()
        {
            var patientId = 99;
            var patients = CreateTestPatients(5);
            patients[0].Id = patientId;

            var repo = new InMemoryPatientRepository(patients);
            var controller = new PatientController(repo);

            var patientToUpdate = new Patient()
            {
                Id = patientId,
                FirstName = "Updated!",
                MiddleName = "Updated!",
                LastName = "Updated!",
                SexAtBirth = "Updated!",
                DateOfBirth = DateTime.Today,
                Height = 50,
                Weight = 99,
            };

            controller.Put(patientId, patientToUpdate);

            var patientFromDb = repo.InternalData.FirstOrDefault(p => p.Id == patientId);
            Assert.NotNull(patientFromDb);

            Assert.Equal(patientToUpdate.FirstName, patientFromDb.FirstName);
            Assert.Equal(patientToUpdate.MiddleName, patientFromDb.MiddleName);
            Assert.Equal(patientToUpdate.LastName, patientFromDb.LastName);
            Assert.Equal(patientToUpdate.SexAtBirth, patientFromDb.SexAtBirth);
            Assert.Equal(patientToUpdate.DateOfBirth, patientFromDb.DateOfBirth);
            Assert.Equal(patientToUpdate.Height, patientFromDb.Height);
            Assert.Equal(patientToUpdate.Weight, patientFromDb.Weight);
        }


        private PatientController CreateController(List<Patient> patients)
        {
            var repo = new InMemoryPatientRepository(patients);
            var controller = new PatientController(repo);
            return controller;
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
    }
}
