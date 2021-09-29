/**
 * Theme: Crovex - Responsive Bootstrap 4 Admin Dashboard
 * Author: Mannatthemes
 * Analytics Dashboard Js
 */



var options = {
  chart: {
      height: 350,
      type: 'line',
      stacked: true,
      toolbar: {
        show: false,
        autoSelected: 'zoom'
      },
      dropShadow: {
        enabled: true,
        top: 12,
        left: 0,
        bottom: 0,
        right: 0,
        blur: 2,
        color: '#45404a2e',
        opacity: 0.35
      },
  },
  colors: ['#2a77f4', '#1ccab8', '#f02fc2'],
  dataLabels: {
      enabled: false
  },
  stroke: {
      curve: 'straight',
      width: [4, 4],
      dashArray: [0, 3]
  },
  grid: {
    borderColor: "#45404a2e",
    padding: {
      left: 0,
      right: 0
    }
  },
  markers: {
    size: 0,
    hover: {
      size: 0
    }
  },
  series: [{
      name: 'New Visits',
      data: [0,60,20,90,45,110,55,130,44,110,75,200]
  }, {
      name: 'Unique Visits',
      data: [0,45,10,75,35,94,40,115,30,105,65,190]
  }],

  

  xaxis: {
      type: 'datetime',
      categories: ["2019-09-19T00:00:00", "2019-09-19T01:30:00", "2019-09-19T02:30:00", "2019-09-19T03:30:00", "2019-09-19T04:30:00", "2019-09-19T05:30:00", "2019-09-19T06:30:00", "2019-09-19T07:30:00", "2019-09-19T08:30:00", "2019-09-19T09:30:00", "2019-09-19T10:30:00", "2019-09-19T11:30:00"],  
      axisBorder: {
        show: true,
        color: '#45404a2e',
      },  
      axisTicks: {
        show: true,
        color: '#45404a2e',
      },                  
  },

  fill: {
    type: 'gradient',
    gradient: {
      gradientToColors: ['#F55555', '#B5AC49', '#6094ea']
    },
  },
  tooltip: {
      x: {
          format: 'dd/MM/yy HH:mm'
      },
  },
  legend: {
    position: 'top',
    horizontalAlign: 'right'
  },
}

var chart = new ApexCharts(
  document.querySelector("#liveVisits"),
  options
);

chart.render();


//Mixed-2

var options = {
  chart: {
      height: 380,
      type: 'line',
      stacked: false,
      toolbar: {
          show: false
      },
      dropShadow: {
        enabled: true,
        top: 10,
        left: 0,
        bottom: 0,
        right: 0,
        blur: 2,
        color: '#45404a2e',
        opacity: 0.35
      },
  },
  dataLabels: {
      enabled: false
  },
  stroke: {
      width: [0, 0, 3]
  },
  series: [{
      name: 'Income',
      type: 'column',
      data: [1.4, 2, 2.5, 1.5, 2.5, 2.8, 3.8, 4.6]
  }, {
      name: 'Cashflow',
      type: 'column',
      data: [1.1, 3, 3.1, 4, 4.1, 4.9, 6.5, 8.5]
  }, {
      name: 'Revenue',
      type: 'line',
      data: [20, 29, 37, 36, 44, 45, 50, 58]
  }],
  colors: ["#20016c", "#77d0ba", "#fa5c7c"],
  xaxis: {
      categories: [2009, 2010, 2011, 2012, 2013, 2014, 2015, 2016],
      axisBorder: {
        show: true,
        color: '#bec7e0',
      },  
      axisTicks: {
        show: true,
        color: '#bec7e0',
    }, 
  },
  yaxis: [
      {
          axisTicks: {
              show: true,
          },
          axisBorder: {
              show: true,
              color: '#20016c'
          },
          labels: {
              style: {
                  color: '#20016c',
              }
          },
          title: {
              text: "Income (thousand crores)"
          },
      },

      {
          axisTicks: {
              show: true,
          },
          axisBorder: {
              show: true,
              color: '#77d0ba'
          },
          labels: {
              style: {
                  color: '#77d0ba',
              },
              offsetX: 10
          },
          title: {
              text: "Operating Cashflow (thousand crores)",
          },
      },
      {
          opposite: true,
          axisTicks: {
              show: true,
          },
          axisBorder: {
              show: true,
              color: '#fa5c7c'
          },
          labels: {
              style: {
                  color: '#fa5c7c',
              }
          },
          title: {
              text: "Revenue (thousand crores)"
          }
      },

  ],
  tooltip: {
      followCursor: true,
      y: {
          formatter: function (y) {
              if (typeof y !== "undefined") {
                  return y + " thousand crores"
              }
              return y;
          }
      }
  },
  grid: {
      borderColor: '#f1f3fa'
  },
  legend: {
      offsetY: -10,
  },
  responsive: [{
      breakpoint: 600,
      options: {
          yaxis: {
              show: false
          },
          legend: {
              show: false
          }
      }
  }]
}

