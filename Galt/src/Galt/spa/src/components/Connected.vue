<template>
    <div id="connected">
        <li class="w3-right">
            <a href="/#/" @click="logout" class="w3-padding-16 w3-hover-gray w3-hover-text-white">
                <span id="icon-package"><i class="fa fa-github"></i></span><span>Disconnect from GitHub</span></router-link>
            </a>
        </li>
        <li class="w3-right"><router-link to="/mypackages" class="w3-padding-16 w3-hover-gray w3-hover-text-white">
            <span id="icon-package"><i class="fa fa-cube"></i></span><span>My Packages</span></router-link></li>
    </div>
</template>

<script>
    import AuthService from '../services/AuthService'

    export default {
        created() {
            AuthService.registerSignedOutCallback(this.onSignedOut);
        },

        beforeDestroy() {
            AuthService.removeSignedOutCallback(this.onSignedOut);
        },
        computed: {
            logoutEndpoint: () => AuthService.logoutEndpoint
        },

        methods: {
            onSignedOut() {
                // this.$router.replace('/');
            },
            logout() {
                AuthService.logout();
            }
        }
    }
</script>