<template>
  <div>
    <div class="container">
      <div class="row">
        <div class="col-xl-3 col-lg-4 col-md-5 col-sm-5 mb-2">
          <div v-if="picked=='day' || picked=='shift'">
            <v-day-selector v-model="date" />
          </div>
          <div v-else>
            <v-range-selector
              :start-date.sync="range.start"
              :end-date.sync="range.end"
              @focus="changeRange()"
            />
          </div>
        </div>
        <div class="col-xl-9 col-lg-8 col-md-7 col-sm-8">
          <div class="row selectView mb-2">
            <div class="col-md-3">
              <label>Вариан просмотра</label>
            </div>
            <div class="col-md-3">
              <input type="radio" id="workShift" value="shift" v-model="picked" />
              <label for="workShift">За смену</label>
            </div>
            <div class="col-md-3">
              <input type="radio" id="perDay" value="day" v-model="picked" />
              <label for="perDay">За день</label>
            </div>
            <div class="col-md-3">
              <input type="radio" id="perRange" value="range" v-model="picked" />
              <label for="perRange">За диапазон</label>
            </div>
          </div>
          <div class="row">
            <div class="col-md-7">
              <Shift v-if="picked=='shift'" />
            </div>
            <div class="col-md-5">
              <Spending />
            </div>
          </div>
        </div>
      </div>
      <div class>
        <ViewShift v-if="picked=='shift'" :selectDay="date" />
        <ViewDay v-if="picked=='day'" :selectDay="date" />
        <ViewRange v-if="picked=='range'" />
      </div>
    </div>
  </div>
</template>

<script>
import VRangeSelector from "vuelendar/components/vl-range-selector";
import VDaySelector from "vuelendar/components/vl-day-selector";
import Shift from "./Shift";
import ViewShift from "./ViewShift"
import ViewDay from "./ViewDay";
import ViewRange from "./ViewRange";
import Spending from "./Spending";

export default {
  components: {
    VRangeSelector,
    VDaySelector,
    Shift,
    ViewShift,
    ViewDay,
    ViewRange,
    Spending,
  },
  data() {
    return {
      timer: null,
      range: {},
      date: null,
      picked: "shift",
      error: true,
      wd: -1
    };
  },
  computed: {
    isAdmin() {
      return this.$store.state.isAdmin;
    }
  },
  watch: {
    date() {
      this.$store.state.date = this.date;
    },
  },
  methods: {
    changeRange() {
      if (this.range.end != null){
        this.$store.state.range = null;
        this.$store.state.range = this.range;
      }
    },
    updatePrice() {
      this.$http
        .get(this.$store.state.host + "getPrice.php")
        .then(function (response) {
          return response.json();
        })
        .then(function (data) {
          data = data['prices'][0];
          this.$store.state.mercuriyAPrice = data['mercuriyAPrice'];
          this.$store.state.pulsarPrice = data['pulsarPrice'];
          this.$store.state.cementPrice = data['cementPrice'];
          this.$store.state.clayPrice = data['clayPrice'];
          this.$store.state.sandPrice = data['sandPrice'];
        });
    },
  },
  mounted() {
    this.updatePrice();
  }
};
</script>

<style lang="scss">
@import "vuelendar/scss/vuelendar.scss";
.selectView {
  background: rgb(228, 228, 228);
  border-radius: 4px;
  border: 1px solid rgb(180, 180, 180);
  padding: 5px;
}
.selectView label {
  margin-bottom: 0 !important;
}
</style>
