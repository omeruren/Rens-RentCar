using FluentValidation;
using GenericRepository;
using Rens_RentCar.Domain.Roles;
using Rens_RentCar.Domain.Shared;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Roles;

[Permission("role:create")]

public sealed record RoleCreateCommand(string Name, bool IsActive) : IRequest<Result<string>>;

public sealed class RoleCreateCommandValidator : AbstractValidator<RoleCreateCommand>
{
    public RoleCreateCommandValidator()
    {
        RuleFor(r => r.Name).NotEmpty().WithMessage("Role name is required.");
    }
}

internal sealed class RoleCreateCommandHandler(IRoleRepository _roleRepository, IUnitOfWork _unitOfWork) : IRequestHandler<RoleCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleCreateCommand request, CancellationToken cancellationToken)
    {
        var isRoleNameExists = await _roleRepository.AnyAsync(r => r.Name.Value == request.Name, cancellationToken);

        if (isRoleNameExists)
            return Result<string>.Failure("Role name is already taken before by someone else.");

        Name name = new(request.Name);
        Role role = new(name, request.IsActive);
        _roleRepository.Add(role);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return "Role Created successfully.";
    }

}