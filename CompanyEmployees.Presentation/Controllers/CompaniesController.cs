using Application.Commands;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/companies")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly ISender _sender;

    public CompaniesController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<IActionResult> GetCompanies()
    {
        var companies = await _sender.Send(new GetCompaniesQuery(TrackChanges: false));
        
        return Ok(companies);
    }

    [HttpGet("{id:guid}", Name = "CompanyById")]
    public async Task<IActionResult> GetCompany(Guid id)
    {
        var company = await _sender.Send(new GetCompanyQuery(id, TrackChanges: false));

        return Ok(company);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto companyForCreationDto)
    {
        if (companyForCreationDto is null)
            return BadRequest("CompanyForCreationDto object is null");

        var company = await _sender.Send(new CreateCompanyCommand(companyForCreationDto));
        
        return CreatedAtRoute("CompanyById", new { id = company.Id }, company);
    }

}
