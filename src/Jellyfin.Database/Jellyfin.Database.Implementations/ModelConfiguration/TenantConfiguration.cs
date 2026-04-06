using Jellyfin.Database.Implementations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jellyfin.Database.Implementations.ModelConfiguration
{
    /// <summary>
    /// Configures the database mapping for the <see cref="Tenant"/> entity.
    /// </summary>
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        /// <summary>
        /// Configures the entity properties and conversions.
        /// </summary>
        /// <param name="builder">The entity type builder.</param>
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.HasKey(t => t.Id);

            // Keep as GUIDs, no string conversion
            builder.Property(t => t.Id);
            builder.Property(t => t.AdminUserId);

            builder.Property(t => t.Name)
                .HasMaxLength(255);

            builder.Property(t => t.MaxScreens);

            builder.Property(t => t.DateCreated);

            builder.Property(t => t.DateModified);
        }
    }
}
