using BandLabTestAssignment.Common.Models;

namespace BandLabTestAssignment.Repository.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<Comment> GetCommentWithUserAsync(int id);
    }
}
