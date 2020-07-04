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
    <h2>Общее кол-во резов за период: {{countBricks}}</h2>
  </div>
</template>
<script>
export default {
  props: { range: "" },
  data() {
    return {
      timer: null,
      bricks: 0,
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
      return this.mercuriyA;
    }
  },
  methods: {
    updateValue() {
      this.$http
        .post(this.$store.state.host + "range.php", {
          dt1: this.range.start,
          dt2: this.range.end
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
          let counters = data.counters;
          if (counters.length > 0) {
            this.mercuriyA = counters[counters.length - 1]["mercuriyA"] - counters[0]["mercuriyA"];
            this.mercuriyR = counters[counters.length - 1]["mercuriyR"] - counters[0]["mercuriyR"];
            this.pulsar = counters[counters.length - 1]["pulsar"] - counters[0]["pulsar"];
          }

          let tenzo = data.tenzo;
          if (tenzo.length > 0) {
            this.cement = tenzo.reduce((sum, elem) => { return parseInt(sum) + parseInt(elem["tenzo"])-6 }, 0);
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
      this.updateValue();
    }
  },
  created() {
    var self = this;
    this.timer = setInterval(function () {
      self.updateValue();
    }, 3000);
    self.updateValue();
  },
  beforeDestroy() {
    clearInterval(this.timer);
  }
}
</script>