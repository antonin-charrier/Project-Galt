import 'babel-polyfill'
import Vue from 'vue'
import Vuex from 'vuex'
import VueResource from 'vue-resource'
import VueRouter from 'vue-router'
import App from './App.vue'
import Home from './pages/Home.vue'
import Package from './pages/Package.vue'
import MyPackages from './pages/MyPackages.vue'

import AuthService from './services/AuthService'

Vue.use(VueRouter);
Vue.use(Vuex);
Vue.use(VueResource);

const routes = [
    { path: '/', component: Home },
    { path: '/mypackages', component: MyPackages },
    { path: '/package/:id', component: Package },
    { path: '/package/:id/:version', component: Package }
];

const router = new VueRouter({
    routes
});

const store = new Vuex.Store({
    state: {
        isConnected: false,
        identity: null
    },
    mutations: {
        connect(state, identity) {
            state.isConnected = true
            state.identity = identity
        },
        disconnect(state) {
            state.isConnected = false
            state.identity = null
        }
    },
    actions: {
        connect(context, identity) {
            context.commit('connect', identity)
        },
        disconnect(context) {
            context.commit('disconnect')
        }
    }
});

const Connecter = {
    computed: {
        isConnected() {
            return this.$store.state.isConnected
        }
    }
};

AuthService.allowedOrigins = ['http://localhost:5000'];

AuthService.logoutEndpoint = '/Account/LogOff';

AuthService.registerAuthenticatedCallback(() => {
    GaltProject.Galt.setIdentity(AuthService.identity)
    store.commit('connect', GaltProject.Galt.getIdentity())
})

AuthService.registerSignedOutCallback(() => {
    GaltProject.Galt.setIdentity(null);
    store.commit('disconnect')
})

if (GaltProject.Galt.getIdentity() != null)
    store.commit('connect', GaltProject.Galt.getIdentity())

const app = new Vue({
    el: '#app',
    router,
    store,
    components: { Connecter },
    render: h => h(App),
});