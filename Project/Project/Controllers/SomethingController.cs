using Microsoft.AspNetCore.Mvc;
using Tutorial9.Model_DTOs;
using Tutorial9.Exceptions;
using Tutorial9.Services;

namespace Tutorial9.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SomethingController : ControllerBase
{
    private IService _service;

    public SomethingController(IService service)
    {
        _service = service;
    }

    // [HttpGet("{visitId}")]
    // public async Task<IActionResult> getVisitInfo(int visitId)
    // {
    //     try
    //     {
    //         VisitInfo visitInfo = await _service.GetVisitInfo(visitId);
    //         return Ok(visitInfo);
    //     }
    //     catch (BadRequestException e)
    //     {
    //         return BadRequest(e.Message);
    //     }
    //     catch (NotFoundException e)
    //     {
    //         return NotFound(e.Message);
    //     }
    //     catch (ConflictException e)
    //     {
    //         return Conflict(e.Message);
    //     }
    //     catch (InternalServerException e)
    //     {
    //         return StatusCode(500, e.Message);
    //     }
    // }
    // //
    // [HttpPost]
    // public async Task<IActionResult> addVisit(VisitPost visitPost)
    // {
    //     try
    //     {
    //         await _service.AddVisit(visitPost);
    //         return Ok();
    //     }
    //     catch (BadRequestException e)
    //     {
    //         return BadRequest(e.Message);
    //     }
    //     catch (NotFoundException e)
    //     {
    //         return NotFound(e.Message);
    //     }
    //     catch (ConflictException e)
    //     {
    //         return Conflict(e.Message);
    //     }
    //     catch (InternalServerException e)
    //     {
    //         return StatusCode(500, e.Message);
    //     }
    // }
}