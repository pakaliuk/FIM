using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIM.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Post> Posts { get; set; }
      
    }
}
