
async function LoadData(url) {

    var formData = new FormData($(".AjaxForm").get(0));
    url = url + new URLSearchParams(formData).toString();
    return await fetch(url, {
        method: "GET"
         })
        .then(response => {
            if (response.status >= 200 && response.status < 300)
                return response.json();
            else if (response.status == 400) {
                let error = new Error("Не корректные данные были посланы на сервер.\nКод ошибки 400.");
                throw error
            }
            else {
                let error = new Error();
                alert("Соединение с сервером разорвано\nВозможно проблеммы с сервером или интернетом.");
                throw error
            }
        })
        .then(json => {
            //console.log(json);
            result = json;
            return json;
        })
        .catch(err => alert(err));

}

