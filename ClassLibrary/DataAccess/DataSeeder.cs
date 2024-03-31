using ClassLibrary.Interfaces;
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
        public DataSeeder(DataContext context, ITagsService tagsService)
        {
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
                Console.WriteLine("Seeding Data");
                await _tagsService.UpdateTagsInDb();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
