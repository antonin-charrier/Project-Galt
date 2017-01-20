<template>
    <div class="home">
        <logo-450x650 class="home-logo"></logo-450x650>
        <p id="home-description">Galt allows you to manage your packages dependencies. You can check if new versions are available or if there is any issue in your dependencies tree.</p>
    </div>
</template>

<script>
import Logo450x650 from '../components/Logo-450x650.vue';

export default {
    components:{
        'logo-450x650' : Logo450x650
    }
}

</script>

<style>
#home-description{
    text-align: center;
    color: #226D71;
}
.home-logo{
    width: 100%;
    height: 300px;
}
</style>