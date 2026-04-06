#pragma warning disable CS1591

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jellyfin.Database.Implementations.Entities;
using MediaBrowser.Model.Dto;

namespace MediaBrowser.Controller.Library
{
    /// <summary>
    /// Interface ITenantManager.
    /// </summary>
    public interface ITenantManager
    {
        /// <summary>
        /// Gets the Tenants.
        /// </summary>
        /// <value>The tenants.</value>
        IEnumerable<Tenant> Tenants { get; }

        /// <summary>
        /// Initializes the user manager and ensures that a user exists.
        /// </summary>
        /// <returns>Awaitable task.</returns>
        Task InitializeAsync();

        /// <summary>
        /// Gets the tenant dto.
        /// </summary>
        /// <param name="tenant">The tenate.</param>
        /// <param name="remoteEndPoint">The remote end point.</param>
        /// <returns>TenantDto.</returns>
        TenantDto GetTenantDto(Tenant tenant, string? remoteEndPoint = null);
    }
}
