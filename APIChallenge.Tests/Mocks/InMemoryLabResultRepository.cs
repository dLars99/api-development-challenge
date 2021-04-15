using System.Collections.Generic;
using APIDevelopmentChallenge.Models;
using APIDevelopmentChallenge.Repositories;
using System.Linq;

namespace APIDevelopmentChallenge.Tests.Mocks
{
    class InMemoryLabResultRepository : ILabResultRepository
    {
        private readonly List<LabResult> _data;
        public List<LabResult> InternalData
        {
            get
            {
                return _data;
            }
        }

        public InMemoryLabResultRepository(List<LabResult> startingData)
        {
            _data = startingData;
        }

        public void Add(LabResult labResult)
        {
            var lastLabResult = _data.Last();
            labResult.Id = lastLabResult.Id + 1;
            _data.Add(labResult);
        }

        public LabResult GetById(int id)
        {
            return _data.FirstOrDefault(lr => lr.Id == id);
        }

        public List<LabResult> GetByPatientId(int id)
        {
            var labResults = _data.Where(lr => lr.PatientId == id).ToList();

            return labResults;
        }

        public void Update(LabResult labResult)
        {
            var currentLabResult = _data.FirstOrDefault(p => p.Id == labResult.Id);
            if (currentLabResult == null)
            {
                return;
            }

            currentLabResult.TestType = labResult.TestType;
            currentLabResult.Result = labResult.Result;
            currentLabResult.PatientId = labResult.PatientId;
            currentLabResult.TimeOfTest = labResult.TimeOfTest;
            currentLabResult.EnteredTime = labResult.EnteredTime;
            currentLabResult.LabName = labResult.LabName;
            currentLabResult.OrderedByProvider = labResult.OrderedByProvider;
            currentLabResult.Measurement = labResult.Measurement;
            currentLabResult.MeasurementUnit = labResult.MeasurementUnit;
        }

    }
}
