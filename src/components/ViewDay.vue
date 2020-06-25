<template>
  <div>
    <h3>Затраты</h3>
    <div class="row">
      <div class="col-md-2">Эл. энергия, Вт</div>
      <div class="col-md-1">{{mercuriy}}</div>
    </div>
    <div class="row">
      <div class="col-md-2">Вода, л</div>
      <div class="col-md-1">{{pulsar}}</div>
    </div>
    <div class="row">
      <div class="col-md-2">Цемент, кг</div>
      <div class="col-md-1">{{cement}}</div>
    </div>
    <div class="row">
      <div class="col-md-2">Песок+керамзит, кг</div>
      <div class="col-md-1">{{sand}}</div>
    </div>
    <h3>Общее кол-во резов: {{countBricks}}</h3>
    <!--<highcharts :options="chartOptions"></highcharts>
    <div>
      <list-bricks :bricks="bricks" />
    </div>-->
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
        series: [
          {
            marker: { enabled: false, symbol: null },
            name: "Резы",
            data: []
          }
        ]
      },
      mercuriyA: 0,
      mercuriyR: 0,
      pulsar: 0,
      cement: 0,
      sand: 0
    };
  },
  computed: {
    countBricks() {
      return this.bricks.length;
    },
    mercuriy() {
      return this.mercuriyA + this.mercuriyR;
    }
  },
  methods: {
    updateValue() {
      this.$http
        .post("http://c98744oh.beget.tech/get.php", {
          //.post("http://bricks.loc/get.php",{
          dt: this.selectDay
        })
        .then(function (response) {
          return response.json();
        })
        .then(function (data) {
          this.mercuriyA = 0;
          this.mercuriyR = 0;
          this.pulsar = 0;
          this.cement = 0;
          this.sand = 0;
          this.bricks = data.bricks;

          /*this.chartOptions.series.push({
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
                  x: this.$moment.utc(data[i]["dt"]).valueOf(),
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
                  x: this.$moment.utc(data[i]["dt"]).valueOf(),
                  y: i + 1
                });
                indexSeries++;
              }
              preDT = this.$moment.utc(data[i]["dt"]).valueOf();
            }
          }*/

          // counters
          let counters = data.counters;
          if (counters.length > 0) {
            this.mercuriyA = counters[counters.length - 1]["mercuriyA"] - counters[0]["mercuriyA"];
            this.mercuriyR = counters[counters.length - 1]["mercuriyR"] - counters[0]["mercuriyR"];
            this.pulsar = counters[counters.length - 1]["pulsar"] - counters[0]["pulsar"];
          }

          let tenzo = data.tenzo;
          if (tenzo.length > 0) {
            this.cement = tenzo.reduce((sum, elem) => { return parseInt(sum) + parseInt(elem["tenzo"]-6)}, 0);
          }
          let rifey = data.rifey;
          if (rifey.length > 0) {
            this.sand = rifey.reduce((sum, elem) => { return parseInt(sum) + parseInt(elem["rifey"])-20 }, 0);
          }
        });
    }
  },
  watch: {
    selectDay: function (val) {
      /*this.chartOptions["series"][0].data = [];
      this.chartOptions.series = [];*/
      this.updateValue();
    }
  },
  created() {
    var self = this;
    /*this.chartOptions["series"][0].data = [];
    this.chartOptions.series = [];*/
    this.timer = setInterval(function () {
      self.updateValue();
    }, 3000);
    self.updateValue();
  },
  beforeDestroy() {
    clearInterval(this.timer);
  }
};
</script>