namespace Rens_RentCar.Domain.Abstraction;

public abstract class BaseEntityDto
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public Guid CreatedBy { get; set; } = default!;

    public string CreatedFullName { get; set; } = default!;
    public string? UpdatedFullName { get; set; } = default!;


    public Guid? UpdatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public bool IsActive { get; set; }
}

