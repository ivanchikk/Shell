using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shell.Application.Features.Directories.Commands.CopyDirectory;
using Shell.Application.Features.Directories.Commands.CreateDirectory;
using Shell.Application.Features.Directories.Commands.DeleteDirectory;
using Shell.Application.Features.Directories.Commands.UpdateDirectory;
using Shell.Application.Features.Directories.Queries.GetDirectory;
using Shell.Application.Features.Directories.Queries.GetDirectoryContent;
using Shell.WebApi.Models.Directory;

namespace Shell.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class DirectoryController(IMapper mapper) : BaseController
    {
        [HttpGet("get")]
        public async Task<ActionResult<GetDirectoryQueryResponse>> Get(string path)
        {
            var query = new GetDirectoryQuery
            {
                Path = path
            };
            var vm = await Mediator.Send(query);

            return Ok(vm);
        }

        [HttpGet("get-content")]
        public async Task<ActionResult<GetDirectoryContentQueryResponse>> GetContent(string path)
        {
            var query = new GetDirectoryContentQuery
            {
                Path = path
            };
            var vm = await Mediator.Send(query);

            return Ok(vm.Content);
        }

        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] CreateDirectoryDto createDirectoryDto)
        {
            var command = mapper.Map<CreateDirectoryCommand>(createDirectoryDto);
            var directoryPath = await Mediator.Send(command);

            return Ok(directoryPath);
        }

        [HttpPost("copy")]
        public async Task<ActionResult<string>> Copy([FromBody] CopyDirectoryDto copyDirectoryDto)
        {
            var command = mapper.Map<CopyDirectoryCommand>(copyDirectoryDto);
            var directoryPath = await Mediator.Send(command);

            return Ok(directoryPath);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateDirectoryDto updateDirectoryDto)
        {
            var command = mapper.Map<UpdateDirectoryCommand>(updateDirectoryDto);
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{path}")]
        public async Task<IActionResult> Delete(string path)
        {
            var command = new DeleteDirectoryCommand
            {
                Path = path
            };
            await Mediator.Send(command);

            return NoContent();
        }
    }
}
