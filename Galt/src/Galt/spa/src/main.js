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
  { path: '/', component: Home},
  { path: '/mypackages', component: MyPackages},
  { path: '/package', component: Package}
];

const router = new VueRouter({
  routes
})

/*const store = new Vuex.store({
  state:{
    count: 0
  },
  mutations:{
    increment(state){
      state.count++
    }
  }
})*/

const app = new Vue({
  el: '#app',
  router,
  render: h => h(App),
});
