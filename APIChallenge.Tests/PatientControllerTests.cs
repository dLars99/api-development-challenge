using System;
using APIDevelopmentChallenge.Tests.Mocks;
using APIDevelopmentChallenge.Models;
using Xunit;

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

        private List<Patient> CreateTestPatients(int count)
        {
            var posts = new List<Patient>();
            for (var i = 1; i <= count; i++)
            {
                posts.Add(new Patient()
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
            return posts;
        }
    }
}
