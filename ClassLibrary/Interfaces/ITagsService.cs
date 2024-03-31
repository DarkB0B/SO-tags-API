using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Interfaces
{
    public interface ITagsService
    {
        Task<List<Tag>> GetTagsAsync(int page, int pageSize, string? order, string? sort);
        Task UpdateTagsInDb();
        Task<int> GetTagsCountAsync();
        Task<List<TagDTO>> GetPopularityList(List<Tag> tags);
        Task<TagDTO> GetPopularity(Tag tag, int? count);
    }
}
