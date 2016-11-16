<template>
  <div id="app">
    <ul class="w3-navbar w3-text-dark-grey w3-border-bottom w3-top w3-light-grey">
      <li><router-link class="w3-hover-gray" to="/">
        <svg version="1.0" id="logo" xmlns="http://www.w3.org/2000/svg" width="32px" height="32px" viewBox="0 0 4500 4500" preserveAspectRatio="xMidYMid meet">
          <g id="layer1" fill="#65aacb" stroke="none">
            <path d="M4105 2109 c-420 -74 -793 -266 -1085 -559 -309 -308 -541 -783 -577 -1179 l-6 -73 64 7 c347 40 735 207 1014 435 87 72 249 235 311 315 229 293 380 658 420 1013 8 73 20 69 -141 41z"/>
          </g>
          <g id="layer2" fill="#226d71" str2oke="none">
            <path d="M2430 4277 c0 -71 33 -285 60 -392 168 -664 674 -1208 1320 -1419 121 -39 289 -76 408 -89 l42 -5 0 51 c0 70 -25 238 -51 350 -163 698 -702 1264 -1393 1462 -95 27 -247 59 -333 70 l-53 7 0 -35z"/>
            <path d="M1890 4280 c-419 -79 -773 -264 -1070 -560 -330 -328 -547 -800 -577 -1252 l-6 -98 29 5 c16 2 63 9 104 15 441 65 884 314 1187 665 215 248 356 517 434 821 30 119 59 306 59 379 0 44 -1 45 -32 44 -18 0 -76 -9 -128 -19z"/>
            <path d="M240 2125 c0 -26 35 -234 50 -300 111 -475 431 -929 851 -1205 230 -151 513 -264 769 -305 151 -24 139 -32 123 87 -115 836 -737 1509 -1563 1691 -131 29 -230 43 -230 32z"/>
          </g>
        </svg>
      </router-link></li>
      <li><input type="text" class="w3-input w3-light-grey" placeholder="Search on Galt"></li>
      <connected v-if="isConnected"></connected>
      <disconnected v-if="!isConnected"></disconnected>
    </ul>
    <div style="height:60px"></div>
    <router-view></router-view>
  </div>
</template> 

<script>
  import Connected from './components/Connected.vue'
  import Disconnected from './components/Disconnected.vue'

  export default {
    //state
    data: function() {
      return {
        
      }
    },
    computed:{
      isConnected () {
        return this.$store.state.isConnected
      }
    },
    //view
    components:{
      'connected': Connected,
      'disconnected': Disconnected
    },
    //actions
    methods:{

    }
  }
</script>

<style>
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
}
#icon-package {
  position:relative;
  right:6px;
}
input[type=text] {
  position: relative;
  left:10px;
  width: 200px;
  box-sizing: border-box;
  border: 2px solid #ccc;
  border-radius: 4px;
  font-size: 16px;
  background-color: white;
  background-image: url('./assets/searchicon.png');
  background-size: 25px;
  background-position: 10px 10px;
  background-repeat: no-repeat;
  padding: 12px 20px 12px 40px;
  -webkit-transition: width 0.4s ease-in-out;
  transition: width 0.4s ease-in-out;
}
input[type=text]:focus {
  width: 350px;
}
</style>
