<template>
  <div>
    <h3>Общее кол-во резов: {{countBricks}}</h3>
    <chart :chart-data="chartdata" ref="chartElem"/>
    <div>
      <list-bricks :bricks="bricks" />
    </div>
  </div>
</template>
<script>
import ListBricks from "./ListBricks";
import Chart from "./Chart.js";

export default {
  components: {
    ListBricks,
    Chart
  },
  props: { selectDay: "" },
  data() {
    return {
      timer: null,
      bricks: [],
      chartdata: {
        datasets: [
          {
            //backgroundColor: "#f87979",
            data: [],
            steppedLine: "middle"
          }
        ]
      }
    };
  },
  computed: {
    countBricks() {
      return this.bricks.length;
    }
  },
  methods: {
    updateValue() {
      this.$http
        .post("http://c98744oh.beget.tech/get.php", {
          //.post("http://bricks.loc/get.php",{
          dt: this.selectDay
        })
        .then(function(response) {
          return response.json();
        })
        .then(function(data) {
          console.log(this.chartdata.datasets[0]);
          this.bricks = data;
          for (let i = this.chartdata.datasets[0].data.length; i < data.length; i++) {
            this.chartdata.datasets[0].data.push({
              x: new Date(data[i]["dt"]),
              y: i
            });
          }
          this.$refs.chartElem.update();
        });
    }
  },
  watch: {
    selectDay: function(val) {
      this.updateValue();
    }
  },
  created() {
    var self = this;
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