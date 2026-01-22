using System.Xml.Linq;
using DSFactory.Core;

namespace DSFactory.Api.Services
{
    public class XmlDataService
    {
        private readonly DataStore _store;

        public XmlDataService(DataStore store)
        {
            _store = store;
        }

        public string ProcessXml(Stream fileStream)
        {
            try
            {
                var doc = XDocument.Load(fileStream);
                int createdCount = 0;

                foreach (var element in doc.Descendants("Structure"))
                {
                    string id = element.Attribute("id")?.Value;
                    string typeStr = element.Attribute("type")?.Value;

                    if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(typeStr))
                        continue;

                    if (!Enum.TryParse(typeStr, true, out StructureType type))
                        continue;

                    if (_store.Structures.ContainsKey(id)) continue;

                    var structure = StructureFactory.Create<string>(type);

                    foreach (var itemNode in element.Descendants("Item"))
                    {
                        structure.Add(itemNode.Value);
                    }
                    
                    if (structure is DirectedGraph<string> graph)
                    {
                        foreach (var edgeNode in element.Descendants("Edge"))
                        {
                            string? from = edgeNode.Attribute("from")?.Value;
                            string? to = edgeNode.Attribute("to")?.Value;

                            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
                            {
                                graph.AddEdge(from, to);
                            }
                        }
                    }

                    _store.Structures[id] = structure;
                    createdCount++;
                }

                return $"Successfully imported {createdCount} structures.";
            }
            catch (Exception ex)
            {
                return $"Error parsing XML: {ex.Message}";
            }
        }
        public byte[] ExportToXml()
        {
            var root = new XElement("Import");

            foreach (var kvp in _store.Structures)
            {
                string id = kvp.Key;
                IDataStructure<string> structure = kvp.Value;

                var structElement = new XElement("Structure",
                    new XAttribute("id", id),
                    new XAttribute("type", structure.StructureType.ToString())
                );

                foreach (var item in structure)
                {
                    structElement.Add(new XElement("Item", item));
                }

                root.Add(structElement);
            }

            var doc = new XDocument(root);
            using (var stream = new MemoryStream())
            {
                doc.Save(stream);
                return stream.ToArray();
            }
        }
    }
}