var chart = new ApexCharts(
  document.querySelector("#apex_mixed2"),
  options
);

chart.render();



//Device-widget


var options = {
  chart: {
      height: 250,
      type: 'donut',
      dropShadow: {
        enabled: true,
        top: 10,
        left: 0,
        bottom: 0,
        right: 0,
        blur: 2,
        color: '#45404a2e',
        opacity: 0.15
    },
  }, 
  plotOptions: {
    pie: {
      donut: {
        size: '65%'
      }
    }
  },
  dataLabels: {
    enabled: false,
    },
    stroke: {
      show: true,
      width: 2,
      colors: ['transparent']
  },
 
  series: [10, 65, 25,],
  legend: {
      show: true,
      position: 'bottom',
      horizontalAlign: 'center',
      verticalAlign: 'middle',
      floating: false,
      fontSize: '14px',
      offsetX: 0,
      offsetY: -13
  },
  labels: [ "Total Orders", "Orders Completed", "Orders Cancelled"],
  colors: ["#34bfa3", "#5d78ff", "#fd3c97"],
 
  responsive: [{
      breakpoint: 600,
      options: {
        plotOptions: {
            donut: {
              customScale: 0.2
            }
          },        
          chart: {
              height: 240
          },
          legend: {
              show: false
          },
      }
  }],

  tooltip: {
    y: {
        formatter: function (val) {
            return   val + " %"
        }
    }
  }
  
}

var chart = new ApexCharts(
  document.querySelector("#ana_device"),
  options
);

chart.render();


var colors = ['#1ecab8', '#fd3c97', '#6d81f5', '#ffb822', '#0dc8de'];
var options = {
  chart: {
      height: 300,
      type: 'bar',
      events: {
        click: function(chart, w, e) {
            console.log(chart, w, e )
        }
    },
    toolbar:{
      show:false
    },
    dropShadow: {
      enabled: true,
      top: 0,
      left: 5,
      bottom: 5,
      right: 0,
      blur: 5,
      color: '#45404a2e',
      opacity: 0.35
  },
  },
  colors: colors,
  plotOptions: {
      bar: {
          dataLabels: {
              position: 'top', // top, center, bottom              
          },
          columnWidth: '30',
          distributed: true,
      },

  },
  dataLabels: {
      enabled: true,
      formatter: function (val) {
          return val + "%";
      },
      offsetY: -20,
      style: {
          fontSize: '12px',
          colors: ["#8997bd"]
      }
  },
  series: [{
      name: 'Inflation',
      data: [ 4.0, 10.1, 6.0, 8.0, 9.1]
  }],
  xaxis: {
      categories: ["Email", "Referral", "Organic", "Direct", "Campaign",],
      position: 'top',
      labels: {
          offsetY: -18,
          style: {
            cssClass: 'apexcharts-xaxis-label',
          },
      },
      axisBorder: {
          show: false
      },
      axisTicks: {
          show: false
      },
      crosshairs: {
          fill: {
              type: 'gradient',
              gradient: {
                  colorFrom: '#D8E3F0',
                  colorTo: '#BED1E6',
                  stops: [0, 100],
                  opacityFrom: 0.4,
                  opacityTo: 0.5,
              }
          }
      },
      tooltip: {
          enabled: true,
          offsetY: -37,
      }
  },
  fill: {
      gradient: {
          type: "gradient",
          gradientToColors: ['#FEB019', '#775DD0', '#FEB019', '#FF4560', '#775DD0'],
      },
  },
  yaxis: {
      axisBorder: {
          show: false
      },
      axisTicks: {
          show: false,
      },
      labels: {
          show: false,
          formatter: function (val) {
              return val + "%";
          }
      }

  },
}

var chart = new ApexCharts(
  document.querySelector("#barchart"),
  options
);

chart.render();



$('#usa').vectorMap({
  map: 'us_aea_en',
  backgroundColor: 'transparent',
  borderColor: '#818181',
  regionStyle: {
    initial: {
      fill: '#7474b126',
    }
  },
  series: {
    regions: [{
        values: {
            "US-VA": '#868ff363',
            "US-PA": '#868ff363',
            "US-TN": '#868ff363',
            "US-WY": '#868ff363',
            "US-WA": '#868ff363',
            "US-TX": '#868ff363',
        },
        attribute: 'fill',
    }]
  },
});

