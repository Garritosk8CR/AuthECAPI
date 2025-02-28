using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthECAPI.Models
{
    public class AppDbContext : IdentityDbContext
    {
        protected AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }
    }
}
