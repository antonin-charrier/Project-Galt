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
    border: none;
    background-color: lightslategray;
    color: white;
    font-family: 'Avenir', Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
}
</style>