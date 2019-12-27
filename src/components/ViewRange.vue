<template>
    <div>
      <h1>Общее кол-во блоков за период: {{countBricks}}</h1>
    </div>
</template>
<script>
export default {
    props: { range: "" },
    data() {
    return {
      timer: null,
      bricks: 0
    };
  },
  computed:{
      countBricks(){
          return this.bricks * 4;
      }
  },
  methods: {
    updateValue() {
     this.$http
        .post("http://c98744oh.beget.tech/range.php",{
            dt1: this.range.start,
            dt2: this.range.end
        })
        .then(function(response){
             this.bricks = response.data;
        });
    }
  },
  watch:{
    selectDay: function(val){
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
}
</script>