import Vue from 'vue';
import App from '@/views/app.vue';
import router from '@/router';

Vue.config.productionTip = false

router.beforeEach((to, from, next) => {
  if (to.meta.title) {
    document.title = to.meta.title
  }
  next(true);
});

new Vue({
  render: h => h(App),
  router: router,
}).$mount('#app');