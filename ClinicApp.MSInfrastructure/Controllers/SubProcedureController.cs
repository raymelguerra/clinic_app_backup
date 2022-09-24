﻿using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Infrastructure.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SubProcedureController : ControllerBase
{
    private readonly ISubProcedure _subprocedure;

    public SubProcedureController(ISubProcedure subprocedure)
    {
        _subprocedure = subprocedure;
    }


    // GET: api/Diagnoses
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubProcedure>>> GetSubProcedure()
    {
        return Ok(await _subprocedure.Get());
    }

    // GET: api/Diagnoses/5
    [HttpGet("{id}")]
    public async Task<ActionResult<SubProcedure>> GetSubProcedure(int id)
    {
        var subprocedure = await _subprocedure.Get(id);

        if (subprocedure == null)
        {
            return NotFound();
        }

        return subprocedure;
    }
}
