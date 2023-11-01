using Application.DataLayer.Requests;
using FluentValidation;

namespace AcmePayLtdAPI.Validators
{
    public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
    {
        public CreateAccountRequestValidator()
        {
            RuleFor(c => c.IdCard)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Length(6, 8)
                .Must(BeAValidIdCard).WithMessage("Id card is not valid."); 
            RuleFor(c => c.FirstName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Must(BeAValidName).WithMessage("First Name is not a valid name.")
                .Length(3,50);
            RuleFor(c => c.LastName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Must(BeAValidName).WithMessage("Last Name is not a valid name.")
                .Length(3, 50);
            RuleFor(c => c.DOB)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .GreaterThan(new DateOnly(1850, 1, 1))
                .LessThan(DateOnly.FromDateTime(DateTime.Today));
            RuleFor(c => c.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .EmailAddress();
            RuleFor(c => c.Username)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Length(3, 20);
            RuleFor(c => c.Password)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty()
               .Length(8, 20);
        }

        protected bool BeAValidName(string name)
        {
            name = name.Replace(" ", "");
            name = name.Replace("-", "");
            return name.All(Char.IsLetter);
        }

        protected bool BeAValidIdCard(string idCard)
        {
            var numberValid = idCard.Substring(0, idCard.Length - 1).All(Char.IsDigit);
            var letterValid = Char.IsLetter(idCard.ElementAt(idCard.Length - 1));
            return numberValid && letterValid;
        }
        
    }
}
