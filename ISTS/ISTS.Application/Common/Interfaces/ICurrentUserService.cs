using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTS.Application.Common.Interfaces
{
    /// <summary>
    /// All Current User Information Related functionality will be placed user
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Get user ID from access_token
        /// </summary>
        string UserId { get; }
    }
}
