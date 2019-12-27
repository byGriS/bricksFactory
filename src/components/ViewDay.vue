<template>
  <div>
      <h1>Общее кол-во блоков за день: {{countBricks}}</h1>
    <div>
        <div v-for="brick in bricks">
            <label>Время отреза:	</label>&#8195;{{brick.dt}}
        </div>
    </div>
  </div>
</template>
<script>
export default {
  props: { selectDay: "" },
  data() {
    return {
      timer: null,
      bricks: []
    };
  },
  computed:{
      countBricks(){
          return this.bricks.length * 4;
      }
  },
  methods: {
    updateValue() {
     this.$http
        .post("http://c98744oh.beget.tech/get.php",{
            dt: this.selectDay
        })
        .then(function(response){
            return response.json();
        })
        .then(function(data){
            this.bricks = data;
        })
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
};
</script>