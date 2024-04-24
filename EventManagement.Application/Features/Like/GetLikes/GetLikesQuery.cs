using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Contracts.Responses;
using MediatR;

namespace EventManagement.Application.Features.Like.GetLikes;

public record GetLikesQuery(GetAllLikesQueryParameters Parameters)
    : IRequest<IEnumerable<LikeDto>>;




