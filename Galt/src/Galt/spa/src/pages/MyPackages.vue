<template>
    <div id="mypackages">
        <div class="favorite-packages">
            <h3 class="my-packages-title"><i class="fa fa-star fa-star-orange" style="font-size:25px; margin-right:10px"></i>My favorite packages</h3>
            <div class="favorite-packages-items">
                <div class="favorite-packages-info">d3</div>
                <div class="color-box color-box-ok"></div>
                <router-link to="/package"><button class="view-button">View</button></router-link>
            </div>
            <div class="favorite-packages-items">
                <div class="favorite-packages-info">Package.Example.2</div>
                <div class="color-box color-box-alert"></div>
                <router-link to="/package"><button class="view-button">View</button></router-link>
            </div>
            <div class="favorite-packages-items">
                <div class="favorite-packages-info">Package.Example.3</div>
                <div class="color-box color-box-issue"></div>
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
    width:70%;
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
    width:70%;
    height:50px;
    line-height: 50px;
}
.color-box{
    width: 30px;
    height: 10px;
    margin-top: 20px;
    margin-bottom: 20px;
    border-radius: 5px;
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
    -moz-osx-font-smoothing: grayscale;

}
</style>