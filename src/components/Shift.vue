<template>
  <div>
    <div>Время смены</div>
    <div>
      <label class="label50">начало:</label>
      <input class="form-control inputTime" v-model="shift.shiftStartHour" />
      <label>ч</label>
      <input class="form-control inputTime" v-model="shift.shiftStartMinute" />
      <label>мин</label>
    </div>
    <div>
      <label class="label50">конец:</label>
      <input class="form-control inputTime" v-model="shift.shiftEndHour" />
      <label>ч</label>
      <input class="form-control inputTime" v-model="shift.shiftEndMinute" />
      <label>мин</label>
    </div>
  </div>
</template>

<script>
export default {
  props: ["shift"]
};
</script>

<style scoped>
.label50{
  width:50px;
  margin-top:15px;
}
.inputTime{
  display: inline-block;
  width: 40px;
  margin: 0 10px;
}
</style>