using ClassLibrary.Interfaces;
using ClassLibrary.Models;
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
        public async Task<IActionResult> Get(int page, int pagesize,string? order,string? sort)
        {
            try
            {
                if (page <= 0)
                {
                    return BadRequest("Wrong page");
                }
                if (pagesize < 10 || pagesize > 100)
                {
                    return BadRequest("Wrong page size");
                }
                List<Tag> tags = await _tagsService.GetTagsAsync(page, pagesize, order, sort);
                List<TagDTO> tagsDTO = await _tagsService.GetPopularityList(tags);
                return Ok(tagsDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
