using APIDevelopmentChallenge.Models;
using System.Collections.Generic;

namespace APIDevelopmentChallenge.Repositories
{
    /// <summary>
    /// Interface to outline methods of a LabResultRepository
    /// </summary>
    public interface ILabResultRepository
    {
        void Add(LabResult labResult);
        LabResult GetById(int id);
        List<LabResult> GetByPatientId(int id);
        void Update(LabResult labResult);
        void Delete(int id);
    }
}