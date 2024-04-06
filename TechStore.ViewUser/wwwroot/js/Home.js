

    const slider = () => {
            const imageList = document.querySelector(".slider")
    const sliderButtons = document.querySelectorAll(".slider .slid-btn")
            sliderButtons.forEach(button => {
        button.addEventListener("click", () => {
            const direction = button.id === "prev-slid" ? -1 : 1
            const scrollAmount = imageList.clientWidth * direction
            imageList.scrollBy({
                left: scrollAmount,
                behavior: "smooth"
            })
        })
    })
        }
    window.addEventListener("load", slider)


// product 
//document.querySelectorAll('butt-sec4')
document.getElementById('button3').addEventListener('click', function () {
    document.getElementById('button3').classList.add('hidden');
    document.getElementById('button4').classList.remove('hidden');
});

document.getElementById('button4').addEventListener('click', function () {
    document.getElementById('button4').classList.add('hidden');
    document.getElementById('button3').classList.remove('hidden');
});

//sectione2
document.getElementById('butt-sec3').addEventListener('click', function () {
    document.getElementById('butt-sec3').classList.add('hidden');
    document.getElementById('butt-sec4').classList.remove('hidden');
});

document.getElementById('butt-sec4').addEventListener('click', function () {
    document.getElementById('butt-sec4').classList.add('hidden');
    document.getElementById('butt-sec3').classList.remove('hidden');
});

    