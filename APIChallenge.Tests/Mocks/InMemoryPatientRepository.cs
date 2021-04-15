using System;
using System.Collections.Generic;
using APIDevelopmentChallenge.Models;
using APIDevelopmentChallenge.Repositories;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIDevelopmentChallenge.Tests.Mocks
{
    class InMemoryPatientRepository : IPatientRepository
    {
        private readonly List<Patient> _data;
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
    }
}
