<template>
  <div id="app">
    <h1><router-link to="/">Hello App!</router-link></h1>
    <p> 
      <router-link to="/foo">Go to Foo</router-link>
      <router-link to="/bar">Go to Bar</router-link>
      <router-link to="/about">Go to About</router-link>
    </p>
    <router-view></router-view>
  </div>
</template>

<script>
</script>

<style>
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}

h1, h2 {
  font-weight: normal;
}

ul {
  list-style-type: none;
  padding: 0;
}

li {
  display: inline-block;
  margin: 0 10px;
}

a {
  color: #42b983;
}

.router-link-active {
  color: red;
}

</style>
