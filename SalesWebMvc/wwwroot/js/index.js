
let currentSlide = 0;
const slides = document.querySelectorAll('.carousel-slide');
let interval;

function showSlide(index) {
    slides.forEach((slide, i) => {
        slide.classList.toggle('active', i === index);
    });
}

function startCarousel() {
    interval = setInterval(() => {
        currentSlide = (currentSlide + 1) % slides.length;
        showSlide(currentSlide);
    }, 5000);
}

function stopCarousel() {
    clearInterval(interval);
}

document.querySelector('.carousel-container').addEventListener('mouseenter', stopCarousel);
document.querySelector('.carousel-container').addEventListener('mouseleave', startCarousel);

showSlide(currentSlide);
startCarousel();

document.addEventListener('DOMContentLoaded', function () {
    const explorarRecursosBtn = document.getElementById('explorarRecursosBtn');
    const recursosSecao = document.getElementById('recursos-secao');

    if (explorarRecursosBtn && recursosSecao) {
        explorarRecursosBtn.addEventListener('click', function () {
            recursosSecao.scrollIntoView({
                behavior: 'smooth', // Para uma rolagem suave
                block: 'start'      // Alinha o topo da seção com o topo da viewport
            });
        });
    }
});