using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuberDinner.Domain.HostAggregate.ValueObjects;
using BuberDinner.Domain.MenuAggregate;
using BuberDinner.Domain.MenuAggregate.Entities;
using BuberDinner.Domain.MenuAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static System.Collections.Specialized.BitVector32;

namespace BuberDinner.Infrastructure.Persistence.Configurations;
public class MenuConfigurations : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        ConfigureMenusTable(builder);
        ConfigureMenusSectionsTable(builder);
        ConfigureMenuDinnerIdsTable(builder);
        ConfigureMenuReviewIdsTable(builder);
    }

    private void ConfigureMenusTable(EntityTypeBuilder<Menu> builder)
    {
        builder.ToTable("Menus"); // this is just to be explicit

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .ValueGeneratedNever()
            .HasConversion(
            id => id.Value,
            value => MenuId.Create(value));

        builder.Property(m => m.Name).HasMaxLength(100);

        builder.Property(m => m.Description).HasMaxLength(100);

        // average rating is a value object and we will use table sharing/splitting to allow it to share the same table as Menu
        // To do that, we will flatten it
        //builder.OwnsOne(m => m.AverageRating, ab =>
        //{
        //    ab.Property(a => a.Value).HasColumnName("Value"); // we can specify the names explicitly like this
        //});
        builder.OwnsOne(m => m.AverageRating);

        builder.Property(m => m.HostId)
            .ValueGeneratedNever()
            .HasConversion(
            id => id.Value,
            value => HostId.Create(value));
    }

    private void ConfigureMenusSectionsTable(EntityTypeBuilder<Menu> builder)
    {
        builder.OwnsMany(m => m.Sections, sb =>
        {
            sb.ToTable("MenuSections");

            sb.WithOwner().HasForeignKey("MenuId");

            // to define the PK as a composite key, we can do this:
            //sb.HasKey(s => new[] {s.Id, etc});
            // but since its a shadow property and not accessible here, we can do:
            sb.HasKey("Id", "MenuId");

            sb.Property(s => s.Id)
              .HasColumnName("MenuSectionId")
              .ValueGeneratedNever()
              .HasConversion(
            id => id.Value,
            value => MenuSectionId.Create(value));

            sb.Property(s => s.Name).HasMaxLength(100);

            sb.Property(s => s.Description).HasMaxLength(100);

            // Configure MenuItems table here:
            sb.OwnsMany(s => s.Items, ib =>
            {
                ib.ToTable("MenuItems");

                ib.WithOwner().HasForeignKey("MenuSectionId", "MenuId");

                ib.HasKey(nameof(MenuItem.Id), "MenuSectionId", "MenuId");

                ib.Property(i => i.Id)
                    .HasColumnName("MenuItemId")
                    .ValueGeneratedNever()
                    .HasConversion(
                    id => id.Value,
                    value => MenuItemId.Create(value));

                ib.Property(s => s.Name).HasMaxLength(100);

                ib.Property(s => s.Description).HasMaxLength(100);
            });
            // The FindNavigation as used below is not available in the nested builder. so do this instead.
            sb.Navigation(s => s.Items).Metadata.SetField("_items");
            sb.Navigation(s => s.Items).UsePropertyAccessMode(PropertyAccessMode.Field);
        });
        //EF cannot populate the Sections property, as it is readonly. We need to populate the underlying field. Need to do the same thing for items above
        builder.Metadata.FindNavigation(nameof(Menu.Sections))!.SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private void ConfigureMenuDinnerIdsTable(EntityTypeBuilder<Menu> builder)
    {
        builder.OwnsMany(m => m.DinnerIds, dib =>
        {
            dib.ToTable("MenuDinnerIds");

            dib.WithOwner().HasForeignKey("MenuId");

            dib.HasKey("Id"); // shadow property, autoincrementing value

            dib.Property(d => d.Value)
                .HasColumnName("DinnerId")
                .ValueGeneratedNever();
        });
        builder.Metadata.FindNavigation(nameof(Menu.DinnerIds))!.SetPropertyAccessMode(PropertyAccessMode.Field);
    }


    private void ConfigureMenuReviewIdsTable(EntityTypeBuilder<Menu> builder)
    {
        builder.OwnsMany(m => m.MenuReviewIds, rib =>
        {
            rib.ToTable("MenuReviewIds");

            rib.WithOwner().HasForeignKey("MenuId");

            rib.HasKey("Id"); // shadow property, autoincrementing value

            rib.Property(d => d.Value)
                .HasColumnName("ReviewId")
                .ValueGeneratedNever();
        });
        builder.Metadata.FindNavigation(nameof(Menu.MenuReviewIds))!.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
