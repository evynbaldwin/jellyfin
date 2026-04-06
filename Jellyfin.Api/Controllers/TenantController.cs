using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Jellyfin.Api.Constants;
using Jellyfin.Api.Extensions;
using Jellyfin.Api.Helpers;
using Jellyfin.Api.Models.UserDtos;
using Jellyfin.Data;
using Jellyfin.Database.Implementations.Enums;
using Jellyfin.Extensions;
using MediaBrowser.Common.Api;
using MediaBrowser.Common.Extensions;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Authentication;
using MediaBrowser.Controller.Configuration;
using MediaBrowser.Controller.Devices;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Net;
using MediaBrowser.Controller.Playlists;
using MediaBrowser.Controller.QuickConnect;
using MediaBrowser.Controller.Session;
using MediaBrowser.Model.Configuration;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Api.Controllers;

/// <summary>
/// User controller.
/// </summary>
[Route("Tenants")]
public class TenantController : BaseJellyfinApiController
{
    private readonly ITenantManager _tenantManager;
    private readonly ISessionManager _sessionManager;
    private readonly INetworkManager _networkManager;
    private readonly IDeviceManager _deviceManager;
    private readonly IAuthorizationContext _authContext;
    private readonly IServerConfigurationManager _config;
    private readonly ILogger _logger;
    private readonly IQuickConnect _quickConnectManager;
    private readonly IPlaylistManager _playlistManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="TenantController"/> class.
    /// </summary>
    /// <param name="tenantManager">Instance of the <see cref="ITenantManager"/> interface.</param>
    /// <param name="sessionManager">Instance of the <see cref="ISessionManager"/> interface.</param>
    /// <param name="networkManager">Instance of the <see cref="INetworkManager"/> interface.</param>
    /// <param name="deviceManager">Instance of the <see cref="IDeviceManager"/> interface.</param>
    /// <param name="authContext">Instance of the <see cref="IAuthorizationContext"/> interface.</param>
    /// <param name="config">Instance of the <see cref="IServerConfigurationManager"/> interface.</param>
    /// <param name="logger">Instance of the <see cref="ILogger"/> interface.</param>
    /// <param name="quickConnectManager">Instance of the <see cref="IQuickConnect"/> interface.</param>
    /// <param name="playlistManager">Instance of the <see cref="IPlaylistManager"/> interface.</param>
    public TenantController(
        ITenantManager tenantManager,
        ISessionManager sessionManager,
        INetworkManager networkManager,
        IDeviceManager deviceManager,
        IAuthorizationContext authContext,
        IServerConfigurationManager config,
        ILogger<UserController> logger,
        IQuickConnect quickConnectManager,
        IPlaylistManager playlistManager)
    {
        _tenantManager = tenantManager;
        _sessionManager = sessionManager;
        _networkManager = networkManager;
        _deviceManager = deviceManager;
        _authContext = authContext;
        _config = config;
        _logger = logger;
        _quickConnectManager = quickConnectManager;
        _playlistManager = playlistManager;
    }

    /// <summary>
    /// Gets a list of users.
    /// </summary>
    /// <param name="isHidden">Optional filter by IsHidden=true or false.</param>
    /// <param name="isDisabled">Optional filter by IsDisabled=true or false.</param>
    /// <response code="200">Users returned.</response>
    /// <returns>An <see cref="IEnumerable{TenantDto}"/> containing the users.</returns>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<TenantDto>> GetTenants(
        [FromQuery] bool? isHidden,
        [FromQuery] bool? isDisabled)
    {
        var users = Get(isHidden, isDisabled, false, false);
        return Ok(users);
    }

    private IEnumerable<TenantDto> Get(bool? isHidden, bool? isDisabled, bool filterByDevice, bool filterByNetwork)
    {
        var tenants = _tenantManager.Tenants;

        var result = tenants
            .OrderBy(u => u.Name)
            .Select(i => _tenantManager.GetTenantDto(i, HttpContext.GetNormalizedRemoteIP().ToString()));

        return result;
    }
}
