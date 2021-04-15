using APIDevelopmentChallenge.Models;
using GearPatch.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace APIDevelopmentChallenge.Repositories
{
    public class PatientRepository : BaseRespository, IPatientRepository
    {
        public PatientRepository(IConfiguration configuration) : base(configuration) { }

        /// <summary>
        /// Method to add a new patient into the database
        /// </summary>
        /// <param name="patient">The patient model to save to the database</param>
        public void Add(Patient patient)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Patient (FirstName, MiddleName, LastName, SexAtBirth, DateOfBirth, Height, Weight,
                                                   InsuranceCompany, MemberId, GroupId, IsPolicyHolder)
                 OUTPUT INSERTED.ID
                             VALUES (@FirstName, @MiddleName, @LastName, @SexAtBirth, @DateOfBirth, @Height, @Weight,
                                     @InsuranceCompany, @MemberId, @GroupId, @IsPolicyHolder);";
                    DbUtils.AddParameter(cmd, "@FirstName", patient.FirstName);
                    DbUtils.AddParameter(cmd, "@MiddleName", patient.MiddleName);
                    DbUtils.AddParameter(cmd, "@LastName", patient.LastName);
                    DbUtils.AddParameter(cmd, "@SexAtBirth", patient.SexAtBirth);
                    DbUtils.AddParameter(cmd, "@DateOfBirth", patient.DateOfBirth);
                    DbUtils.AddParameter(cmd, "@Height", patient.Height);
                    DbUtils.AddParameter(cmd, "@Weight", patient.Weight);
                    DbUtils.AddParameter(cmd, "@InsuranceCompany", patient.InsuranceCompany);
                    DbUtils.AddParameter(cmd, "@MemberId", patient.MemberId);
                    DbUtils.AddParameter(cmd, "@GroupId", patient.GroupId);
                    DbUtils.AddParameter(cmd, "@IsPolicyHolder", patient.IsPolicyHolder);

                    patient.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Method to retrieve the full list of patient records
        /// </summary>
        public List<Patient> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, FirstName, MiddleName, LastName, SexAtBirth, DateOfBirth, Height, Weight,
                               InsuranceCompany, MemberId, GroupId, IsPolicyHolder
                          FROM Patient";
                    var patientList = new List<Patient>();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        patientList.Add(NewPatientFromDb(reader));
                    }
                    reader.Close();
                    return patientList;
                }
            }
        }

        /// <summary>
        /// Method to retrieve a single patient record by the patient's id
        /// </summary>
        /// <param name="id">The id of the patient to be found</param>
        /// <returns>An instance of a Patient using retrieved data</returns>
        public Patient GetById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, FirstName, MiddleName, LastName, SexAtBirth, DateOfBirth, Height, Weight,
                               InsuranceCompany, MemberId, GroupId, IsPolicyHolder
                          FROM Patient
                         WHERE Id = @Id";
                    DbUtils.AddParameter(cmd, "@Id", id);
                    Patient patient = null;
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        patient = NewPatientFromDb(reader);
                    }
                    reader.Close();
                    return patient;
                }
            }

        }

        /// <summary>
        /// Method to update an existing patient record in the database
        /// </summary>
        /// <param name="id">The id of the patient to be updated</param>
        /// <param name="patient">The updated patient data to be saved to the database</param>
        public void Update(Patient patient)
        {
            using (var conn = Connection)
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Patient 
                                           SET FirstName = @FirstName,
                                               MiddleName = @MiddleName,
                                               LastName = @LastName,
                                               SexAtBirth = @SexAtBirth,
                                               DateOfBirth = @DateOfBirth,
                                               Height = @Height,
                                               Weight = @Weight,
                                               InsuranceCompany = @InsuranceCompany,
                                               MemberId = @MemberId,
                                               GroupId = @GroupId,
                                               IsPolicyHolder = @IsPolicyHolder
                                         WHERE Id = @Id;";

                    DbUtils.AddParameter(cmd, "@FirstName", patient.FirstName);
                    DbUtils.AddParameter(cmd, "@MiddleName", patient.MiddleName);
                    DbUtils.AddParameter(cmd, "@LastName", patient.LastName);
                    DbUtils.AddParameter(cmd, "@SexAtBirth", patient.SexAtBirth);
                    DbUtils.AddParameter(cmd, "@DateOfBirth", patient.DateOfBirth);
                    DbUtils.AddParameter(cmd, "@Height", patient.Height);
                    DbUtils.AddParameter(cmd, "@Weight", patient.Weight);
                    DbUtils.AddParameter(cmd, "@InsuranceCompany", patient.InsuranceCompany);
                    DbUtils.AddParameter(cmd, "@MemberId", patient.MemberId);
                    DbUtils.AddParameter(cmd, "@GroupId", patient.GroupId);
                    DbUtils.AddParameter(cmd, "@IsPolicyHolder", patient.IsPolicyHolder);
                    DbUtils.AddParameter(cmd, "@Id", patient.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Method to remove a patient from the database
        /// </summary>
        /// <param name="id">Id of the patient to be removed</param>
        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM LabResult WHERE PatientId=@Id;
                                        DELETE FROM Patient WHERE Id=@Id;";
                    DbUtils.AddParameter(cmd, "@Id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Method to create a new instance of a patient model with data
        /// retrieved from the database
        /// <param name="reader"></param>
        /// <returns>The new instance of a Patient model</returns>
        /// </summary>
        private Patient NewPatientFromDb(SqlDataReader reader)
        {
            return new Patient()
            {
                Id = DbUtils.GetInt(reader, "Id"),
                FirstName = DbUtils.GetString(reader, "FirstName"),
                MiddleName = DbUtils.GetString(reader, "MiddleName"),
                LastName = DbUtils.GetString(reader, "LastName"),
                SexAtBirth = DbUtils.GetString(reader, "SexAtBirth"),
                DateOfBirth = DbUtils.GetDateTime(reader, "DateOfBirth"),
                Height = DbUtils.GetInt(reader, "Height"),
                Weight = DbUtils.GetInt(reader, "Weight"),
                InsuranceCompany = DbUtils.GetString(reader, "InsuranceCompany"),
                MemberId = DbUtils.GetString(reader, "MemberId"),
                GroupId = DbUtils.GetString(reader, "GroupId"),
                IsPolicyHolder = DbUtils.GetNullableBool(reader, "IsPolicyHolder")
            };
        }
    }
}
