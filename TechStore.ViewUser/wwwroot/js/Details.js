window.onload = function () {
    var imgs = document.querySelectorAll(".controller img");
    var mainImage = document.querySelector(".image img");
    var hs = document.querySelector("h2");

    imgs.forEach((ele) => {
        ele.onclick = function () {
            mainImage.src = ele.dataset.src; // Change the source of the main image
        };
    });
};

document.getElementById('addToCartButton').addEventListener('click', function (event) {
    event.preventDefault(); // Prevent default form submission

    var form = document.getElementById('addToCartForm');
    var formData = new FormData(form);

    // Make an AJAX request to submit the form data
    fetch(form.action, {
        method: form.method,
        body: formData
    })
        .then(response => {
            if (response.ok) {
                // Handle success response here (e.g., show a success message)
                console.log('Item added to cart successfully.');
            } else {
                // Handle error response here (e.g., show an error message)
                console.error('Error adding item to cart.');
            }
        })
        .catch(error => {
            console.error('Error adding item to cart:', error);
        });
});