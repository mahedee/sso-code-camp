using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTS.Application.Common.Exceptions
{
    public class IdentityResultException : Exception
    {
        public string ErrorMessage { get; set; }

        public IdentityResultException(IdentityResult identityResult, string message) : base(message)
        {
            ErrorMessage = string.Join("\n", identityResult.Errors.Select(x => x.Description).ToList());
        }
    }
}
