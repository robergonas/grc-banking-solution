namespace GRC.Risk.Application.DTOs;

public class ControlDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int TypeId { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public int NatureId { get; set; }
    public string NatureName { get; set; } = string.Empty;
    public int FrequencyId { get; set; }
    public string FrequencyName { get; set; } = string.Empty;
    public Guid OwnerId { get; set; }
    public string? OwnerName { get; set; }
    public DateTime? ImplementationDate { get; set; }
    public int StatusId { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public int? EffectivenessRating { get; set; }
    public string? EffectivenessText => EffectivenessRating.HasValue
        ? GetEffectivenessText(EffectivenessRating.Value)
        : null;
    public DateTime? LastTestedDate { get; set; }
    public DateTime? NextTestDate { get; set; }
    public decimal? Cost { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    private static string GetEffectivenessText(int rating)
    {
        return rating switch
        {
            5 => "Highly Effective",
            4 => "Effective",
            3 => "Moderately Effective",
            2 => "Somewhat Effective",
            1 => "Ineffective",
            _ => "Not Rated"
        };
    }
}