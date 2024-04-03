using ClassLibrary.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.DataAccess
{
    public class DataSeeder
    {
        private readonly DataContext _context;
        private readonly ITagsService _tagsService;
        private readonly ILogger _logger;
        public DataSeeder(DataContext context, ITagsService tagsService, ILogger<DataSeeder> logger)
        {
            _logger = logger;
            _tagsService = tagsService;
            _context = context;
        }

        public bool IsDbEmpty()
        {
            return !_context.Tags.Any();
        }

        public async Task SeedDataAsync()
        {
            try
            {
                _logger.LogInformation("Seeding Data");
                await _tagsService.UpdateTagsInDb();
                
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while seeding the database: " + ex);
                throw;
            }
        }
    }
}
