using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool HasSynonyms { get; set; }
        public bool IsRequired { get; set; }
        public bool IsModeratorOnly { get; set; }
        public int Count { get; set; }
    }
}
