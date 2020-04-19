import Vue from 'vue'
import App from './App.vue'
import VueResource from 'vue-resource'
import moment from 'moment';
import HighchartsVue from 'highcharts-vue'
import Highcharts from 'highcharts';

import store from './stores/store'


Vue.prototype.$moment = moment;
Vue.config.productionTip = false
Vue.use(VueResource);
Vue.http.options.emulateJSON = true;
Vue.use(HighchartsVue)

Highcharts.setOptions({
  lang: {
    loading: "Загрузка...",
    weekdays: [
      "Воскресенье",
      "Понедельник",
      "Вторник",
      "Среда",
      "Четверг",
      "Пятница",
      "Суббота"
    ],
    shortMonths: [
      "Янв",
      "Фев",
      "Март",
      "Апр",
      "Май",
      "Июнь",
      "Июль",
      "Авг",
      "Сент",
      "Окт",
      "Нояб",
      "Дек"
    ]
  }
});

new Vue({
  store: store,
  render: function (h) { return h(App) },
}).$mount('#app')
