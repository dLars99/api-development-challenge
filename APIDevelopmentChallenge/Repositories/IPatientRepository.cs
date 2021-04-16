using APIDevelopmentChallenge.Models;
using System;
using System.Collections.Generic;

namespace APIDevelopmentChallenge.Repositories
{
    public interface IPatientRepository
    {
        /// <summary>
        /// Interface to outline methods in the Patient Repository
        /// </summary>
        /// <param name="patient"></param>
        void Add(Patient patient);
        List<Patient> GetAll();
        Patient GetById(int id);
        void Update(Patient patient);
        void Delete(int id);
        List<Patient> GetByLabs(string query, DateTime startDate, DateTime endDate);
    }
}