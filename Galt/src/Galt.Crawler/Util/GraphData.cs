using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// ============================================================
//  Made by Léo alias "The Warlod of the Eight Peaks, Scarsnik"
// ============================================================

namespace Galt.Crawler.Util
{
    public class GraphData
    {
        Dictionary<string, object> _info;
        Dictionary<string, List<Dictionary<string, string>>> _graph;

        // Convert the given VPackage in a JSON object with all the informations needed
        public Dictionary<string, object> ConvertGraphData(VPackage vPackage)
        {
            _graph = new Dictionary<string, List<Dictionary<string, string>>>();
            _graph.Add("nodes", new List<Dictionary<string, string>>());
            _graph.Add("links", new List<Dictionary<string, string>>());

            _info = new Dictionary<string, object>();
            _info.Add("graph", _graph);
            _info.Add("versionConflict", new List<Dictionary<string, object>>());
            _info.Add("toUpdate", new List<Dictionary<string, string>>());

            string version = vPackage.Version.Major + "." + vPackage.Version.Minor + "." + vPackage.Version.Revision;
            _graph["nodes"].Add(VPackageToDictionary(vPackage.PackageId, _graph["nodes"].Count.ToString(), "source", version, vPackage.LastVersion));
            AddDependency(vPackage, "0");

            // Add the warnings on nodes withs version conflict
            foreach (Dictionary<string, string> currentNode in _graph["nodes"])
            {
                foreach (Dictionary<string, string> otherNode in _graph["nodes"])
                {
                    if (currentNode["name"] == otherNode["name"]
                        && currentNode["id"] != otherNode["id"]
                        && currentNode.ContainsKey("version")
                        && currentNode["version"] != otherNode["version"])
                    {
                        if ((currentNode.ContainsKey("entity") && currentNode["entity"] != "platform")||!currentNode.ContainsKey("entity"))
                        {
                            if (!currentNode.Keys.Contains("warning") && !otherNode.Keys.Contains("warning"))
                            {
                                currentNode.Add("warning", "versionConflict");
                                otherNode.Add("warning", "versionConflict");
                            }
                            else
                            {
                                currentNode["warning"] = "versionConflict";
                                otherNode["warning"] = "versionConflict";
                            }

                            foreach (Dictionary<string,string> link in _graph["links"])
                            {
                                if (currentNode["id"] == link["target"] || otherNode["id"] == link["target"])
                                {
                                    if (link.ContainsKey("warning")) link["warning"] = "versionConflict";
                                    else link.Add("warning", "versionConflict");
                                }
                            }

                            // Adding all version conflicts in the list of issues
                            List<Dictionary<string, object>> versionConflicts = (List<Dictionary<string, object>>)_info["versionConflict"];
                            bool contains = false;
                            foreach (Dictionary<string, object> conflict in versionConflicts)
                            {
                                if ((string)conflict["name"] == currentNode["name"]) contains = true;
                            }
                            if (!contains)
                            {
                                versionConflicts.Add(new Dictionary<string, object>());
                                versionConflicts[versionConflicts.Count - 1].Add("name", currentNode["name"]);
                                versionConflicts[versionConflicts.Count - 1].Add("versions", new List<string>());

                                versionConflicts[versionConflicts.Count - 1].Add("origine", new Dictionary<string, List<string>>());
                                if (!((Dictionary<string, List<string>>)versionConflicts[versionConflicts.Count - 1]["origine"]).ContainsKey(currentNode["version"]))
                                    ((Dictionary<string, List<string>>)versionConflicts[versionConflicts.Count - 1]["origine"]).Add(currentNode["version"], getDirectParents(currentNode["id"]));
                                if (!((Dictionary<string, List<string>>)versionConflicts[versionConflicts.Count - 1]["origine"]).ContainsKey(otherNode["version"]))
                                    ((Dictionary<string, List<string>>)versionConflicts[versionConflicts.Count - 1]["origine"]).Add(otherNode["version"], getDirectParents(otherNode["id"]));
                            }
                        }
                    }
                }
            }
            return _info;
        }

