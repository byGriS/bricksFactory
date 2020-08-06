<template>
  <div>
    <h2 v-if="loading">Загрузка данных ...</h2>
    <h2>Общее кол-во резов за период: {{countBricks}}</h2>
  </div>
</template>
<script>
export default {
  data() {
    return {
      timer: null,
      bricks: 0,
      mercuriyA: 0,
      mercuriyR: 0,
      pulsar: 0,
      cement: 0,
      clay: 0,
      sand: 0,
      loading: false
    };
  },
  computed: {
    countBricks() {
      return this.bricks.length;
    },
    mercuriy() {
      return this.mercuriyA / 1000.0;
    },
    range(){
      return this.$store.state.range;
    }
  },
  methods: {
    updateValue() {
      this.loading = true;
      this.$http
        .post(this.$store.state.host + "range.php", {
          dt1: this.range.start,
          dt2: this.range.end
        })
        .then(function (response) {
          return response.json();
        })
        .then(function (data) {
          this.$store.state.mercuriyA = 0;
          //this.$store.state.mercuriyR = 0;
          this.$store.state.pulsar = 0;
          this.$store.state.cement = 0;
          this.$store.state.clay = 0;
          this.$store.state.sand = 0;
          this.bricks = data.bricks;
          let counters = data.counters;
          if (counters.length > 0) {
            this.$store.state.mercuriyA = counters[counters.length - 1]["mercuriyA"] - counters[0]["mercuriyA"];
            //this.$store.state.mercuriyR = counters[counters.length - 1]["mercuriyR"] - counters[0]["mercuriyR"];
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
          this.loading = false;
        });
      this.$http.post(this.$store.state.host + "getShiftsDataDay.php", {
        dt: this.range.start
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
    range(){
      this.updateValue();
    }
  }
}
</script>