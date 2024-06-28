using BandLabTestAssignment.Common.Models;
using BandLabTestAssignment.Data;
using BandLabTestAssignment.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BandLabTestAssignment.Repository
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Comment> GetCommentWithUserAsync(int id)
        {
            var comment = await _context.Comments.Include(x => x.Creator).FirstOrDefaultAsync(x => x.Id == id);

            return comment;
        }
    }
}
