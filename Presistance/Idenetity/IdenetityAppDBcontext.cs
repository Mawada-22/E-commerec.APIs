using Domain.Entities.Idenetity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistance.Idenetity
{
    public class IdenetityAppDBcontext : IdentityDbContext<User>
    {
        public IdenetityAppDBcontext(DbContextOptions<IdenetityAppDBcontext>options):base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Address>().ToTable("Adresses");
            builder.Entity<User>()
           .HasOne(u => u.address)
           .WithOne(a => a.User)
           .HasForeignKey<Address>(a => a.UserId);
        }
    }
}
