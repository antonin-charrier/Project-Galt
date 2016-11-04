<template>
  <main-layout>
      
    <h1>Package 1</h1>
    <div class="package-info">
    <h2>
        <ul>
            <li>Version 0.1</li>
            <li>PackageOwner</li>
            <li>Published on 01/01/2000</li>
        </ul>
    </h2>
    <i class="fa fa-star" style="font-size:40px; color:orange"></i>
    <p>Lorem ipsum</p>
    </div>
    <div class="issues">
        <h3>Issues</h3>
        <p>Lorem ipsum</p>
    </div>
    <div class="new-versions">
        <h3>New versions</h3>
        <p>Lorem ipsum</p>
    </div>
  </main-layout>
</template>

<script>
  import VLink from '../components/VLink.vue'
  import MainLayout from '../layouts/App.vue'

  export default {
    components: {
      MainLayout,
      VLink
    }
  }
  
</script>
