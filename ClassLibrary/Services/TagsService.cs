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
        public async Task<List<Tag>> GetTagsAsync(int page, int pageSize, string? order, string? sort)
        {
            try
            {
                List<Tag> tags = new List<Tag>();
                if (page < 1)
                {
                    throw new ArgumentOutOfRangeException("Page number must be greater than 0");
                }
                if (pageSize < 5 || pageSize > 100)
                {
                    throw new ArgumentOutOfRangeException("Incorrect page size");
                }

                int tagsAmmount = await _context.Tags.CountAsync();

                if (page * pageSize + 100 > tagsAmmount)
                {
                    throw new ArgumentOutOfRangeException("Page out of range");
                }
                if (string.IsNullOrEmpty(sort) || sort == "popularity")
                {
                    if (string.IsNullOrEmpty(order) || (order != "asc"))
                    {
                        tags = await _context.Tags.OrderByDescending(tags => tags.Count).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                    }
                    else
                    {
                        tags = await _context.Tags.OrderBy(tags => tags.Count).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                    }
                }
                else if (sort == "name")
                {
                    if (string.IsNullOrEmpty(order) || (order != "asc"))
                    {
                        tags = await _context.Tags.OrderByDescending(tags => tags.Name).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                    }
                    else
                    {
                        tags = await _context.Tags.OrderBy(tags => tags.Name).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                    }
                }
                else
                {
                    throw new ArgumentException("Incorrect sort parameter");
                }
                return tags;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        public async Task<int> GetTagsCountAsync()
        {
            int count = await _context.Tags.SumAsync(tags => tags.Count);
            Console.WriteLine(" COUNTING RETURNED: " + count);
            return count;
        }

        public async Task UpdateTagsInDb()
        {
            try
            {
                if (_context.Tags.Count() > 0)
                {
                    await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Tags");
                }
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
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<TagDTO>> GetPopularityList(List<Tag> tags)
        {
            List<TagDTO> dtos = new List<TagDTO>();
            int count = await GetTagsCountAsync();
            foreach(Tag tag in tags)
            {
                dtos.Add(await GetPopularity(tag, count));
            }
            return dtos;
        }

        public async Task<TagDTO> GetPopularity(Tag tag, int? count)
        {
            if (!count.HasValue && count < 0)
            {
                count = await GetTagsCountAsync();
            }
            TagDTO dto = new TagDTO(tag, count);
            return dto;
        }
    }
}
