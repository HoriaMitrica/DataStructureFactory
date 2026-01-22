using Microsoft.AspNetCore.Mvc;
using DSFactory.Core;
using DSFactory.Api.Services;

namespace DSFactory.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StructuresController : ControllerBase
    {
        private readonly DataStore _store;
        private readonly XmlDataService _xmlService;

        public StructuresController(DataStore store, XmlDataService xmlService)
        {
            _store = store;
            _xmlService = xmlService;
        }

        [HttpPost]
        public IActionResult CreateStructure(string id, StructureType type)
        {
            if (_store.Structures.ContainsKey(id))
                return Conflict($"Structure '{id}' already exists.");

            var structure = StructureFactory.Create<string>(type);
            _store.Structures[id] = structure;

            return CreatedAtAction(nameof(GetStructureInfo), new { id = id }, structure);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStructure(string id)
        {
            if (!_store.Structures.TryRemove(id, out _))
            {
                return NotFound($"Structure '{id}' not found.");
            }
            return NoContent();
        }

        [HttpGet("{id}")]
        public IActionResult GetStructureInfo(string id)
        {
            if (!_store.Structures.TryGetValue(id, out var ds))
                return NotFound();

            return Ok(new
            {
                Id = id,
                Type = ds.TypeName,
                Count = ds.Count
            });
        }

        [HttpGet]
        public IActionResult GetAllStructures()
        {
            var summary = _store.Structures.Select(kvp => new
            {
                Id = kvp.Key,
                Type = kvp.Value.TypeName,
                Count = kvp.Value.Count
            });
            return Ok(summary);
        }


        [HttpPost("{id}/items")]
        public IActionResult AddItem(string id, [FromBody] string value)
        {
            if (!_store.Structures.TryGetValue(id, out var ds))
                return NotFound($"Structure '{id}' not found.");

            ds.Add(value);
            return Ok($"Added '{value}'.");
        }


        [HttpDelete("{id}/items")]
        public IActionResult PopItem(string id)
        {
            if (!_store.Structures.TryGetValue(id, out var ds))
                return NotFound($"Structure '{id}' not found.");

            if (ds.Count == 0)
                return BadRequest("Structure is empty.");

            var item = ds.Remove();
            return Ok(new { Removed = item, Remaining = ds.Count });
        }


        [HttpGet("{id}/visual")]
        public IActionResult GetVisual(string id)
        {
            if (!_store.Structures.TryGetValue(id, out var ds))
                return NotFound();

            return Ok(ds.Display());
        }

        [HttpPost("import")]
        public IActionResult Import(IFormFile file)
        {
            if (file == null) return BadRequest();
            using var stream = file.OpenReadStream();
            return Ok(_xmlService.ProcessXml(stream));
        }

        [HttpGet("export")]
        public IActionResult Export()
        {
            if (_store.Structures.IsEmpty) return BadRequest("No data.");

            var content = _xmlService.ExportToXml();
            string fileName = $"DataFactory_Export_{DateTime.Now:yyyyMMdd_HHmmss}.xml";
            return File(content, "application/xml", fileName);
        }

        [HttpPost("smart-load")]
        public IActionResult SmartLoad(string id, [FromBody] List<string> data)
        {
            if (_store.Structures.ContainsKey(id))
                return Conflict($"ID '{id}' already exists.");

            var structure = SmartFactory.SmartCreate(data);

            _store.Structures[id] = structure;

            return Ok(new
            {
                Message = "Smart Analysis Complete",
                Decision = structure.TypeName,
                Count = structure.Count,
                Visual = structure.Display()
            });
        }

        [HttpPost("{id}/edge")]
        public IActionResult AddEdge(string id, string from, string to)
        {
            if (!_store.Structures.TryGetValue(id, out var ds))
                return NotFound($"Structure '{id}' not found.");

            if (ds is DirectedGraph<string> graph)
            {
                graph.AddEdge(from, to);
                return Ok($"Connected ({from}) -> ({to})");
            }

            return BadRequest($"Structure '{id}' is a {ds.TypeName}. It does not support Edges.");
        }
    }
}