using ClassLibrary.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StackOverflow_tag_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagsService _tagsService;
        public TagsController(ITagsService tagsService)
        {
            _tagsService = tagsService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _tagsService.GetTagsAsync(0,0));
        }
        [HttpPut]
        public async Task<IActionResult> Put()
        {
            try
            {
                await _tagsService.UpdateTagsInDb();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
