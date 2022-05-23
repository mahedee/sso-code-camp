using FluentValidation;

namespace ISTS.Application.Commands.Client
{
    public class CreateClientSecretCommandValidator : AbstractValidator<CreateClientSecretCommand>
    {
        public CreateClientSecretCommandValidator()
        {
            RuleFor(x => x.ClientSecret).NotEmpty().NotNull();
            RuleFor(x => x.ClientId).NotNull().NotEqual(0);
        }
    }
}
