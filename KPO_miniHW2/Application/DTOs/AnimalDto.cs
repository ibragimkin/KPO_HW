namespace ZooManagementSystem.Application.DTOs;

/// <summary>
/// DTO для животного.
/// </summary>
public class AnimalDto
{
    public Guid Id { get; set; }
    public string Species { get; set; } = default!;
    public string Name { get; set; } = default!;
    public DateOnly BirthDate { get; set; }
    public string Gender { get; set; } = default!;
    public string FavoriteFood { get; set; } = default!;
    public string Status { get; set; } = default!;
    public Guid? EnclosureId { get; set; }
}
