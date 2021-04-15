using APIDevelopmentChallenge.Models;

namespace APIDevelopmentChallenge.Repositories
{
    public interface ILabResultRepository
    {
        void Add(LabResult labResult);
        LabResult GetById(int id);
    }
}