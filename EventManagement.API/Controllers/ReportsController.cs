using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Features.Reports.AddReport;
using EventManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using EventManagement.Application.Features.Reports.GetReports;
using EventManagement.Application.Features.Reports.SetStatusToSeen;

namespace EventManagement.API.Controllers;

/// <summary>
/// endpoint for reports
/// </summary>
/// <param name="mediator"></param>
[Route("api")]
[ApiController]
public class ReportsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// add a report to an event
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("event-reports")]
    public async Task<ActionResult<int>> AddReport(AddEventReportCommand command)
    {
        var reportId = await mediator.Send(command);
        return Ok(new {reportId});
    }

    /// <summary>
    /// add a report to a review
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("review-reports")]
    public async Task<ActionResult<int>> AddReport(AddReviewReportCommand command)
    {
        var reportId = await mediator.Send(command);
        return Ok(new { reportId });
    }

    /// <summary>
    /// get all event reports
    /// </summary>
    /// <param name="queryParameters"></param>
    /// <returns></returns>
    [HttpGet("event-reports")]
    public async Task<ActionResult<IEnumerable<Report>>> GetAllReports(
        [FromQuery] GetAllEventReportsQueryParameters queryParameters)
    {
        var (reports, paginationMetadata) = await mediator.Send(new GetAllEventReportsQuery(queryParameters));
        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
        return Ok(reports);

    }

    /// <summary>
    /// get all review reports
    /// </summary>
    /// <param name="queryParameters"></param>
    /// <returns></returns>
    [HttpGet("review-reports")]
    public async Task<ActionResult<IEnumerable<Report>>> GetAllReports(
        [FromQuery] GetAllReviewReportsQueryParameters queryParameters)
    {
        var (reports, paginationMetadata) = await mediator.Send(new GetAllReviewReportsQuery(queryParameters));
        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
        return Ok(reports);

    }


    /// <summary>
    /// change the status of a report to seen
    /// </summary>
    /// <param name="reportId"></param>
    /// <returns></returns>
    [HttpPatch("reports/{reportId}/seen")]
    public async Task<ActionResult> SetStatusSeen(int reportId)
    {
        await mediator.Send(new SetReportStatusCommand(reportId, ReportStatus.Seen));
        return NoContent();
    }
}
