import Vue from 'vue'
import Vuex from 'vuex'
import VueRouter from 'vue-router'
import App from './App.vue'
import Home from './pages/Home.vue'
import Package from './pages/Package.vue'
import MyPackages from './pages/MyPackages.vue'

Vue.use(VueRouter);
Vue.use(Vuex);

const routes = [
    { path: '/', component: Home },
    { path: '/mypackages', component: MyPackages },
    { path: '/package', component: Package }
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
            console.log("Connect mutation called")
            state.isConnected = true
            state.identity = identity
        }
    },
    actions: {
        connect(context, identity) {
            console.log("Connect action called")
            context.commit('connect', identity)
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

if (GaltProject.Galt.getIdentity() != null)
    store.commit('connect', GaltProject.Galt.getIdentity())

const app = new Vue({
    el: '#app',
    router,
    store,
    components: { Connecter },
    render: h => h(App),
});