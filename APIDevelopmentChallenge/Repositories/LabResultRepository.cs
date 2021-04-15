using APIDevelopmentChallenge.Models;
using GearPatch.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace APIDevelopmentChallenge.Repositories
{
    public class LabResultRepository : BaseRespository, ILabResultRepository
    {
        public LabResultRepository(IConfiguration configuration) : base(configuration) { }

        /// <summary>
        /// Method to add a new lab result into the database
        /// </summary>
        /// <param name="labResult">The lab result model to save to the database</param>
        public void Add(LabResult labResult)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO LabResult (TestType, Result, PatientId, TimeOfTest, EnteredTime, LabName, OrderedByProvider,
                                                   Measurement, MeasurementUnit)
                 OUTPUT INSERTED.ID
                             VALUES (@TestType, @Result, @PatientId, @TimeOfTest, @EnteredTime, @LabName, @OrderedByProvider,
                                     @Measurement, @MeasurementUnit);";
                    DbUtils.AddParameter(cmd, "@TestType", labResult.TestType);
                    DbUtils.AddParameter(cmd, "@Result", labResult.Result);
                    DbUtils.AddParameter(cmd, "@PatientId", labResult.PatientId);
                    DbUtils.AddParameter(cmd, "@TimeOfTest", labResult.TimeOfTest);
                    DbUtils.AddParameter(cmd, "@EnteredTime", labResult.EnteredTime);
                    DbUtils.AddParameter(cmd, "@LabName", labResult.LabName);
                    DbUtils.AddParameter(cmd, "@OrderedByProvider", labResult.OrderedByProvider);
                    DbUtils.AddParameter(cmd, "@Measurement", labResult.Measurement);
                    DbUtils.AddParameter(cmd, "@MeasurementUnit", labResult.MeasurementUnit);

                    labResult.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Method to retrieve a single lab result record by the lab result's id
        /// </summary>
        /// <param name="id">The id of the lab result to be found</param>
        /// <returns>An instance of a Lab Result using retrieved data</returns>
        public LabResult GetById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT lr.Id AS LabResultId, lr.TestType, lr.Result, lr.PatientId, lr.TimeOfTest, lr.EnteredTime, 
                               lr.LabName, lr.OrderedByProvider, lr.Measurement, lr.MeasurementUnit,

                               p.Id AS PatientId, p.FirstName, p.MiddleName, p.LastName, p.SexAtBirth, p.DateOfBirth, 
                               p.Height, p.Weight, p.InsuranceCompany, p.MemberId, p.GroupId, p.IsPolicyHolder
                          FROM LabResult lr
                     LEFT JOIN Patient p on p.Id = lr.PatientId
                         WHERE lr.Id = @Id";
                    DbUtils.AddParameter(cmd, "@Id", id);
                    LabResult labResult = null;
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        labResult = NewLabResultFromDb(reader);
                    }
                    reader.Close();
                    return labResult;
                }
            }
        }

        public List<LabResult> GetByPatientId(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT lr.Id AS LabResultId, lr.TestType, lr.Result, lr.PatientId, lr.TimeOfTest, lr.EnteredTime, 
                               lr.LabName, lr.OrderedByProvider, lr.Measurement, lr.MeasurementUnit,

                               p.Id AS PatientId, p.FirstName, p.MiddleName, p.LastName, p.SexAtBirth, p.DateOfBirth, 
                               p.Height, p.Weight, p.InsuranceCompany, p.MemberId, p.GroupId, p.IsPolicyHolder
                          FROM LabResult lr
                     LEFT JOIN Patient p on p.Id = lr.PatientId
                         WHERE lr.PatientId = @Id";
                    DbUtils.AddParameter(cmd, "@Id", id);
                    var labResults = new List<LabResult>();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        labResults.Add(NewLabResultFromDb(reader));
                    }
                    reader.Close();
                    return labResults;
                }
            }
        }

        private LabResult NewLabResultFromDb(SqlDataReader reader)
        {
            return new LabResult()
            {
                Id = DbUtils.GetInt(reader, "LabResultId"),
                TestType = DbUtils.GetString(reader, "TestType"),
                Result = DbUtils.GetString(reader, "Result"),
                PatientId = DbUtils.GetInt(reader, "PatientId"),
                Patient = new Patient()
                {
                    Id = DbUtils.GetInt(reader, "PatientId"),
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
                },
                TimeOfTest = DbUtils.GetDateTime(reader, "TimeOfTest"),
                EnteredTime = DbUtils.GetDateTime(reader, "EnteredTime"),
                LabName = DbUtils.GetString(reader, "LabName"),
                OrderedByProvider = DbUtils.GetString(reader, "OrderedByProvider"),
                Measurement = DbUtils.GetNullableDecimal(reader, "Measurement"),
                MeasurementUnit = DbUtils.GetString(reader, "MeasurementUnit"),
            };
        }
    }
}
