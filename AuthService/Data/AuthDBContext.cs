using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data
{
    public class AuthDBContext : IdentityDbContext<IdentityUser>
    {
        public AuthDBContext(DbContextOptions<AuthDBContext> options) : base(options)
        { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var pacientUserID = "5bf2857f-3d64-447f-824a-b8125a00a014";
            var asistentUserID = "212d6dca-8a13-45cc-9492-919ec0c39e8f";
            var medicUserID = "697479e1-3bd9-4c68-8866-0d86a042b168";
            var administratorUserID = "17403251-b4fa-4fa5-8bfb-72bca64cbdef";

            var roles = new List<IdentityRole>
            {
                new IdentityRole {
                    Id = pacientUserID,
                    ConcurrencyStamp = pacientUserID,
                    Name = "Pacient",
                    NormalizedName = "PACIENT"
                },
                new IdentityRole {
                    Id = asistentUserID,
                    ConcurrencyStamp = asistentUserID,
                    Name = "Asistent",
                    NormalizedName = "ASISTENT"
                },
                new IdentityRole {
                    Id = medicUserID,
                    ConcurrencyStamp = medicUserID,
                    Name = "Medic",
                    NormalizedName = "MEDIC"
                },
                new IdentityRole {
                    Id = administratorUserID,
                    ConcurrencyStamp = administratorUserID,
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                }
            };
            modelBuilder.Entity<IdentityRole>().HasData(roles);

        }
    }
}
