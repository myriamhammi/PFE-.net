document.getElementById("extractTransformLink").addEventListener("click", function (e) {
    e.preventDefault();

    // Formulaire de chargement de fichier
    const uploadForm = `
        <form id="fileUploadForm" enctype="multipart/form-data">
            <div class="form-group">
                <label for="file">Choisir un fichier Excel :</label>
                <input type="file" class="form-control" id="file" name="file" accept=".xls,.xlsx" required />
            </div>
            
            <div class="form-group">
                <label for="mapping" class="text-muted">Mappage des Colonnes (JSON)</label>
                <textarea class="form-control" id="mapping" name="mapping" rows="6" placeholder='{"Nom": "LastName", "Prénom": "FirstName", "Âge": "Age"}' required></textarea>
            </div>
            
            <button type="submit" class="btn btn-primary">Envoyer</button>
        </form>
    `;
    document.getElementById("uploadFormContainer").innerHTML = uploadForm;

    // Gestion du formulaire de soumission
    document.getElementById("fileUploadForm").addEventListener("submit", async function (e) {
        e.preventDefault();

        const fileInput = document.getElementById("file");
        const file = fileInput.files[0];

        if (!file) {
            alert("Veuillez sélectionner un fichier Excel.");
            return;
        }

        const mappingInput = document.getElementById("mapping");
        const mapping = mappingInput.value.trim();

        if (!mapping) {
            alert("Veuillez fournir un mappage valide.");
            return;
        }

        const formData = new FormData();
        formData.append("file", file);
        formData.append("mapping", mapping);  // Envoi du mappage en JSON

        try {
            const response = await fetch("/ETL/ChargerDonnees", {
                method: "POST",
                body: formData,
            });

            if (!response.ok) throw new Error("Erreur lors du chargement des données.");

            const message = await response.text();
            afficherMessage("✅ " + message, "success");
        } catch (error) {
            afficherMessage("❌ " + error.message, "danger");
        }
    });
});

function afficherMessage(message, type) {
    const statusDiv = document.getElementById("statusMessage");
    statusDiv.innerHTML = `
        <div class="alert alert-${type}" role="alert">
            ${message}
        </div>
    `;
}
