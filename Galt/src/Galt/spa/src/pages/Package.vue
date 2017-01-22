<template>
    <div id="outer">
        <div v-show="loading" class="loading-div">
            <bounce-loader class="spinner" :loading="loading" :color="color" :size="size"></bounce-loader>
        </div>
        <div v-show="!loading" id="package">
            <div class="flex-package">
                <h1 class="package-name"><a class="package-link" :href="'https://www.nuget.org/packages/'+packageName" target="_blank">{{ packageName }}</a></h1>
                <div class="package-info">
                <div class="flex-bloc">
                    <h4 class="flex-info-text">
                        <select id="version-dropdown" v-model="currentVersion">
                            <option v-for="option in options" v-bind:value="option.value">
                                {{ option.text }}
                            </option>
                        </select>
                        <span class="flex-info-item">{{ authors }}</span>
                        <span class="flex-info-item">{{ date }}</span>
                    </h4>
                    <i class="fa fa-star fa-star-orange" v-if="isConnected && fav" v-on:click="removeFav"></i>
                    <i class="fa fa-star fa-star-grey" v-if="isConnected && !fav" v-on:click="addFav"></i>
                </div>
                <p id="description">{{ description }}<p>
                </div>
            </div>
            <div class="flex-bloc">
                <div class="flex-retrieve" v-if="!graphDisplayed && !graphLoading">
                    <button class="graph-button" v-on:click="displayGraph">Retrieve graph data</button>
                </div>
                <div v-if="graphLoading && !graphDisplayed" class="flex-loading">
                    <bounce-loader class="spinner" :loading="graphLoading" :color="color" :size="size"></bounce-loader>
                    <div class="loading-message">Loading package dependencies. It may take several minutes...</div>
                </div>
                <div id="graph-container" v-show="graphDisplayed && !graphLoading">
                    <div id="graph"></div>
                </div>
                <div style="overflow: auto" class="flex-issues-versions" v-if="graphDisplayed && !graphLoading">
                    <div class="issues">
                        <h3>Conflicts</h3>
                        <div v-for="conflict in conflicts">{{ conflict }}</div>
                    </div>
                    <div class="new-versions">
                        <h3>New versions</h3>
                        <div v-for="version in toUpdate">{{ version }}</div>
                    </div>
                </div>
            </div>
            <!--<div class="flex-refresh" v-if="graphDisplayed && !graphLoading">
                <button class="graph-button" v-on:click="refreshGraph"><i class="fa fa-refresh refresh"></i>Refresh graph data</button>
            </div>-->
            <router-view></router-view>
        </div>
    </div>
</template>

