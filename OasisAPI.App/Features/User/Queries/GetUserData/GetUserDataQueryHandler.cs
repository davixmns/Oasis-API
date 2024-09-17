using AutoMapper;
using Domain.Entities;
using MediatR;
using OasisAPI.App.Result;
using OasisAPI.Infra.Repositories;

namespace OasisAPI.App.Features.User.Queries.GetUserData;

public class GetUserDataQueryHandler : IRequestHandler<GetUserDataQuery, AppResult<OasisUserResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public GetUserDataQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<AppResult<OasisUserResponseDto>> Handle(GetUserDataQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.GetRepository<OasisUser>().GetAsync(u => u.Id == request.UserId);
        
        if(user is null)
            return AppResult<OasisUserResponseDto>.Fail("User not found");
        
        var userResponse = _mapper.Map<OasisUserResponseDto>(user);
        
        return AppResult<OasisUserResponseDto>.Success(userResponse);
    }
}