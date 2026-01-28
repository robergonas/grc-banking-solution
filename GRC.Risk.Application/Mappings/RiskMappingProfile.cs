using AutoMapper;
using GRC.Risk.Application.DTOs;
using GRC.Risk.Domain.Aggregates.ControlAggregate;
using GRC.Risk.Domain.Aggregates.MitigationPlanAggregate;
using GRC.Risk.Domain.Aggregates.RiskAggregate;
//using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GRC.Risk.Application.Mappings;

public class RiskMappingProfile : Profile
{
    public RiskMappingProfile()
    {
        // Risk mappings
        CreateMap<Domain.Aggregates.RiskAggregate.Risk, RiskDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => RiskCategory.FromValue<RiskCategory>(src.CategoryId).Name))
            .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => RiskType.FromValue<RiskType>(src.TypeId).Name))
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => RiskStatus.FromValue<RiskStatus>(src.StatusId).Name))
            .ForMember(dest => dest.LatestAssessment, opt => opt.MapFrom(src => src.LatestAssessment));

        CreateMap<Domain.Aggregates.RiskAggregate.Risk, RiskDetailDto>()
            .IncludeBase<Domain.Aggregates.RiskAggregate.Risk, RiskDto>()
            .ForMember(dest => dest.Assessments, opt => opt.MapFrom(src => src.Assessments))
            .ForMember(dest => dest.Indicators, opt => opt.MapFrom(src => src.Indicators));

        // Risk Assessment mappings
        CreateMap<RiskAssessment, RiskAssessmentDto>();

        // Risk Indicator mappings
        CreateMap<RiskIndicator, RiskIndicatorDto>();

        // Control mappings
        CreateMap<Control, ControlDto>()
            .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => ControlType.FromValue<ControlType>(src.TypeId).Name))
            .ForMember(dest => dest.NatureName, opt => opt.MapFrom(src => ControlNature.FromValue<ControlNature>(src.NatureId).Name))
            .ForMember(dest => dest.FrequencyName, opt => opt.MapFrom(src => ControlFrequency.FromValue<ControlFrequency>(src.FrequencyId).Name))
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => ControlStatus.FromValue<ControlStatus>(src.StatusId).Name));

        // Mitigation Plan mappings
        CreateMap<MitigationPlan, MitigationPlanDto>()
            .ForMember(dest => dest.StrategyName, opt => opt.MapFrom(src => MitigationStrategy.FromValue<MitigationStrategy>(src.StrategyId).Name))
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => MitigationPlanStatus.FromValue<MitigationPlanStatus>(src.StatusId).Name))
            .ForMember(dest => dest.TotalActions, opt => opt.MapFrom(src => src.TotalActionsCount))
            .ForMember(dest => dest.CompletedActions, opt => opt.MapFrom(src => src.CompletedActionsCount))
            .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue));

        CreateMap<MitigationPlan, MitigationPlanDetailDto>()
            .IncludeBase<MitigationPlan, MitigationPlanDto>()
            .ForMember(dest => dest.Actions, opt => opt.MapFrom(src => src.Actions));

        // Mitigation Action mappings
        CreateMap<MitigationAction, MitigationActionDto>()
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => MitigationActionStatus.FromValue<MitigationActionStatus>(src.StatusId).Name))
            .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue));
    }
}