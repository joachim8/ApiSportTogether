#SportTogether

SportTogether est une plateforme sociale dédiée aux amateurs de sport, où les utilisateurs peuvent publier et rejoindre des sorties sportives, discuter via messagerie instantanée, suivre un fil d'actualité, et gérer leurs sorties. L'application est construite avec Blazor pour le front-end et une API ASP.NET Core pour le back-end. Elle utilise Entity Framework Core pour la gestion des données.
Fonctionnalités

    Ajout d'annonces de sorties sportives : Les utilisateurs peuvent créer des annonces pour organiser des événements sportifs, avec la possibilité de spécifier le type de sport, la date, le lieu et le nombre de participants.
    Messagerie instantanée : Communication en temps réel entre utilisateurs, facilitant l'organisation des événements.
    Fil d'actualité : Un flux dynamique des dernières annonces, messages, et événements des utilisateurs.
    Gestion des sorties : Possibilité pour les utilisateurs de rejoindre, annuler ou gérer les détails de leurs sorties.
    Sécurité et gestion des utilisateurs : Authentification et autorisation sécurisées avec prise en charge des rôles.

Technologies utilisées

    Blazor (WebAssembly) : Utilisé pour l'interface utilisateur interactive et réactive.
    ASP.NET Core API : Fournit des endpoints pour les fonctionnalités principales comme la gestion des utilisateurs, des annonces, des messages, etc.
    Entity Framework Core : Pour l'accès et la gestion des données dans la base de données.
    SignalR : Gère la messagerie instantanée et les notifications en temps réel.
    SQL Server : Base de données utilisée pour stocker les informations liées aux utilisateurs, annonces, et interactions.

Structure du projet

    Client : Application Blazor (WebAssembly) contenant l'interface utilisateur.
    Server : API ASP.NET Core pour gérer la logique métier et les opérations de données.
    Shared : Contient les modèles partagés entre l'API et l'application cliente.
    Data : Gestion des entités avec Entity Framework Core et accès à la base de données.

Installation
Prérequis

    .NET SDK 7.0+
    SQL Server (ou une autre base de données prise en charge par Entity Framework Core)

Étapes

    Cloner le dépôt :

    bash

git clone https://github.com/username/SportTogether.git
cd SportTogether

Configuration de la base de données : Mettez à jour la chaîne de connexion dans appsettings.json avec vos informations de base de données :

json

"ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=SportTogetherDB;Trusted_Connection=True;"
}

Migrations et création de la base de données : Exécutez les commandes suivantes pour appliquer les migrations et créer la base de données :

bash

cd Server
dotnet ef database update

Lancer le projet : Démarrez l'application avec Visual Studio ou via la ligne de commande :

bash

    dotnet run --project Server

    L'application sera disponible à l'adresse https://localhost:5001.

API Endpoints

Voici quelques exemples d'endpoints exposés par l'API :

    GET /api/annonces : Récupérer toutes les annonces de sorties.
    POST /api/annonces : Créer une nouvelle annonce.
    GET /api/messages : Récupérer la liste des messages d'une conversation.
    POST /api/messages : Envoyer un message.

Pour plus de détails, vous pouvez consulter la documentation Swagger disponible à https://localhost:5001/swagger.
Contribuer

Les contributions sont les bienvenues. Veuillez soumettre une pull request ou ouvrir une issue si vous avez des suggestions ou des améliorations.
