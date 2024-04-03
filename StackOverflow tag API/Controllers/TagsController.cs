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
        private readonly ILogger _logger;
        public TagsController(ITagsService tagsService, ILogger<TagsController> logger)
        {
            _logger = logger;
            _tagsService = tagsService;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int page, int pagesize,string? order,string? sort)
        {
            try
            {
                _logger.LogInformation("Getting tags from database");
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
                _logger.LogError("Error Occured: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> Put()
        {
            try
            {
                _logger.LogInformation("Updating tags in database");
                await _tagsService.UpdateTagsInDb();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
