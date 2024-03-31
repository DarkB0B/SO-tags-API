using ClassLibrary.DataAccess;
using ClassLibrary.Interfaces;
using ClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Services
{
    public class TagsService : ITagsService
    {
        private readonly IApiService _apiService;
        private readonly DataContext _context;
        
        public TagsService(DataContext context, IApiService apiService)
        {
            _context = context;
            _apiService = apiService;
        }
        public async Task<List<Tag>> GetTagsAsync(int page, int pageSize)
        {
            return await _context.Tags.ToListAsync();
        }

        public Task<int> GetTagsCountAsync()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateTagsInDb()
        {
            try
            {
                List<Tag> tags = new List<Tag>();
                for (int i = 1; i <= 10; i++)
                {
                    List<Tag> tagList = await _apiService.GetTagsAsync(i, 100);
                    tags.AddRange(tagList);
                }
                await _context.Tags.AddRangeAsync(tags);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
