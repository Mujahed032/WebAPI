using Microsoft.AspNetCore.Mvc;
using ReasonWebApi.Dto;
using ReasonWebApi.Interface;
using ReasonWebApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace ReasonWebApi.Repository
{
    public class ReasonRepository : IReasonRepository
    {
        private readonly string _connectionString;

        public ReasonRepository(string connectionString)
        {
            _connectionString = connectionString;
        }


        public async Task<bool> DeleteReasonAsync(int reasonId, string deletedBy)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand("uspDelete", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@ReasonId", reasonId);
                command.Parameters.AddWithValue("@DeletedBy", deletedBy);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                return rowsAffected > 0;
            }
        }

        public async Task<IEnumerable<Reason>> GetAllReasonsAsync()
        {
            var reasons = new List<Reason>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT * FROM tblReason", connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var reason = new Reason
                        {
                            ReasonId = (int)reader["ReasonId"],
                            IsPublished = (bool)reader["IsPublished"],
                            PrimaryReason = (bool)reader["PrimaryReason"],
                            ReasonName = (string)reader["ReasonName"],
                            ReasonCode = (int)reader["ReasonCode"],
                            ReasonType = (string)reader["ReasonType"],
                            ThirdPartyNumber = (int)reader["ThirdPartyNumber"],
                            Description = (string)reader["Description"],
                            PublishedBy = reader["PublishedBy"] != DBNull.Value ? (string)reader["PublishedBy"] : null,
                            DatePublished = reader["DatePublished"] != DBNull.Value ? (DateTime)reader["DatePublished"] : DateTime.MinValue,
                            DisplayOnWeb = reader["DisplayOnWeb"] != DBNull.Value && (bool)reader["DisplayOnWeb"],
                            SortOrder = reader["SortOrder"] != DBNull.Value ? (int)reader["SortOrder"] : 0,
                            Tag = reader["Tag"] != DBNull.Value ? (string)reader["Tag"] : null,
                            Comments = reader["Comments"] != DBNull.Value ? (string)reader["Comments"] : null,
                            IPAddress = (string)reader["IPAddress"],
                            CreatedBy = (string)reader["CreatedBy"],
                            DateCreated = reader["DateCreated"] != DBNull.Value ? (DateTime)reader["DateCreated"] : DateTime.MinValue,
                            UpdatedBy = reader["UpdatedBy"] != DBNull.Value ? (string)reader["UpdatedBy"] : null,
                            LastUpdated = reader["LastUpdated"] != DBNull.Value ? (DateTime)reader["LastUpdated"] : DateTime.MinValue,
                            IsDeleted = (bool)reader["IsDeleted"],
                            DeletedBy = reader["DeletedBy"] != DBNull.Value ? (string)reader["DeletedBy"] : null,
                            DateDeleted = reader["DateDeleted"] != DBNull.Value ? (DateTime)reader["DateDeleted"] : DateTime.MinValue
                        };

                        reasons.Add(reason);
                    }
                }
            }

            return reasons;
        }

        public async Task<Reason> GetReasonByIdAsync(int reasonId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand("uspSearchReasonById", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@ReasonId", reasonId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Reason
                        {
                            ReasonId = (int)reader["ReasonId"],
                            IsPublished = (bool)reader["IsPublished"],
                            PrimaryReason = (bool)reader["PrimaryReason"],
                            ReasonName = (string)reader["ReasonName"],
                            ReasonCode = (int)reader["ReasonCode"],
                            ReasonType = (string)reader["ReasonType"],
                            ThirdPartyNumber = (int)reader["ThirdPartyNumber"],
                            Description = (string)reader["Description"],
                            PublishedBy = reader["PublishedBy"] != DBNull.Value ? (string)reader["PublishedBy"] : null,
                            UpdatedBy = reader["UpdatedBy"] != DBNull.Value ? (string)reader["UpdatedBy"] : null
                        };
                    }
                }
            }
            
            return new Reason { ReasonName = "No reason found with the given ID." };
        }

        public async Task<IEnumerable<Reason>> SearchReasonByNameAsync(string reasonName)
        {
            var reasons = new List<Reason>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand("uspSearchReasonByName", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@ReasonName", reasonName);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var reason = new Reason
                        {
                            ReasonId = (int)reader["ReasonId"],
                            IsPublished = (bool)reader["IsPublished"],
                            PrimaryReason = (bool)reader["PrimaryReason"],
                            ReasonName = (string)reader["ReasonName"],
                            ReasonCode = (int)reader["ReasonCode"],
                            ReasonType = (string)reader["ReasonType"],
                            ThirdPartyNumber = (int)reader["ThirdPartyNumber"],
                            Description = (string)reader["Description"],
                            PublishedBy = reader["PublishedBy"] != DBNull.Value ? (string)reader["PublishedBy"] : null,
                            UpdatedBy = reader["UpdatedBy"] != DBNull.Value ? (string)reader["UpdatedBy"] : null
                        };

                        reasons.Add(reason);
                    }
                }
            }

            return reasons;
        }

      

        public async Task<Reason> AddReasonAsync(Reason reason)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("uspInsertReason", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@IsPublished", reason.IsPublished);
                command.Parameters.AddWithValue("@PrimaryReason", reason.PrimaryReason);
                command.Parameters.AddWithValue("@ReasonName", reason.ReasonName);
                command.Parameters.AddWithValue("@ReasonCode", reason.ReasonCode);
                command.Parameters.AddWithValue("@ReasonType", reason.ReasonType);
                command.Parameters.AddWithValue("@ThirdPartyNumber", reason.ThirdPartyNumber);
                command.Parameters.AddWithValue("@Description", reason.Description);
                command.Parameters.AddWithValue("@PublishedBy", reason.PublishedBy);
                command.Parameters.AddWithValue("@CreatedBy", reason.CreatedBy);

                // Add other parameters as needed

                await command.ExecuteNonQueryAsync();
            }

            return reason;
        }

        public async Task<Reason> UpdateReasonAsync(int Id,Reason reason)
        {
            
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var command = new SqlCommand("uspUpdateReason", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@ReasonId", Id);
                    command.Parameters.AddWithValue("@IsPublished", reason.IsPublished);
                    command.Parameters.AddWithValue("@PrimaryReason", reason.PrimaryReason);
                    command.Parameters.AddWithValue("@ReasonName", reason.ReasonName);
                    command.Parameters.AddWithValue("@ReasonCode", reason.ReasonCode);
                    command.Parameters.AddWithValue("@ReasonType", reason.ReasonType);
                    command.Parameters.AddWithValue("@ThirdPartyNumber", reason.ThirdPartyNumber);
                    command.Parameters.AddWithValue("@Description", reason.Description);
                    command.Parameters.AddWithValue("@PublishedBy", reason.PublishedBy);
                    command.Parameters.AddWithValue("@UpdatedBy", reason.UpdatedBy);

                    await command.ExecuteNonQueryAsync();
                }

                return reason;
            }

        
    }
}
