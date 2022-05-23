using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using ISTS.Application.Common.Interfaces.Identity;

namespace ISTS.Application.Commands.Permission
{
    public class CreatePermissionCommandValidator : AbstractValidator<CreatePermissionCommand>
    {
        private readonly IApiResourceRepository _apiResourceRepository;
        public CreatePermissionCommandValidator(IApiResourceRepository apiResourceRepository)
        {
            _apiResourceRepository = apiResourceRepository;
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.DisplayName).NotNull().NotEmpty();
            RuleFor(x => x.ApiResourceId).NotNull().NotEmpty().MustAsync(async (apiResourceId, token) =>
            {
                var apiResourceInfo = await _apiResourceRepository.GetApiResourceAsync(apiResourceId);
                return apiResourceInfo != null;
            }).WithMessage("Invalid API information");
        }
    }
}
