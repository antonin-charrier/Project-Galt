<template>
  <div id="app">
    <ul class="w3-navbar w3-text-dark-grey w3-border-bottom w3-top w3-light-grey" style="overflow: visible">
      <li><router-link class="w3-hover-gray" to="/">
        <logo-32x32></logo-32x32>
      </router-link></li>
      <li style="height: 54px"><search></search></li>
      <connected v-if="isConnected"></connected>
      <disconnected v-else></disconnected>
    </ul>
    <div style="height:60px"></div>
    <router-view></router-view>
  </div>
</template>

<script>
    import Connected from './components/Connected.vue'
    import Disconnected from './components/Disconnected.vue'
    import Search from './components/search/Search.vue'
    import Logo32x32 from './components/Logo-32x32.vue'

    export default {
        //state
        data: function() {
            return {

            }
        },
        computed: {
            isConnected() {
                return this.$store.state.isConnected
            }
        },
        //view
        components: {
            'connected': Connected,
            'disconnected': Disconnected,
            'search': Search,
            'logo-32x32': Logo32x32
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
        position: relative;
        right: 6px;
    }
    
    .search{
        position: relative;
        left: 10px;
        width: 250px;
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
    
    .search:focus {
        width: 450px;
    }
</style>