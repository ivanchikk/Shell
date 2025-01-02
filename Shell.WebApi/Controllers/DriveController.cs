using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shell.Application.Features.Directories.Queries.GetDirectoryContent;
using Shell.Application.Features.Drives.Commands.CreateDrive;
using Shell.Application.Features.Drives.Commands.DeleteDrive;
using Shell.Application.Features.Drives.Commands.UpdateDrive;
using Shell.Application.Features.Drives.Queries.GetDrive;
using Shell.Application.Features.Drives.Queries.GetDriveContent;
using Shell.WebApi.Models.Drive;

namespace Shell.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class DriveController(IMapper mapper) : BaseController
    {
        [HttpGet("get")]
        public async Task<ActionResult<GetDriveQueryResponse>> Get(string name)
        {
            var query = new GetDriveQuery
            {
                Name = name
            };
            var vm = await Mediator.Send(query);

            return Ok(vm);
        }

        [HttpGet("get-content")]
        public async Task<ActionResult<GetDirectoryContentQueryResponse>> GetContent(string name)
        {
            var query = new GetDriveContentQuery
            {
                Name = name
            };
            var vm = await Mediator.Send(query);

            return Ok(vm.Content);
        }

        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] CreateDriveDto createDriveDto)
        {
            var command = mapper.Map<CreateDriveCommand>(createDriveDto);
            var driveName = await Mediator.Send(command);

            return Ok(driveName);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateDriveDto updateDriveDto)
        {
            var command = mapper.Map<UpdateDriveCommand>(updateDriveDto);
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            var command = new DeleteDriveCommand
            {
                Name = name
            };
            await Mediator.Send(command);

            return NoContent();
        }
    }
}
