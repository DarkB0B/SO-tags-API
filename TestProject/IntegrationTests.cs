
using ClassLibrary.DataAccess;
using ClassLibrary.Interfaces;
using ClassLibrary.Models;
using ClassLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using StackOverflow_tag_API.Controllers;
using System.Net;

namespace TestProject
{
    public class IntegrationTests
    {
        [Fact]
        public async Task Get_Returns_OK_With_Correct_Data()
        {
            Random rnd = new Random();
            var mockTagsService = new Mock<ITagsService>();
            
            List<TagDTO> tags = new List<TagDTO>();
            
            for (int i = 0; i < 10; i++)
            {
                Tag tag = new Tag { Count = 5, HasSynonyms = false, IsModeratorOnly = false, IsRequired = false, Name = "test" + i };
                int popularity = rnd.Next(4, 30);
                tags.Add(new TagDTO(tag, popularity));
            }
            List<Tag> tags1 = new List<Tag>();
            var expectedTags = tags;
            mockTagsService.Setup(service => service.GetTagsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(tags1);
            mockTagsService.Setup(service => service.GetPopularityList(tags1)).ReturnsAsync(tags);
            var controller = new TagsController(mockTagsService.Object, Mock.Of<ILogger<TagsController>>());

            var result = await controller.Get(1, 10, "asc", "popularity") as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.True(expectedTags.All(tag => ((List<TagDTO>)result.Value).Contains(tag)));
            Assert.Equal(10, ((List<TagDTO>)result.Value).Count);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Get_Returns_BadRequest_On_Invalid_Page(int page)
        {
            var controller = new TagsController(Mock.Of<ITagsService>(), Mock.Of<ILogger<TagsController>>());

            var result = await controller.Get(page, 10, "asc", "popularity") as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
        }
        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(30)]
        public async Task Get_Returns_Ok_On_Valid_Page(int page)
        {

            var controller = new TagsController(Mock.Of<ITagsService>(), Mock.Of<ILogger<TagsController>>());

            var result = await controller.Get(page, 10, "asc", "popularity") as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        }

        
    }
}