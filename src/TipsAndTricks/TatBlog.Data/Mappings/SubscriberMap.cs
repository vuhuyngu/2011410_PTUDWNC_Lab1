using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TatBlog.Core.Entities;

namespace TatBlog.Data.Mappings;

public class SubscriberMap
{
    public void Configure(EntityTypeBuilder<Subscriber> builder)
    {
        builder.ToTable("Subscribers");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.CancelReason)
            .HasMaxLength(500);

        builder.Property(a => a.Email)
            .HasMaxLength(150);
        builder.Property(a => a.SubDated)
            .HasColumnType("datetime");
        builder.Property(a => a.UnsubDated)
            .HasColumnType("datetime");
        builder.Property(a => a.AdminNotes)
            .HasMaxLength(500);
    }
}