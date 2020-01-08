using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SkipTake_Demo.Entities
{
    public class Toy
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int DogId { get; set; }
        [ForeignKey("DogId")]
        public Dog Dog { get; set; }
    }
}
