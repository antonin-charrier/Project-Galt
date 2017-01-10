<template>
    <div id="graph">
        <button class="graph-button" v-on:click="drawGraph" v-if="button">Generate graph</button>
    </div>
</template>

<script>
import Graph from "../scripts/graph.js"

export default{
    data: function () {
        return {
            button : true
        }
    },
    methods: {
        drawGraph: function () {
            this.button = !this.button;
            
            //HTTP request to get package info <--
            var packageInfo = '{"nodes" : [{"id":0,"name": "Code.Cake", "entity":"source", "version":" 0.14"},{"id":1,"name": "Cake.Core", "warning":"toUpdate", "version":"0.16.2", "lastVersion":"0.18.3"},{"id":2,"name": "Cake.Common", "version":">= 0.16.2"},{"id":3,"name": ".NETFramework", "version": "4.5", "entity":"platform"},{"id":4,"name": ".NETStandard", "version": "1.6", "entity":"platform"},{"id":5,"name": "NETStandard.Library", "version":">= 1.6.0"},{"id":6,"name": "Microsoft.Win32.Registry", "version":">= 4.0.0"},{"id":7,"name": "System.Diagnostics.Process", "version":"4.1.0"},{"id":8,"name": "System.Runtime.InteropServices.RuntimeInformation", "version":">= 4.0.0","warning":"versionConflict"},{"id":9,"name": "System.Runtime.Loader", "version":"4.0.0"},{"id":10,"name": "Microsoft.Etensions.DependencyModel", "version":">= 1.0.0"},{"id":11,"name": "System.Xml.XmlDocument", "version":">= 4.0.1"},{"id":12,"name": "System.Xml.XPath", "version":">= 4.0.1"},{"id":13,"name": "System.Xml.XPath.XmlDocument", "version":">= 4.0.1"},{"id":14,"name": "System.Runtime.Serialization.Json", "version":">= 4.0.2"},{"id":15,"name": "System.Xml.ReaderWriter", "version":">= 4.0.11", "warning":"toUpdate"},{"id":16,"name": "System.ComponentModel.TypeConverter", "version":">= 4.1.0", "lastVersion":"4.2.0"},{"id":17,"name": ".NETStandard", "version": "1.6", "entity":"platform"},{"id":18,"name": "System.Runtime.InteropServices.RuntimeInformation", "version":">= 3.19.0", "warning":"versionConflict"},],"links":[{"source":0, "target":1},{"source":0, "target":2},{"source":1, "target":17},{"source":17, "target":5},{"source":17, "target":6},{"source":17, "target":7},{"source":17, "target":8},{"source":17, "target":9},{"source":17, "target":10},{"source":3, "target":1},{"source":4, "target":11},{"source":4, "target":12},{"source":4, "target":13},{"source":4, "target":14},{"source":4, "target":15},{"source":4, "target":18},{"source":16, "target":15},{"source":4, "target":16},{"source":2, "target":3},{"source":2, "target":4}]}'

            return Graph.drawGraph(packageInfo)
        },
        resizeGraph: function () {
            return Graph.resizeGraph()
        }
    },
    mounted() {
        this.$nextTick(function () {
            window.addEventListener('resize', this.resizeGraph);
        })
    }
}
</script>

<style>
.graph-button{
    margin: auto;
    cursor: pointer;
    height: 30px;
    font-size: 13px;
    border-radius: 3px;
    border: 1px solid black;
    background-color: lightslategray;
    color: black;
    font-family: 'Avenir', Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
}
.graph-button:hover{
    background-color: slategray;
}
#endMarkers{
	fill:black;
}
.invisible
{
    display:none;
}
.clearButton
{
    fill: #333;
    cursor:pointer;
}
.textClearButton
{
    cursor:pointer;
}
.link{
	fill: none;
	stroke: black;
	stroke-width: 1px;
}
.default{
	fill: #07A901;
}
.source{
	fill: #0000ff
}
.platform{
	fill: #FF0DFF
}
.toUpdate{
	fill: #ff9b00
}
.versionConflict{
	fill: red
}
#graph{
    display: -webkit-flex;
    display: flex;
    -webkit-flex-direction: column;
    flex-direction: column;
    border-style: solid;
    width: 100%;
    margin-left: 50px;
    background-color: gray;
}
.node{
	stroke: black;
}
text{
	fill: #fff;
	text-shadow: 0 2px 0 #000, 2px 0 0 #000, 0 -1px 0 #000, -1px 0 0 #000;
	stroke:none;
}

/*Display of the tooltip*/
.title{
	text-align:center;
}
.green{
	color:green;
	text-align: center;
}
.warning{
	color:orange;
	text-align: center;
}
.error{
	color:red;
	text-align: center;
}
.d3-tip{
  line-height: 1;
  font-weight: bold;
  padding: 12px;
  background: rgba(0, 0, 0, 0.8);
  color: #fff;
  border-radius: 2px;
}

/* Creates a small triangle extender for the tooltip */
.d3-tip:after {
  box-sizing: border-box;
  display: inline;
  font-size: 10px;
  width: 100%;
  line-height: 1;
  color: rgba(0, 0, 0, 0.8);
  content: "\25BC";
  position: absolute;
  text-align: center;
}

/* Style northward tooltips differently */
.d3-tip.n:after {
  margin: -5px 0 0 0;
  top: 100%;
  left: 0;
}
</style>