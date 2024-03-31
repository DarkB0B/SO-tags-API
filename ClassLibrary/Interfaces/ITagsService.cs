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
        Task<List<Tag>> GetTagsAsync(int page, int pageSize);
        Task UpdateTagsInDb();
        Task<int> GetTagsCountAsync();
    }
}
