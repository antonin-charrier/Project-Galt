<template>
    <div class="favorite-packages-items">
        <div class="favorite-packages-info">{{ favorite.packageId }}</div>
        <color-box-ok v-if="favorite.state === 'ok'"></color-box-ok>
        <color-box-alert v-if="favorite.state === 'alert'"></color-box-alert>
        <color-box-issue v-if="favorite.state === 'issue'"></color-box-issue>
        <router-link :to="'/package/'+favorite.packageId"><button class="view-button">View</button></router-link>
    </div>
</template>

<script>
import ColorBoxOK from '../components/ColorBoxOK.vue'
import ColorBoxAlert from '../components/ColorBoxAlert.vue'
import ColorBoxIssue from '../components/ColorBoxIssue.vue'

export default {
    props: ['favorite'],
    components: {
        'color-box-ok' : ColorBoxOK,
        'color-box-alert' : ColorBoxAlert,
        'color-box-issue' : ColorBoxIssue
    }
}
</script>

<style>

</style>