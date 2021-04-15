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

    }
}
