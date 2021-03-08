using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NAdmin.Persistence;
using Newtonsoft.Json;

namespace NAdmin
{
    public class NAdminController : Controller 
    {
        private readonly IEnumerable<INAdminEntity> _entities;
        private readonly NAdminDbContext _dbContext;

        public NAdminController(IEnumerable<INAdminEntity> entities, NAdminDbContext dbContext)
        {
            _entities = entities;
            _dbContext = dbContext;
        }
        
        [HttpGet]
        [Route("/admin/{entity}")]
        public async Task<IActionResult> List([FromRoute] string entity)
        {
            var entityType = _entities
                .Select(x => x.GetType())
                .FirstOrDefault(x => x.Name.Equals(entity, StringComparison.InvariantCultureIgnoreCase));

            var set = await _dbContext.Query(entityType)
                .Where(x => true).ToListAsync();

            return Ok(set);
        }

        [HttpPost]
        [Route("/admin/{entity}/Create")]
        public async Task<IActionResult> Create([FromRoute] string entity)
        {
            using var bodyStream = new StreamReader(Request.Body);
            var body = await bodyStream.ReadToEndAsync();
            
            var entityType = _entities
                .Select(x => x.GetType())
                .FirstOrDefault(x => x.Name.Equals(entity, StringComparison.InvariantCultureIgnoreCase));

            var obj = JsonConvert.DeserializeObject(body, entityType);

            _dbContext.Add(obj);
            await _dbContext.SaveChangesAsync();
            
            return Ok();
        }
    }
}
