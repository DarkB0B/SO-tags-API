using ClassLibrary.DataAccess;
using ClassLibrary.Interfaces;
using ClassLibrary.Models;
using ClassLibrary.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class UnitTests
    {
        [Fact]
        public async Task GetTagsAsync_Returns_Correct_Data()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var context = new DataContext(options))
            {
                List<Tag> tags = new List<Tag>();
                for (int i = 0; i < 30; i++)
                {
                    tags.Add(new Tag { Count = 5, HasSynonyms = false, IsModeratorOnly = false, IsRequired = false, Name = "test" + i });
                }
                context.Tags.AddRange(tags);
                await context.SaveChangesAsync();
            }

            var mockApiService = new Mock<IApiService>();

            var service = new TagsService(new DataContext(options), mockApiService.Object, Mock.Of<ILogger<TagsService>>());

            var result = await service.GetTagsAsync(1, 30, "asc", "name");
            var result2 = await service.GetTagsAsync(1, 10, null, "name");

            Assert.Equal(30, result.Count);
            Assert.Equal(10, result2.Count);
            Assert.Equal("test0", result[0].Name);
            Assert.Equal("test9", result2[0].Name);
        }
        [Fact]
        public async Task GetTaskAsync_ThrowsError_On_Wrong_Data()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var context = new DataContext(options))
            {
                List<Tag> tags = new List<Tag>();
                for (int i = 0; i < 100; i++)
                {
                    tags.Add(new Tag { Count = 5, HasSynonyms = false, IsModeratorOnly = false, IsRequired = false, Name = "test" + i });
                }
                context.Tags.AddRange(tags);
                await context.SaveChangesAsync();
            }
            var mockApiService = new Mock<IApiService>();

            var service = new TagsService(new DataContext(options), mockApiService.Object, Mock.Of<ILogger<TagsService>>());

            Assert.Throws<ArgumentOutOfRangeException>(() => service.GetTagsAsync(1, 0, null, null).GetAwaiter().GetResult());
            Assert.Throws<ArgumentOutOfRangeException>(() => service.GetTagsAsync(1, 102, null, null).GetAwaiter().GetResult());
            Assert.Throws<ArgumentOutOfRangeException>(() => service.GetTagsAsync(0, 99, null, null).GetAwaiter().GetResult());
            Assert.Throws<ArgumentException>(() => service.GetTagsAsync(1, 100, null, "whatever").GetAwaiter().GetResult());
            
        }
    }
}
