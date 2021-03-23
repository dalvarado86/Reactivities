using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IDataContext
    {
        DbSet<Activity> Activities { get; set; }
        DbSet<ActivityAttendee> ActivityAttendees { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancelationToken);
    }
}