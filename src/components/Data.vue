<template>
  <div class="container">
    <router-link to="main" class="btn btn-warning">Назад</router-link>
    <div>
      <div class="d-flex align-items-center flex-wrap mt-2">
        <label class="m-1">Cмены</label>
        <select class="form-control form-control-sm selectStages m-1" v-model="selectShiftId">
          <option
            v-for="elem in shifts"
            v-bind:key="elem.id"
            :value="elem.id"
          >Смена {{getIndex(elem)}}</option>
        </select>
        <button class="btn btn-sm btn-danger m-1" @click="deleteShift">Удалить</button>
        <button class="btn btn-sm btn-success m-1" @click="addShift">Добавить</button>
        <button class="btn btn-sm btn-primary m-1" @click="changeShift">{{btnChange}}</button>
      </div>
      <div>
        <div>
          <label class="label50 marginR10">начало:</label>
          <input
            v-if="editting"
            class="form-control inputTime"
            v-model="selectedShift.shiftStartHour"
          />
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
          <input
            v-if="editting"
            class="form-control inputTime"
            v-model="selectedShift.shiftEndHour"
          />
          <label v-else>{{selectedShift.shiftEndHour}}</label>
          <label class="marginR10">ч</label>
          <input
            v-if="editting"
            class="form-control inputTime"
            v-model="selectedShift.shiftEndMinute"
          />
          <label v-else>{{selectedShift.shiftEndMinute}}</label>
          <label>мин</label>
        </div>
        <div>
          <label class="marginR10">норматив выработки в час:</label>
          <input v-if="editting" class="form-control inputTime" v-model="selectedShift.norm" />
          <label v-else>{{selectedShift.norm}}</label>
          <label class="marginR10">шт</label>
        </div>
      </div>
    </div>

    <div>
      <div class="d-flex align-items-center flex-wrap mt-2">
        <label class="m-1">Блоки</label>
        <select class="form-control form-control-sm selectStages m-1" v-model="selectedTypeBlocks">
          <option v-for="elem in typesBlock" v-bind:key="elem.id" :value="elem">{{elem.title}}</option>
        </select>
        <button class="btn btn-sm btn-danger m-1" @click="deleteTypeBlock">Удалить</button>
        <button class="btn btn-sm btn-success m-1" @click="addTypeBlock">Добавить</button>
        <button class="btn btn-sm btn-primary m-1" @click="changeTypeBlock">{{btnChangeBlocks}}</button>
      </div>
      <div>
        <div>
          <label class="label100 marginR10">название:</label>
          <input
            v-if="edittingBlocks"
            class="form-control inputTitle"
            v-model="selectedTypeBlocks.title"
          />
          <label v-else>{{selectedTypeBlocks.title}}</label>
        </div>
        <div>
          <label class="label100 marginR10">кол-во за отрез:</label>
          <input
            v-if="edittingBlocks"
            class="form-control inputTime"
            v-model="selectedTypeBlocks.countBticks"
          />
          <label v-else>{{selectedTypeBlocks.countBticks}}</label>
        </div>
      </div>
    </div>

    <div>
      <h4>Цены за материалы</h4>
      <div>
        <label class="label200 marginR10">электроэнергия, за 1 кВт, руб:</label>
        <input class="form-control inputPrice" v-model="mercuriyAPrice" />
      </div>
      <div>
        <label class="label200 marginR10">вода, за 1 м3, руб:</label>
        <input class="form-control inputPrice" v-model="pulsarPrice" />
      </div>
      <div>
        <label class="label200 marginR10">цемент, за 1 кг, руб:</label>
        <input class="form-control inputPrice" v-model="cementPrice" />
      </div>
      <div>
        <label class="label200 marginR10">щебень, за 1 кг, руб:</label>
        <input class="form-control inputPrice" v-model="clayPrice" />
      </div>
      <div>
        <label class="label200 marginR10">песок, за 1 кг, руб:</label>
        <input class="form-control inputPrice" v-model="sandPrice" />
      </div>
      <button class="btn btn-sm btn-primary" @click="savePrice()">Сохранить цены</button>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      shifts: [{ id: 1 }, { id: 2 }],
      selectShiftId: 1,
      editting: false,

      typesBlock: [{ id: 1, title: "Перегородка", countBticks: 4 }],
      selectedTypeBlocks: { id: 1, title: "Перегородка", countBticks: 4 },
      edittingBlocks: false,

      mercuriyAPrice: 0,
      pulsarPrice: 0,
      cementPrice: 0,
      clayPrice: 0,
      sandPrice: 0,
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
    btnChange() {
      if (this.editting) return "Сохранить";
      else return "Изменить";
    },
    btnChangeBlocks() {
      if (this.edittingBlocks) return "Сохранить";
      else return "Изменить";
    },
    isAdmin() {
      return this.$store.state.isAdmin;
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
        .post(this.$store.state.host + "addShift.php", {
          shiftStartHour: this.selectedShift.shiftStartHour,
          shiftStartMinute: this.selectedShift.shiftStartMinute,
          shiftEndHour: this.selectedShift.shiftEndHour,
          shiftEndMinute: this.selectedShift.shiftEndMinute,
          norm: this.selectedShift.norm
        })
        .then(function (response) {
          this.updateListShifts(true);
        });
    },
    deleteShift() {
      if (this.shifts.length == 1) {
        alert("Кол-во смен: 1. Нельзя удалять все смены");
        return;
      }
      if (confirm("Точно удалить смену?")) {
        this.$http
          .post(this.$store.state.host + "deleteShift.php", {
            id: this.selectedShift.id
          })
          .then(function (response) {
            this.selectShiftId = 1;
            this.updateListShifts();
          });
      }
    },
    changeShift() {
      this.editting = !this.editting;
      if (this.editting) return;
      if (this.selectedShift.shiftEndHour >= 24) {
        this.selectedShift.shiftEndHour = 23;
        this.selectedShift.shiftEndMinute = 59;
      }
      this.$http
        .post(this.$store.state.host + "changeShift.php", {
          shiftStartHour: this.selectedShift.shiftStartHour,
          shiftStartMinute: this.selectedShift.shiftStartMinute,
          shiftEndHour: this.selectedShift.shiftEndHour,
          shiftEndMinute: this.selectedShift.shiftEndMinute,
          norm: this.selectedShift.norm,
          id: this.selectedShift.id
        })
        .then(function (response) {
          this.updateListShifts();
          this.$store.state.shift = this.selectedShift;
          this.$store.state.norm = this.selectedShift.norm;
        });
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
    addTypeBlock() {
      this.$http
        .post(this.$store.state.host + "addBrickShift.php", {
          title: 'Новый тип',
          countBricks: 4
        })
        .then(function (response) {
          this.updateListTypesBlock(true);
        });
    },
    deleteTypeBlock() {
      if (this.typesBlock.length == 1) {
        alert("Кол-во типов блоков: 1. Нельзя удалять все типы");
        return;
      }
      if (confirm("Точно удалить тип блока?")) {
        this.$http
          .post(this.$store.state.host + "deleteBrickType.php", {
            id: this.selectedTypeBlocks.id
          })
          .then(function (response) {
            this.updateListTypesBlock();
          });
      }
    },
    changeTypeBlock() {
      this.edittingBlocks = !this.edittingBlocks;
      if (this.edittingBlocks) return;
      if (this.selectedTypeBlocks.countBticks <= 0) {
        alert("Кол-во блоков за отрез должно быть больше нуля");
        return;
      }
      this.$http
        .post(this.$store.state.host + "changeBrickType.php", {
          id: this.selectedTypeBlocks.id,
          title: this.selectedTypeBlocks.title,
          countbricks: this.selectedTypeBlocks.countBticks
        })
        .then(function (response) {
          this.updateListTypesBlock();
        })
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
          this.selectedTypeBlocks = this.typesBlock[0];
          if (newType)
            this.selectedTypeBlocks = this.typesBlock[this.typesBlock.length - 1];
        });
    },
    updatePrice() {
      this.$http
        .get(this.$store.state.host + "getPrice.php")
        .then(function (response) {
          return response.json();
        })
        .then(function (data) {
          data = data['prices'][0];
          this.mercuriyAPrice = data['mercuriyAPrice'];
          this.pulsarPrice= data['pulsarPrice'];
          this.cementPrice= data['cementPrice'];
          this.clayPrice= data['clayPrice'];
          this.sandPrice= data['sandPrice'];
        });
    },
    savePrice() {
      if (this.mercuriyAPrice <= 0 || this.pulsarPrice <= 0 || this.cementPrice <= 0 || this.clayPrice <= 0 || this.sandPrice <= 0) {
        alert("Цены должны быть больше нуля и кратны единицы");
        return;
      }
      this.$http
        .post(this.$store.state.host + "changePrice.php", {
          mercuriyAPrice: this.mercuriyAPrice,
          pulsarPrice: this.pulsarPrice,
          cementPrice: this.cementPrice,
          clayPrice: this.clayPrice,
          sandPrice: this.sandPrice,
        })
    }
  },
  mounted() {
    this.updateListShifts();
    this.updateListTypesBlock();
    this.updatePrice();
  }
}
</script>

<style scoped>
.selectStages {
  width: 125px;
}
.marginR10 {
  margin-right: 10px;
  margin-left: 10px;
}
.label50 {
  width: 50px;
  margin-top: 5px;
  margin-bottom: 5px;
}
.label100 {
  width: 125px;
  margin-top: 5px;
  margin-bottom: 5px;
}
.label200 {
  width: 220px;
  margin-top: 5px;
  margin-bottom: 5px;
}
.inputTime {
  display: inline-block;
  width: 40px;
  margin: 0 10px;
  padding: 5px;
  text-align: center;
}
.inputTitle {
  display: inline-block;
  width: 200px;
  margin: 0 10px;
  padding: 5px;
  text-align: center;
}
.inputPrice {
  display: inline-block;
  width: 100px;
  margin: 0 10px;
  padding: 5px;
  text-align: center;
}
</style>