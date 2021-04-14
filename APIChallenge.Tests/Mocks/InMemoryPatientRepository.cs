using System;
using System.Collections.Generic;
using APIDevelopmentChallenge.Models;
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
    }
}
