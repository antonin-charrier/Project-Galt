module.exports = {
    drawGraph: function(data) {
        var width = d3.select("#graph")._groups[0][0].offsetWidth;
        var height = d3.select("#graph")._groups[0][0].offsetHeight;
        var currentGraph;

        //data = JSON.parse(data);
        var node;
        var link;
        var allNodeDisplayed = true;

        var graph = {
            "simulation":"",
            "completGraph":{"data":JSON.parse(JSON.stringify(data)), "link":"", "node":""},
            "problemsGraph":{"data":JSON.parse(JSON.stringify(data)), "link":"", "node":""}
        };

        d3.select("#graph").selectAll("svg").remove();

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
            .append("text").text("Display Issues")
            .attr("class", "textClearButton")
            .attr("x", 10)
            .attr("y", 25)
            .on("click", clearButtonClick);

        svg.append("defs").append("marker")
            .attr("id", "endMarkersNormal")
            .attr("markerWidth", "12")
            .attr("markerHeight", "8")
            .attr("refX", "18.5")
            .attr("refY", "4")
            .attr("orient", "auto")
            .append("polyline").attr("points", "0,0 12,4 0,8");

        svg.append("defs").append("marker")
            .attr("id", "endMarkersVersionConflict")
            .attr("markerWidth", "12")
            .attr("markerHeight", "8")
            .attr("refX", "18.5")
            .attr("refY", "4")
            .attr("orient", "auto")
            .append("polyline").attr("points", "0,0 12,4 0,8");

        //Declaration of tooltip to display informations in nodes
        var tip = d3.tip()
            .attr('class', 'd3-tip')
            .html(function(d) { 
                var chaine = "";
                chaine += '<div class="title">' + d.name + "</div>";
                if (d.warning == "toUpdate") chaine += '<br/><div class="warning">To update</div>';
                else if (d.warning == "versionConflict") chaine += '<br><div class = "error">Version Conflict</div>';
                else chaine += '<br><div class = "green">Up to date</div>';
                if (d.version != null) chaine += "<br/>Version " + d.version + "<br/>";
                if (d.lastVersion != null) chaine += "Last version " + d.lastVersion + "<br/>";
                return chaine; 
        });

        graph.simulation = d3.forceSimulation()
            .force("link", d3.forceLink().id(function(d) { return d.id; }))
            .force("charge", d3.forceManyBody())
            .force("center", d3.forceCenter(width / 2, height / 2));

        

        svg.call(tip);

        currentGraph = graph.completGraph;
        createNodesAndLinks("completGraph");

        currentGraph = graph.problemsGraph;
        clearNodes();
        createNodesAndLinks("problemsGraph");

        d3.selectAll(".problemsGraph").attr("style", "display:none");

        changeSimulation("completGraph")
        
        currentGraph = graph.completGraph;

        // =======================================================
        //       Functions start here !!!
        //========================================================

        // Change the graph used in the simulation
        function changeSimulation (graphName)
        {
            currentGraph = graph[graphName];
            graph.simulation
                .nodes(graph[graphName].data.nodes)
                .on("tick", ticked);

            graph.simulation.force("link")
                .links(graph[graphName].data.links)
                .distance(100);

            graph.simulation.force('charge')
                .strength(-400);
            
            graph.simulation.restart();
        }

         
        function definedLinkStrength(d)
        {
            var strength = 2;

            if (d.target.entity === "platform") strength = 4;

            return strength;
        }

        function definedLinkDistance(d)
        {
            var distance = 100;

            if (d.target.entity === "platform") distance = 0.0001;

            return distance;
        }

        function ticked() {
            currentGraph.link
                .attr("x1", function(d) { return d.source.x; })
                .attr("y1", function(d) { return d.source.y; })
                .attr("x2", function(d) { return d.target.x; })
                .attr("y2", function(d) { return d.target.y; });

            currentGraph.node
                .attr("cx", function(d) { return d.x; })
                .attr("cy", function(d) { return d.y; });

                width = d3.select("#graph")._groups[0][0].offsetWidth;
                height = d3.select("#graph")._groups[0][0].offsetHeight;
                graph.simulation.force("center", d3.forceCenter(width / 2, height / 2));
        }

        function clearButtonClick()
        {
            if (allNodeDisplayed)
            {
                d3.selectAll(".completGraph").attr("style", "display:none");
                d3.selectAll(".problemsGraph").attr("style", "display:initial");
                currentGraph = graph.problemsGraph;
                changeSimulation("problemsGraph")
            }
            else
            {
                d3.selectAll(".completGraph").attr("style", "display:initial");
                d3.selectAll(".problemsGraph").attr("style", "display:none");
                 changeSimulation("completGraph")
            }
            
           
            allNodeDisplayed = !allNodeDisplayed;
        }

        //Create all the nodes and links from the currentData array
        function createNodesAndLinks(graphName)
        {
            var currentG = d3.selectAll(".movable").append("g").attr("class", graphName);

            currentGraph.link = currentG.append("g")
                .attr("class", "links")
                .selectAll("line")
                .data(graph[graphName].data.links)
                .enter()
                .append("line")
                .attr("class", function (d){
                    if(d.warning === "versionConflict") return "versionConflictLink";
                    else return "normalLink";
                })
                .attr("stroke-width", function(d) { return Math.sqrt(2); })
                .attr("marker-end", function (d){
                    if(d.warning === "versionConflict") return 'url("#endMarkersVersionConflict")';
                    else return 'url("#endMarkersNormal")';
                });

            currentGraph.node = currentG.append("g")
                .attr("class", "nodes")
                .selectAll("circle")
                .attr("class", "nodes zoomable circle")
                .data(graph[graphName].data.nodes)
                .enter()
                .append("circle")
                .attr("r", function (d){
                    if (d.entity == "source") return 20;
                    return 10;
                })
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

        // Hide all the child any problem of the given node
        function clearNodes()
        {
            var removedNode = true;
            while (removedNode)
            {
                var nodeToRemove = [];
                var linkToRemove = [];
                removedNode = false;

                // Choose node with any problem to delete
                for (var i = 0; i <= graph.problemsGraph.data.nodes.length-1; i++)
                {
                    if ((graph.problemsGraph.data.nodes[i].entity == "platform") || (graph.problemsGraph.data.nodes[i].warning != "versionConflict" && graph.problemsGraph.data.nodes[i].entity != "source"))
                    {
                        var j = 0;
                        var isParent = false;
                        while(j <= graph.problemsGraph.data.links.length-1 && !isParent)
                        {
                            isParent = false;
                            if (graph.problemsGraph.data.links[j].source == graph.problemsGraph.data.nodes[i].id) 
                            {
                                isParent = true
                            }
                            j++
                        }
                        if (!isParent)
                        {
                            removedNode = true;
                            nodeToRemove = removeNode(graph.problemsGraph.data.nodes[i], nodeToRemove, graph.problemsGraph.data);
                            linkToRemove = removeLink(graph.problemsGraph.data.nodes[i], linkToRemove, graph.problemsGraph.data);

                        }
                    }
                }

                // clear the nodes array 
                if (nodeToRemove != [] && nodeToRemove.length != 0)
                {
                    for(var i = 0; i <= nodeToRemove.length-1; i++)
                    {
                        graph.problemsGraph.data.nodes.splice((nodeToRemove[i]-i), 1);
                    }
                    nodeToRemove = [];
                }
                // clear the links array
                if (linkToRemove != [] && linkToRemove.length != 0)
                {
                    linkToRemove.sort(function(a, b) {
                        return a - b;
                    });
                    for(var i = 0; i <= linkToRemove.length-1; i++)
                    {
                        graph.problemsGraph.data.links.splice((linkToRemove[i]-i), 1);
                    }
                    linkToRemove = [];
                }
            }
        }

        // Return the array of node to delete with index of the given node
        function removeNode (node, nodeToRemove, currentData)
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
        function removeLink (node, linkToRemove, currentData)
        {
            if (node != undefined)
            {
                for (var i = 0; i <= currentData.links.length-1; i++)
                {
                    if ((graph.problemsGraph.data.links[i].source == node.id || graph.problemsGraph.data.links[i].target == node.id) && linkToRemove.indexOf(i) == -1)
                    { 
                        linkToRemove.push(i);
                    }
                }
            }

            return linkToRemove;
        }

        //Choose a class for the given node
        function chooseClass(d)
        {
            switch(d.entity)
            {
                case "source" :
                    return "node source";
                case "platform" :
                    return "node platform";
            }

            switch(d.warning)
            {
                case "toUpdate":
                    return "node toUpdate";
                case "versionConflict":
                    return "node versionConflict";
                default :
                    return "node default";
            }
        }

        function dragstarted(d) {
            if (!d3.event.active) graph.simulation.alphaTarget(0.3).restart();
            d.fx = d.x;
            d.fy = d.y;
        }

        function dragged(d) {
            d.fx = d3.event.x;
            d.fy = d3.event.y;
        }

        function dragended(d) {
            if (!d3.event.active) graph.simulation.alphaTarget(0);
            d.fx = null;
            d.fy = null;
        }

        var zoom = d3.zoom()
            .scaleExtent([1, 40])
            .translateExtent([[-100, -100], [width + 90, height + 100]])
            .on("zoom", zoomed);

        svg.call(zoom);

        function zoomed() {
            d3.selectAll(".movable").attr("transform", d3.event.transform);
        }

        function resetted() {
            svg.transition()
                .duration(750)
                .call(zoom.transform, d3.zoomIdentity);
    }}
};


