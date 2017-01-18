<template>
    <div id="outer">
        <div v-show="loading" id="loading-div">
            <bounce-loader :loading="loading" :color="color" :size="size"></bounce-loader>
        </div>
        <div v-show="!loading" id="package">
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
                    <i class="fa fa-star fa-star-orange" v-if="fav" v-on:click="addFav"></i>
                    <i class="fa fa-star fa-star-grey" v-if="!fav" v-on:click="addFav"></i>
                </div>
                <p id="description">{{ description }}<p>
                </div>
                <div class="flex-bloc">
                    <graph :package="packageId" :version="currentVersion"></graph>
                    <div class="flex-issues-versions">
                        <div class="issues">
                            <h3>Issues</h3>
                            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse nibh leo, blandit ac ante eget, mollis ornare dui. Proin nec mollis tellus. Cras fermentum at dui non elementum. Aliquam erat volutpat.</p>
                        </div>
                        <div class="new-versions">
                            <h3>New versions</h3>
                            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse nibh leo, blandit ac ante eget, mollis ornare dui. Proin nec mollis tellus. Cras fermentum at dui non elementum. Aliquam erat volutpat.</p>
                        </div>
                    </div>
                </div>
            <router-view></router-view>
        </div>
    </div>
</template>

<script>
    import $ from 'jquery'
    import Graph from "../components/Graph.vue"
    import BounceLoader from 'vue-spinner/src/BounceLoader.vue'

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
                graphDisplayed: false
            }
        },
        methods: {
            addFav: function() {
                this.fav = !this.fav
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
                    }, function(response) {}.bind(this));
                }
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
                return this.$route.params.id
            },
            packageName: function() {
                if (!this.loading) return this.packageId;
            }
        },
        watch: {
            '$route': function() {
                this.loading = true;
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
            'graph': Graph,
            'bounce-loader': BounceLoader
        }
    }
</script>

<style>
    #outer {
        width: 100%;
        height: 90%;
    }
    
    #loading-div {
        height: 100%;
        display: flex;
        align-items: center;
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
        margin-bottom: 30px;
    }
    
    .flex-info-text {
        display: -webkit-flex;
        display: flex;
        width: 85%;
        margin-left: 50px;
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
        margin-left: 50px;
        margin-right: 50px;
    }
    
    .flex-issues-versions {
        text-align: justify;
        display: -webkit-flex;
        display: flex;
        -webkit-flex-direction: column;
        flex-direction: column;
        width: 300px;
        min-height: 600px;
        padding-left: 10px;
        padding-right: 10px;
        margin-left: 20px;
        margin-right: 40px;
        background-color: dimgrey;
        color: white;
    }
</style>