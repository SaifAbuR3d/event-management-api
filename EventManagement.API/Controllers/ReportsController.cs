﻿using EventManagement.Application.Contracts.Requests;
using EventManagement.Application.Features.Reports.AddReport;
using EventManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static EventManagement.Domain.Constants;
using System.Text.Json;
using EventManagement.Application.Features.Reports.GetReports;

namespace EventManagement.API.Controllers;

/// <summary>
/// endpoint for reports
/// </summary>
/// <param name="mediator"></param>
[Route("api/[controller]")]
[ApiController]
public class ReportsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// add a report
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<int>> AddReport(AddReportCommand command)
    {
        var reportId = await mediator.Send(command);
        return Ok(new {reportId});
    }

    /// <summary>
    /// get all reports
    /// </summary>
    /// <param name="queryParameters"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Report>>> GetAllReports(
        [FromQuery] GetAllQueryParameters queryParameters)
    {
        var (reports, paginationMetadata) = await mediator.Send(new GetAllReportsQuery(queryParameters));
        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
        return Ok(reports);

    }
}