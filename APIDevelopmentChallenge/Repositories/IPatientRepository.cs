using APIDevelopmentChallenge.Models;
using System.Collections.Generic;

namespace APIDevelopmentChallenge.Repositories
{
    public interface IPatientRepository
    {
        void Add(Patient patient);
        List<Patient> Get();
    }
}