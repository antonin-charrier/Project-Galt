<template>
    <div id="mypackages">
        <div class="favorite-packages">
            <h3 class="my-packages-title"><i class="fa fa-star fa-star-orange no-cursor" style="font-size:25px; margin-right:10px"></i>My favorites</h3>
            <span v-show="!loaded">Loading...</span>
            <span v-show="failed">No favorite found.</span>
            <favorite-package v-for="favorite in favorites" :favorite="favorite"></favorite-package>
        </div>
        <router-view></router-view>
    </div>
</template>

<script>
    import FavoritePackage from '../components/FavoritePackage.vue'
    import {
        getAsync
    } from '../helpers/apiHelper.js'
    import AuthService from '../services/AuthService'

    export default {
        data: function() {
            return {
                favorites: [],
                loaded: false
            }
        },
        computed: {
            failed: function() {
                return this.loaded && this.favorites.length == 0
            }
        },
        components: {
            'favorite-package': FavoritePackage
        },
        created: function() {
            getAsync("api/package", "favorites", AuthService.accessToken)
                .then(function(response) {
                        this.favorites = response;
                        this.loaded = true;
                    }.bind(this),
                    function(response) {
                        this.loaded = true;
                    })
        },
        watch: {
            'AuthService.isConnected': function(newValue) {
                if (!newValue) this.$router.replace({
                    route: "/"
                })
            }
        }
    }
</script>

<style>
    #mypackages {
        display: -webkit-flex;
        display: flex;
        width: 100%;
    }
    
    .favorite-packages {
        display: -webkit-flex;
        display: flex;
        -webkit-flex-direction: column;
        flex-direction: column;
        width: 50%;
        margin-left: 25%;
        height: 100%;
    }
    
    .recent-packages {
        display: -webkit-flex;
        display: flex;
        -webkit-flex-direction: column;
        flex-direction: column;
        width: 50%;
        height: 100%
    }
    
    .my-packages-title {
        text-align: center;
        margin-bottom: 20px;
    }
    
    .favorite-packages-items {
        display: -webkit-flex;
        display: flex;
        -webkit-justify-content: space-bewteen;
        justify-content: space-between;
        margin-left: 25px;
        margin-right: 25px;
        margin-bottom: 10px;
        background-color: lightgray;
        border-radius: 3px;
    }
    
    .favorite-packages-info {
        margin-left: 10px;
        width: 80%;
        height: 50px;
        line-height: 50px;
    }
    
    .recent-packages-items {
        display: -webkit-flex;
        display: flex;
        -webkit-justify-content: space-bewteen;
        justify-content: space-between;
        margin-left: 25px;
        margin-right: 25px;
        margin-bottom: 10px;
        background-color: lightgray;
        border-radius: 3px;
    }
    
    .recent-packages-info {
        margin-left: 10px;
        width: 80%;
        height: 50px;
        line-height: 50px;
    }
    
    .color-box {
        width: 40px;
        height: 20px;
        margin-top: 15px;
        margin-bottom: 15px;
        border-radius: 3px;
        cursor: help;
    }
    
    .color-box-ok {
        background-color: limegreen;
    }
    
    .color-box-alert {
        background-color: orange;
    }
    
    .color-box-issue {
        background-color: red;
    }    
    
    .color-box-notloaded {
        background-color: grey;
    }
    
    .view-button {
        cursor: pointer;
        height: 30px;
        margin: 10px;
        font-size: 13px;
        border-radius: 3px;
        border: 1px solid black;
        background-color: lightslategray;
        color: black;
        font-family: 'Avenir', Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        z-index: 1;
    }
    
    .view-button:hover {
        background-color: slategray;
    }
    
    .tooltip .tooltiptext {
        display: -webkit-flex;
        display: flex;
        position: relative;
        visibility: hidden;
        width: 120px;
        text-align: center;
        background-color: black;
        color: white;
        border-radius: 6px;
        padding: 5px 0;
        top: 150%;
        left: 50%;
        margin-left: -60px;
        font-size: 12px;
        z-index: 2;
    }
    
    .tooltip .tooltiptext::after {
        content: "";
        position: absolute;
        bottom: 100%;
        left: 50%;
        z-index: 2;
        margin-left: -5px;
        border-width: 5px;
        border-style: solid;
        border-color: transparent transparent black transparent;
    }
    
    .tooltip:hover .tooltiptext {
        visibility: visible;
        z-index: 2;
    }
    
    .no-cursor {
        cursor: auto;
    }
</style>