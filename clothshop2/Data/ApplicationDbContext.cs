using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace clothshop2.Data
{
    public class ApplicationDbContext : IdentityDbContext<Areas.Identity.Data.clothshop2User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
