<template>
    <div class="favorite-packages-items">
        <div class="favorite-packages-info">{{ name }}</div>
        <color-box-ok v-if="status === 'Ok'"></color-box-ok>
        <color-box-alert v-if="status === 'Alert'"></color-box-alert>
        <color-box-issue v-if="status === 'Issue'"></color-box-issue>
        <color-box-notloaded v-if="status === 'NotLoaded'"></color-box-notloaded>
        <router-link :to="'/package/'+name"><button class="view-button">View</button></router-link>
    </div>
</template>

<script>
    import ColorBoxOK from '../components/colorbox/ColorBoxOK.vue'
    import ColorBoxAlert from '../components/colorbox/ColorBoxAlert.vue'
    import ColorBoxIssue from '../components/colorbox/ColorBoxIssue.vue'
    import ColorBoxNotLoaded from '../components/colorbox/ColorBoxNotLoaded.vue'

    export default {
        props: ['state', 'name'],
        components: {
            'color-box-ok': ColorBoxOK,
            'color-box-alert': ColorBoxAlert,
            'color-box-issue': ColorBoxIssue,
            'color-box-notloaded': ColorBoxNotLoaded
        },
        computed: {
            status: function() {
                return this.state || 'NotLoaded';
            }
        }
    }
</script>

<style>

</style>