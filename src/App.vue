<template>
  <div>
    <nav class="navbar navbar-dark bg-dark sticky-top">
      <div class="container">
        <a class="navbar-brand" href="/">Производство</a>
      </div>
    </nav>
    <div class="container">
      <div class="row">
        <div class="col-4">
          <label>Вариан просмотра</label>
          <br />
          <input type="radio" id="workShift" value="shift" v-model="picked" />
          <label for="workShift">За смену</label>
          <br />
          <input type="radio" id="perDay" value="day" v-model="picked" />
          <label for="perDay">За день</label>
          <br />
          <input type="radio" id="perRange" value="range" v-model="picked" />
          <label for="perRange">За диапазон</label>
        </div>
        <div class="col-4">
          <Shift v-if="picked=='shift'" :shift="shift" />
        </div>
        <div class="col-4">
          <div v-if="picked=='day' || picked=='shift'">
            <v-day-selector v-model="date" />
          </div>
          <div v-else>
            <v-range-selector :start-date.sync="range.start" :end-date.sync="range.end" />
          </div>
        </div>
      </div>
      <div class="col-sm-6 col-md-7 col-lg-8 col-xl-9">
        <ViewShift v-if="picked=='shift'" />
        <ViewDay v-if="picked=='day'" :selectDay="date" />
        <ViewRange v-if="picked=='range'" :range="range" />
      </div>
    </div>
  </div>
</template>

<script>
import VRangeSelector from "vuelendar/components/vl-range-selector";
import VDaySelector from "vuelendar/components/vl-day-selector";
import Shift from "./components/Shift";
import ViewShift from "./components/ViewShift"
import ViewDay from "./components/ViewDay";
import ViewRange from "./components/ViewRange";

export default {
  components: {
    VRangeSelector,
    VDaySelector,
    Shift,
    ViewShift,
    ViewDay,
    ViewRange
  },
  data() {
    return {
      range: {},
      date: null,
      picked: "shift",
      shift: {
        shiftStartHour: 8,
        shiftStartMinute: 0,
        shiftEndHour: 17,
        shiftEndMinute: 0
      }
    };
  },
  watch: {
    picked: function(val) {
      /*
      switch(val){
        case "shift":
          this.date = null;
          this.range = {};
      }
      if (val == "day") this.range = {};
      else this.date = null;*/
    }
  }
};
</script>

<style lang="scss">
@import "vuelendar/scss/vuelendar.scss";
nav {
  margin-bottom: 10px;
}
</style>
