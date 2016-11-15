import Vue from 'vue'
import VueRouter from 'vue-router'
import App from './App.vue'
import Home from './pages/Home.vue'
import About from './pages/About.vue'

Vue.use(VueRouter)

const Foo = { template: '<div>foo</div>' }
const Bar = { template: '<div>bar</div>' }

const routes = [
  { path: '/', component: Home},
  { path: '/foo', component: Foo },
  { path: '/bar', component: Bar },
  { path: '/about', component: About}
]

const router = new VueRouter({
  routes
})

const app = new Vue({
  el: '#app',
  router,
  render: h => h(App)
});
