using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace APIDevelopmentChallenge.Repositories
{
    /// <summary>
    /// Base repostory class for interacting with the SQL database
    /// </summary>
    public abstract class BaseRespository
    {
        private readonly string _connectionString;
        public BaseRespository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        protected SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }
    }
}
