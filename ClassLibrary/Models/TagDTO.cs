using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models
{
    public class TagDTO : Tag
    {
        public double Popularity { get; set; }

        public TagDTO(Tag tag, int? tagCount)
        {
            Id = tag.Id;
            Name = tag.Name;
            Count = tag.Count;
            HasSynonyms = tag.HasSynonyms;
            IsModeratorOnly = tag.IsModeratorOnly;
            IsRequired = tag.IsRequired;


            Popularity = (((double)tag.Count / (int)tagCount) * 100);

        }
    }
}
