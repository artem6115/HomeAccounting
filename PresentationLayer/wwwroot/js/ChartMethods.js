
function ParseData(json) {
    let labels = [];
    let points = [];
    for (let i = 0; i < json.length; i++) {
        labels[i] = json[i].description;
        points[i] = json[i].x;
        console.log(json[i]);


    }
    console.log(labels);
    console.log(points);

    let data = {
        labels: labels,
        datasets: [{
            label: 'Сумма',
            data :points,
        }],
    };
    return data;
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
