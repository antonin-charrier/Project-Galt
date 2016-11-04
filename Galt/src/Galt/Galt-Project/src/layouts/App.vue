<template>
  <div id="app">
    <ul class="w3-navbar w3-border w3-top w3-lightgrey">
      <li><v-link href="/"><img id="logo" src=".././assets/Galt.logo_noname.450.png"></v-link></li>
      <li><input type="text" class="w3-input w3-border-0 w3-padding-16" placeholder="Search on Galt"></li>
      <li class="w3-right"><v-link href="/mypackages" class="w3-padding-16">My Packages</v-link></li>
    </ul>
    <div style="height:60px"></div>
  <slot></slot>
  </div>
</template>

<script>
  import VLink from '../components/VLink.vue'

  export default {
    components: {
      VLink
    }
  }
</script>

<style>
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}
#logo {
  height:37px;
}
</style>
