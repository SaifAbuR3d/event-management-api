using EventManagement.Application.Abstractions.Persistence;
using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Features.Identity;
using MediatR;

namespace EventManagement.Application.Features.IVRs.GetIvrs;

public record GetIvrsQuery(GetAllIvrsQueryParameters Parameters)
    : IRequest<(IEnumerable<IvrDto>, PaginationMetadata)>;

public class GetIvrsQueryHandler(IIvrRepository ivrRepository, ICurrentUser currentUser, 
    IUserRepository userRepository)
    : IRequestHandler<GetIvrsQuery, (IEnumerable<IvrDto>, PaginationMetadata)>
{
    public async Task<(IEnumerable<IvrDto>, PaginationMetadata)> Handle(
        GetIvrsQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAdmin)
        {
            throw new UnauthorizedException("Only admins can view IVRs.");
        }

        if(request.Parameters.OnlyAttendees == true && request.Parameters.OnlyOrganizers == true)
        {
            throw new BadRequestException("Only one of OnlyAttendees and OnlyOrganizers can be true.");
        }

        var (ivrs, paginationMetadata) = await ivrRepository.GetAllAsync(request.Parameters,
            cancellationToken);

        List<IvrDto> ivrDtos = []; 

        foreach(var ivr in ivrs)
        {
            var ivrDto = new IvrDto
            {
                Id = ivr.Id,
                UserId = int.Parse(await userRepository.GetIdByUserId(ivr.UserId, cancellationToken)
                    ?? throw new CustomException("Invalid State: User has no Id")), 
                UserName = await userRepository.GetUserNameByUserId(ivr.UserId, cancellationToken)
                    ?? throw new CustomException("Invalid State: User has no UserName"),
                ProfilePictureUrl = await userRepository.GetProfilePictureByUserId(ivr.UserId, cancellationToken),
                AdminMessage = ivr.AdminMessage,
                Status = ivr.Status,
                DocumentType = ivr.Document.DocumentType,
                DocumentUrl = ivr.Document.DocumentFileUrl,
                CreationDate = ivr.CreationDate,
                LastModified = ivr.CreationDate
            };

            ivrDtos.Add(ivrDto); 
        }

        return (ivrDtos, paginationMetadata); 

    }
}