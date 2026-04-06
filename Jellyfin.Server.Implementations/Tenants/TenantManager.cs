#pragma warning disable CA1307

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jellyfin.Data;
using Jellyfin.Data.Enums;
using Jellyfin.Data.Events;
using Jellyfin.Data.Events.Users;
using Jellyfin.Database.Implementations;
using Jellyfin.Database.Implementations.Entities;
using Jellyfin.Database.Implementations.Enums;
using Jellyfin.Extensions;
using Jellyfin.Server.Implementations.Users;
using MediaBrowser.Common;
using MediaBrowser.Common.Extensions;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Authentication;
using MediaBrowser.Controller.Configuration;
using MediaBrowser.Controller.Drawing;
using MediaBrowser.Controller.Events;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Net;
using MediaBrowser.Model.Configuration;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Server.Implementations.Tenants
{
    /// <summary>
    /// Manages the creation and retrieval of <see cref="Tenant"/> instances.
    /// </summary>
    public partial class TenantManager : ITenantManager
    {
        private readonly IDbContextFactory<JellyfinDbContext> _dbProvider;
        private readonly IEventManager _eventManager;
        private readonly IApplicationHost _appHost;
        private readonly IServerConfigurationManager _serverConfigurationManager;

        private readonly IDictionary<Guid, Tenant> _tenants;

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantManager"/> class.
        /// </summary>
        /// <param name="dbProvider">The database provider.</param>
        /// <param name="eventManager">The event manager.</param>
        /// <param name="networkManager">The network manager.</param>
        /// <param name="appHost">The application host.</param>
        /// <param name="imageProcessor">The image processor.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="serverConfigurationManager">The system config manager.</param>
        /// <param name="passwordResetProviders">The password reset providers.</param>
        /// <param name="authenticationProviders">The authentication providers.</param>
        public TenantManager(
            IDbContextFactory<JellyfinDbContext> dbProvider,
            IEventManager eventManager,
            INetworkManager networkManager,
            IApplicationHost appHost,
            IImageProcessor imageProcessor,
            ILogger<UserManager> logger,
            IServerConfigurationManager serverConfigurationManager,
            IEnumerable<IPasswordResetProvider> passwordResetProviders,
            IEnumerable<IAuthenticationProvider> authenticationProviders)
        {
            _dbProvider = dbProvider;
            _eventManager = eventManager;
            _appHost = appHost;
            _serverConfigurationManager = serverConfigurationManager;

            _tenants = new ConcurrentDictionary<Guid, Tenant>();
            using var dbContext = _dbProvider.CreateDbContext();
            foreach (var tenant in dbContext.Tenants.AsEnumerable())
            {
                _tenants.Add(tenant.Id, tenant);
            }
        }

        /// <inheritdoc />
        public IEnumerable<Tenant> Tenants => _tenants.Values;

        /// <inheritdoc />
        public async Task InitializeAsync()
        {
            // TODO: Refactor the startup wizard so that it doesn't require a user to already exist.
            if (_tenants.Any())
            {
                return;
            }

            // _logger.LogWarning("No users, creating one with username {UserName}", defaultName);

            var dbContext = await _dbProvider.CreateDbContextAsync().ConfigureAwait(false);
            await using (dbContext.ConfigureAwait(false))
            {
            }
        }

        /// <summary>
        /// Returns Tenant DTO.
        /// </summary>
        /// <param name="tenant">Tenant.</param>
        /// <param name="remoteEndPoint">EndPoint.</param>
        /// <returns>TenantDTO.</returns>
        public TenantDto GetTenantDto(Tenant tenant, string? remoteEndPoint = null)
        {
            var castReceiverApplications = _serverConfigurationManager.Configuration.CastReceiverApplications;
            return new TenantDto
            {
                Id = tenant.Id,
                Name = tenant.Name,
                AdminUserId = tenant.AdminUserId,
                DateCreated = tenant.DateCreated,
                DateModified = tenant.DateModified
            };
        }
    }
}
