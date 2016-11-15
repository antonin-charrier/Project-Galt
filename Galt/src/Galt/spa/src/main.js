import Vue from 'vue'
import VueRouter from 'vue-router'
import App from './App.vue'
import Home from './pages/Home.vue'
import Package from './pages/Package.vue'
import MyPackages from './pages/MyPackages.vue'

Vue.use(VueRouter);

const routes = [
  { path: '/', component: Home},
  { path: '/mypackages', component: MyPackages},
  { path: '/package', component: Package}
];

const router = new VueRouter({
  routes
})

Vue.component('disconnect-button', {
  template: '<li class="w3-right"><router-link to="/disconnect" class="w3-padding-16 w3-hover-gray w3-hover-text-white"><span id="icon-package"><i class="fa fa-sign-out"></i></span><span>Disconnect from GitHub</span></router-link></li>'
})

const app = new Vue({
  el: '#app',
  router,
  render: h => h(App),
});
