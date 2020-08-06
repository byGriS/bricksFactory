import Vue from "vue"
import Vuex from "vuex"

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    shift: {
      shiftStartHour: 8,
      shiftStartMinute: 0,
      shiftEndHour: 20,
      shiftEndMinute: 0,
      norm: 75
    },
    norm: 75,
    host: "http://u0838029bricks.isp.regruhosting.ru/",
    isAdmin: false,
    mercuriyA: 0,
    mercuriyR: 0,
    pulsar: 0,
    cement: 0,
    clay: 0,
    sand: 0,
    mercuriyAPrice: 0,
    pulsarPrice: 0,
    cementPrice: 0,
    clayPrice: 0,
    sandPrice: 0,
    totalBlocks: 0,
    countBlocks: 0 ,
    date: null,
    range: null,
  }
})