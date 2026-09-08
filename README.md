# Dreamers — Run Beyond Limits

> **"Every Dream Has A Journey."**

**Dreamers** is a story-driven 3D endless-runner adventure game developed in **Unity 6 (C#)** for **Android (Portrait Orientation)**.

---

## 🌟 Game Vision & Pillars
1. **Narrative & Purpose**: Running is not just a game loop—it represents the effort, struggles, growth, and milestones of 5 Computer Science & Engineering students striving to achieve their career dreams.
2. **Modular Character System**: Five distinct protagonists with individual storylines, career themes, and unique abilities sharing a common high-performance runner engine:
   - **Lingaraj** — *Entrepreneurship* (Venture Boost)
   - **Bhuvanesh** — *Game Development* (Glitch Phase)
   - **Meeha** — *Full-Stack Development* (Full-Stack Shield)
   - **Vedika** — *Cloud Engineering* (Cloud Surge)
   - **Lakashna** — *AI Engineering* (Neural Optimizer)
3. **Structured Progression**: Story Mode (finite level objectives with narrative cutscenes), Endless Mode (infinite high-score speed runs), and Challenge Mode (special modifiers).
4. **Clean, Zero-Allocation Architecture**: Data-driven design using ScriptableObjects, Event Channels, and Object Pooling to ensure a smooth 60 FPS on Android mobile devices.

---

## 📁 Repository Structure
```text
Dreamers/
├── Assets/
│   ├── Dreamers/
│   │   ├── Art/            # 3D models, characters, environments, obstacles, collectibles, props
│   │   ├── Animations/     # Animation clips, locomotion, action states, animator controllers
│   │   ├── Audio/          # BGM, dynamic mixer channels, sound effects, voice lines
│   │   ├── Data/           # ScriptableObjects (Characters, Dreams, Chapters, Levels, Missions)
│   │   ├── Materials/      # URP materials
│   │   ├── Textures/       # Optimized mobile texture atlases
│   │   ├── Shaders/        # Mobile-friendly URP shaders
│   │   ├── VFX/            # Visual particle systems & trail effects
│   │   ├── Prefabs/        # Assembled prefabs for gameplay, environment chunks, and UI
│   │   ├── Scenes/         # SCN_Boot, SCN_MainMenu, SCN_Gameplay
│   │   ├── Scripts/        # Clean C# modular architecture (Core, Data, Gameplay, UI, Save, etc.)
│   │   ├── Editor/         # Custom inspectors, procedural track validators, level authoring tools
│   │   └── Tests/          # Unity Test Framework (EditMode & PlayMode suites)
│   └── ThirdParty/         # Open-source packages and dependencies only
├── Documentation/          # Complete GDD, Technical Architecture, Character & Level docs
└── README.md
```

---

## 🛠️ Tech Stack & Constraints
- **Engine**: Unity 6 LTS
- **Language**: C# (.NET Standard / Unity Core)
- **Target Platform**: Android (Portrait)
- **Render Pipeline**: Universal Render Pipeline (URP) with SRP Batcher
- **License / Dependencies**: 100% Free & Open-Source assets/tools only.
