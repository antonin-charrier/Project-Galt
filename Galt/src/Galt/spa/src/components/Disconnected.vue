<template>
    <div id="disconnected">
        <li class="w3-right">
            <a href="/#/" @click="login" class="w3-padding-16 w3-hover-gray w3-hover-text-white">
                <span id="icon-package"><i class="fa fa-sign-in"></i></span><span>Connect with GitHub</span>
            </a>
        </li>
    </div>
</template>

<script>
    import AuthService from '../services/AuthService'

    export default {
        methods: {
            login() {
                AuthService.login();
            }
        }
    }
</script>