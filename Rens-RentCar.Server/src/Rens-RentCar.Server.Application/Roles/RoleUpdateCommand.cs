using FluentValidation;
using GenericRepository;
using Rens_RentCar.Domain.Roles;
using Rens_RentCar.Domain.Shared;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Roles;

public sealed record RoleUpdateCommand(Guid Id, string Name, bool IsActive) : IRequest<Result<string>>;

public sealed class RoleUpdateCommandValidator : AbstractValidator<Role>
{
    public RoleUpdateCommandValidator()
    {
        RuleFor(r => r.Name).NotEmpty().WithMessage("Role name is required.");
    }
}


internal sealed class RoleUpdateCommandHandler(IRoleRepository _roleRepository, IUnitOfWork _unitOfWork) : IRequestHandler<RoleUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleUpdateCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
        if (role is null)
            return Result<string>.Failure("Role not found.");

        var isRoleNameExists = await _roleRepository.AnyAsync(r => r.Name.Value == request.Name, cancellationToken);

        if (isRoleNameExists)
            return Result<string>.Failure("Role name is already taken before by someone else.");

        Name name = new(request.Name);

        role.SetName(name);
        role.SetStatus(request.IsActive);

        _roleRepository.Update(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "Role updated successfully.";

    }
}