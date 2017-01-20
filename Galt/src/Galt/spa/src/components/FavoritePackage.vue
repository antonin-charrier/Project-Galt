<template>
    <div class="favorite-packages-items">
        <div class="favorite-packages-info">{{ favorite }}</div>
        <color-box-ok v-if="state === 'ok'"></color-box-ok>
        <color-box-alert v-if="state === 'alert'"></color-box-alert>
        <color-box-issue v-if="state === 'issue'"></color-box-issue>
        <color-box-notloaded v-if="state === 'notloaded'"></color-box-notloaded>
        <router-link :to="'/package/'+favorite"><button class="view-button">View</button></router-link>
    </div>
</template>

<script>
    import ColorBoxOK from '../components/colorbox/ColorBoxOK.vue'
    import ColorBoxAlert from '../components/colorbox/ColorBoxAlert.vue'
    import ColorBoxIssue from '../components/colorbox/ColorBoxIssue.vue'
    import ColorBoxNotLoaded from '../components/colorbox/ColorBoxNotLoaded.vue'

    export default {
        props: ['favorite'],
        components: {
            'color-box-ok': ColorBoxOK,
            'color-box-alert': ColorBoxAlert,
            'color-box-issue': ColorBoxIssue,
            'color-box-notloaded': ColorBoxNotLoaded
        },
        computed: {
            state: function() {
                return this.favorite.state || 'notloaded';
            }
        }
    }
</script>

<style>

</style>