<template>
  <div>
    <div>
      Cмены
      <select class="form-control form-control-sm selectInline" v-model="selectShiftId">
        <option
          v-for="elem in shifts"
          v-bind:key="elem.id"
          :value="elem.id"
        >Смена {{getIndex(elem)}}</option>
      </select>
    </div>
    <div class="row">
      <div class="col-md-12">
        <div>
          <label class="label50 marginR10">начало:</label>
          <label>{{selectedShift.shiftStartHour}} : {{getMinutesFormat(selectedShift.shiftStartMinute)}}</label>
        </div>
        <div>
          <label class="label50 marginR10">конец:</label>
          <label>{{selectedShift.shiftEndHour}} : {{getMinutesFormat(selectedShift.shiftEndMinute)}}</label>
        </div>
        <div>
          <label class="label50 marginR10">норматив:</label>
          <label>{{selectedShift.norm}}</label>
          <label class="marginR10">шт/ч</label>
        </div>
        <div>
          <label class="label50 marginR10">тип блока:</label>
          <select
            v-if="edittingBlocks && isAdmin"
            class="form-control form-control-sm selectInline"
            v-model="selectedTypeBlocks"
          >
            <option v-for="elem in typesBlock" v-bind:key="elem.id" :value="elem">{{elem.title}}</option>
          </select>
          <label v-else class="label100 marginR10">{{selectedTypeBlocks.title}}</label>

          <button
            v-if="isAdmin"
            class="btn btn-sm btn-primary"
            @click="changeTypeBlock()"
          >{{btnTypeText}}</button>
          <button
            v-if="edittingBlocks"
            class="btn btn-sm btn-warning"
            @click="disChangeTypeBlock()"
          >Отмена</button>
        </div>
        <label class="label150 marginR10">кол-во блоков за отрез:</label>
        <label>{{selectedTypeBlocks.countBticks}}</label>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      shifts: [{ id: 1 }, { id: 2 }],
      selectShiftId: 1,

      typesBlock: [{ id: 1, title: "Перегородка", countBticks: 4 }],
      selectedTypeBlocks: { id: 1, title: "Перегородка", countBticks: 4 },
      edittingBlocks: false,

    };
  },
  computed: {
    selectedShift() {
      for (let i = 0; i < this.shifts.length; i++) {
        if (this.shifts[i].id == this.selectShiftId) return this.shifts[i];
      }
      return {
        id: 1,
        shiftStartHour: 0,
        shiftStartMinute: 0,
        shiftEndHour: 23,
        shiftEndMinute: 59,
        norm: 100
      };
    },
    isAdmin() {
      return this.$store.state.isAdmin;
    },
    btnTypeText() {
      return this.edittingBlocks ? "Сохранить" : "Изменить";
    },
    date() {
      return this.$store.state.date;
    }
  },
  watch: {
    selectShiftId() {
      this.$store.state.shift = this.selectedShift;
      this.getCurrentTypeBlock();
    },
    date() {
      this.getCurrentTypeBlock();
    }
  },
  methods: {
    getMinutesFormat(input) {
      if (input != null)
        return input.toString().padStart(2, "0");
    },
    getIndex(elem) {
      for (var i = 0; i < this.shifts.length; i++) {
        if (this.shifts[i].id == elem.id) return i + 1;
      }
    },
    updateListShifts(newShift = false) {
      this.$http
        .post(this.$store.state.host + "getShift.php")
        .then(function (response) {
          return response.json();
        })
        .then(function (data) {
          this.shifts = [];
          for (let i = 0; i < data.length; i++) {
            this.shifts.push({
              id: data[i].id,
              shiftStartHour: data[i].starthour,
              shiftStartMinute: data[i].startminute,
              shiftEndHour: data[i].endhour,
              shiftEndMinute: data[i].endminute,
              norm: data[i].norm
            });
          }
          this.selectShiftId = this.shifts[0].id;
          if (newShift)
            this.selectShiftId = this.shifts[this.shifts.length - 1].id;
        });
    },
    updateListTypesBlock(newType = false) {
      this.$http
        .get(this.$store.state.host + "getBrickType.php")
        .then(function (response) {
          return response.json();
        })
        .then(function (data) {
          data = data['brickType'];
          this.typesBlock = [];
          for (let i = 0; i < data.length; i++) {
            this.typesBlock.push({
              id: data[i].id,
              title: data[i].title,
              countBticks: data[i].countbricks
            });
          }
          this.getCurrentTypeBlock();
        });
    },
    getCurrentTypeBlock() {
      this.$http
        .post(this.$store.state.host + "getShiftsData.php", {
          dt: this.date,
          shiftStartHour: this.selectedShift.shiftStartHour
        })
        .then(function (response) {
          return response.json();
        })
        .then(function (data) {
          if (data.length != 0) {
            this.selectedTypeBlocks = null;
            this.typesBlock.forEach(element => {
              if (element.title == data[0]['bricktitle'] && element.countBticks == data[0]['brickcount']) {
                this.selectedTypeBlocks = element;
                this.$store.state.countBlocks = this.selectedTypeBlocks.countBticks;
              }
            });
            if (this.selectedTypeBlocks == null) {
              this.selectedTypeBlocks = { title: data[0]['bricktitle'],  countBticks:data[0]['brickcount'] }
              this.$store.state.countBlocks = data[0]['brickcount'];
            }
          } else {
            this.selectedTypeBlocks = this.typesBlock[0];
            this.$store.state.countBlocks = this.selectedTypeBlocks.countBticks;
          }

        });
    },
    changeTypeBlock() {
      if (this.$store.state.date == null)
        return;
      if (this.edittingBlocks) {
        this.$http
          .post(this.$store.state.host + "setTypeBlockShift.php", {
            dt: this.$store.state.date,
            shiftStartHour: this.selectedShift.shiftStartHour,
            shiftStartMinute: this.selectedShift.shiftStartMinute,
            shiftEndHour: this.selectedShift.shiftEndHour,
            shiftEndMinute: this.selectedShift.shiftEndMinute,
            brickTitle: this.selectedTypeBlocks.title,
            brickCount: this.selectedTypeBlocks.countBticks
          })
          .then(function (response) {
            this.$store.state.countBlocks = this.selectedTypeBlocks.countBticks;
          })
      }
      this.edittingBlocks = !this.edittingBlocks;

    },
    disChangeTypeBlock(){
      this.edittingBlocks = false;
    }
  },
  mounted() {
    this.updateListShifts();
    this.updateListTypesBlock();
  }
};
</script>

<style scoped>
.marginR10 {
  margin-right: 10px;
  margin-left: 10px;
}
.marginT10 {
  margin: 10px 0 !important;
}
.label50 {
  width: 100px;
}
.label150 {
  width: 180px;
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
  width: 150px;
  padding: 5px;
}
.btn {
  margin: 0 5px;
}
</style>