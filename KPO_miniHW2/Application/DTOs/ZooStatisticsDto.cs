namespace ZooManagementSystem.Application.DTOs;

/// <summary>
/// DTO для представления статистики по зоопарку.
/// </summary>
public class ZooStatisticsDto
{
    /// <summary>
    /// Общее количество животных в зоопарке.
    /// </summary>
    public int TotalAnimals { get; set; }

    /// <summary>
    /// Количество свободных вольеров.
    /// </summary>
    public int FreeEnclosures { get; set; }
}
