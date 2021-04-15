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

    }
}
