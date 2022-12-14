using AwesomeChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeChat.Persistence.Mapping
{
    internal class UserMapper : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {

            builder.ToTable("User");
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).HasDefaultValueSql("NEWID()");
            builder.Property(p => p.Firstname).HasColumnType("nvarchar(40)");
            builder.Property(p => p.Lastname).HasColumnType("nvarchar(40)");
            builder.Property(p => p.Password).HasColumnType("nvarchar(40)");
            builder.Property(p => p.EmailAdress).HasColumnType("nvarchar(40)");
            builder.Property(p => p.PlaceOfBirth).HasColumnType("nvarchar(40)");
            builder.Property(p => p.BirthOfDate).HasColumnType("DateTime");
            builder.Property(p => p.Gender).HasColumnType("int");
            builder.Property(p => p.isActive).HasDefaultValue(true);
        }
    }
}
