using GenericRepository;
using Rens_RentCar.Domain.Reservations;
using TS.MediatR;
using TS.Result;

namespace Rens_RentCar.Server.Application.Reservations;

public sealed record ReservationDeleteCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class ReservationDeleteCommandHandler(
    IReservationRepository reservationRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ReservationDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ReservationDeleteCommand request, CancellationToken cancellationToken)
    {
        Reservation? reservation = await reservationRepository.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (reservation is null)
        {
            return Result<string>.Failure("Reservation not found.");
        }

        if (reservation.Status != Status.Pending)
        {
            return Result<string>.Failure("This reservation cannot be changed.");
        }

        reservation.Delete();
        reservationRepository.Update(reservation);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Reservation deleted successfully.";
    }
}