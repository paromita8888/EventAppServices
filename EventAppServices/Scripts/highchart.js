 $(function () {
     //service call
     var _data = [];
     var _chart;
     setInterval(function () {
         $.ajax({
             type: "GET",
             url: "http://techvantage2016live.azurewebsites.net/Api/UserAttendances/StallReport",
             dataType: "json",
             success: function (data) {
                 _data = [];
                 var totalCount = 0; 
                 $.each(data, function (item) { 
                     _data.push({ name: data[item].StallName, x: item, y: data[item].Count });
                     totalCount = totalCount + data[item].Count; 
                 });
                 console.log(_data);

                 // Create the chart
                 if (_chart == undefined) {
                     $('#employee_list_bar').highcharts({
                         chart: {
                             type: 'column',
                             height: 550
                         },
                         title: {
                             text: 'Stall Attendee Dashboard'
                         },
                         xAxis: {
                             title: {
                                 text: 'Stalls'
                             },
                             categories: []
                         },
                         yAxis: {
                             title: {
                                 text: 'No. of Visitors'
                             }
                         },
                         legend: {
                             enabled: false
                         },
                         plotOptions: {
                             series: {
                                 borderWidth: 0,
                                 dataLabels: {
                                     enabled: true,
                                     format: '{point.y:1f}'
                                 }
                             }
                         },
                         Sleep:2000,
                         tooltip: {
                             pointFormat: '<span style="color:{point.color}">{point.StallName}</span>: <b>{point.y:2f}</b> of <b>' + totalCount + '</b><br/>'
                         }, 
                         series: [{
                             colorByPoint: true,
                             data: _data
                         }]
                     });
                     _chart = $('#employee_list_bar').highcharts();
                 } else {
                     _chart = $('#employee_list_bar').highcharts();
                     _chart.series[0].setData(_data, true);
                 }
             },
             error: function (data) { 
             }
         });
     }, 6000)
 });

