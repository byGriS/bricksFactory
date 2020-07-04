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
    <highcharts :options="chartOptions"></highcharts>
    <!--<div>
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
  props: ["selectDay"],
  data() {
    return {
      timer: null,
      bricks: [],
      chartOptions: {
        chart: { type: "line" },
        credits: {
          enabled: false
        },
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
    shift() {
      return this.$store.state.shift;
    },
    norm() {
      return this.$store.state.norm;
    },
    mercuriy() {
      return this.mercuriyA;
    }
  },
  methods: {
    updateValue() {
      this.$http
        .post(this.$store.state.host + "getByShift.php", {
          dt: this.selectDay,
          starthour: this.shift.shiftStartHour,
          startminute: this.shift.shiftStartMinute,
          endhour: this.shift.shiftEndHour,
          endminute: this.shift.shiftEndMinute,
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
          if (this.chartOptions.series.length < 1)
            this.chartOptions.series.push({
              marker: { enabled: false, symbol: null },
              name: "Резы",
              data: []
            });

          var indexSeries = 2; //первая линий - сами блоки, вторая линия - отклонение
          for (let i = this.chartOptions["series"][0].data.length; i < this.bricks.length; i++) {
            this.chartOptions["series"][0].data.push({ x: this.$moment.utc(this.bricks[i]["dt"]).valueOf(), y: i + 1 });
            if (i == 0) {
              var preDT = this.$moment.utc(this.bricks[i]["dt"]).valueOf();
              var preDTNorm = this.$moment.utc(this.bricks[i]["dt"]).valueOf();
              this.chartOptions.series.push({
                marker: { enabled: false, symbol: null },
                name: "Отклонение",
                data: [],
                color: "#00ff00"
              });
              this.chartOptions["series"][1].data.push({ x: preDTNorm, y: 0 });
            } else {
              // формирования линий простоев /*
              if (this.chartOptions["series"][0].data[i].x - preDT > 300000 && this.chartOptions["series"][0].data[i].x - preDT < 3600000) {
                this.chartOptions.series.push({
                  marker: { enabled: false, symbol: null },
                  name: "Простой",
                  data: [],
                  color: "#ffaa00"
                });
                this.chartOptions["series"][indexSeries].data.push({ x: preDT, y: i });
                this.chartOptions["series"][indexSeries].data.push({ x: this.$moment.utc(this.bricks[i]["dt"]).valueOf(), y: i + 1 });
                indexSeries++;
              }
              if (this.chartOptions["series"][0].data[i].x - preDT > 3600000) {
                this.chartOptions.series.push({
                  marker: { enabled: false, symbol: null },
                  name: "Простой",
                  data: [],
                  color: "#ff0000"
                });
                this.chartOptions["series"][indexSeries].data.push({ x: preDT, y: i });
                this.chartOptions["series"][indexSeries].data.push({ x: this.$moment.utc(this.bricks[i]["dt"]).valueOf(), y: i + 1 });
                indexSeries++;
              }
              preDT = this.$moment.utc(this.bricks[i]["dt"]).valueOf();
              // */ формирования линий простоев

              //определение отклонения от норматива /*
              let devDT = this.chartOptions["series"][0].data[i].x - this.chartOptions["series"][0].data[0].x;
              //if (devDT > 3600000) {
              let deviation = (i + 1) - devDT / 3600000 * this.shift.norm;
              this.chartOptions["series"][1].data.push({ x: this.$moment.utc(this.bricks[i]["dt"]).valueOf(), y: deviation });
              //preDTNorm = this.$moment.utc(data[i]["dt"]).valueOf();
              //}
              // */ определение отклонения от норматива
            }
          }

          // counters
          let counters = data.counters;
          if (counters.length > 0) {
            this.mercuriyA = counters[counters.length - 1]["mercuriyA"] - counters[0]["mercuriyA"];
            this.mercuriyR = counters[counters.length - 1]["mercuriyR"] - counters[0]["mercuriyR"];
            this.pulsar = counters[counters.length - 1]["pulsar"] - counters[0]["pulsar"];
          }

          let tenzo = data.tenzo;
          if (tenzo.length > 0) {
            this.cement = tenzo.reduce((sum, elem) => { return parseInt(sum) + parseInt(elem["tenzo"]) - 6 }, 0);
          }
          let rifey = data.rifey;
          if (rifey.length > 0) {
            this.sand = rifey.reduce((sum, elem) => { return parseInt(sum) + parseInt(elem["rifey"]) - 20 }, 0);
          }
        });
    }
  },
  watch: {
    selectDay: function (val) {
      if (this.chartOptions["series"].length > 0) {
        this.chartOptions["series"][0].data = [];
      }
      this.chartOptions.series = [];
      this.updateValue();
    },
    shift() {
      if (this.chartOptions["series"].length > 0) {
        this.chartOptions["series"][0].data = [];
      }
      this.chartOptions.series = [];
      this.updateValue();
    },
    norm() {
      if (this.chartOptions["series"].length > 0) {
        this.chartOptions["series"][0].data = [];
      }
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
    }, 3000);
    self.updateValue();
  },
  beforeDestroy() {
    clearInterval(this.timer);
  }
};
</script>