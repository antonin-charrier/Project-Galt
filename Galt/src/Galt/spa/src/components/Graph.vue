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
            return Graph.drawGraph("graph")
        }
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
	fill: #ff0000
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
.toUpdate, .platform, .default, .source{
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
.red{
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