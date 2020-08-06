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
        credits: { enabled: false },
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
      return this.mercuriyA / 1000.0;
    }
  },
  methods: {
    updateValue() {
      let dt2 = this.selectDay;
      if (parseInt(this.shift.shiftEndHour) < parseInt(this.shift.shiftStartHour)) {
        dt2 = this.$moment(this.selectDay).add(1, 'day').format('YYYY-MM-DD');
      }
      this.$http
        .post(this.$store.state.host + "getByShift.php", {
          dt1: this.selectDay,
          dt2: dt2,
          starthour: this.shift.shiftStartHour,
          startminute: this.shift.shiftStartMinute,
          endhour: this.shift.shiftEndHour,
          endminute: this.shift.shiftEndMinute,
        })
        .then(function (response) {
          return response.json();
        })
        .then(function (data) {
          this.$store.state.mercuriyA = 0;
          this.$store.state.mercuriyR = 0;
          this.$store.state.pulsar = 0;
          this.$store.state.cement = 0;
          this.$store.state.clay = 0;
          this.$store.state.sand = 0;
          this.bricks = data.bricks;
          this.$store.state.totalBlocks = data.bricks.length;
          if (this.chartOptions.series.length < 1)
            this.chartOptions.series.push({
              marker: { enabled: false, symbol: null },
              name: "Резы",
              data: [],
              turboThreshold: 0
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
            this.$store.state.mercuriyA = counters[counters.length - 1]["mercuriyA"] - counters[0]["mercuriyA"];
            this.$store.state.mercuriyR = counters[counters.length - 1]["mercuriyR"] - counters[0]["mercuriyR"];
            this.$store.state.pulsar = (counters[counters.length - 1]["pulsar"] - counters[0]["pulsar"]) / 1000.0;
          }

          let tenzo = data.tenzo;
          if (tenzo.length > 0) {
            this.$store.state.cement = tenzo.reduce((sum, elem) => { return parseInt(sum) + parseInt(elem["tenzo"]) - 6 }, 0);
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
    }, 10000);
    self.updateValue();
  },
  beforeDestroy() {
    clearInterval(this.timer);
  }
};
</script>