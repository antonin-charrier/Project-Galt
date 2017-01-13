<template>
    <div>
        <router-link to="/package" href="#">Version {{ versions }}</router-link>
    </div>
</template>
<script>
export default {
    props: ['version'],
    computed: {
        versions: function(){
            return this.version.ListVPackage[0];
        }
    }
}
</script>
