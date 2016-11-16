<template>
    <div id="package">
            <h1 class="package-name">d3</h1>
            <div class="package-info">
            <div class="flex-bloc">
                <h3 class="flex-info-text">
                    <span class="">Version 4.3.0</span>
                    <span class="flex-info-item">By mbostock</span>
                    <span class="flex-info-item">Published on 05/11/2016</span>
                </h3>
                <i class="fa fa-star fa-star-grey" v-if="fav" v-on:click="addFav"></i>
                <i class="fa fa-star fa-star-orange" v-if="!fav" v-on:click="addFav"></i>
            </div>
            <p id="description">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse nibh leo, blandit ac ante eget, mollis ornare dui. Proin nec mollis tellus. Cras fermentum at dui non elementum. Aliquam erat volutpat. Sed tempor odio a porta sollicitudin. Sed mattis placerat augue nec cursus. Praesent ac odio quis libero gravida dictum. Curabitur pretium mattis elit, at convallis est pharetra quis. In convallis risus eros, non porta massa tristique vel. Donec non iaculis dui, eget rutrum libero.</p>
            </div>
            <div class="flex-bloc">
                <div id="flex-graph">
                </div>
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
    export default {
        data: function () {
            return {
                fav: false
            }
        },
        methods: {
            addFav: function () {
                this.fav = !this.fav
            }
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
    #flex-graph{
        display: -webkit-flex;
        display: flex;
        -webkit-flex-direction: column;
        flex-direction: column;
        border-style: solid;
        width: 100%;
        min-height: 450px;
        margin-left: 50px;
    }
    .flex-issues-versions{
        text-align: justify;
        display: -webkit-flex;
        display: flex;
        -webkit-flex-direction: column;
        flex-direction: column;
        width: 400px;
        padding-left: 10px;
        padding-right: 10px;
        margin-left: 20px;
        margin-right: 40px;
        background-color: dimgrey;
        color: white;
    }
</style>