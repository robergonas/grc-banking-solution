using GRC.BuildingBlocks.Domain.SeedWork;
using GRC.Risk.Domain.Exceptions;

namespace GRC.Risk.Domain.Aggregates.RiskAggregate;
public class RiskIndicator : Entity
{
    public Guid RiskId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string MeasurementUnit { get; private set; }
    public string? DataSource { get; private set; }
    public int MeasurementFrequency { get; private set; }
    public decimal? ThresholdGreen { get; private set; }
    public decimal? ThresholdYellow { get; private set; }
    public decimal? ThresholdRed { get; private set; }
    public Guid OwnerId { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }    
    public Risk Risk { get; private set; }
    private RiskIndicator() { }
    public static RiskIndicator Create(
        Guid riskId,
        string name,
        string description,
        string measurementUnit,
        int measurementFrequency,
        Guid ownerId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new RiskDomainException("Indicator name is required");

        if (string.IsNullOrWhiteSpace(measurementUnit))
            throw new RiskDomainException("Measurement unit is required");

        return new RiskIndicator
        {
            Id = Guid.NewGuid(),
            RiskId = riskId,
            Name = name,
            Description = description,
            MeasurementUnit = measurementUnit,
            MeasurementFrequency = measurementFrequency,
            OwnerId = ownerId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }
    public void SetThresholds(decimal? green, decimal? yellow, decimal? red)
    {
        ThresholdGreen = green;
        ThresholdYellow = yellow;
        ThresholdRed = red;
        UpdatedAt = DateTime.UtcNow;
    }
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
    public void UpdateDetails(string name, string description, string measurementUnit)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new RiskDomainException("Indicator name is required");

        Name = name;
        Description = description;
        MeasurementUnit = measurementUnit;
        UpdatedAt = DateTime.UtcNow;
    }
}