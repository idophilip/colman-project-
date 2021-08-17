$(document).ready(function () {
    var canvas = document.getElementById("title");
    var ctx = canvas.getContext("2d");
    ctx.font = "30px Arial";
    ctx.strokeStyle = "black";
    ctx.strokeText("TaskMan", 10, 50);
});

const API_KEY = '0dd9af9e62914962bee54928200709';
let isHot;
let isDay;

$.get(`https://api.weatherapi.com/v1/current.json?key=${API_KEY}&q=Tel-Aviv`, (data, status) => {
    if (data.current.temp_c > 22) {
        isHot = true;
    } else {
        isHot = false;
    }

    $('#weather-img').attr('src', 'https:' + data.current.condition.icon);
    $('#weather').html(`${data.current.temp_c}°C `)

    if (data.current.is_day === 1) {
        isDay = false;
        prefixString = 'Good Day, '
    } else {
        isDay = true;
        prefixString = 'Good Night, '
    }

    $('#greetings').html(`${prefixString}${$('#greetings').html()}`);
});

function deleteGeneric(name, formSubmit='') {
    swal({
        title: "Are you sure?",
        text: `Once deleted, you will not be able to recover this ${name}!`,
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                if (formSubmit != '') {
                    $(`#delete-${name}-${formSubmit}`).submit();
                } else {
                    $(`#delete-${name}`).submit();
                }

                swal(`${name} has been deleted successfully`, {
                    icon: "success",
                });

            } else {
                swal("The action has been aborted.");
            }
        });
}