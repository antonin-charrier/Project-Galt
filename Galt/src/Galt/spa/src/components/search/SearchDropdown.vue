<template>
    <div class="w3-white w3-border search-dropdown" v-show="!noResults || working">
        <span v-show="working" style="text-anchor:middle">Working...</span>
        <span v-show="noResults" style="text-anchor: middle">No results for "{{query}}".</span>
        <search-hit v-for="package in packages" :package="package"></search-hit>
    </div>
</template>
<script>
    import SearchHit from './SearchHit.vue'

    export default {
        props: ['packages', 'working', 'query'],
        components: {
            'search-hit': SearchHit
        },
        computed: {
            noResults: function() {
                return this.packages.length == 0;
            }
        }
    }
</script>
<style>
    .search-dropdown {
        top: 100%;
        position: absolute;
        max-height: 513px;
        overflow-y: scroll;
        overflow-x: hidden;
    }
</style>