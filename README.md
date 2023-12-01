﻿# Mesh Game

### Elenco punti svolti per l'esame
- **Impostazioni generali**
    - Splash Screen
        - aggiunto Assets/Icons/logo
    - Icona gioco
    - aggiunto Assets/Icons/icon

- **Main Menu**
    - Load Game
        - Vedi scena: MainMenu
        - Vedi script: Assets/Scripts/MainMenuUIManager -> LoadScene
    - Credits Screen
        - Vedi scena: MainMenu -> Canvas -> CreditsPanel

- **Tutorial**
    - Schermata statica
        - Vedi scena: MainMenu -> Canvas -> TutorialPanel
        - Vedi scena: MainMenu -> Canvas -> CubeTutorial
        - Vedi scena: MainMenu -> Canvas -> SphereTutorial
        - Vedi scena: MainMenu -> Canvas -> PowerUpTutorial

- **GamePlay**
    - PowerUp
        - Vedi scena: GameScene -> GameManager
        - Vedi script: Assets/Scripts/GameManager
        - I PowerUp vengono creati a runtime e sono elencati nella lista *SpawnablePowerUps*
    - Gioco a tempo
        - Vedi scena: GameScene -> Canvas -> GamePanel -> Panel -> Countdown
        - Vedi script: Assets/Scripts/GameManager
    - Presenza di nemici
        - Vedi script: Assets/Scripts/Entities/Enemy
        - Vedi prefab: Assets/Prefabs/CubeEnemy
        - Vedi prefab: Assets/Prefabs/SphereEnemy

- **Strutture**
    - Singleton
        - Vedi script: Assets/Scripts/MainMenuUIManager
        - Vedi script: Assets/Scripts/GameManager
        - Vedi script: Assets/Scripts/GameMusicManager
        - Vedi script: Assets/Scripts/GameSettings
        - Vedi script: Assets/Scripts/GameUIManager
    - Coroutines
        - Vedi script: Assets/Scripts/GameUIManager -> Numero di coroutine: 3
        - Vedi script: Assets/Scripts/Entities/Attacks/CubeAttack -> Numero di coroutine: 1
        - Vedi script: Assets/Scripts/Entities/Attacks/SphereAttack -> Numero di coroutine: 2
        - Vedi script: Assets/Scripts/GameManager -> Numero di coroutine: 2
        - Vedi script: Assets/Scripts/MainMenuUIManager  -> Numero di coroutine: 1
    - Enums
        - Vedi script: Assets/Scripts/Entities/HitType
        - Vedi script: Assets/Scripts/Entities/PowerUpType
        - Vedi script: Assets/Scripts/GameEvents
    - Classi statiche
        - Vedi script: Assets/Scripts/GameEvents
        - Vedi script: Assets/Scripts/Utils/GeneralUI
    - Presenza di ereditarietà
        - Vedi script: Assets/Scripts/Entities/Attacks/CubeAttack -> estende Assets/Scripts/Entities/Attacks/AttackController
        - Vedi script: Assets/Scripts/Entities/Attacks/SphereAttack -> estende Assets/Scripts/Entities/Attacks/AttackController
        - Vedi script: Assets/Scripts/Entities/Player-> estende Assets/Scripts/Entities/PlayableEntity
        - Vedi script: Assets/Scripts/Entities/Enemy-> estende Assets/Scripts/Entities/PlayableEntity
        - Vedi script: Assets/Scripts/Entities/PlayableEntity-> estende Assets/Scripts/Entities/LivingEntity
        - I singleton elencati sopra estendono Assets/Scripts/Utils/Singleton o Assets/Scripts/Utils/UnloadableSingleton
    - Delegates
        - Vedi script: Assets/Scripts/Entities/Attacks/AttackController-> Numero di delegate: 3
        - Vedi script: Assets/Scripts/Entities/Animations/OnAnimationFireDelegate -> Numero di delegate: 2
        - Vedi script: Assets/Scripts/GameEvents -> Numero di delegate: 6
    - Eventi
        - Vedi script: Assets/Scripts/Entities/Animations/CubeAnimation-> Numero di event: 2
        - Vedi script: Assets/Scripts/Entities/Animations/SphereAnimation-> Numero di event: 2
        - Vedi script: Assets/Scripts/GameEvents -> Numero di event: 9
    - Animazioni originali
        - Vedi animation: Assets/Animations/CubePlayer/CubePlayer_Primary
        - Vedi animation: Assets/Animations/CubePlayer/CubePlayer_Secondary
        - Vedi animation: Assets/Animations/SpherePlayer/SpherePlayer_Primary
        - Vedi animation: Assets/Animations/SpherePlayer/SpherePlayer_Secondary
    - Sound

        - Soundtrack
        - Vedi assets: Assets/Audio/Music/match_game
        - Vedi assets: Assets/Audio/Music/menu_game
        - Altri suoni (usati in Assets/Scripts/Entities/Attacks/CubeAttack e Assets/Scripts/Entities/Attacks/SphereAttack)
        - Vedi assets: Assets/Audio/SFX/fall.wav
        - Vedi assets: Assets/Audio/SFX/fast_whoosh.mp3
        - Vedi assets: Assets/Audio/SFX/gunshot.wav
        - Vedi assets: Assets/Audio/SFX/wind_swoosh.mp3
    - Raycast
        - Vedi script: Assets/Scripts/Entities/Attacks/AttackController
    - User Interface
        - Vedi scena: GameScene -> Canvas

- **Extra**
    - Particelle
        - Vedi assets: Assets/FX/ProjectileFX.prefab