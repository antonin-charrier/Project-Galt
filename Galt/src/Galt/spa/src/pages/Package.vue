<template>
    <div id="package">
            <h1 class="package-name"><a class="package-link" :href="'https://www.nuget.org/packages/'+packageName" target="_blank">{{ packageName }}</a></h1>
            <div class="package-info">
            <div class="flex-bloc">
                <h3 class="flex-info-text">
                    <div class="w3-dropdown-hover">
                        <button class="w3-btn w3-white" v-on:click="displayVersions"><span class="actual-version">Version {{ packageCurrentVersion }}</span><div style="font-size: 30px; color: grey; padding-left: 10px" class="fa fa-sort-desc"></div></button>
                        <div class="w3-dropdown-content w3-border version-options" v-if="versions">
                            <version-option></version-option>

                        </div>
                    </div>
                    <span class="flex-info-item">By</span><a class="flex-info-item-bis package-link" :href="'https://www.nuget.org/profiles/'+packageOwner" target="_blank">{{ packageOwner }}</a>
                    <span class="flex-info-item">Published on</span><span class="flex-info-item-bis">{{ packagePDate }}</span>
                </h3>
                <i class="fa fa-star fa-star-orange" v-if="fav" v-on:click="addFav"></i>
                <i class="fa fa-star fa-star-grey" v-if="!fav" v-on:click="addFav"></i>
            </div>
            <p id="description">{{ packageDescription }}</div>
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

var VersionsMenu = {
  template: ''
}

export default {
    data: function () {
        return {
            fav: false,
            versions: false,
            packageName : 'Code.Cake',  
            packageCurrentVersion : '0.14.0',
            packageVersions : ['0.14.0', '0.13.0', '0.12.0', '0.11.0', '0.10.0', '0.8.3', '0.8.2', '0.8.1', '0.8.0', '0.7.4', '0.7.3', '0.7.2', '0.7.1', '0.6.2', '0.6.1', '0.6.0', '0.6.0-r', '0.3.1', '0.3.0', '0.2.2', '0.2.1', '0.2.0', '0.1.0-r02', '0.1.0-beta'],
            packageOwner : 'olivier-spinelli',
            packagePDate : '10/19/2016',
            packageDescription : 'Code.Cake library contains Code.Cake.dll (0.14.0) that CodeCakeBuilder applications uses.'
        }
    },
    methods: {
        addFav: function () {
            this.fav = !this.fav
        },
        displayVersions: function () {
            this.versions = !this.versions
        }
    },
    created: function () {
          this.$http.get('/api/package/infopackage?packageId=code.cake').then((response) => {
            console.log(response);
          }, (response) => {
            console.log("test failed");
          });
    
        VersionsMenu.template = VersionsMenu.template + '<div>';
        for(var i=0; i<this.packageVersions.length ;i++){
            VersionsMenu.template = VersionsMenu.template + '<router-link to="/package" href="#">Version ' + this.packageVersions[i] + '</router-link>'
        }
        VersionsMenu.template = VersionsMenu.template + '</div>';
    },
    components: {
        'graph' : Graph,
        'version-option' : VersionsMenu
    }
}
</script>

<style>
    h1.package-name{
        margin-left: 50px;
    }
    .package-link:link{
       text-decoration: none; 
    }
    .package-link:hover{
        text-decoration: underline;
    }
    .flex-bloc{
        display: -webkit-flex;
        display: flex;
        margin-bottom: 30px;
    }
    .flex-info-text{
        display: -webkit-flex;
        display: flex;
        width: 85%;
        margin-left: 50px;
    }
    .flex-info-item{
        margin-left: 40px;
    }
    .flex-info-item-bis{
        margin-left: 10px;
    }
    .w3-dropdown-hover{
        margin-top: -5px;
    }
    .actual-version, .version-options{
        color: #2c3e50;
    }
    .version-options{
        font-size: 16px;
    }
    .w3-dropdown-content{
        height: 300px;
        width: 100%;
        overflow: auto;
    }
    .fa-star{
        font-size:40px;
        cursor: pointer;
    }
    .fa-star-orange{
        color: orange;
    }
    .fa-star-grey{
        color: grey;
    }
    #description{
        text-align: justify;
        display: -webkit-flex;
        display: flex;
        width: 90%;
        margin-left: 50px;
        margin-right: 50px;
    }
    .flex-issues-versions{
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