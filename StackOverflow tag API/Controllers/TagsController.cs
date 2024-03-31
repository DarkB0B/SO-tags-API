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
            this._tagsService = tagsService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _tagsService.GetTagsAsync(0,0));
        }
    }
}
