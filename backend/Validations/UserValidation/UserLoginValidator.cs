using FluentValidation;
using TodoListAPI.DTOs.Users.Auth;

namespace TodoListAPI.Validations.UserValidation
{
    public class UserLoginValidator : AbstractValidator<LoginRequestDto>
    {
        public UserLoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}

