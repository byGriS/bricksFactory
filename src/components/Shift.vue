<template>
  <div>
    <div>Cмены</div>
    <select class="form-control selectInline" v-model="selectShiftId">
      <option v-for="elem in shifts" v-bind:key="elem.id" :value="elem.id">Смена {{getIndex(elem)}}</option>
    </select>
    <button class="btn btn-success" @click="addShift">Добавить</button>
    <button class="btn btn-danger" @click="deleteShift">Удалить</button>
    <div>
      <label class="label50 marginR10">начало:</label>
      <input v-if="editting" class="form-control inputTime" v-model="selectedShift.shiftStartHour" />
      <label v-else>{{selectedShift.shiftStartHour}}</label>
      <label class="marginR10">ч</label>
      <input
        v-if="editting"
        class="form-control inputTime"
        v-model="selectedShift.shiftStartMinute"
      />
      <label v-else>{{selectedShift.shiftStartMinute}}</label>
      <label>мин</label>
    </div>
    <div>
      <label class="label50 marginR10">конец:</label>
      <input v-if="editting" class="form-control inputTime" v-model="selectedShift.shiftEndHour" />
      <label v-else>{{selectedShift.shiftEndHour}}</label>
      <label class="marginR10">ч</label>
      <input v-if="editting" class="form-control inputTime" v-model="selectedShift.shiftEndMinute" />
      <label v-else>{{selectedShift.shiftEndMinute}}</label>
      <label>мин</label>
    </div>
    <button class="btn btn-primary marginT10" @click="changeShift">{{btnChange}}</button>
  </div>
</template>

<script>
export default {
  data() {
    return {
      shifts: [{ id: 1 }, { id: 2 }],
      selectShiftId: 1,
      editting: false
    };
  },
  computed: {
    selectedShift() {
      for (let i = 0; i < this.shifts.length; i++) {
        if (this.shifts[i].id == this.selectShiftId) return this.shifts[i];
      }
    },
    btnChange() {
      if (this.editting) return "Сохранить";
      else return "Изменить время смены";
    }
  },
  watch: {
    selectShiftId() {
      this.$store.state.shift = this.selectedShift;
    }
  },
  methods: {
    getIndex(elem) {
      for (var i = 0; i < this.shifts.length; i++) {
        if (this.shifts[i].id == elem.id) return i + 1;
      }
    },
    addShift() {
      this.$http
        .post("http://c98744oh.beget.tech/addShift.php", {
          shiftStartHour: this.selectedShift.shiftStartHour,
          shiftStartMinute: this.selectedShift.shiftStartMinute,
          shiftEndHour: this.selectedShift.shiftEndHour,
          shiftEndMinute: this.selectedShift.shiftEndMinute
        })
        .then(function(response) {
          this.updateListShifts(true);
        });
    },
    deleteShift() {
      if (this.shifts.length == 1) {
        alert("Кол-во смен: 1. Нельзя удалять все смены");
        return;
      }
      this.$http
        .post("http://c98744oh.beget.tech/deleteShift.php", {
          id: this.selectedShift.id
        })
        .then(function(response) {
          this.selectShiftId = 1;
          this.updateListShifts();
        });
    },
    changeShift() {
      this.editting = !this.editting;
      if (this.selectedShift.shiftEndHour>=24){
        this.selectedShift.shiftEndHour = 23;
        this.selectedShift.shiftEndMinute = 59;
      }
      this.$http
        .post("http://c98744oh.beget.tech/changeShift.php", {
          shiftStartHour: this.selectedShift.shiftStartHour,
          shiftStartMinute: this.selectedShift.shiftStartMinute,
          shiftEndHour: this.selectedShift.shiftEndHour,
          shiftEndMinute: this.selectedShift.shiftEndMinute,
          id: this.selectedShift.id
        })
        .then(function(response) {
          this.updateListShifts();
          this.$store.state.shift = this.selectedShift;
        });
    },
    updateListShifts(newShift = false) {
      this.$http
        .post("http://c98744oh.beget.tech/getShift.php")
        .then(function(response) {
          return response.json();
        })
        .then(function(data) {
          this.shifts = [];
          for (let i = 0; i < data.length; i++) {
            this.shifts.push({
              id: data[i].id,
              shiftStartHour: data[i].starthour,
              shiftStartMinute: data[i].startminute,
              shiftEndHour: data[i].endhour,
              shiftEndMinute: data[i].endminute
            });
          }
          if (newShift)
            this.selectShiftId = this.shifts[this.shifts.length - 1].id;
        });
    }
  },
  mounted() {
    this.updateListShifts();
  }
};
</script>

<style scoped>
.marginR10 {
  margin-right: 10px;
}
.marginT10 {
  margin: 10px 0 !important;
}
.label50 {
  width: 50px;
  margin-top: 15px;
  margin-right: 10px;
}
.inputTime {
  display: inline-block;
  width: 40px;
  margin: 0 10px;
  padding: 5px;
  text-align: center;
}
.selectInline {
  display: inline-block;
  width: 100px;
  padding: 5px;
}
.btn {
  margin: 0 5px;
}
</style>