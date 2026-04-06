using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jellyfin.Database.Implementations.Entities
{
    /// <summary>
    /// Represents a tenant household.
    /// </summary>
    public class Tenant
    {
        /// <summary>
        /// Gets or sets the tenant identifier.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the tenant name.
        /// </summary>
        [MaxLength(255)]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the admin user identifier.
        /// </summary>
        public string AdminUserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the maximum allowed simultaneous streams.
        /// </summary>
        public int MaxScreens { get; set; } = 1;

        /// <summary>
        /// Gets or sets the date the tenant was created.
        /// </summary>
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the date the tenant was last modified.
        /// </summary>
        public DateTime DateModified { get; set; } = DateTime.UtcNow;
    }
}
