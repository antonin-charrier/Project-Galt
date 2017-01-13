using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// ============================================================
//  Made by Léo alias "The Warlod of the Eight Peaks, Scarsnik"
// ============================================================

namespace Galt.Crawler.Util
{
    public class ConvertGraphData
    {

        Dictionary<string, List<Dictionary<string, string>>> _graph;

        // Convert the given VPackage in a JSON object with all the informations needed
        public void convertGraphData(VPackage vPackage)
        {
            _graph = new Dictionary<string, List<Dictionary<string, string>>>();
            _graph.Add("nodes", new List<Dictionary<string, string>>());
            _graph.Add("links", new List<Dictionary<string, string>>());

            _graph["nodes"].Add(VPackageToDictionary(vPackage.PackageId, _graph.Count.ToString(), "source", vPackage.Version.ToString()));
            AddDependency(vPackage, "0");

            // Add the warnings on nodes withs issues
            foreach (Dictionary<string, string> currentNode in _graph["nodes"])
            {
                foreach (Dictionary<string, string> otherNode in _graph["nodes"])
                {
                    if (currentNode["id"] != otherNode["id"] && currentNode["version"] != currentNode["version"])
                    {
                        currentNode.Add("warning", "versionConflict");
                        otherNode.Add("warning", "versionConflict");
                    }
                }
            }
        }

        // Add dependencies of the given VPackage in the JSON object
        private void AddDependency(VPackage vPackage, string ParentId)
        {
            string id = "0";
            foreach (Framework framework in vPackage.Dependencies.DicDependencies.Keys)
            {
                _graph["nodes"].Add(VPackageToDictionary(framework.FrameworkId, _graph.Count.ToString(), "platform", vPackage.Version.ToString()));
                foreach (VPackage newVPackage in vPackage.Dependencies.DicDependencies[framework])
                {
                    id = _graph.Count.ToString();
                    _graph["nodes"].Add(VPackageToDictionary(newVPackage.PackageId, id, null, vPackage.Version.ToString()));
                    _graph["links"].Add(CreateLink(ParentId, id));
                    AddDependency(newVPackage, id);
                }
            }
        }

        // Convert a VPackage to a Dictionary
        private Dictionary<string, string> VPackageToDictionary(string name, string id, string entity, string version)
        {
            Dictionary<string, string> dico = new Dictionary<string, string>();

            dico.Add("id", id);
            dico.Add("name", name);
            if (entity != null || entity != "") dico.Add("entity", entity);
            if (version != null || version != "") dico.Add("version", version);

            return dico;
        }

        //Create a Dictionnary for a link
        private Dictionary<string, string> CreateLink(string source, string target)
        {
            Dictionary<string, string> dico = new Dictionary<string, string>();

            dico.Add("source", source);
            dico.Add("target", target);

            return dico;
        }

        public Dictionary<string, List<Dictionary<string, string>>> Graph
        {
            get { return _graph; }
        }
    }
}
