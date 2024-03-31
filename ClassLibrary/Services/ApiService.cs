using ClassLibrary.DataAccess;
using ClassLibrary.Interfaces;
using ClassLibrary.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json;
using System.Net;

namespace ClassLibrary.Services
{
    public class ApiService : IApiService
    {
        private readonly DataContext _context;
        private readonly HttpClient httpClient;

        public ApiService(DataContext context)
        {
            _context = context;
            var clientHandler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate };
            httpClient = new HttpClient(clientHandler);
            httpClient.BaseAddress = new Uri("https://api.stackexchange.com/2.3/");
        }
        
        public async Task<List<Tag>> GetTagsAsync(int page, int pageSize)
        {
            string url = $"tags?page={page}&pagesize={pageSize}&order=desc&sort=popular&site=stackoverflow";
            if(page <= 0)
            {
                page = 1;
            }
            if(pageSize <= 0)
            {
                pageSize = 10;
            }
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if(response.IsSuccessStatusCode)
            {
                var tagsResult = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
                List<Tag> tags = new List<Tag>();
                foreach(dynamic item in tagsResult.items)
                {
                    Tag tag = new Tag
                    {
                        Name = item.name,
                        HasSynonyms = item.has_synonyms,
                        IsRequired = item.is_required,
                        IsModeratorOnly = item.is_moderator_only,
                        Count = item.count,
                    };
                    tags.Add(tag);
                }
                return tags;
            }
            
            else
            {
                throw new Exception($"SO API Status Code = {response.StatusCode.ToString()}");
            }
        }
        

    }
}
