# 🎮 MiniRPG  
### Gioco 2D sviluppato in **C#** e **WPF**

<p align="center">
  <img src="https://img.shields.io/badge/C%23-.NET-blue?style=for-the-badge&logo=csharp" />
  <img src="https://img.shields.io/badge/WPF-Desktop-purple?style=for-the-badge" />
  <img src="https://img.shields.io/badge/Pattern-MVVM-green?style=for-the-badge" />
  <img src="https://img.shields.io/badge/Architecture-OOP-red?style=for-the-badge" />
</p>

---

## 📌 Descrizione del Progetto

**MiniRPG** è un videogioco 2D sviluppato in **C# utilizzando WPF**.  
Il progetto dimostra l’utilizzo avanzato di:

- Programmazione ad Oggetti (OOP)
- Pattern **MVVM**
- Game Loop personalizzato
- Animazioni tramite sprite sheet
- Gestione eventi e delegati
- Collision detection

Il giocatore controlla un personaggio che:

- Si muove nell’area di gioco  
- Combatte nemici generati dinamicamente  
- Potenzia le proprie statistiche tra un livello e l’altro  

Il gioco termina automaticamente al **livello 10**, riportando l’utente alla schermata iniziale.

---

# 🎯 Obiettivi Didattici

Il progetto dimostra l’utilizzo di:

✔ Classi astratte e polimorfismo  
✔ Ereditarietà e overriding  
✔ Incapsulamento tramite proprietà con setter privati  
✔ Strutture dati (`List<T>`, `ObservableCollection<T>`)  
✔ Eventi e delegati  
✔ Pattern MVVM in WPF  
✔ Game Loop personalizzato  
✔ Animazioni tramite sprite sheet  
✔ Collision detection  
✔ Interfaccia grafica con `Canvas` e `UserControl`  

---

# 🏗 Architettura del Progetto

L’applicazione è organizzata in livelli logici per garantire separazione delle responsabilità.

---

## 📂 1️⃣ Models

Contengono la **logica pura del gioco**.

- `Entity` (classe astratta) → base per Player e Enemy  
- `Player` → movimento, attacco, rigenerazione, upgrade  
- `Enemy` → comportamento base dei nemici  
- `Goblin`, `Vampire` → implementazioni concrete  
- `Stats` → gestione di vita, danno e velocità  

---

## ⚙ 2️⃣ Services

Servizi indipendenti dalla UI.

- `GameLoop` → aggiorna continuamente lo stato del gioco (~60 FPS)  
- `InputService` → gestione input da tastiera  
- `EnemySpawner` → genera nemici ai bordi della mappa  
- `CollisionService` → rilevamento collisioni  
- `AnimationService` → gestione animazioni tramite sprite sheet  

---

## 🧠 3️⃣ ViewModels

Collegano logica di gioco e interfaccia.

- `GameViewModel`  
- `UpgradeViewModel`  

Gestiscono eventi, binding e comunicazione tra GameLoop e View.

---

## 🖥 4️⃣ Views

Interfaccia grafica WPF.

- `GameView`
- `UpgradeView`
- `MainWindow` (navigazione tra schermate)

---

# 🔄 Funzionamento del Gioco

## 🎮 Game Loop

Il **GameLoop** è il cuore del gameplay e viene eseguito circa **60 volte al secondo**.

Si occupa di:

- Leggere l’input
- Aggiornare il Player
- Aggiornare i nemici
- Gestire collisioni e danni
- Verificare fine livello
- Inviare eventi al ViewModel

---

## 🧍 Player

Il Player implementa:

- Movimento con **accelerazione e attrito**
- Attacco con **cooldown**
- Animazioni (Idle, Walk, Attack, Hurt, Death)
- Rigenerazione vita (5 HP/sec)
- Upgrade di danno, vita massima e velocità
- Gestione monete

La rigenerazione utilizza un timer interno e il metodo `Stats.Heal()`.

---

## 👾 Nemici

I nemici:

- Inseguono il player
- Evitano sovrapposizioni (separazione)
- Attaccano con cooldown
- Hanno animazioni dedicate
- Muiono con animazione personalizzata

`Goblin` e `Vampire` hanno statistiche differenti.

---

# ⬆ Sistema di Upgrade

Dopo ogni livello viene mostrata una schermata di potenziamento.

Il giocatore può migliorare:

- 💥 Danno
- ❤️ Vita massima
- ⚡ Velocità

I costi aumentano progressivamente e **non vengono mai resettati**.

Gestione tramite `UpgradeViewModel`.

---

# 🏁 Fine del Gioco

Quando il livello supera il 10:

1. `GameLoop` invia evento `GameCompleted`
2. Il ViewModel lo inoltra
3. La `GameView` notifica la `MainWindow`
4. Ritorno alla schermata iniziale

---

# 🧩 Pattern MVVM

Il progetto utilizza il pattern **MVVM** per separare:

- **Model** → logica del gioco  
- **ViewModel** → logica della UI  
- **View** → interfaccia grafica  

Vantaggi:

✔ Codice più pulito  
✔ Maggiore testabilità  
✔ Separazione logica/grafica  

---

# 📊 Strutture Dati Utilizzate

- `List<Enemy>` → logica interna GameLoop  
- `ObservableCollection<Enemy>` → aggiornamento automatico UI  
- Eventi (`Action`, delegati) → comunicazione tra componenti  

---

# 🧠 Tecniche OOP Utilizzate

| Concetto | Applicazione |
|----------|-------------|
| Astrazione | `Entity` è classe astratta |
| Ereditarietà | `Player` e `Enemy` derivano da `Entity` |
| Polimorfismo | `Goblin` e `Vampire` derivano da `Enemy` |
| Incapsulamento | Setter privati in `Stats` |
| Composizione | `Player` contiene `Stats`, `AnimationService`, ecc. |

---

# 🖌 Interfaccia Grafica

La UI utilizza:

- `Canvas` per disegno sprite  
- Binding MVVM  
- `UserControl` modulari  
- `MainWindow` per navigazione  

La barra della vita è disegnata manualmente con due immagini sovrapposte.
Lo sprite della moneta è stato disegnato.
Gli spritesheet li ho trovato online.

---

# 🚧 Sviluppi Futuri

MiniRPG è un progetto in continua evoluzione.

Durante la realizzazione ho imparato e consolidato nuove competenze, sia dal punto di vista tecnico (OOP, MVVM, gestione eventi, animazioni, architettura software) sia dal punto di vista progettuale, comprendendo meglio come strutturare un'applicazione complessa e scalabile.

Il progetto può essere ulteriormente migliorato con:

- ✨ Nuovi tipi di nemici e boss
- 🗺 Sistema di mappe e ambientazioni differenti
- 🎵 Effetti sonori e musica di sottofondo
- 💾 Sistema di salvataggio
- 🎯 Bilanciamento avanzato del gameplay
- 🧪 Refactoring e ottimizzazioni delle performance

Nel corso dell’anno mi impegnerò a:

- Continuare lo sviluppo del gioco
- Migliorare l’architettura e la qualità del codice
- Aggiungere nuove funzionalità

---

# 🏆 Conclusione

**MiniRPG** è un progetto completo che dimostra:

- ✔ Padronanza della Programmazione ad Oggetti  
- ✔ Capacità di strutturare un'applicazione complessa  
- ✔ Uso corretto di WPF e MVVM  
- ✔ Gestione animazioni, input, collisioni e logica di gioco  
- ✔ Integrazione coerente di più componenti  

---

## 🚀 Autore: Benkaref Mohammed

Progetto personale sviluppato a scopo didattico e dimostrativo.

---
