using APIDevelopmentChallenge.Models;
using GearPatch.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIDevelopmentChallenge.Repositories
{
    public class PatientRepository : BaseRespository, IPatientRepository
    {
        public PatientRepository(IConfiguration configuration) : base(configuration) { }

        /// <summary>
        /// Method to add a new patient into the database
        /// </summary>
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
                        patientList.Add(new Patient()
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
                        });
                    }
                    reader.Close();
                    return patientList;
                }
            }
        }
    }
}
