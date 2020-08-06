<template>
  <div>
    <h4>Затраты</h4>
    <div>
      <label class="label150 marginR10">Эл. энергия, кВт</label>
      <label>
        {{mercuriy}}
        <span v-if="isAdmin">({{mercuriyCost}} руб.)</span>
      </label>
    </div>
    <div>
      <label class="label150 marginR10">Вода, м3</label>
      <label>
        {{pulsar}}
        <span v-if="isAdmin">({{pulsarCost}} руб.)</span>
      </label>
    </div>
    <div>
      <label class="label150 marginR10">Цемент, кг</label>
      <label>
        {{cement}}
        <span v-if="isAdmin">({{cementCost}} руб.)</span>
      </label>
    </div>
    <div>
      <label class="label150 marginR10">Керамзит, кг</label>
      <label>
        {{clay}}
        <span v-if="isAdmin">({{clayCost}} руб.)</span>
      </label>
    </div>
    <div>
      <label class="label150 marginR10">Песок, кг</label>
      <label>
        {{sand}}
        <span v-if="isAdmin">({{sandCost}} руб.)</span>
      </label>
    </div>
    <div v-if="isAdmin">
      <label class="label150 marginR10">Общие расходы</label>
      <label>{{totalCost}}</label>
    </div>
    <div v-if="isAdmin">
      <label class="label150 marginR10">Цена за 1 блок</label>
      <label>{{blockCost}}</label>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {}
  },
  computed: {
    isAdmin() {
      return this.$store.state.isAdmin;
    },
    mercuriy() {
      return this.$store.state.mercuriyA / 1000.0;
    },
    pulsar() {
      return this.$store.state.pulsar;
    },
    cement() {
      return this.$store.state.cement;
    },
    clay() {
      return this.$store.state.clay;
    },
    sand() {
      return this.$store.state.sand;
    },
    mercuriyCost() {
      return (this.$store.state.mercuriyA / 1000.0 * this.$store.state.mercuriyAPrice).toFixed(2);
    },
    pulsarCost() {
      return (this.$store.state.pulsar * this.$store.state.pulsarPrice).toFixed(2);
    },
    cementCost() {
      return (this.$store.state.cement * this.$store.state.cementPrice).toFixed(2);
    },
    clayCost() {
      return (this.$store.state.clay * this.$store.state.clayPrice).toFixed(2);
    },
    sandCost() {
      return (this.$store.state.sand * this.$store.state.sandPrice).toFixed(2);
    },
    totalCost() {
      return parseFloat(this.mercuriyCost) + parseFloat(this.pulsarCost) + parseFloat(this.cementCost) + parseFloat(this.clayCost) + parseFloat(this.sandCost);
    },
    blockCost(){
      if (this.$store.state.totalBlocks * this.$store.state.countBlocks == 0)
        return 0;
      return (this.totalCost / (this.$store.state.totalBlocks * this.$store.state.countBlocks)).toFixed(2);
    },
    countBlocks(){
      return this.$store.state.countBlocks;
    }
  }
}
</script>

<style scoped>
.label150 {
  width: 125px;
}
</style>