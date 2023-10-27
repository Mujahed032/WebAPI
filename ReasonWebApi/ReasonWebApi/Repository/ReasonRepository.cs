using Microsoft.AspNetCore.Mvc;
using ReasonWebApi.Dto;
using ReasonWebApi.Interface;
using ReasonWebApi.Models;
using Serilog;
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



        public async Task<IEnumerable<Reason>> GetAllReasonsAsync(string reasonName = null, int pageNumber = 1, int pageSize = 10, string sortBy = "ReasonId", string sortOrder = "asc")
        {
            var reasons = new List<Reason>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string sqlQuery = "SELECT * FROM tblReason WHERE IsDeleted = 0";
                if (!string.IsNullOrEmpty(reasonName))
                {
                    sqlQuery += " AND ReasonName LIKE '%' + @ReasonName + '%'";
                }
                sqlQuery += $" ORDER BY {sortBy} {sortOrder} OFFSET {pageSize * (pageNumber - 1)} ROWS FETCH NEXT {pageSize} ROWS ONLY";

                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    if (!string.IsNullOrEmpty(reasonName))
                    {
                        command.Parameters.AddWithValue("@ReasonName", reasonName);
                    }
                    command.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                    command.Parameters.AddWithValue("@PageSize", pageSize);

                    try
                    {
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

                        Log.Information("Retrieved reasons with pagination and filtering. ReasonName: {ReasonName}, PageNumber: {PageNumber}, PageSize: {PageSize}, SortBy: {SortBy}, SortOrder: {SortOrder}", reasonName, pageNumber, pageSize, sortBy, sortOrder);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Error occurred in GetAllReasonsAsync. ReasonName: {ReasonName}, PageNumber: {PageNumber}, PageSize: {PageSize}, SortBy: {SortBy}, SortOrder: {SortOrder}", reasonName, pageNumber, pageSize, sortBy, sortOrder);
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

                        if ((bool)reader["IsDeleted"])
                        {
                            Log.Information("Record has been deleted.");
                            return new Reason { ReasonName = "Record has been deleted." };
                        }
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

            Log.Information("No reason found with the given ID.");
            return new Reason { ReasonName = "No reason found with the given ID." };
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
