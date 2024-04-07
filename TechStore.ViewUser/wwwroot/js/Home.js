

//document.getElementById('button3').addEventListener('click', function () {
//    document.getElementById('button3').classList.add('hidden');
//    document.getElementById('button4').classList.remove('hidden');
//});

//document.getElementById('button4').addEventListener('click', function () {
//    document.getElementById('button4').classList.add('hidden');
//    document.getElementById('button3').classList.remove('hidden');
//});



document.getElementById('button3').addEventListener('click', function () {
    $('#carouselExampleControls1').carousel('prev');
});
document.getElementById('button4').addEventListener('click', function () {
    $('#carouselExampleControls1').carousel('next');
});

