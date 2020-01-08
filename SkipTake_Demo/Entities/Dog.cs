using System.Collections.Generic;

namespace SkipTake_Demo.Entities
{
    public class Dog
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ParentName { get; set; }

        public OwnerId OwnerId { get; set; }

        public List<Toy> Toys { get; set; }
    }

    public enum OwnerId
    {
        Bob,
        Joe
    }
}
