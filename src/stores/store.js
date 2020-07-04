import Vue from "vue"
import Vuex from "vuex"

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    shift: {
      shiftStartHour: 8,
      shiftStartMinute: 0,
      shiftEndHour: 17,
      shiftEndMinute: 0,
      norm: 100
    },
    norm: 100,
    pass: "195",
    host: "http://u0838029bricks.isp.regruhosting.ru/"
  }
})