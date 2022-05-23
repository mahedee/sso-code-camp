using System.Security.Claims;
using ISTS.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ISTS.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get User ID from Access Token
        /// </summary>
        public string? UserId => _httpContextAccessor.HttpContext?.User.FindFirstValue("sub");
    }
}
