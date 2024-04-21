using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public class AppContext(DbContextOptions<AppContext> options) : IdentityDbContext<UserEntity>(options)
{
    public string context { get; set; } = null!;
    public DbSet<AddressEntity> Addresses { get; set; }
}
