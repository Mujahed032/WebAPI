using ReasonWebApi.Dto;
using ReasonWebApi.Models;

namespace ReasonWebApi.Interface
{
    public interface IReasonRepository
    {
        Task<IEnumerable<Reason>> GetAllReasonsAsync();
        Task<Reason> GetReasonByIdAsync(int reasonId);
       
        
        Task<Reason> AddReasonAsync(Reason reason);
        Task<Reason> UpdateReasonAsync(int Id, Reason reason);
        Task<bool> DeleteReasonAsync(int reasonId, string deletedBy);
        Task<IEnumerable<Reason>> SearchReasonByNameAsync(string reasonName);
    }
}
