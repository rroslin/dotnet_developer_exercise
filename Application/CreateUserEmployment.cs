using Domain;
using FluentValidation;

namespace Application;

public record CreateUserEmploymentRequest(
    int UserId,
    string Company,
    decimal Salary,
    DateTime StartDate,
    DateTime? EndDate
);

public record CreateUserEmploymentResponse(
    int Id,
    string Company,
    decimal Salary,
    int MonthsOfExperience,
    DateTime StartDate,
    DateTime? EndDate
);

public class CreateUserEmploymentRequestValidator : AbstractValidator<CreateUserEmploymentRequest>
{
    public CreateUserEmploymentRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull()
            .WithMessage("User ID is required.");

        RuleFor(x => x.Company)
            .NotEmpty()
            .WithMessage("Company name is required.");

        RuleFor(x => x.Salary)
            .NotNull()
            .WithMessage("Salary is required.");


        RuleFor(x => x.Salary)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Salary must be greater than or equal to 0.");


        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required.");
        
        When(x => x.EndDate.HasValue, () =>
        {
            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .WithMessage("End date must be greater than or equal to start date.");
        });
    }
}

public static class CreateUserEmploymentMappingExtensions
{

    public static Employment ToEmployment(this CreateUserEmploymentRequest request)
    {
        return new Employment
        {
            Company = request.Company,
            Salary = request.Salary,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
        };
    }

    public static CreateUserEmploymentResponse ToCreateUserEmploymentResponse(this Employment employment)
    {
        return new CreateUserEmploymentResponse(
            employment.Id,
            employment.Company,
            employment.Salary,
            employment.MonthsOfExperience,
            employment.StartDate,
            employment.EndDate
        );
    }
}