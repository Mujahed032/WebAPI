using ReasonWebApi.Dto;
using ReasonWebApi.Models;

namespace ReasonWebApi.Interface
{
    public interface IReasonRepository
    {
        Task<IEnumerable<Reason>> GetAllReasonsAsync(string reasonName = null, int pageNumber = 1, int pageSize = 10, string sortBy = "ReasonId", string sortOrder = "asc");
        Task<Reason> GetReasonByIdAsync(int reasonId);
       
        
        Task<Reason> AddReasonAsync(Reason reason);
        Task<Reason> UpdateReasonAsync(int Id, Reason reason);
        Task<bool> DeleteReasonAsync(int reasonId, string deletedBy);
       
    }
}
