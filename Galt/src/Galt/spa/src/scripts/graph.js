module.exports = {
    drawGraph: function(id){
        var data = [
            {"id":0, "name": "Code.Cake","parent":"", "entity":"source", "version":" 0.14"},
            {"id":1, "name": "Cake.Core", "parent":"0", "entity":"toUpdate", "version":">= 0.16.2"},
            {"id":2, "name": "Cake.Common", "parent":"0", "version":">= 0.16.2"},
            {"id":3, "name": ".NETFramework", "parent":"1", "version": "4.5", "entity":"platform"},
            {"id":4, "name": ".NETStandard", "parent":"1","version": "1.6", "entity":"platform"},
            {"id":5, "name": "NETStandard.Library", "parent":"4", "version":">= 1.6.0"},
            {"id":6, "name": "Microsoft.Win32.Registry", "parent":"4", "version":">= 4.0.0"},
            {"id":7, "name": "System.Diagnostics.Process", "parent":"4", "version":"4.1.0"},
            {"id":8, "name": "System.Runtime.InteropServices.RuntimeInformation", "parent":"4", "version":">= 4.0.0"},
            {"id":9, "name": "System.Runtime.Loader", "parent":"4", "version":"4.0.0"},
            {"id":10, "name": "Microsoft.Etensions.DependencyModel", "parent":"4", "version":">= 1.0.0"},
            {"id":11, "name": "System.Xml.XmlDocument", "parent":"18", "version":">= 4.0.1"},
            {"id":12, "name": "System.Xml.XPath", "parent":"18", "version":">= 4.0.1"},
            {"id":13, "name": "System.Xml.XPath.XmlDocument", "parent":"18", "version":">= 4.0.1"},
            {"id":14, "name": "System.Runtime.Serialization.Json", "parent":"18", "version":">= 4.0.2"},
            {"id":15, "name": "System.Xml.ReaderWriter", "parent":"18", "version":">= 4.0.11"},
            {"id":16, "name": "System.ComponentModel.TypeConverter", "parent":"18", "version":">= 4.1.0"},
            {"id":17, "name": ".NETFramework", "parent":"2", "version": "4.5", "entity":"platform"},
            {"id":18, "name": ".NETStandard", "parent":"2", "version": "1.6", "entity":"platform"}
        ];

        // Traitement of the data to change its format
        var stratify = d3.stratify()
            .parentId(function(d) { return d.parent;});
            
        var root = stratify(data)
            .sort(function(a, b) { return (a.height - b.height) || a.id.localeCompare(b.id); });

        var treeData = root;

        // Set the dimensions and margins of the diagram
        var margin = {top: 20, right: 90, bottom: 30, left: 90},
            width = 960 - margin.left - margin.right,
            height = 500 - margin.top - margin.bottom;

        // append the svg object to the body of the page
        // appends a 'group' element to 'svg'
        // moves the 'group' element to the top left margin
        var svg = d3.select("#graph").append("svg")
            .attr("width", function(d){return d3.select("#graph")._groups[0][0].offsetWidth})
            .attr("height", function(d){return d3.select("#graph")._groups[0][0].offsetHeight})
        .append("g")
            .attr("transform", "translate("
                + margin.left + "," + margin.top + ")");

        height = d3.selectAll("#graph")._groups[0][0].offsetHeight;
        width = d3.selectAll("#graph")._groups[0][0].offsetwidth;
        // Tooltip to display additional informations in nodes
        var tip = d3.tip()
            .attr('class', 'd3-tip')
            .html(function(d) { 
                dp = d.data.data;
                var chaine = "";
                chaine += '<div class="title">' + dp.name + "</div>";
                if (dp.entity == "toUpdate") chaine += '<br/><div class="red">To update</div>';
                else if (dp.entity == null) chaine += '<br><div class = "green">Up to date</div>';
                if (dp.version != null) chaine += "<br/>Version " + dp.version + "<br/>";
                if (dp.lastVersion != null) chaine += "Last version " + dp.lastVersion + "<br/>";
                return chaine; 
        });

        svg.call(tip);

        var i = 0,
            duration = 750,
            root;

        // declares a tree layout and assigns the size
        var treemap = d3.tree().size([height, width]);

        // Assigns parent, children, height, depth
        root = d3.hierarchy(treeData, function(d) { return d.children; });
        root.x0 = height / 2;
        root.y0 = 0;

        // Collapse after the second level
        root.children.forEach(collapse);

        update(root);

        // Collapse the node and all it's children
        function collapse(d) {
        if(d.children) {
            d._children = d.children
            d._children.forEach(collapse)
            d.children = null
        }
        }

        function update(source) {

        // Assigns the x and y position for the nodes
        var treeData = treemap(root);

        // Compute the new tree layout.
        var nodes = treeData.descendants(),
            links = treeData.descendants().slice(1);

        // Normalize for fixed-depth.
        nodes.forEach(function(d){ d.y = d.depth * 180});

        // ****************** Nodes section ***************************

        // Update the nodes...
        var node = svg.selectAll('g.node')
            .data(nodes, function(d) {return d.id || (d.id = ++i); });

        // Enter any new modes at the parent's previous position.
        var nodeEnter = node.enter().append('g')
            .attr('class', 'node')
            .attr("transform", function(d) {
                return "translate(" + source.y0 + "," + source.x0 + ")";
            })
            .on('click', click);

        // Add Circle for the nodes
        nodeEnter.append('circle')
            .attr('r', 1e-6)
            .attr("class", function (d) {
                if (d.data.data.entity === "node source") {
                return "source";
                } 
                else if (d.data.data.entity === "toUpdate"){
                    return "node toUpdate";
                }
                else if (d.data.data.entity === "platform")
                {
                    return "node platform";
                }
                else {
                return "node default";
            }})

            .on('mouseover', tip.show)
            .on('mouseout', tip.hide);

        // Add labels for the nodes
        nodeEnter.append('text')
            .attr("dy", ".35em")
            .attr("x", function(d) {
                return d.children || d._children ? -13 : 13;
            })
            .attr("text-anchor", function(d) {
                return d.children || d._children ? "end" : "start";
            })
            .text(function(d) { return d.data.data.name; });

        // UPDATE
        var nodeUpdate = nodeEnter.merge(node);

        // Transition to the proper position for the node
        nodeUpdate.transition()
            .duration(duration)
            .attr("transform", function(d) { 
                return "translate(" + d.y + "," + d.x+ ")";
            });

        // Update the node attributes and style
        nodeUpdate.select('circle.node')
            .attr('r', 10)
            .attr('cursor', 'pointer');


        // Remove any exiting nodes
        var nodeExit = node.exit().transition()
            .duration(duration)
            .attr("transform", function(d) {
                return "translate(" + source.y + "," + source.x + ")";
            })
            .remove();

        // On exit reduce the node circles size to 0
        nodeExit.select('circle')
            .attr('r', 1e-6);

        // On exit reduce the opacity of text labels
        nodeExit.select('text')
            .style('fill-opacity', 1e-6);

        // ****************** links section ***************************

        // Update the links...
        var link = svg.selectAll('path.link')
            .data(links, function(d) { return d.id; });

        // Enter any new links at the parent's previous position.
        var linkEnter = link.enter().insert('path', "g")
            .attr("class", "link")
            .attr('d', function(d){
                var o = {x: source.x0, y: source.y0}
                return diagonal(o, o)
            });

        // UPDATE
        var linkUpdate = linkEnter.merge(link);

        // Transition back to the parent element position
        linkUpdate.transition()
            .duration(duration)
            .attr('d', function(d){ return diagonal(d, d.parent) });

        // Remove any exiting links
        var linkExit = link.exit().transition()
            .duration(duration)
            .attr('d', function(d) {
                var o = {x: source.x, y: source.y}
                return diagonal(o, o)
            })
            .remove();

        // Store the old positions for transition.
        nodes.forEach(function(d){
            d.x0 = d.x;
            d.y0 = d.y;
        });

        // Creates a curved (diagonal) path from parent to the child nodes
        function diagonal(s, d) {

            path = `M ${s.y} ${s.x}
                    C ${(s.y + d.y) / 2} ${s.x},
                    ${(s.y + d.y) / 2} ${d.x},
                    ${d.y} ${d.x}`

            return path
        }

        // Toggle children on click.
        function click(d) {
            if (d.children) {
                d._children = d.children;
                d.children = null;
            } else {
                d.children = d._children;
                d._children = null;
            }
            update(d);
        }
        }
    }
};

