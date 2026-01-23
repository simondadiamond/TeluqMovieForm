# Formulaire de Réservation de Film - TELUQ

Application Blazor WebAssembly pour la gestion des demandes de réservation de films.

## 🔗 Démo en ligne

**Lien de démonstration:** [https://teluqmovieform.netlify.app](https://teluqmovieform.netlify.app/)

## 🚀 Comment exécuter le projet

### Prérequis
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) ou version ultérieure

### Étapes d'exécution

1. **Cloner le dépôt**
   ```bash
   git clone https://github.com/simondadiamond/TeluqMovieForm.git
   cd TeluqMovieForm
   ```

2. **Restaurer les dépendances**
   ```bash
   dotnet restore
   ```

3. **Exécuter l'application**
   ```bash
   dotnet run
   ```

4. **Accéder à l'application**
   
   Ouvrir votre navigateur et accéder à : `https://localhost:5275` (ou le port indiqué dans la console)

### Build pour production
```bash
dotnet publish -c Release
```

---

## 📋 Schéma d'organisation du formulaire et validations

```mermaid
graph TD
    User([Utilisateur]) -->|Soumet| Form[Formulaire de Réservation]

    subgraph Validations
        direction TB
        
        %% SECTION 1
        Movie(<b>1. Sélection du Film</b>) --> MovieCheck{URL valide & < 200 car.?}
        MovieCheck -- Non --> Err1[Erreur: Lien invalide]
        MovieCheck -- Oui --> OK1([OK])

        %% SECTION 2
        Applicant(<b>2. Coordonnées Demandeur</b>) --> NameCheck{Prénom/Nom valides?<br/>Requis, Regex, < 50 car.}
        NameCheck -- Non --> Err2[Erreur: Format/Longueur invalide]
        NameCheck -- Oui --> EmailCheck{Courriel valide?<br/>Requis, Format, < 100 car.}
        EmailCheck -- Non --> Err3[Erreur: Format invalide]
        EmailCheck -- Oui --> OK2([OK])

        %% SECTION 3
        Optional(<b>3. Champs Optionnels</b>) --> PhoneCheck{Téléphone valide?<br/>10 chiffres, Indicatif OK}
        PhoneCheck -- Non --> Err4[Erreur: Format invalide]
        PhoneCheck -- Oui --> PostalCheck{Code Postal valide?<br/>Format A1A 1A1}
        PostalCheck -- Non --> Err5[Erreur: Format invalide]
        PostalCheck -- Oui --> OK3([OK])

        %% SECTION 4
        Security(<b>4. Sécurité</b>) --> OddCheck{Nombre impair et positif?}
        OddCheck -- Non --> Err6[Erreur: Doit être impair et positif]
        OddCheck -- Oui --> OK4([OK])
    end

    Form --> Movie & Applicant & Optional & Security

    OK1 & OK2 & OK3 & OK4 --> Final{Toutes validations OK?}
    Final -- Oui --> Success[Soumission Réussie]
    Final -- Non --> Block[Bloquer Soumission]

    %% High Contrast Error Styles
    %% Fill: Light Red, Stroke: Dark Red, Text: Black
    classDef errorBox fill:#ffcccc,stroke:#b30000,stroke-width:2px,color:#000000;
    class Err1,Err2,Err3,Err4,Err5,Err6 errorBox;
    
    style Success fill:#ccffcc,stroke:#00aa00,color:#000000
```
---

## 🔧 Attributs de validation personnalisés

### 1. **OddNumberAttribute**
Vérifie que le nombre saisi est impair.
- Permet `null` si utilisé avec `[Required]` séparément
- Valide que : `n % 2 ≠ 0`
- Valide implicitement que le nombre est positif (n > 0) pour éviter les erreurs logiques
- Message d'erreur : "Le nombre doit être impair et positif."
- **Note technique :** Retourne `validationContext.MemberName` pour assurer l'association correcte avec le champ dans l'UI Blazor

### 2. **CanadianPhoneAttribute**
Valide un numéro de téléphone au format canadien/nord-américain.
- Accepte les chiffres, espaces, tirets et parenthèses
- Extrait exactement 10 chiffres du format saisi
- Vérifie que l'indicatif régional (1er chiffre) ≠ 0 ou 1
- Vérifie que le numéro local (4e chiffre) ≠ 0 ou 1
- Permet `null` ou vide (champ optionnel)
- **Note technique :** Retourne `validationContext.MemberName` pour assurer l'association correcte avec le champ dans l'UI Blazor

### 3. **CanadianPostalCodeAttribute**
Valide un code postal au format canadien.
- Format accepté : `A1A 1A1` ou `A1A1A1` (avec ou sans espace)
- Expression régulière : `^[A-Za-z]\d[A-Za-z][ ]?\d[A-Za-z]\d$`
- Permet `null` ou vide (champ optionnel)
- **Note technique :** Retourne `validationContext.MemberName` pour assurer l'association correcte avec le champ dans l'UI Blazor

---

## ⚙️ Logique de soumission

### Fonctionnement
1. **Validation en temps réel :** Les champs sont validés à la sortie (focusout) et lors de la soumission
2. **OnValidSubmit :** La soumission n'est déclenchée que si toutes les validations passent avec succès
3. **Message de confirmation :** Affichage d'un message de succès avec les détails de la réservation directement dans la page
4. **Réinitialisation :** Bouton "Effectuer une autre demande" pour retourner au formulaire vierge

### Note technique importante
Les attributs de validation personnalisés retournent explicitement `validationContext.MemberName` dans leur `ValidationResult`. Cette approche résout un problème connu de Blazor où les messages de validation des attributs personnalisés ne s'affichent pas correctement dans les composants `ValidationMessage`. 

Cette solution est basée sur la recommandation officielle de l'équipe ASP.NET Core (voir [issue #38258](https://github.com/dotnet/aspnetcore/issues/38258)) qui indique que les `ValidationResult` doivent explicitement spécifier le nom du membre pour que le `DataAnnotationsValidator` puisse associer correctement les erreurs aux champs pendant la validation de soumission.

---

## 🛠️ Technologies utilisées

- **.NET 10** - Framework principal
- **Blazor WebAssembly** - Framework frontend
- **Bootstrap 5** - Framework CSS
- **Bootstrap Icons** - Bibliothèque d'icônes
- **DataAnnotations** - Système de validation

---

## 📁 Structure du projet

```
TeluqMovieForm/
├── Models/
│   └── MovieRegistrationModel.cs    # Modèle avec validations et attributs personnalisés
├── Services/
│   └── IMovieService.cs             # Interface du service de films
├── Pages/
│   └── MovieForm.razor              # Composant principal du formulaire
├── wwwroot/                         # Fichiers statiques
└── Program.cs                       # Point d'entrée de l'application
```

---

## 📝 Licence

Ce projet a été développé dans le cadre d'un test technique pour TELUQ.
