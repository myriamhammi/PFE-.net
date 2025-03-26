document.addEventListener("DOMContentLoaded", function () {
    const extractBtn = document.querySelector("#extractTransformLink");  // Sélection correcte de l'élément
    const uploadFormContainer = document.getElementById("uploadFormContainer");

    // Vérification si l'élément existe
    if (!extractBtn) {
        console.error("Le lien 'Cliquer ici' n'a pas été trouvé !");
        return; // Sort de la fonction si l'élément n'existe pas
    }

    extractBtn.addEventListener("click", function (event) {
        event.preventDefault(); // Empêche le comportement par défaut du lien
        console.log("Lien cliqué, affichage du formulaire...");
        showUploadForm();
    });

    function showUploadForm() {
        uploadFormContainer.innerHTML = `
            <div class="card p-4 mt-3 shadow">
                <h2 class="h5">Télécharger un fichier</h2>
                <input type="file" id="fileInput" class="form-control my-2" />
                <button id="extractDataBtn" class="btn btn-success mt-2">Extraire les données</button>
                <p id="fileName" class="mt-2 text-muted"></p>
            </div>
        `;

        const fileInput = document.getElementById("fileInput");
        const extractDataBtn = document.getElementById("extractDataBtn");
        const fileNameDisplay = document.getElementById("fileName");

        fileInput.addEventListener("change", function () {
            if (fileInput.files.length > 0) {
                fileNameDisplay.textContent = `Fichier sélectionné : ${fileInput.files[0].name}`;
            }
        });

        extractDataBtn.addEventListener("click", function () {
            if (!fileInput.files.length) {
                alert("Veuillez sélectionner un fichier avant d'extraire les données !");
                return;
            }
            alert(`Extraction des données du fichier : ${fileInput.files[0].name}`);
            // Vous pouvez ajouter un envoi de fichier ici
        });
    }
});
