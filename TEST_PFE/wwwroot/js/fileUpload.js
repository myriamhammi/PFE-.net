document.addEventListener("DOMContentLoaded", function () {
    const extractBtn = document.querySelector("#extractTransformLink");
    const uploadFormContainer = document.getElementById("uploadFormContainer");

    if (!extractBtn) {
        console.error("Le lien 'Cliquer ici' n'a pas été trouvé !");
        return;
    }

    extractBtn.addEventListener("click", function (event) {
        event.preventDefault();
        showUploadForm();
    });

    function showUploadForm() {
        uploadFormContainer.innerHTML = `
            <div class="card p-4 mt-3 shadow">
                <h2 class="h5">Télécharger un fichier</h2>
                <input type="file" id="fileInput" class="form-control my-2" />
                <button id="extractDataBtn" class="btn btn-success mt-2">Extraire les données</button>
                <p id="fileName" class="mt-2 text-muted"></p>
                <div id="statusMessage" class="mt-2"></div>
            </div>
        `;

        const fileInput = document.getElementById("fileInput");
        const extractDataBtn = document.getElementById("extractDataBtn");

        extractDataBtn.addEventListener("click", function () {
            if (!fileInput.files.length) {
                alert("Veuillez sélectionner un fichier !");
                return;
            }

            const file = fileInput.files[0];
            const formData = new FormData();
            formData.append("file", file);

            fetch('https://localhost:44365/ETL/ChargerDonnees', {
                method: 'POST',
                body: formData
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error(`Erreur HTTP : ${response.status}`);
                    }
                    return response.json();
                })
                .then(result => {
                    console.log("✅ Données envoyées et traitées avec succès :", result);
                    document.getElementById("statusMessage").innerHTML =
                        `<div class="alert alert-success">✅ ${result.message}</div>`;
                })

                .catch(error => {
                    console.error("❌ Erreur lors de l'envoi ou du traitement : ", error);
                    document.getElementById("statusMessage").innerHTML =
                        `<div class="alert alert-danger">❌ Une erreur s'est produite lors du traitement.</div>`;
                });
        });
    }
});
