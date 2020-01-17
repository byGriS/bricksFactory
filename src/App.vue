<template>
  <div>
    <nav class="navbar navbar-dark bg-dark sticky-top">
      <div class="container">
        <a class="navbar-brand" href="/">Производство</a>
      </div>
    </nav>
    <div class="container">
      <div class="row">
        <div class="col-sm-6 col-md-5 col-lg-4 col-xl-3">
          <br/>
          <label>Вариан просмотра</label>
          <br/>
           <input type="radio" id="one" value="day" v-model="picked" />
          <label for="one">За день</label>
          <br />
          <input type="radio" id="two" value="range" v-model="picked" />
          <label for="two">За диапазон</label>
          <div v-if="picked=='day'">
            <v-day-selector v-model="date" />
          </div>
          <div v-else>
            <v-range-selector :start-date.sync="range.start" :end-date.sync="range.end" />
          </div>
        </div>
        <div class="col-sm-6 col-md-7 col-lg-8 col-xl-9">
          <ViewDay v-if="date!=null" v-bind:selectDay="date"/>
          <ViewRange v-if="range.end != null" v-bind:range="range"/>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import VRangeSelector from "vuelendar/components/vl-range-selector";
import VDaySelector from "vuelendar/components/vl-day-selector";
import ViewDay from "./components/ViewDay";
import ViewRange from "./components/ViewRange";

export default {
  components: {
    VRangeSelector,
    VDaySelector,
    ViewDay,
    ViewRange
  },
  data() {
    return {
      range: {},
      date: null,
      picked: "day"
    };
  },
  watch: {
    picked: function(val) {
      if (val == "day") this.range = {};
      else this.date = null;
    }
  }
};
</script>

<style lang="scss">
@import "vuelendar/scss/vuelendar.scss";
</style>
