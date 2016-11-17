<template>
    <div id="mypackages">
        <div class="favorite-packages">
            <h3 class="my-packages-title"><i class="fa fa-star fa-star-orange" style="font-size:25px; margin-right:10px"></i>My favorite packages</h3>
            <div class="favorite-packages-items">
                <div class="favorite-packages-info">d3</div>
                <color-box-ok></color-box-ok>
                <router-link to="/package"><button class="view-button">View</button></router-link>
            </div>
            <div class="favorite-packages-items">
                <div class="favorite-packages-info">Package.Example.2</div>
                <color-box-alert></color-box-alert>
                <router-link to="/package"><button class="view-button">View</button></router-link>
            </div>
            <div class="favorite-packages-items">
                <div class="favorite-packages-info">Package.Example.3</div>
                <color-box-issue></color-box-issue>
                <router-link to="/package"><button class="view-button">View</button></router-link>
            </div>
        </div>
        <div class="recent-packages">
            <h3 class="my-packages-title">Recent packages</h3>
            <div class="recent-packages-items">
                <div class="recent-packages-info">d3</div>
                <router-link to="/package"><button class="view-button">View</button></router-link>
            </div>
        </div>
        <router-view></router-view>
    </div>
</template>

<script>
import ColorBoxOK from '../components/ColorBoxOK.vue'
import ColorBoxAlert from '../components/ColorBoxAlert.vue'
import ColorBoxIssue from '../components/ColorBoxIssue.vue'

export default {
    components: {
        'color-box-ok' : ColorBoxOK,
        'color-box-alert' : ColorBoxAlert,
        'color-box-issue' : ColorBoxIssue
    }
}

</script>

<style>
#mypackages{
    display: -webkit-flex;
    display: flex;
    width: 100%;
}
.favorite-packages{
    display: -webkit-flex;
    display: flex;
    -webkit-flex-direction: column;
    flex-direction: column;
    width:50%;
    height: 100%;
}
.recent-packages{
    display: -webkit-flex;
    display: flex;
    -webkit-flex-direction: column;
    flex-direction: column;
    width:50%;
    height: 100%
}
.my-packages-title{
    text-align: center;
    margin-bottom: 20px;
}
.favorite-packages-items{
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
.favorite-packages-info{
    margin-left: 10px;
    width:80%;
    height:50px;
    line-height: 50px;
}
.recent-packages-items{
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
.recent-packages-info{
    margin-left: 10px;
    width:80%;
    height:50px;
    line-height: 50px;
}
.color-box{
    width: 40px;
    height: 20px;
    margin-top: 15px;
    margin-bottom: 15px;
    border-radius: 3px;
    cursor: help;
}
.color-box-ok{
    background-color: limegreen;
}
.color-box-alert{
    background-color: orange;
}
.color-box-issue{
    background-color: red;
}
.view-button{
    cursor: pointer;
    height: 30px;
    margin: 10px;
    font-size: 13px;
    border-radius: 3px;
    border: none;
    background-color: lightslategray;
    color: white;
    font-family: 'Avenir', Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale
}
.tooltip .tooltiptext {
    visibility: hidden;
    z-index: 100;
}
.tooltip .tooltiptext::after {
    z-index: 100;
}
.tooltip:hover .tooltiptext {
    visibility: visible;
    z-index: 100;
}
</style>