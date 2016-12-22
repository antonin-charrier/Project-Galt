<template>
    <div id="package">
            <h1 class="package-name">Code.Cake</h1>
            <div class="package-info">
            <div class="flex-bloc">
                <h3 class="flex-info-text">
                    <div class="w3-dropdown-hover">
                        <button class="w3-btn w3-white" v-on:click="displayVersions"><span class="actual-version">Version 0.14.0</span></button>
                        <div class="w3-dropdown-content w3-border version-options" v-if="versions">
                            <router-link to="/package" href="#">Version 0.14.0</router-link>
                            <router-link to="/package" href="#">Version 0.13.0</router-link>
                            <router-link to="/package" href="#">Version 0.12.0</router-link>
                            <router-link to="/package" href="#">Version 0.11.0</router-link>
                            <router-link to="/package" href="#">Version 0.10.0</router-link>
                            <router-link to="/package" href="#">Version 0.8.3</router-link>
                            <router-link to="/package" href="#">Version 0.8.2</router-link>
                            <router-link to="/package" href="#">Version 0.8.1</router-link>
                            <router-link to="/package" href="#">Version 0.8.0</router-link>
                            <router-link to="/package" href="#">Version 0.7.4</router-link>
                            <router-link to="/package" href="#">Version 0.7.3</router-link>
                            <router-link to="/package" href="#">Version 0.7.2</router-link>
                            <router-link to="/package" href="#">Version 0.7.1</router-link>
                            <router-link to="/package" href="#">Version 0.6.2</router-link>
                            <router-link to="/package" href="#">Version 0.6.1</router-link>
                            <router-link to="/package" href="#">Version 0.6.0</router-link>
                            <router-link to="/package" href="#">Version 0.6.0-r</router-link>
                            <router-link to="/package" href="#">Version 0.3.1</router-link>
                            <router-link to="/package" href="#">Version 0.3.0</router-link>
                            <router-link to="/package" href="#">Version 0.2.2</router-link>
                            <router-link to="/package" href="#">Version 0.2.1</router-link>
                            <router-link to="/package" href="#">Version 0.2.0</router-link>
                            <router-link to="/package" href="#">Version 0.1.0-r02</router-link>
                            <router-link to="/package" href="#">Version 0.1.0-beta</router-link>
                        </div>
                    </div>
                    <span class="flex-info-item">By olivier-spinelli</span>
                    <span class="flex-info-item">Published on 10/19/2016</span>
                </h3>
                <i class="fa fa-star fa-star-orange" v-if="fav" v-on:click="addFav"></i>
                <i class="fa fa-star fa-star-grey" v-if="!fav" v-on:click="addFav"></i>
            </div>
            <p id="description">
                Code.Cake library contains Code.Cake.dll (0.14.0) that CodeCakeBuilder applications uses.
            </div>
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

export default {
    data: function () {
        return {
            fav: false,
            versions: false
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
    /*created: function () {
        root = "Code.Cake";
        console.log("foo");
        this.$http.get('/request/RootPackage/' + root)
            .then((response) => {
                // success callback
                return response.text();
            }, (response) => {
                // error callback
            })
    },*/
    components: {
        'graph' : Graph
    }
}
</script>

<style>
    h1.package-name{
        margin-left: 50px;
        margin-bottom: 0px;
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
    .w3-dropdown-hover{
        margin-top: -5px;
    }
    .actual-version, .version-options{
        color: #2c3e50;
    }
    .version-options{
        font-size: 14px;
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
        margin-top: 0px;
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