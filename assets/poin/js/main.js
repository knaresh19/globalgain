
$('.number-normal .data').rollNumber({
    number: 123456,
    fontStyle: {
        'font-size': 100,
        color: '#FF0000'
    }
});

$diy = $('.number-diy .data');
$total_target = $('.number-diy2 .data');
$total_saving = $('.number-diy3 .data');

//rolling_number();
//rolling_total_target();
//rolling_total_saving();

//setInterval(function () {
    //rolling_number();
    //rolling_total_target();
    //rolling_total_saving();
//}, 7000);

function rolling_number() {
    $diy.rollNumber({
        number: $diy[0].dataset.number,
        speed: 500,
        interval: 200,
        rooms: 4,
        space: 32,
        symbol: ',',
        fontStyle: {
            'font-size': 18,
            'font-family': 'LetsgoDigital',
            'font-color': 'white',
        }
    });
}

function rolling_total_target() {
    $total_target.rollNumber({
        number: $total_target[0].dataset.number,
        speed: 500,
        interval: 200,
        rooms: 9,
        space: 32,
        symbol: ',',
        fontStyle: {
            'font-size': 18,
            'font-family': 'LetsgoDigital',
            'font-color': 'white',
        }
    });
}

function rolling_total_saving() {
    $total_saving.rollNumber({
        number: $total_saving[0].dataset.number,
        speed: 500,
        interval: 200,
        rooms: 9,
        space: 32,
        symbol: ',',
        fontStyle: {
            'font-size': 18,
            'font-family': 'LetsgoDigital',
            'font-color': 'white',
        }
    });
}
