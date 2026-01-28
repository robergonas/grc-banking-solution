using AutoMapper;
using GRC.Risk.Application.DTOs;
using GRC.Risk.Domain.Aggregates.ControlAggregate;
using MediatR;

namespace GRC.Risk.Application.Queries.GetControls;

public class GetControlsQueryHandler : IRequestHandler<GetControlsQuery, PagedResult<ControlDto>>
{
    private readonly IControlRepository _controlRepository;
    private readonly IMapper _mapper;

    public GetControlsQueryHandler(
        IControlRepository controlRepository,
        IMapper mapper)
    {
        _controlRepository = controlRepository;
        _mapper = mapper;
    }

    public async Task<PagedResult<ControlDto>> Handle(GetControlsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Control> controls;

        if (request.TypeId.HasValue)
        {
            controls = await _controlRepository.GetByTypeAsync(request.TypeId.Value, cancellationToken);
        }
        else if (request.StatusId.HasValue)
        {
            controls = await _controlRepository.GetByStatusAsync(request.StatusId.Value, cancellationToken);
        }
        else if (request.OwnerId.HasValue)
        {
            controls = await _controlRepository.GetByOwnerAsync(request.OwnerId.Value, cancellationToken);
        }
        else
        {
            controls = new List<Control>();
        }

        var filteredControls = controls;

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            filteredControls = controls.Where(c =>
                c.Code.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                c.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase)
            );
        }

        var totalCount = filteredControls.Count();
        var pagedControls = filteredControls
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var controlDtos = _mapper.Map<List<ControlDto>>(pagedControls);

        return new PagedResult<ControlDto>(controlDtos, totalCount, request.PageNumber, request.PageSize);
    }
}