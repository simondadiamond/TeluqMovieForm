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
    User([Utilisateur]) -->|Soumet| Start{Début Validation}

    subgraph "1. Sélection du Film"
        direction TB
        Movie[<b>MovieUrl</b>] --> M_Req{Est rempli?}
        M_Req -- Non --> ErrM1[Erreur: Requis]
        M_Req -- Oui --> M_Url{Format URL valide?}
        M_Url -- Non --> ErrM2[Erreur: Lien invalide]
        M_Url -- Oui --> M_Len{Longueur <= 200?}
        M_Len -- Non --> ErrM3[Erreur: > 200 caractères]
        M_Len -- Oui --> M_OK([OK])
    end

    subgraph "2. Coordonnées Demandeur"
        direction TB
        %% Last Name
        LName[<b>Nom (ApplicantLastName)</b>] --> LN_Req{Est rempli?}
        LN_Req -- Non --> ErrLN1[Erreur: Requis]
        LN_Req -- Oui --> LN_Len{Longueur <= 50?}
        LN_Len -- Non --> ErrLN2[Erreur: > 50 caractères]
        LN_Len -- Oui --> LN_Reg{Regex: Lettres/Accents/-/' ?}
        LN_Reg -- Non --> ErrLN3[Erreur: Caractères invalides]
        LN_Reg -- Oui --> LN_OK([OK])

        %% First Name
        FName[<b>Prénom (ApplicantFirstName)</b>] --> FN_Req{Est rempli?}
        FN_Req -- Non --> ErrFN1[Erreur: Requis]
        FN_Req -- Oui --> FN_Len{Longueur <= 50?}
        FN_Len -- Non --> ErrFN2[Erreur: > 50 caractères]
        FN_Len -- Oui --> FN_Reg{Regex: Lettres/Accents/-/' ?}
        FN_Reg -- Non --> ErrFN3[Erreur: Caractères invalides]
        FN_Reg -- Oui --> FN_OK([OK])

        %% Email
        Email[<b>Courriel</b>] --> E_Auto[Auto: Trim + Lowercase]
        E_Auto --> E_Req{Est rempli?}
        E_Req -- Non --> ErrE1[Erreur: Requis]
        E_Req -- Oui --> E_Fmt{Format courriel?}
        E_Fmt -- Non --> ErrE2[Erreur: Invalide]
        E_Fmt -- Oui --> E_Len{Longueur <= 100?}
        E_Len -- Non --> ErrE3[Erreur: > 100 caractères]
        E_Len -- Oui --> E_OK([OK])
    end

    subgraph "3. Champs Optionnels"
        direction TB
        %% Phone
        Phone[<b>Téléphone</b>] --> P_Null{Est vide?}
        P_Null -- Oui --> P_OK([OK])
        P_Null -- Non --> P_Reg{Caractères valides?<br/>(Chiffres, espaces, tirets, par.)}
        P_Reg -- Non --> ErrP1[Erreur: Caractères invalides]
        P_Reg -- Oui --> P_Len{10 chiffres exacts?}
        P_Len -- Non --> ErrP2[Erreur: Longueur != 10]
        P_Len -- Oui --> P_Area{Indicatif/Local commence<br/>par 0 ou 1?}
        P_Area -- Oui --> ErrP3[Erreur: Format invalide]
        P_Area -- Non --> P_OK

        %% Postal Code
        Postal[<b>Code Postal</b>] --> Po_Null{Est vide?}
        Po_Null -- Oui --> Po_OK([OK])
        Po_Null -- Non --> Po_Reg{Regex: A1A 1A1?}
        Po_Reg -- Non --> ErrPo1[Erreur: Invalide]
        Po_Reg -- Oui --> Po_Auto[Auto: Majuscules + Espace]
        Po_Auto --> Po_OK
    end

    subgraph "4. Sécurité"
        direction TB
        Odd[<b>Nombre Impair</b>] --> O_Req{Est rempli?}
        O_Req -- Non --> ErrO1[Erreur: Requis]
        O_Req -- Oui --> O_Logic{Impair ET Positif?<br/>(n % 2 != 0 && n > 0)}
        O_Logic -- Non --> ErrO2[Erreur: Doit être impair et positif]
        O_Logic -- Oui --> O_OK([OK])
    end

    Start --> Movie & LName & FName & Email & Phone & Postal & Odd
    
    M_OK & LN_OK & FN_OK & E_OK & P_OK & Po_OK & O_OK --> Final{Tout valide?}
    Final -- Oui --> Success[Soumission Réussie]
    Final -- Non --> Block[Bloquer Soumission]

    %% Error Styles
    style ErrM1 fill:#ffcccc,stroke:#ff0000
    style ErrM2 fill:#ffcccc,stroke:#ff0000
    style ErrM3 fill:#ffcccc,stroke:#ff0000
    style ErrLN1 fill:#ffcccc,stroke:#ff0000
    style ErrLN2 fill:#ffcccc,stroke:#ff0000
    style ErrLN3 fill:#ffcccc,stroke:#ff0000
    style ErrFN1 fill:#ffcccc,stroke:#ff0000
    style ErrFN2 fill:#ffcccc,stroke:#ff0000
    style ErrFN3 fill:#ffcccc,stroke:#ff0000
    style ErrE1 fill:#ffcccc,stroke:#ff0000
    style ErrE2 fill:#ffcccc,stroke:#ff0000
    style ErrE3 fill:#ffcccc,stroke:#ff0000
    style ErrP1 fill:#ffcccc,stroke:#ff0000
    style ErrP2 fill:#ffcccc,stroke:#ff0000
    style ErrP3 fill:#ffcccc,stroke:#ff0000
    style ErrPo1 fill:#ffcccc,stroke:#ff0000
    style ErrO1 fill:#ffcccc,stroke:#ff0000
    style ErrO2 fill:#ffcccc,stroke:#ff0000
    style Success fill:#ccffcc,stroke:#00aa00
```
---

## 🔧 Attributs de validation personnalisés

### 1. **OddNumberAttribute**
Vérifie que le nombre saisi est impair.
- Permet `null` si utilisé avec `[Required]` séparément
- Valide que : `n ≠ 0` et `n % 2 ≠ 0`
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