        private List<string> getDirectParents (string id)
        {
            List<string> list = new List<string>();

            foreach (Dictionary<string, string> link in _graph["links"])
            {
                if (link["target"] == id)
                {
                    foreach (Dictionary<string, string> node in _graph["nodes"])
                    {
                        if (node["id"] == link["source"])
                        {
                            if (node.ContainsKey("entity") && node["entity"] == "platform")
                            {
                                List<string> newResearch = getDirectParents(link["source"]);
                                foreach (string newEntry in newResearch)
                                {
                                    if (!list.Contains(newEntry)) list.Add(newEntry);
                                }
                            }
                            else
                            {
                                if(!list.Contains(node["name"])) list.Add(node["name"]);
                            }
                        }
                    }
                }
            }

            return list;
        }

        // Add dependencies of the given VPackage in the JSON object
        private void AddDependency(VPackage vPackage, string ParentId)
        {
            string id = "0";
            string newParentId = ParentId;
            foreach (string framework in vPackage.Dependencies.Keys)
            {
                id = _graph["nodes"].Count.ToString();
                //Add the framework to the graph
                if (vPackage.Dependencies[framework].Count() != 0 && framework != "Unsupported,Version=v0.0")
                {
                    _graph["nodes"].Add(VPackageToDictionary(framework, id, "platform", vPackage.Version.ToString(), null));
                    _graph["links"].Add(CreateLink(ParentId, id));
                    newParentId = id;
                }

                // Adding of all the package in the framework. Ignore present package
                foreach (VPackage newVPackage in vPackage.Dependencies[framework])
                {
                    id = _graph["nodes"].Count.ToString();
                    bool found = false;
                    string idFound = "0";

                    // Search if the package is already in the graph
                    foreach(Dictionary<string, string> node in _graph["nodes"])
                    {
                        string version = newVPackage.Version.Major + "." + newVPackage.Version.Minor + "." + newVPackage.Version.Revision;
                        if (node["name"] == newVPackage.PackageId && node["version"] == version)
                        {
                            found = true;
                            idFound = node["id"];
                        }
                    }

                    if (!found)
                    {
                        string version = newVPackage.Version.Major + "." + newVPackage.Version.Minor + "." + newVPackage.Version.Revision;
                        _graph["nodes"].Add(VPackageToDictionary(newVPackage.PackageId, id, null, version, newVPackage.LastVersion));
                        _graph["links"].Add(CreateLink(newParentId, id));
                        AddDependency(newVPackage, id);
                    }
                    else
                    {
                        _graph["links"].Add(CreateLink(newParentId, idFound));
                    }
                    
                }
            }
        }

        // Convert create a Dictionary for a node
        private Dictionary<string, string> VPackageToDictionary(string name, string newId, string entity, string version, string lastVersion)
        {
            Dictionary<string, string> dico = new Dictionary<string, string>();

            dico.Add("id", newId);
            dico.Add("name", name);
            if (!String.IsNullOrWhiteSpace(entity))
                dico.Add("entity", entity);
            if (!String.IsNullOrWhiteSpace(version) && entity != "platform")
                dico.Add("version", version);

            if (entity != "platform" && version != lastVersion)
            {
                dico.Add("warning", "toUpdate");
                bool contains = false;
                foreach (Dictionary<string, string> dic in (List<Dictionary<string, string>>) _info["toUpdate"])
                {
                    if (dic["name"] == name) contains = true;
                }

                if (!contains)
                {
                    List<Dictionary<string, string>> toUpdate = (List<Dictionary<string, string>>)_info["toUpdate"];
                    toUpdate.Add(new Dictionary<string, string>());
                    toUpdate[toUpdate.Count - 1].Add("name", name);
                    toUpdate[toUpdate.Count-1].Add("currentVersion", version);
                    toUpdate[toUpdate.Count-1].Add("lastVersion", lastVersion);
                }
            }

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
