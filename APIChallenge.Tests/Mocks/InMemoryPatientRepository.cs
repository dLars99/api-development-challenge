using System.Collections.Generic;
using APIDevelopmentChallenge.Models;
using APIDevelopmentChallenge.Repositories;
using System.Linq;
using System;

namespace APIDevelopmentChallenge.Tests.Mocks
{
    class InMemoryPatientRepository : IPatientRepository
    {
        private readonly List<Patient> _data;
        private readonly List<LabResult> _labResultData;

        public List<Patient> InternalData
        {
            get
            {
                return _data;
            }
        }

        public InMemoryPatientRepository(List<Patient> startingData)
        {
            _data = startingData;
            _labResultData = CreateTestLabResults(_data.Count);
        }

        public void Add(Patient patient)
        {
            var lastPatient = _data.Last();
            patient.Id = lastPatient.Id + 1;
            _data.Add(patient);
        }

        public List<Patient> GetAll()
        {
            return _data;
        }

        public Patient GetById(int id)
        {
            return _data.FirstOrDefault(p => p.Id == id);
        }

        public void Update(Patient patient)
        {
            var currentPatient = _data.FirstOrDefault(p => p.Id == patient.Id);
            if (currentPatient == null)
            {
                return;
            }

            currentPatient.FirstName = patient.FirstName;
            currentPatient.MiddleName = patient.MiddleName;
            currentPatient.LastName = patient.LastName;
            currentPatient.SexAtBirth = patient.SexAtBirth;
            currentPatient.DateOfBirth = patient.DateOfBirth;
            currentPatient.Height = patient.Height;
            currentPatient.Weight = patient.Weight;
        }

        public void Delete(int id)
        {
            var patientToDelete = _data.FirstOrDefault(p => p.Id == id);
            if (patientToDelete == null)
            {
                return;
            }

            _data.Remove(patientToDelete);
        }

        public List<Patient> GetByLabs(string query, DateTime startDate, DateTime endDate)
        {
            var matchingLabResults = _labResultData.Where(lr => lr.TestType == query && lr.TimeOfTest > startDate && lr.TimeOfTest < endDate).ToList();
            var patientIds = matchingLabResults.Select(mlr => mlr.PatientId).ToList();
            return _data.Where(p => patientIds.Contains(p.Id)).ToList();
        }

        private List<LabResult> CreateTestLabResults(int count)
        {
            var labResults = new List<LabResult>();
            for (var i = 1; i <= count; i++)
            {
                labResults.Add(new LabResult()
                {
                    Id = i,
                    TestType = i % 2 == 0 ? "TestType-2" : "TestType-1",
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
    }
}
