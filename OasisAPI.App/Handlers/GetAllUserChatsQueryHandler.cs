using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OasisAPI.App.Commands;
using OasisAPI.Infra.Repositories;
using OasisAPI.Models;

namespace OasisAPI.App.Handlers;

public class GetAllUserChatsQueryHandler : IRequestHandler<GetAllUserChatsQuery, AppResult<IEnumerable<OasisChat>>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetAllUserChatsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<AppResult<IEnumerable<OasisChat>>> Handle(GetAllUserChatsQuery request, CancellationToken cancellationToken)
    {
        var chats = await _unitOfWork.GetRepository<OasisChat>()
            .GetAll()
            .Where(c => c.OasisUserId == request.UserId)
            .ToListAsync(cancellationToken);
        
        return AppResult<IEnumerable<OasisChat>>.SuccessResponse(chats);
    }
}