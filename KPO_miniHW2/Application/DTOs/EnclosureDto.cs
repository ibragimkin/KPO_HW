namespace ZooManagementSystem.Application.DTOs;

/// <summary>
/// DTO для вольера.
/// </summary>
public class EnclosureDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = default!;
    public double Size { get; set; }
    public int MaxCapacity { get; set; }
    public List<Guid> AnimalIds { get; set; } = new();
}
