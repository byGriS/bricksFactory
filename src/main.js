import Vue from 'vue'
import App from './App.vue'
import VueResource from 'vue-resource'
import moment from 'moment';


Vue.prototype.$moment = moment;
Vue.config.productionTip = false
Vue.use(VueResource);
Vue.http.options.emulateJSON = true;

new Vue({
  render: function (h) { return h(App) },
}).$mount('#app')
