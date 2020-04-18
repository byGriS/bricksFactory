import { Line, mixins  } from 'vue-chartjs'
const { reactiveProp } = mixins

export default {
  extends: Line,
  mixins: [reactiveProp],
  data: () => ({
    options: {
      legend: {
        display: false
      },
      scales: {
        yAxes: [{
          ticks: {
            min: 0,
          }
        }],
        xAxes: [{
          type: 'time',
          time: {
            unit: 'minute',
            displayFormats: {
              minute: 'H:mm'
            }
          }
        }]
      }
    }
  }),
  methods:{
    update(){
      this.renderChart(this.chartData, this.options);
    }
  },
  mounted() {
    console.log("op");
    this.renderChart(this.chartdata, this.options);
  }
}