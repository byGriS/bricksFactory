<template>
  <div>
    <h3>Общее кол-во резов: {{countBricks}}</h3>
    <highcharts :options="chartOptions"></highcharts>
    <div>
      <list-bricks :bricks="bricks" />
    </div>
  </div>
</template>
<script>
import ListBricks from "./ListBricks";

export default {
  components: {
    ListBricks
  },
  props: ["selectDay"],
  data() {
    return {
      timer: null,
      bricks: [],
      chartOptions: {
        chart: { type: "line" },
        title: { text: "" },
        xAxis: { type: "datetime" },
        yAxis: { title: { text: "" } },
        legend: { enabled: false },
        series: [
          {
            marker: { enabled: false, symbol: null },
            name: "Блоки",
            data: []
          }
        ]
      }
    };
  },
  computed: {
    countBricks() {
      return this.bricks.length;
    },
    shift() {
      return this.$store.state.shift;
    }
  },
  methods: {
    updateValue() {
      this.$http
        .post("http://c98744oh.beget.tech/getByShift.php", {
          //.post("http://bricks.loc/get.php",{
          dt: this.selectDay,
          starthour: this.shift.shiftStartHour,
          startminute: this.shift.shiftStartMinute,
          endhour: this.shift.shiftEndHour,
          endminute: this.shift.shiftEndMinute
        })
        .then(function(response) {
          return response.json();
        })
        .then(function(data) {
          this.bricks = data;
          this.chartOptions.series.push({
            marker: { enabled: false, symbol: null },
            name: "Блоки",
            data: []
          });

          var indexSeries = 1;
          for (
            let i = this.chartOptions["series"][0].data.length;
            i < data.length;
            i++
          ) {
            this.chartOptions["series"][0].data.push({
              x: this.$moment.utc(data[i]["dt"]).valueOf(),
              y: i + 1
            });
            if (i == 0) var preDT = this.$moment.utc(data[i]["dt"]).valueOf();
            else {
              if (
                this.chartOptions["series"][0].data[
                  this.chartOptions["series"][0].data.length - 1
                ].x -
                  preDT >
                  300000 &&
                this.chartOptions["series"][0].data[
                  this.chartOptions["series"][0].data.length - 1
                ].x -
                  preDT <
                  3600000
              ) {
                this.chartOptions.series.push({
                  marker: { enabled: false, symbol: null },
                  name: "Блоки",
                  data: [],
                  color: "#ffaa00"
                });
                this.chartOptions["series"][indexSeries].data.push({
                  x: preDT,
                  y: i
                });
                this.chartOptions["series"][indexSeries].data.push({
                  x: this.$moment.utc(data[i]["dt"]).valueOf(),
                  y: i + 1
                });
                indexSeries++;
              }
              if (
                this.chartOptions["series"][0].data[
                  this.chartOptions["series"][0].data.length - 1
                ].x -
                  preDT >
                3600000
              ) {
                this.chartOptions.series.push({
                  marker: { enabled: false, symbol: null },
                  name: "Блоки",
                  data: [],
                  color: "#ff0000"
                });
                this.chartOptions["series"][indexSeries].data.push({
                  x: preDT,
                  y: i
                });
                this.chartOptions["series"][indexSeries].data.push({
                  x: this.$moment.utc(data[i]["dt"]).valueOf(),
                  y: i + 1
                });
                indexSeries++;
              }
              preDT = this.$moment.utc(data[i]["dt"]).valueOf();
            }
          }
        });
    }
  },
  watch: {
    selectDay: function(val) {
      this.chartOptions["series"][0].data = [];
      this.chartOptions.series = [];
      this.updateValue();
    },
    shift() {
      this.chartOptions["series"][0].data = [];
      this.chartOptions.series = [];
      this.updateValue();
    }
  },
  created() {
    var self = this;
    this.chartOptions["series"][0].data = [];
    this.chartOptions.series = [];
    this.timer = setInterval(function() {
      self.updateValue();
    }, 3000);
    self.updateValue();
  },
  beforeDestroy() {
    clearInterval(this.timer);
  }
};
</script>