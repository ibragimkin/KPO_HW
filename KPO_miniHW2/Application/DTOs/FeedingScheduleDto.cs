namespace ZooManagementSystem.Application.DTOs;

/// <summary>
/// DTO для записи расписания кормления.
/// </summary>
public class FeedingScheduleDto
{
    public Guid Id { get; set; }
    public Guid AnimalId { get; set; }
    public string Time { get; set; } = default!; 
    public string FoodType { get; set; } = default!;
    public bool IsCompleted { get; set; }
}
