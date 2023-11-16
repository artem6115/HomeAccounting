
function GetDatePie(DataSet) {
    let labels = [];
    let points = [];
    let sum = 0;
    for (let i = 0; i < DataSet.length; i++) {
        sum += DataSet[i].x;
    }
    for (let i = 0; i < DataSet.length; i++) {
        labels[i] = DataSet[i].description + " (" + Math.round((DataSet[i].x/sum)*100).toString() +"%)" ;
        points[i] = DataSet[i].x;


    }


    let data = {
        labels: labels,
        datasets: [{
            label: 'Сумма',
            data :points,
            hoverOffset: 100
        }],
    };
    return data;
}

function GetDateBar(DataSet,GroupMonth = false) {
    let labels = [];
    let points = [];
    let months = ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"];
    for (let i = 0; i < DataSet.length; i++) {
        if (GroupMonth)
            labels[i] = months[DataSet[i].x-1];
        else
            labels[i] = DataSet[i].x;
        points[i] = DataSet[i].y;
    }


    let data = {
        labels: labels,
        datasets: [{
            label: 'Количество ',
            data: points,
        }],
    };
    return data;
}


function Update() {

    document.getElementById('canvas-' + CurrentCanvas).remove();
  

    let cnvd = document.getElementById("canvas-Div-" + CurrentCanvas)

    let cnv = document.createElement('canvas');
    cnv.id = 'canvas-' + CurrentCanvas;
    cnvd.append(cnv);
    return cnv;
}    
//function ParseData(json) {
//    let labels = [];
//    let data = [];
//    for (let i = 0; i < json.length; i++) {
//        labels[i] = json[i].Description;
//        data[i] = json[i].X;

//    }
//    let data = {
//        labels: labels,
//        datasets: [{
//            label: 'Сумма',
//            data: data,

//            hoverOffset: 4
//        }]
//    };
//    return data;
//}
