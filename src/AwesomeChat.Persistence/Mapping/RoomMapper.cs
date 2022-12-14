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
    public class RoomMapper : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.ToTable("Rooms");
            builder.Property(s => s.RoomName).IsRequired().HasMaxLength(100);
            builder.HasOne(s => s.Admin)
                .WithMany(u => u.Rooms)
                .IsRequired();
        }
    }
}
