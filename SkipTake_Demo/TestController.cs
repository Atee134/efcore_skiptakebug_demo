using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkipTake_Demo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkipTake_Demo
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class TestController : Controller
    {
        private static Random R = new Random();

        private readonly DogsDbContext context;

        public TestController(DogsDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Test([FromQuery] bool evaluateBeforeSkipTake = false)
        {
            context.Database.Migrate();

            if (!await context.Dogs.AnyAsync())
            {
                for (int i = 0; i < 100; i++)
                {
                    context.Dogs.Add(new Dog
                    {
                        Name = "Dog#" + i,
                        OwnerId = R.Next(0, 2) == 0 ? OwnerId.Bob : OwnerId.Joe,
                        ParentName = "Dog#" + R.Next(0, 100),
                        Toys = new List<Toy>
                        {
                            new Toy
                            {
                                Name = "Toy#" + R.Next(0,1000)
                            }
                        }
                    });
                }

                await context.SaveChangesAsync();
            }

            IQueryable<Dog> dogQuery = context.Dogs
                .Include(d => d.Toys);

            IQueryable<DogTuple> query = from dog in dogQuery
                                         join parent in context.Dogs on new { name = dog.ParentName, ownerId = dog.OwnerId } equals new { name = parent.Name, ownerId = parent.OwnerId }
                                            into queryResult
                                            from parent in queryResult.DefaultIfEmpty()
                                            select new DogTuple { Dog = dog, Parent = parent };

            if (evaluateBeforeSkipTake)
            {
                var whatever = await query.ToListAsync();
            }

            query = query.Skip(10).Take(10);

            var result = await query.ToListAsync();

            int countWhereToysAreExcluded = result.Count(d => d.Dog.Toys == null);

            return Json(new { evaluatedBeforeSkipTake = evaluateBeforeSkipTake, countOfNavigationPropertiesExcluded = countWhereToysAreExcluded });
        }

        class DogTuple
        {
            public Dog Dog { get; set; }
            public Dog Parent { get; set; }
        }
    }
}
