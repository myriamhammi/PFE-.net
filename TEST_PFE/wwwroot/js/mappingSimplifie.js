document.getElementById("extractTransformLink").addEventListener("click", function (e) {
    e.preventDefault();

    const formHTML = `
    <form id="fileUploadForm" enctype="multipart/form-data">
        <div class="form-group">
            <label for="file">Choisir un fichier :</label>
            <input type="file" class="form-control" id="file" name="file" accept=".xlsx, .xls, .csv, .json" required />
        </div>

        <div class="form-group">
            <label for="tableName">Nom de la table :</label>
            <input type="text" class="form-control" id="tableName" name="tableName" required />
        </div>

        <!-- CHAMP mapping caché requis -->
        <input type="hidden" id="mapping" name="mapping" value="{}" />

        <div id="mappingContainer" class="mt-4"></div>

        <button type="submit" class="btn btn-primary mt-3">Envoyer</button>
    </form>
`;

    document.getElementById("uploadFormContainer").innerHTML = formHTML;

    const fileInput = document.getElementById("file");
    fileInput.addEventListener("change", handleFile);

    document.getElementById("fileUploadForm").addEventListener("submit", handleSubmit);
});


function handleFile(e) {
    const file = e.target.files[0];
    if (!file) return;

    const reader = new FileReader();
    reader.onload = function (e) {
        const data = new Uint8Array(e.target.result);
        const fileExtension = file.name.split('.').pop().toLowerCase();

        if (fileExtension === 'xlsx' || fileExtension === 'xls') {
            const workbook = XLSX.read(data, { type: "array" });
            const firstSheet = workbook.Sheets[workbook.SheetNames[0]];
            const jsonData = XLSX.utils.sheet_to_json(firstSheet, { header: 1 });
            const headers = jsonData[0];
            afficherMapping(headers);
        } else if (fileExtension === 'csv') {
            const text = new TextDecoder().decode(data);
            const rows = text.split("\n").map(row => row.split(","));
            const headers = rows[0];
            afficherMapping(headers);
        } else if (fileExtension === 'json') {
            const jsonData = JSON.parse(new TextDecoder().decode(data));
            const headers = Object.keys(jsonData[0]);
            afficherMapping(headers);
        } else {
            afficherMessage("Format de fichier non pris en charge.", "danger");
        }
    };
    reader.readAsArrayBuffer(file);
}



function afficherMapping(headers) {
    const container = document.getElementById("mappingContainer");
    container.innerHTML = "<h5 class='text-muted mb-3'>Associez un type à chaque colonne :</h5>";

    let mappingData = {};

    function removeDiacritics(str) {
        return str.normalize("NFD").replace(/[\u0300-\u036f]/g, "");
    }

    headers.forEach(header => {
        const cleanedHeader = removeDiacritics(header.trim())
            .replace(/\s+/g, "_")
            .replace(/[^a-zA-Z0-9_]/g, "")
            .toUpperCase();

    


        const div = document.createElement("div");
        div.classList.add("form-group", "row", "align-items-center");

        const col1 = document.createElement("div");
        col1.classList.add("col-6");
        const label = document.createElement("label");
        label.innerText = header;  // Afficher le nom original
        col1.appendChild(label);

        const col2 = document.createElement("div");
        col2.classList.add("col-6");
        const select = document.createElement("select");
        select.classList.add("form-control");
        select.name = "mappingType";
        select.dataset.col = cleanedHeader; // Utiliser le nom nettoyé

        // Ajouter les options de type
        ["Texte", "Nombre", "Date", "Booléen"].forEach(type => {
            const option = document.createElement("option");
            option.value = type.toLowerCase();
            option.innerText = type;
            select.appendChild(option);
        });

        // Ajouter l'événement de changement pour mettre à jour le mappingData
        select.addEventListener("change", function () {
            mappingData[cleanedHeader] = select.value;
            document.getElementById("mapping").value = JSON.stringify(mappingData);
        });

        // Déclencher un changement initial pour remplir les valeurs de mappingData
        select.dispatchEvent(new Event('change'));

        col2.appendChild(select);
        div.appendChild(col1);
        div.appendChild(col2);
        container.appendChild(div);
    });

    // Mettre à jour le champ caché avec les valeurs initiales de mappingData
    document.getElementById("mapping").value = JSON.stringify(mappingData);
}





async function handleSubmit(event) {
    event.preventDefault();

    const formData = new FormData(document.getElementById("fileUploadForm"));
    console.log("mapping field:", formData.get("mapping"));

    try {
        const response = await fetch("https://localhost:44365/ETL/ChargerDonneesTransformees", {
            method: "POST",
            body: formData
        });

        const contentType = response.headers.get("content-type");

        if (!response.ok) {
            let errorMessage;

            if (contentType && contentType.includes("application/json")) {
                const errorData = await response.json();
                errorMessage = errorData.message || "Erreur inconnue";
            } else {
                const text = await response.text();
                errorMessage = `Erreur du serveur : ${text}`;
            }

            console.error(errorMessage);
            alert(errorMessage);
            return;
        }

        const result = await response.json();

        alert(result.message);

        if (result.data) {
            afficherTableau(result.data);

            // ➕ Affiche la modale contenant le tableau
            const modal = new bootstrap.Modal(document.getElementById("resultModal"));
            modal.show();
        }

    } catch (error) {
        console.error("Erreur réseau ou autre :", error);
        alert("Erreur de soumission du formulaire : " + error.message);
    }
}


function afficherTableau(data) {
    const container = document.getElementById("tableContainer");

    if (!container) {
        console.error("L'élément avec l'id 'tableContainer' est introuvable dans le DOM !");
        return;
    }

    container.innerHTML = "";

    if (!data || data.length === 0) {
        container.innerHTML = "<p>Aucune donnée à afficher.</p>";
        return;
    }

    const table = document.createElement("table");
    table.className = "table table-bordered table-striped mt-3";

    const thead = document.createElement("thead");
    const headerRow = document.createElement("tr");
    Object.keys(data[0]).forEach(key => {
        const th = document.createElement("th");
        th.textContent = key;
        headerRow.appendChild(th);
    });
    thead.appendChild(headerRow);
    table.appendChild(thead);

    const tbody = document.createElement("tbody");
    data.forEach(row => {
        const tr = document.createElement("tr");
        Object.values(row).forEach(value => {
            const td = document.createElement("td");
            td.textContent = value;
            tr.appendChild(td);
        });
        tbody.appendChild(tr);
    });
    table.appendChild(tbody);

    container.appendChild(table);
}



function afficherMessage(message, type) {
    const statusDiv = document.getElementById("statusMessage");
    statusDiv.innerHTML = `
        <div class="alert alert-${type}" role="alert">
            ${message}
        </div>
    `;
}