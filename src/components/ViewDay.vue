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
  props: { selectDay: "" },
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
        credits: { enabled: false },
        series: [
          {
            marker: { enabled: false, symbol: null },
            name: "Резы",
            data: []
          }
        ]
      },
    };
  },
  computed: {
    countBricks() {
      return this.bricks.length;
    },
  },
  methods: {
    updateValue() {
      this.$http
        .post(this.$store.state.host + "get.php", {
          dt: this.selectDay
        })
        .then(function (response) {
          return response.json();
        })
        .then(function (data) {
          this.$store.state.mercuriyA = 0;
          this.$store.state.pulsar = 0;
          this.$store.state.cement = 0;
          this.$store.state.clay = 0;
          this.$store.state.sand = 0;
          this.bricks = data.bricks;
          this.$store.state.totalBlocks = data.bricks.length;
          this.chartOptions.series.push({
            marker: { enabled: false, symbol: null },
            name: "Блоки",
            data: [],
            turboThreshold: 0
          });

          var indexSeries = 1;
          for (let i = this.chartOptions["series"][0].data.length; i < this.bricks.length; i++) {
            this.chartOptions["series"][0].data.push({
              x: this.$moment.utc(this.bricks[i]["dt"]).valueOf(),
              y: i + 1
            });
            if (i == 0) var preDT = this.$moment.utc(this.bricks[i]["dt"]).valueOf();

            else {
              if (this.chartOptions["series"][0].data[this.chartOptions["series"][0].data.length - 1].x - preDT > 300000 &&
                this.chartOptions["series"][0].data[this.chartOptions["series"][0].data.length - 1].x - preDT < 3600000) {
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
                  x: this.$moment.utc(this.bricks[i]["dt"]).valueOf(),
                  y: i + 1
                });
                indexSeries++;
              }
              if (this.chartOptions["series"][0].data[this.chartOptions["series"][0].data.length - 1].x - preDT > 3600000) {
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
                  x: this.$moment.utc(this.bricks[i]["dt"]).valueOf(),
                  y: i + 1
                });
                indexSeries++;
              }
              preDT = this.$moment.utc(this.bricks[i]["dt"]).valueOf();
            }
          }

          // counters
          let counters = data.counters;
          if (counters.length > 0) {
            this.$store.state.mercuriyA = counters[counters.length - 1]["mercuriyA"] - counters[0]["mercuriyA"];
            this.$store.state.pulsar = (counters[counters.length - 1]["pulsar"] - counters[0]["pulsar"]) / 1000.0;
          }

          let tenzo = data.tenzo;
          if (tenzo.length > 0) {
            this.$store.state.cement = tenzo.reduce((sum, elem) => { return parseInt(sum) + parseInt(elem["tenzo"] - 6) }, 0);
          }
          let rifey = data.rifey;
          if (rifey.length > 0) {
            let notNullClay = 0;
            var max = rifey.reduce((sum, elem) => {
              if (elem['clay'] == 0)
                elem['clay'] = notNullClay;
              else
                notNullClay = elem['clay'];
              sum.total += elem["rifey"] - 30;
              sum.preClay += notNullClay - 30;
              return sum;
            }, { total: 0, preClay: 0 });
            this.$store.state.sand = max.total - max.preClay;
            this.$store.state.clay = max.preClay;
          }
        });

      this.$http.post(this.$store.state.host + "getShiftsDataDay.php", {
        dt: this.$store.state.date
      })
        .then(function (response) {
          if (response.data.length > 0) {
            this.$store.state.countBlocks = response.data[0]['brickcount'];
          } else {
            this.$store.state.countBlocks = 4;
          };
        })
    }
  },
  watch: {
    selectDay: function (val) {
      this.chartOptions["series"][0].data = [];
      this.chartOptions.series = [];
      this.updateValue();
    }
  },
  created() {
    var self = this;
    this.chartOptions["series"][0].data = [];
    this.chartOptions.series = [];
    this.timer = setInterval(function () {
      self.updateValue();
    }, 10000);
    self.updateValue();
  },
  beforeDestroy() {
    clearInterval(this.timer);
  }
};
</script>