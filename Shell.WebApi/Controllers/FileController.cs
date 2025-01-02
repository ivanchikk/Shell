using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shell.Application.Features.Files.Commands.CopyFile;
using Shell.Application.Features.Files.Commands.CreateFile;
using Shell.Application.Features.Files.Commands.DeleteFile;
using Shell.Application.Features.Files.Commands.UpdateFile;
using Shell.Application.Features.Files.Queries.GetFile;
using Shell.Application.Features.Files.Queries.SearchFiles;
using Shell.WebApi.Models.File;

namespace Shell.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class FileController(IMapper mapper) : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<GetFileQueryResponse>> Get(string path)
        {
            var query = new GetFileQuery
            {
                Path = path
            };
            var vm = await Mediator.Send(query);

            return Ok(vm);
        }

        [HttpGet("search")]
        public async Task<ActionResult<SearchFileQueryResponse>> Search(string path, string? name, DateTime? creationDate)
        {
            var query = new SearchFileQuery
            {
                Path = path,
                Name = name,
                CreationDate = creationDate
            };
            var vm = await Mediator.Send(query);

            return Ok(vm.Files);
        }

        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] CreateFileDto createFileDto)
        {
            var command = mapper.Map<CreateFileCommand>(createFileDto);
            var filePath = await Mediator.Send(command);

            return Ok(filePath);
        }

        [HttpPost("copy")]
        public async Task<ActionResult<string>> Copy([FromBody] CopyFileDto copyFileDto)
        {
            var command = mapper.Map<CopyFileCommand>(copyFileDto);
            var filePath = await Mediator.Send(command);

            return Ok(filePath);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateFileDto updateFileDto)
        {
            var command = mapper.Map<UpdateFileCommand>(updateFileDto);
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{path}")]
        public async Task<IActionResult> Delete(string path)
        {
            var command = new DeleteFileCommand
            {
                Path = path
            };
            await Mediator.Send(command);

            return NoContent();
        }
    }
}
