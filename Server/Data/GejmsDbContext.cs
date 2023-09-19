using Gejms.Server.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gejms.Server.Data;

public class GejmsDbContext : IdentityDbContext<User>
{
    public GejmsDbContext(DbContextOptions<GejmsDbContext> options) : base(options)
    {
    }

    public DbSet<TetrisScore> TetrisScores { get; set; }
    public DbSet<CheatingTries> Cheaters { get; set; }
}
