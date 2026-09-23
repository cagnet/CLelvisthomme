# Évaluation des candidats AGL – Développeur Full Stack Angular / C#

## Présentation générale

Le présent projet est utilisé pour évaluer les candidats selon deux axes techniques principaux :

- les compétences **front-end**, principalement à travers l’évaluation de la maîtrise d’Angular ;
- les compétences **back-end**, autour de .NET, C# et SQL.

Bien que les compétences techniques représentent une part importante de l’évaluation, d’autres critères seront également pris en compte :

- le respect des consignes ;
- la pertinence des choix effectués et la capacité à prendre des décisions en cas de doute ;
- la gestion du dépôt Git.

Ces éléments sont tout aussi importants que la maîtrise pure des langages et des frameworks.

Le projet est mis à disposition de chaque candidat via un dépôt Git qui lui est propre. Tous les commits devront être effectués sur le dépôt fourni.

---

## Consignes — Back-end

Le code fourni est fonctionnel et compile. Il est volontairement simple et épuré.

Pour cette partie, il est demandé au candidat de lire et de comprendre le code existant avant de commencer les développements.

Chacune des quatre demandes ci-dessous devra être développée dans une branche spécifique, puis fusionnée dans la branche `develop` avant de passer à l'étape suivante. La gestion du dépôt Git fait partie intégrante de l'évaluation des compétences du candidat.

- Corriger la route de création d'un `Order` en proposant une solution plus élégante.

- Ajouter à la ressource `Customer` les quatre propriétés suivantes : `Nom`, `Prénom`, `Email` et `Adresse`, et les exposer dans l'API.

- Implémenter la couche de persistance des données dans une base de données SQL Server. Vous pouvez choisir la solution de votre choix, par exemple Dapper ou Entity Framework. La racine du projet contient un script SQL correspondant à la solution actuelle ; vous êtes libre de l'utiliser, de l'adapter ou de proposer votre propre solution.

- Proposer une nouvelle implémentation du projet respectant les principes de la Clean Architecture. Aucune structure de solution n'est imposée : le découpage, les responsabilités, les dépendances et les choix d'implémentation proposés devront pouvoir être expliqués et justifiés par le candidat.

### Bonus — éléments différenciants

- Validation des données saisies.

- Tests unitaires dans le projet utilisant la nouvelle architecture, avec la bibliothèque de votre choix. A minima, les règles métier suivantes pourront être testées :
  - impossible de créer un `Order` pour un `Customer` désactivé ;
  - impossible d'avoir un `Amount` négatif sur un `Order`.

- Gestion cohérente des codes d'erreur HTTP au niveau des contrôleurs.

### Contraintes techniques

- .NET 8 minimum.
- Langage C#.
- Base de données SQL Server.
- Exposition et documentation de l'API via Swagger / OpenAPI.

---

## Consignes — Application Angular pour CustomerOrders API

Construisez une application Angular consommant l'API précédemment modifiée et permettant de gérer les `Customers` et leurs `Orders`.

Vous pouvez travailler sur les deux projets en parallèle si vous le souhaitez, mais une attention particulière devra être portée à la gestion des branches et des commits.

### Socle obligatoire

- Liste des clients avec leur nom et leur statut actif/inactif clairement identifiable.

- Création et modification d'un client à l'aide d'un formulaire réactif, avec validation du nom.

- Affichage du détail d'un client et de ses commandes, avec la possibilité de créer une nouvelle commande.

- Modification et suppression d'une commande.

- Suppression d'un client, avec affichage d'un message clair et compréhensible lorsque l'API refuse l'opération (`409 Conflict`).

- Affichage lisible des erreurs de validation (`400 Bad Request`) dans les formulaires, champ par champ.

- Service Angular dédié aux appels HTTP, avec des interfaces TypeScript typées reflétant les contrats de l'API :
  - `Customer`
  - `Order`
  - `CreateCustomer`
  - `UpdateCustomer`
  - `CreateOrder`
  - `UpdateOrder`

- Routing entre la liste des clients et le détail d'un client.

- Gestion des états de chargement et des listes vides.

### Bonus — éléments différenciants

- Tableau de bord simple présentant :
  - le nombre de clients actifs et inactifs ;
  - le nombre de commandes ;
  - le montant total des commandes ;
  - le montant moyen des commandes.

  L'API ne possède pas d'endpoint dédié à ces informations : les données devront être croisées côté front à partir de `/customers` et `/orders`. Il s'agit notamment d'évaluer la capacité à composer les données avec RxJS (`forkJoin`, `combineLatest`, etc.) ou à utiliser des Signals dérivés.

- `HttpInterceptor` centralisant et normalisant les différents formats d'erreur de l'API dans un modèle unique exploitable par l'interface utilisateur.

- Gestion de l'état avec Angular Signals, ou avec une autre solution telle que NgRx/Akita si son utilisation est justifiée.

- Recherche, filtrage et tri côté client sur la liste des clients et/ou des commandes.

- Lazy loading des routes.

- Tests unitaires comprenant au minimum :
  - un service HTTP testé avec `HttpTestingController` ;
  - un composant comportant une logique non triviale.

- Prise en compte des principes de base d'accessibilité : labels de formulaire, gestion du focus, contrastes, etc.

- Design responsive simple.

- Configuration propre de l'URL de l'API via les fichiers d'environnement Angular.

### Contraintes techniques

- Angular 17 ou supérieur, avec composants standalone.

- TypeScript en mode `strict`, sans utilisation de `any` non justifiée.

- `HttpClient` et RxJS pour les appels réseau ; pas d'utilisation directe de `fetch`.

- Formulaires réactifs (`ReactiveFormsModule`) ; pas de `ngModel` pour les formulaires de création ou de modification.

- Aucune bibliothèque de composants UI n'est imposée. Angular Material est autorisé, mais non obligatoire.

### Hors périmètre

- Authentification et autorisation.
- Persistance autre que celle fournie par le back-end.
- Design graphique élaboré.

## Bonus — Conteneurisation et déploiement

En complément des développements demandés, le candidat peut proposer une solution permettant de **conteneuriser et déployer l'application.**

La solution technique reste libre : Docker, Docker Compose ou toute autre approche de conteneurisation jugée pertinente.

L'objectif n'est pas uniquement d'obtenir une application fonctionnelle dans un conteneur, mais également d'évaluer :

- la compréhension des principes de conteneurisation ;
- la séparation entre configuration et image applicative ;
- la gestion des dépendances nécessaires à l'exécution ;
- la simplicité de construction et de lancement de l'application ;
- la qualité et la maintenabilité des fichiers de configuration produits ;
- la prise en compte des problématiques de sécurité ;
- la capacité à documenter la procédure de build et de déploiement.

Une attention particulière sera portée aux **choix effectués et à leur justification.**

La mise en place d'une chaîne CI/CD permettant de construire, tester et/ou déployer automatiquement l'application pourra également être proposée et sera considérée comme un bonus supplémentaire.

**Il n'est pas attendu qu'une infrastructure cloud réelle soit nécessairement provisionnée** : une solution démontrable localement ou documentée de manière suffisamment précise peut être considérée comme valide.
