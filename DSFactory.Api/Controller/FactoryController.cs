using Microsoft.AspNetCore.Mvc;
using DSFactory.Core;
using DSFactory.Api.Services;

namespace DSFactory.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FactoryController : ControllerBase
    {
        private readonly DataStore _store;

        public FactoryController(DataStore store)
        {
            _store = store;
        }

        [HttpPost("create")]
        public IActionResult Create(string id, StructureType type)
        {
            if (_store.Structures.ContainsKey(id)) 
                return Conflict($"A structure with ID '{id}' already exists.");

            IDataStructure<string> structure = StructureFactory.Create<string>(type);

            _store.Structures[id] = structure;
            return Ok($"Success! Created {structure.TypeName} with ID: {id}");
        }

        [HttpPost("add")]
        public IActionResult Add(string id, string value)
        {
            if (!_store.Structures.TryGetValue(id, out var ds)) 
                return NotFound("ID not found.");

            ds.Add(value);
            return Ok($"Added '{value}'. Current Count: {ds.Count}");
        }

        [HttpGet("remove")]
        public IActionResult Remove(string id)
        {
            if (!_store.Structures.TryGetValue(id, out var ds)) 
                return NotFound("ID not found.");

            if (ds.Count == 0) 
                return BadRequest("Structure is empty.");

            var item = ds.Remove();
            
            return Ok(new { Removed = item, RemainingCount = ds.Count });
        }

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            var summary = _store.Structures.Select(kvp => new 
            { 
                Id = kvp.Key, 
                Type = kvp.Value.TypeName, 
                Count = kvp.Value.Count 
            });
            return Ok(summary);
        }

        [HttpGet("draw")]
        public IActionResult Draw(string id)
        {
            if (!_store.Structures.TryGetValue(id, out var ds)) 
                return NotFound("ID not found.");

            return Ok(ds.Display());
        }
    }
}