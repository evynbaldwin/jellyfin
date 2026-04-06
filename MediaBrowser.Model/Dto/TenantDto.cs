using System;

namespace MediaBrowser.Model.Dto
{
    /// <summary>
    /// Data transfer object for a Tenant.
    /// </summary>
    public class TenantDto
    {
        /// <summary>
        /// Gets or sets the Guid.
        /// </summary>
        /// <value>The primary Guid tag.</value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the primary Name tag.
        /// </summary>
        /// <value>The primary Name tag.</value>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the primary adminuserid tag.
        /// </summary>
        /// <value>The primary adminuserid tag.</value>
        public string AdminUserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the primary maxscreens tag.
        /// </summary>
        /// <value>The primary maxscreens tag.</value>
        public int MaxScreens { get; set; }

        /// <summary>
        /// Gets or sets the primary datecreated tag.
        /// </summary>
        /// <value>The primary datecreated tag.</value>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the primary datemodified tag.
        /// </summary>
        /// <value>The primary datemodified tag.</value>
        public DateTime DateModified { get; set; }
    }
}
