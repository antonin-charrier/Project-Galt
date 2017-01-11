<template>
    <div id="package">
            <h1 class="package-name"><a class="package-link" :href="'https://www.nuget.org/packages/'+packageName" target="_blank">{{ packageId }}</a></h1>
            <div class="package-info">
            <div class="flex-bloc">
                <h4 class="flex-info-text">
                    <div class="w3-dropdown-hover">
                        <button class="w3-btn w3-white" v-on:click="displayVersions"><span class="actual-version">Version {{ packageVersion }}</span><div style="font-size: 20px; color: grey; padding-left: 10px" class="fa fa-sort-desc"></div></button>
                        <div class="w3-dropdown-content w3-border version-options" v-if="versionsDisplayed">
                            <versions-dropdown v-for="version in versions" :package="request"></versions-dropdown>
                        </div>
                    </div>
                    <span class="flex-info-item">By {{ authors }}</span>
                    <span class="flex-info-item">Published on {{ date }}</span>
                </h4>
                <i class="fa fa-star fa-star-orange" v-if="fav" v-on:click="addFav"></i>
                <i class="fa fa-star fa-star-grey" v-if="!fav" v-on:click="addFav"></i>
            </div>
            <p id="description">{{ description }}</div>
            <div class="flex-bloc">
                <graph></graph>
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
</template>

<script>
    import Graph from "../components/Graph.vue"
    import VersionsDropdown from "../components/VersionsDropdown.vue"

    export default {
        data: function() {
            return {
            request: undefined,
            ready: false,
            fav: false,
            versionsDisplayed: false,
            versions: [],
            packageVersion : '0.14.0',
            }
        },
        methods: {
            addFav: function() {
                this.fav = !this.fav
            },
            displayVersions: function() {
            this.versionsDisplayed = !this.versionsDisplayed
            }
        },
        created: function() {
            this.$http.get('/api/package/infopackage?packageId=' + this.packageId).then((response) => {
                this.request = JSON.parse(response.body);
                this.versions = this.request.ListVPackage;
                console.log(this.versions);
                this.ready = true;
            
                for(var i=this.request.ListVPackage.length-1; i>=0; i--){
                    VersionsMenu.template = VersionsMenu.template + '<router-link to="/package" href="#">Version ' + this.request.ListVPackage[i] + '</router-link>'
                }
                VersionsMenu.template = VersionsMenu.template + '</div>';

                console.log(this.request);
            }, (response) => {
                console.log("Request error");
            });
        },
        computed: {
            description: function() {
                return this.ready ? this.request.Description : 'Loading...';
            },
            authors: function() {
                return this.ready ? this.request.Authors.toString() : 'Loading';
            },
            date: function() {
                return this.ready ? this.request.Timestamp : 'Loading';
            },
            packageId: function() {
                return this.$route.params.id
            }
        },
        components: {
            'graph': Graph,
        'versions-dropdown' : VersionsDropdown
        }
    }
</script>

<style>
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
    
    .version-options{
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