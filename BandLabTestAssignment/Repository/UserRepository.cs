using BandLabTestAssignment.Common.Models;
using BandLabTestAssignment.Data;
using BandLabTestAssignment.Repository.Interfaces;

namespace BandLabTestAssignment.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
