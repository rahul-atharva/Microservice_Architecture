using DataAccess.Api;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class MyMDBController<TEntity, TRepository> : ControllerBase
         where TEntity : class, IEntity
         where TRepository : IRepository<TEntity>
    {
        private readonly TRepository repository;

        public MyMDBController(TRepository repository)
        {
            this.repository = repository;
        }


        // GET: api/[controller]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TEntity>>> Get()
        {
            return await repository.GetAll();
        }

        // GET: api/[controller]/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TEntity>> Get(int id)
        {
            var member = await repository.Get(id);
            if (member == null)
            {
                return NotFound();
            }
            return member;
        }

        // PUT: api/[controller]/5
        [HttpPut("{id}")]
        public async virtual Task<IActionResult> Put(int id, TEntity member)
        {
            if (id != member.Id)
            {
                return BadRequest();
            }
            await repository.Update(member);
            return NoContent();
        }

        // POST: api/[controller]
        [HttpPost]
        public async Task<ActionResult<TEntity>> Post(TEntity member)
        {
            await repository.Add(member);
            return CreatedAtAction("Get", new { id = member.Id }, member);
        }

        // DELETE: api/[controller]/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TEntity>> Delete(int id)
        {
            var member = await repository.Delete(id);
            if (member == null)
            {
                return NotFound();
            }
            return member;
        }

    }
}
