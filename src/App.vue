<template>
  <div>
    <nav class="navbar navbar-expand navbar-dark bg-dark sticky-top">
      <div class="container">
        <div class="collapse navbar-collapse" id="navbarSupportedContent">
          <div class="navbar-nav mr-auto">
            <a class="navbar-brand" :class="error?'fault':'ok'" href="/">Производство</a>
          </div>
          <div class>
            <router-link  v-if="isAdmin" to="data" class="mr-3 btn btn-info">База данных</router-link>
            <button v-if="!isAdmin" class="btn btn-success" @click="toAdmin()">Админ</button>
            <button v-if="isAdmin" class="btn btn-primary" @click="toStaff()">Сотрудник</button>
          </div>
        </div>
      </div>
    </nav>
  <router-view></router-view>
    
  </div>
</template>

<script>
import VRangeSelector from "vuelendar/components/vl-range-selector";
import VDaySelector from "vuelendar/components/vl-day-selector";
import Shift from "./components/Shift";
import ViewShift from "./components/ViewShift"
import ViewDay from "./components/ViewDay";
import ViewRange from "./components/ViewRange";

export default {
  components: {
  },
  data() {
    return {
      timer: null,
      error: true,
      wd: -1
    };
  },
  computed: {
    isAdmin() {
      return this.$store.state.isAdmin;
    }
  },
  methods: {
    watchdog() {
      this.$http
        .post(this.$store.state.host + "getTest.php", {
        })
        .then(function (response) {
          return response.json();
        })
        .then(function (data) {
          if (this.wd == -1) {
            this.wd = data['test'][0]['test'];
          } else {
            if (data['test'][0]['test'] != this.wd) {
              this.error = false;
              this.wd = data['test'][0]['test'];
            } else {
              this.error = true;
            }
          }
        })
    },
    toAdmin() {
      let pass = prompt("Введите пароль");
      if (pass == "") return;

      this.$http.post(this.$store.state.host + "checkAdmin.php", {
        password: pass
      })
        .then(function (response) {
          if (response.data == 1)
            this.$store.state.isAdmin = true;
        })
    },
    toStaff() {
      this.$store.state.isAdmin = false;
    }
  },
  created() {
    var self = this;
    this.timer = setInterval(function () {
      self.watchdog();
    }, 10000);
    self.watchdog();
  },
  beforeDestroy() {
    clearInterval(this.timer);
  }
};
</script>

<style lang="scss">
@import "vuelendar/scss/vuelendar.scss";
nav {
  margin-bottom: 10px;
}
.ok {
  color: rgb(221, 255, 221) !important;
}
.fault {
  color: rgb(255, 199, 199) !important;
}
</style>
