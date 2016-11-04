<template>
  <main-layout>
    <p>My Packages...</p>
    <v-link href="/package">Package 1</v-link>
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
