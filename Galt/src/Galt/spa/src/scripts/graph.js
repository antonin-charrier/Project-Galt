module.exports = {
    simulation : 0,
    
    drawGraph: function() {
        var div = d3.select("#graph");
        var width = div._groups[0][0].offsetWidth;
        var height = div._groups[0][0].offsetHeight;

        var file = "data.json";
        var currentData;

        //json file
        var data = {
            "nodes" : [
                {"name": "Code.Cake", "entity":"source", "version":" 0.14"},
                {"name": "Cake.Core", "entity":"toUpdate", "version":"0.16.2", "lastVersion":"0.18.3"},
                {"name": "Cake.Common", "version":">= 0.16.2"},
                {"name": ".NETFramework", "version": "4.5", "entity":"platform"},
                {"name": ".NETStandard", "version": "1.6", "entity":"platform"},
                {"name": "NETStandard.Library", "version":">= 1.6.0"},
                {"name": "Microsoft.Win32.Registry", "version":">= 4.0.0"},
                {"name": "System.Diagnostics.Process", "version":"4.1.0"},
                {"name": "System.Runtime.InteropServices.RuntimeInformation", "version":">= 4.0.0"},
                {"name": "System.Runtime.Loader", "version":"4.0.0"},
                {"name": "Microsoft.Etensions.DependencyModel", "version":">= 1.0.0"},
                {"name": "System.Xml.XmlDocument", "version":">= 4.0.1"},
                {"name": "System.Xml.XPath", "version":">= 4.0.1"},
                {"name": "System.Xml.XPath.XmlDocument", "version":">= 4.0.1"},
                {"name": "System.Runtime.Serialization.Json", "version":">= 4.0.2"},
                {"name": "System.Xml.ReaderWriter", "version":">= 4.0.11", "entity":"toUpdate"},
                {"name": "System.ComponentModel.TypeConverter", "version":">= 4.1.0", "lastVersion":"4.2.0"}
            ],
            "links":[
                {"source":"Code.Cake", "target":"Cake.Core"},
                {"source":"Code.Cake", "target":"Cake.Common"},
                {"source":"Cake.Core", "target":".NETFramework"},
                {"source":"Cake.Core", "target":".NETStandard"},
                {"source":".NETStandard", "target":"NETStandard.Library"},
                {"source":".NETStandard", "target":"Microsoft.Win32.Registry"},
                {"source":".NETStandard", "target":"System.Diagnostics.Process"},
                {"source":".NETStandard", "target":"System.Runtime.InteropServices.RuntimeInformation"},
                {"source":".NETStandard", "target":"System.Runtime.Loader"},
                {"source":".NETStandard", "target":"Microsoft.Etensions.DependencyModel"},
                //{"source":".NETFramework", "target":"Cake.Core"},
                {"source":".NETStandard", "target":"System.Xml.XmlDocument"},
                {"source":".NETStandard", "target":"System.Xml.XPath"},
                {"source":".NETStandard", "target":"System.Xml.XPath.XmlDocument"},
                {"source":".NETStandard", "target":"System.Runtime.Serialization.Json"},
                {"source":".NETStandard", "target":"System.Xml.ReaderWriter"},
                {"source":"System.ComponentModel.TypeConverter", "target":"System.Xml.ReaderWriter"},
                {"source":".NETStandard", "target":"System.ComponentModel.TypeConverter"},
                {"source":"Cake.Common", "target":".NETFramework"},
                {"source":"Cake.Common", "target":".NETStandard"}
            ]
        };

        var node;
        var link;
        var allNodeDisplayed = true;

        currentData =(JSON.parse(JSON.stringify(data)));

        var svg = d3.select("#graph").append("svg")
            .attr("width", "100%")
            .attr("height", "100%")
            .call(d3.zoom()
                .scaleExtent([1 / 2, 4])
                .on("zoom", zoomed))
            .append("g")
            .attr("class", "movable");
        
        var clearButton = d3.selectAll("svg").append("rect")
            .attr("height", 50)
            .attr("width", 130)
            .attr("class", "clearButton")
            .attr ("cx", 10)
            .attr("cy", 10)
            .on("click", clearButtonClick);
            d3.selectAll("svg")
            .append("text").text("Show Problems")
            .attr("class", "textClearButton")
            .attr("x", 10)
            .attr("y", 25)
            .on("click", clearButtonClick);

        svg.append("defs").append("marker")
            .attr("id", "endMarkers")
            .attr("markerWidth", "6")
            .attr("markerHeight", "4")
            .attr("refX", "12")
            .attr("refY", "2")
            .attr("orient", "auto")
            .append("polyline").attr("points", "0,0 6,2 0,4");

        simulation = d3.forceSimulation()
            .force("link", d3.forceLink().id(function(d) { return d.name; }))
            .force("charge", d3.forceManyBody())
            .force("center", d3.forceCenter(width / 2, height / 2))
            
        //Declaration of tooltip to display informations in nodes
        var tip = d3.tip()
            .attr('class', 'd3-tip')
            .html(function(d) { 
                var chaine = "";
                chaine += '<div class="title">' + d.name + "</div>";
                if (d.entity == "toUpdate") chaine += '<br/><div class="red">To update</div>';
                else if (d.entity == null) chaine += '<br><div class = "green">Up to date</div>';
                if (d.version != null) chaine += "<br/>Version " + d.version + "<br/>";
                if (d.lastVersion != null) chaine += "Last version " + d.lastVersion + "<br/>";
                return chaine; 
        });

        svg.call(tip);

        createNodesAndLinks();

        

        node.append("title")
            .text(function(d) { return d.name; });

        simulation
            .nodes(currentData.nodes)
            .on("tick", ticked);

        simulation.force("link")
            .links(currentData.links)
            .distance(100);

        simulation.force('charge')
            .strength(-400);

        function ticked() {
            link
                .attr("x1", function(d) { return d.source.x; })
                .attr("y1", function(d) { return d.source.y; })
                .attr("x2", function(d) { return d.target.x; })
                .attr("y2", function(d) { return d.target.y; });

            node
                .attr("cx", function(d) { return d.x; })
                .attr("cy", function(d) { return d.y; });
        }

        function clearButtonClick()
        {
            if (allNodeDisplayed)
            {
                clearNodes();
            }
            else
            {
                currentData = (JSON.parse(JSON.stringify(data)));
                showNodes();
            }
            simulation.restart();
            allNodeDisplayed = !allNodeDisplayed;
        }

        //Create all the nodes and links from the currentData array
        function createNodesAndLinks()
        {
            link = svg.append("g")
                .attr("class", "link")
                .selectAll("line")
                .data(currentData.links)
                .enter()
                .append("line")
                .attr("class", "zoomable")
                .attr("stroke-width", function(d) { return Math.sqrt(2); })
                .attr("marker-end", 'url("#endMarkers")');

            node = svg.append("g")
                .selectAll("circle")
                .attr("class", "nodes zoomable circle")
                .data(currentData.nodes)
                .enter()
                .append("circle")
                .attr("r", 10)
                .call(d3.drag()
                    .on("start", dragstarted)
                    .on("drag", dragged)
                    .on("end", dragended))
                .attr("class", function (d) {
                    return chooseClass(d);
                })
                .on('mouseover', tip.show)
                .on('mouseout', tip.hide);
        }

        function showNodes()
        {
            node
            .exit()
            .attr("class", chooseClass);

            link
            .exit()
            .attr("class", chooseClass);

            simulation.nodes(currentData.nodes);
        }

        // Hide all the child any problem of the given node
        function clearNodes()
        {
            var removedNode = true;
            while (removedNode)
            {
                console.log("NOUVEAU TOUR D'INSPECTION !!!");
                var nodeToRemove = [];
                var linkToRemove = [];
                removedNode = false;

                // Choose node with any problem to delete
                for (var i = 0; i <= currentData.nodes.length-1; i++)
                {
                    if (currentData.nodes[i].entity != "toUpdate")
                    {
                        var j = 0;
                        var isParent = false;
                        while(j <= currentData.links.length-1 && !isParent)
                        {
                            isParent = false;
                            if (currentData.links[j].source == currentData.nodes[i]) 
                            {
                                isParent = true
                            }
                            j++
                        }
                        if (!isParent)
                        {
                            removedNode = true;
                            nodeToRemove = removeNode(currentData.nodes[i], nodeToRemove);
                            linkToRemove = removeLink(currentData.nodes[i], linkToRemove);
                        }
                    }
                }

                // clear the node array 
                if (nodeToRemove != [])
                {
                    console.log("Voici le tableau de NODES à nettoyer : ", currentData.nodes);
                    console.log("Nodes à supprimer : ", nodeToRemove);
                    for(var i = 0; i <= nodeToRemove.length-1; i++)
                    {
                        console.log("Node en cours de suppression : ", nodeToRemove[i]-i);
                        currentData.nodes.splice((nodeToRemove[i]-i), 1);
                    }
                    nodeToRemove = [];
                }

                // clear the link array
                if (linkToRemove != [])
                {
                    linkToRemove.sort(function(a, b) {
                        return a - b;
                    });
                    console.log("Voici le tableau de LIENS à nettoyer : ", currentData.links);
                    console.log("Liens à supprimer : ", linkToRemove);
                    for(var i = 0; i <= linkToRemove.length-1; i++)
                    {
                        console.log("Lien en cours de suppression : ", linkToRemove[i]-i);
                        currentData.links.splice((linkToRemove[i]-i), 1);
                    }
                    linkToRemove = [];
                }
            }
            console.log("PLUS RIEN A SIGNALER CHEF !!! Voici le tableau nettoyé de NODES : ", currentData.nodes);
            console.log("Voici le tableau nettoyé de LIENS : ", currentData.links);

            // Clear all the nodes and links
            svg.selectAll(".node")
            .data(currentData.nodes)
            .exit()
            .attr("class", "invisible");

            svg.selectAll("line")
            .data(currentData.links)
            .exit()
            .attr("class", "invisible");

            recolorNodes();
        }

        // Return the array of node to delete with index of the given node
        function removeNode (node, nodeToRemove)
        {
            if (node != undefined)
            {
                //console.log("On detruit la node : ", node);
                for (var i = 0; i <= currentData.nodes.length; i++)
                {
                    if (currentData.nodes[i] == node)
                    { 
                        nodeToRemove.push(i);
                    }
                    
                }
            }

            return nodeToRemove;
        }
        // Return the array of links to delete with index of the links associate given node
        function removeLink (node, linkToRemove)
        {
            if (node != undefined)
            {
                //console.log("On detruit les liens de la node : ", node);
                for (var i = 0; i <= currentData.links.length-1; i++)
                {
                    if (currentData.links[i].source == node || currentData.links[i].target == node)
                    { 
                        linkToRemove.push(i);
                    }
                    
                }
            }

            return linkToRemove;
        }
        
        //Refresh the class of all nodes
        function recolorNodes ()
        {
            svg.selectAll(".node")
            .data(currentData.nodes)
            .attr("class", function(d){return chooseClass(d)});
        }

        //Choose a class for the given node
        function chooseClass(d)
        {
            switch(d.entity)
            {
                case "source" :
                    return "node source";
                case "toUpdate":
                    return "node toUpdate";
                case "platform" :
                    return "node platform";
                default :
                    return "node default";
            }
        }

        function dragstarted(d) {
            if (!d3.event.active) simulation.alphaTarget(0.3).restart();
            d.fx = d.x;
            d.fy = d.y;
        }

        function dragged(d) {
            d.fx = d3.event.x;
            d.fy = d3.event.y;
        }

        function dragended(d) {
            if (!d3.event.active) simulation.alphaTarget(0);
            d.fx = null;
            d.fy = null;
        }

        function zoomed() {
            var transform = d3.event.transform;
            d3.selectAll(".movable").attr("transform", function(d) {
                return "translate(" + transform.applyX(d3.event.transform.x/6) + "," + transform.applyY(d3.event.transform.y/6) + ")";
            });
        }
    },

    resizeGraph: function() {
        var div = d3.select("#graph");
        var width = div._groups[0][0].offsetWidth;
        var height = div._groups[0][0].offsetHeight;

        simulation.force("center", d3.forceCenter(width / 2, height / 2));
        simulation.restart();
    }
};