<script>
    import $ from 'jquery'
    import BounceLoader from 'vue-spinner/src/BounceLoader.vue'
    import {
        postAsync
    } from '../helpers/apiHelper.js'
    import AuthService from '../services/AuthService'
    import GraphScript from '../scripts/graph.js'

    export default {
        data: function() {
            return {
                request: undefined,
                fav: false,
                loading: true,
                color: '#226D71',
                versionsDisplayed: false,
                currentVersion: '',
                options: [],
                toUpdate: [],
                conflicts: '',
                graphLoading: false,
                graphDisplayed: false
            }
        },
        methods: {
            addFav: function() {
                this.fav = true
                postAsync("api/package", "fav", AuthService.accessToken, {
                        packageId: this.packageId
                    })
                    .then(function(response) {
                            if (response == false)
                                this.fav = false;
                        }.bind(this),
                        function(response) {
                            this.fav = false
                        }.bind(this)
                    )
            },
            removeFav: function() {
                this.fav = false
                postAsync("api/package", "unfav", AuthService.accessToken, {
                        packageId: this.packageId
                    })
                    .then(function(response) {
                            if (response == false)
                                this.fav = true;
                        }.bind(this),
                        function(response) {
                            this.fav = true
                        }.bind(this)
                    )
            },
            displayVersions: function() {
                this.versionsDisplayed = !this.versionsDisplayed
            },
            changeVersion: function() {
                this.$router.push({
                    path: '/package/' + this.packageId + '/' + this.currentVersion
                });
                this.getInfoPackage();
            },
            redirect: function() {
                if (this.$route.params.version) {
                    this.currentVersion = this.$route.params.version;
                    this.getInfoPackage(this.currentVersion)
                } else {
                    this.$http.get('/api/package/lastversion?packageId=' + this.packageId).then(function(response) {
                        this.$router.push({
                            path: '/package/' + this.packageId + '/' + this.currentVersion
                        })
                        this.getInfoPackage(response.body);
                    });
                }
                if (this.isConnected)
                    postAsync("api/package", "isFav", AuthService.accessToken, {
                        packageId: this.packageId
                    })
                    .then(function(response) {
                        this.fav = response;
                    }.bind(this))
            },
            getInfoPackage: function(version) {
                this.$http.get('/api/package/infopackage?packageId=' + this.packageId + '&version=' + version).then(function(response) {
                    this.request = JSON.parse(response.body);
                    this.options = [];
                    for (var i = 0; i < this.request.ListVPackage.length; i++) {
                        this.options.push({
                            text: 'Version ' + this.request.ListVPackage[i],
                            value: this.request.ListVPackage[i]
                        })
                    }
                    this.currentVersion = version;
                    this.loading = false;
                }, function(response) {}.bind(this));
            },
            displayGraph: function() {
                this.graphLoading = true;
                this.$http.get('/api/package/graph?packageId=' + this.packageId + '&version=' + this.currentVersion).then(function(response) {
                    this.graphDisplayed = !this.graphDisplayed;
                    var data = JSON.parse(response.body);
                    console.log(data);
                    var toUpdate = "";
                    for(var i=0; i<data.toUpdate.length; i++) {
                        toUpdate = toUpdate + data.toUpdate[i].name + " (" + data.toUpdate[i].currentVersion + ") → " + data.toUpdate[i].lastVersion + "|";
                    }
                    var conflicts = "";
                    for(var i=0; i<data.versionConflict.length; i++){
                        conflicts = conflicts + data.versionConflict[i].name + " : " + data.versionConflict[i].versions + "|";
                    }
                    this.toUpdate = toUpdate.split("|");
                    this.conflicts = conflicts.split("|");
                    GraphScript.drawGraph(data.graph);
                    this.graphLoading = false;
                }, function(response) {}.bind(this));
            },
            refreshGraph: function() {
                console.log("foo");
            }
        },
        created: function() {
            this.redirect();
        },
        computed: {
            description: function() {
                if (!this.loading) return this.request.Description;
            },
            authors: function() {
                if (!this.loading) return 'By ' + this.request.Authors.toString();
            },
            date: function() {
                if (!this.loading) return 'Published on ' + this.request.PublicationDate;
            },
            packageId: function() {
                return this.$route.params.id;
            },
            packageName: function() {
                if (!this.loading) return this.packageId;
            },
            isConnected: function() {
                return AuthService.isConnected;
            }
        },
        watch: {
            '$route': function() {
                this.loading = true;
                this.graphDisplayed = false;
                this.redirect();
            },
            currentVersion: function(newValue) {
                this.$router.push({
                    path: '/package/' + this.packageId + '/' + newValue
                })
                this.redirect();
            }
        },
        components: {
            'bounce-loader': BounceLoader
        }
    }
</script>

