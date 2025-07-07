document.addEventListener('DOMContentLoaded', function () {
    // Créer des particules pour l'arrière-plan
    createParticles();

    // Ajouter la validation du formulaire
    setupFormValidation();
});

function createParticles() {
    const particlesContainer = document.querySelector('.login-particles');
    const particleCount = 15;

    if (!particlesContainer) return;

    for (let i = 0; i < particleCount; i++) {
        const size = Math.random() * 20 + 5; // Taille entre 5 et 25px
        const particle = document.createElement('div');
        particle.classList.add('particle');

        // Positionner aléatoirement
        const posX = Math.random() * 100;
        const posY = Math.random() * 100;

        // Appliquer les styles
        particle.style.width = `${size}px`;
        particle.style.height = `${size}px`;
        particle.style.left = `${posX}%`;
        particle.style.top = `${posY}%`;
        particle.style.opacity = (Math.random() * 0.3 + 0.1).toString(); // Opacité entre 0.1 et 0.4

        // Animation avec délai aléatoire
        particle.style.animationDuration = `${Math.random() * 10 + 10}s`; // Entre 10 et 20 secondes
        particle.style.animationDelay = `${Math.random() * 5}s`; // Délai entre 0 et 5 secondes

        particlesContainer.appendChild(particle);
    }
}

function setupFormValidation() {
    const loginForm = document.getElementById('loginForm');
    const emailInput = document.getElementById('email');
    const passwordInput = document.getElementById('password');
    const errorAlert = document.getElementById('errorAlert');

    if (!loginForm) return;

    loginForm.addEventListener('submit', function (e) {
        let isValid = true;

        // Réinitialiser les messages d'erreur
        errorAlert.classList.add('d-none');
        document.querySelectorAll('.invalid-feedback').forEach(el => el.textContent = '');
        document.querySelectorAll('.is-invalid').forEach(el => el.classList.remove('is-invalid'));

        // Valider l'email
        if (!emailInput.value.trim()) {
            showError(emailInput, 'L\'email est requis');
            isValid = false;
        } else if (!isValidEmail(emailInput.value)) {
            showError(emailInput, 'Veuillez entrer un email valide');
            isValid = false;
        }

        // Valider le mot de passe
        if (!passwordInput.value.trim()) {
            showError(passwordInput, 'Le mot de passe est requis');
            isValid = false;
        }

        // Si le formulaire n'est pas valide, empêcher la soumission
        if (!isValid) {
            e.preventDefault();
        }
    });
}

function showError(input, message) {
    input.classList.add('is-invalid');
    const feedbackElement = input.nextElementSibling;
    if (feedbackElement && feedbackElement.classList.contains('invalid-feedback')) {
        feedbackElement.textContent = message;
    }
}

function isValidEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
}

// Effet de focus sur les champs de formulaire
document.querySelectorAll('.form-control').forEach(input => {
    input.addEventListener('focus', function () {
        this.parentElement.classList.add('focused');
    });

    input.addEventListener('blur', function () {
        if (!this.value) {
            this.parentElement.classList.remove('focused');
        }
    });
});