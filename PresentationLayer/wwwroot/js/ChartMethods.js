
function ParseData(json) {
    let labels = [];
    let points = [];
    let sum = 0;
    for (let i = 0; i < json.length; i++) {
        sum += json[i].x;
    }
    for (let i = 0; i < json.length; i++) {
        labels[i] = json[i].description + " (" + Math.round((json[i].x/sum)*100).toString() +"%)" ;
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
            hoverOffset: 100
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