<style>
    #outer {
        width: 100%;
        height: 90%;
    }
    
    .flex-package {
        flex: 0 1 auto;
    }
    
    #package {
        display: flex;
        flex-flow: column;
        height: 100%;
    }
    
    .package-info {
        margin-left: 50px;
    }

    .loading-div {
        height: 100%;
    }
    
    .flex-loading {
        width: 100%;
    }
    
    .loading-message {
        text-align: center;
        margin-top: 20px;
        color: #226D71;
    }
    
    #loading-div div {
        margin: auto;
    }
    
    h1.package-name {
        margin-left: 50px;
    }
    
    .package-link:link {
        text-decoration: none;
    }
    
    .package-link:hover {
        text-decoration: underline;
    }
    
    .flex-bloc {
        display: -webkit-flex;
        display: flex;
        margin-bottom: 10px;
        flex: 1;
        align-items: center;
    }
    
    .flex-retrieve {
        flex: 1;
        display: flex;
        align-items: center;
        justify-content: center;
    }

    .flex-refresh {
        display: flex;
        align-items: center;
        justify-content: center;
    }
    
    .flex-info-text {
        display: -webkit-flex;
        display: flex;
        width: 85%;
    }
    
    .graph-button {
        cursor: pointer;
        height: 30px;
        font-size: 13px;
        border-radius: 3px;
        border: 1px solid #11383a;
        background-color: #226D71;
        color: white;
        font-family: 'Avenir', Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
    }
    
    .graph-button:hover {
        background-color: #3d7072;
    }

    .refresh {
        color: white;
        font-size : 15px;
        text-align: right;
        margin-right: 5px;
    }

    .flex-info-item {
        margin-left: 40px;
    }
    
    .w3-dropdown-hover {
        margin-top: -5px;
    }
    
    .actual-version,
    .version-options {
        color: #2c3e50;
    }
    
    .version-options {
        font-size: 16px;
    }
    
    .w3-dropdown-content {
        height: 300px;
        width: 100%;
        overflow: auto;
    }
    
    .fa-star {
        font-size: 40px;
        cursor: pointer;
    }
    
    .fa-star-orange {
        color: orange;
    }
    
    .fa-star-grey {
        color: grey;
    }
    
    #description {
        text-align: justify;
        display: -webkit-flex;
        display: flex;
        width: 90%;
        margin-right: 50px;
    }
    
    .spinner {
        display: flex;
        align-items: center;
        justify-content: center;
        height: 100%;
    }
    
    .flex-issues-versions {
        display: -webkit-flex;
        display: flex;
        -webkit-flex-direction: column;
        flex-direction: column;
        flex: 1;
        height: 100%;
        padding: 10px;
        margin-left: 20px;
        margin-right: 40px;
        background-color: #226D71;
        color: white;
    }
    
    #endMarkers {
        fill: lightgray;
    }
    
    .invisible {
        display: none;
    }
    
    .clearButton {
        fill: #333;
        cursor: pointer;
    }
    
    .textClearButton {
        cursor: pointer;
    }
    
    .link {
        fill: none;
        stroke: lightgray;
        stroke-width: 1px;
        cursor: default;
    }
    
    .default {
        fill: limegreen;
    }
    
    .source {
        fill: white
    }
    
    .platform {
        fill: #323332
    }
    
    .toUpdate {
        fill: yellow
    }
    
    .versionConflict {
        fill: red
    }
    
    #graph-container {
        display: -webkit-flex;
        display: flex;
        -webkit-flex-direction: column;
        flex-direction: column;
        border-style: solid;
        flex: 3;
        height: 100%;
        margin-left: 50px;
        background-color: #226D71;
    }
    
    #graph {
        width: 100%;
        height: 100%;
        cursor: move;
    }
    
    .node {
        stroke: lightgray;
        cursor: help;
    }
    
    text {
        fill: #fff;
        text-shadow: 0 2px 0 #000, 2px 0 0 #000, 0 -1px 0 #000, -1px 0 0 #000;
        stroke: none;
    }
    /*Display of the tooltip*/
    
    .title {
        text-align: center;
    }
    
    .green {
        color: green;
        text-align: center;
    }
    
    .warning {
        color: orange;
        text-align: center;
    }
    
    .error {
        color: red;
        text-align: center;
    }
    
    .d3-tip {
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