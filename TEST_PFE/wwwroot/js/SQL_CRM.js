//// Fonction pour afficher les tables dans le tableau
//function displayTables(tables) {
//    const tbody = document.querySelector("#sql-tables tbody");
//    tbody.innerHTML = ''; // Vider le tableau avant d'ajouter les nouvelles lignes

//    tables.forEach(name => {
//        const row = document.createElement("tr");

//        const tdName = document.createElement("td");
//        tdName.textContent = name;

//        const tdAction = document.createElement("td");
//        const btn = document.createElement("button");
//        btn.textContent = "Charger dans Dynamics";
//        btn.className = "btn btn-warning btn-sm";

//        const loadingText = "Chargement...";
//        const retryText = "Réessayer";

//        // Crée une cellule pour afficher le statut
//        const tdStatus = document.createElement("td");

//        btn.onclick = () => {
//            btn.disabled = true;
//            btn.textContent = loadingText;

//            fetch(`/api/dynamics/${name}`, { method: 'POST' })
//                .then(res => {
//                    if (!res.ok) {
//                        throw new Error(`Erreur HTTP : ${res.status}`);
//                    }
//                    return res.json();
//                })
//                .then(data => {
//                    tdStatus.textContent = "✅ " + data.message;
//                })
//                .catch(err => {
//                    tdStatus.textContent = "❌ Échec du chargement";
//                    console.error("Erreur lors du chargement des données vers Dynamics:", err);
//                    btn.disabled = false;
//                    btn.textContent = "Réessayer";
//                });
//        };


//        tdAction.appendChild(btn);
//        row.appendChild(tdName);
//        row.appendChild(tdAction);
//        row.appendChild(tdStatus);
//        tbody.appendChild(row);
//    });
//}

//// Fonction pour récupérer les tables et les afficher dans le tableau
//function fetchAndDisplayTables() {
//    fetch('/api/chargement/tables')
//        .then(res => {
//            if (!res.ok) {
//                throw new Error(`Erreur HTTP : ${res.status}, ${res.statusText}`);
//            }
//            return res.json(); // Retourne le JSON directement
//        })
//        .then(tables => {
//            displayTables(tables); // Afficher les tables dans le tableau
//            const tablesContainer = document.getElementById('tablesContainer');
//            tablesContainer.classList.remove('d-none'); // Afficher le tableau
//        })
//        .catch(err => {
//            console.error("Erreur lors du chargement des tables SQL", err);
//            alert(`Erreur lors du chargement des tables. Détails de l'erreur : ${err.message}`);
//        });
//}


//// Ajouter l'événement de clic sur le lien "Découvrir"
//const showTablesLink = document.getElementById('showTablesLink');
//showTablesLink.addEventListener('click', function (e) {
//    e.preventDefault(); // Empêcher le comportement par défaut du lien
//    fetchAndDisplayTables(); // Récupérer et afficher les tables
//});


// Fonction pour afficher les tables dans le tableau
function displayTables(tables) {
    const tbody = document.querySelector("#sql-tables tbody");
    tbody.innerHTML = ''; // Vider le tableau avant d'ajouter les nouvelles lignes

    tables.forEach(name => {
        const row = document.createElement("tr");

        const tdName = document.createElement("td");
        tdName.textContent = name;

        const tdAction = document.createElement("td");
        const btn = document.createElement("button");
        btn.textContent = "Créer dans Dynamics";  // Mise à jour du texte
        btn.className = "btn btn-success btn-sm"; // Style du bouton

        const loadingText = "Chargement...";
        const retryText = "Réessayer";

        // Crée une cellule pour afficher le statut
        const tdStatus = document.createElement("td");

        btn.onclick = () => {
            btn.disabled = true;
            btn.textContent = loadingText;

            fetch(`/api/dynamics/create-table/${name}`, { method: 'POST' })
                .then(res => {
                    if (!res.ok) {
                        return res.json().then(errorData => {
                            throw new Error(`Erreur HTTP : ${res.status}, Message: ${errorData.error}`);
                        });
                    }
                    return res.json();
                })
                .then(data => {
                    tdStatus.textContent = "✅ " + data.message;
                })
                .catch(err => {
                    tdStatus.textContent = "❌ Échec du chargement";
                    console.error("Erreur lors du chargement des données vers Dynamics:", err);
                    btn.disabled = false;
                    btn.textContent = retryText;
                });
        };


        tdAction.appendChild(btn);
        row.appendChild(tdName);
        row.appendChild(tdAction);
        row.appendChild(tdStatus);
        tbody.appendChild(row);
    });
}

// Fonction pour récupérer les tables et les afficher dans le tableau
function fetchAndDisplayTables() {
    fetch('/api/chargement/tables')
        .then(res => {
            if (!res.ok) {
                throw new Error(`Erreur HTTP : ${res.status}, ${res.statusText}`);
            }
            return res.json(); // Retourne le JSON directement
        })
        .then(tables => {
            displayTables(tables); // Afficher les tables dans le tableau
            const tablesContainer = document.getElementById('tablesContainer');
            tablesContainer.classList.remove('d-none'); // Afficher le tableau
        })
        .catch(err => {
            console.error("Erreur lors du chargement des tables SQL", err);
            alert(`Erreur lors du chargement des tables. Détails de l'erreur : ${err.message}`);
        });
}


// Ajouter l'événement de clic sur le lien "Découvrir"
const showTablesLink = document.getElementById('showTablesLink');
showTablesLink.addEventListener('click', function (e) {
    e.preventDefault(); // Empêcher le comportement par défaut du lien
    fetchAndDisplayTables(); // Récupérer et afficher les tables
});
