<template>
    <div style="
    top: 100%;
    position:absolute;
    " class="w3-white w3-border">
        <search-hit v-for="package in packages" :package="package"></search-hit>
    </div>
</template>
<script>
    import SearchHit from './SearchHit.vue'

    export default {
        props: ['packages'],
        components: {
            'search-hit': SearchHit
        }
    }
</script>