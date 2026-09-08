# Mixamo & Real-World 3D Character Integration Guide
**Project:** Dreamers — Run Beyond Limits  
**Engine:** Unity 6 | **License Constraint:** 100% Free & Open-Source

---

## 1. 🌟 Free 3D Character Sources

### 1. Adobe Mixamo (100% Free)
- **URL**: [https://www.mixamo.com](https://www.mixamo.com)
- **What it provides**:
  - Free rigged 3D human characters matching our 5 CSE students (e.g. casual students, business suits, tech hoodies).
  - Motion-Captured animation clips for endless runners:
    - **`Running`** (Fast Run / Jog)
    - **`Jump`** (Jump Start, Air Loop, Landing)
    - **`Slide / Crouch`** (Baseball Slide / Running Slide)
    - **`Stumble / Trip`** (Stumble Backward / Forward)
    - **`Death / Crash`** (Hard Fall / Knockout)
    - **`Victory / Cheering`** (Celebration Dance)
- **Export Settings from Mixamo**:
  - Format: **`FBX for Unity (.fbx)`**
  - Skin: **`With Skin`** (for character model) or **`Without Skin`** (for additional animation clips)
  - Frames per Second: **`60 FPS`**

### 2. Ready Player Me (100% Free Pixar/Disney Stylized 3D Avatars)
- **URL**: [https://readyplayer.me](https://readyplayer.me)
- **What it provides**:
  - Full-body custom stylized 3D avatar creator matching the Pixar / Disney modern runner aesthetic (exact hair styles, open cardigans, hoodies, hoop earrings, chinos, sneakers).
  - 1-Click export to **`.glb`** (standard 3D format) with full Mixamo / Humanoid compatible skeletal rig!
- **How to use in Dreamers**:
  1. Customize your character on [readyplayer.me](https://readyplayer.me).
  2. Download the `.glb` model.
  3. Drag into Unity 6 (via UnityGLTF / UniVRM) or place in `Assets/Dreamers/Art/Characters/<CharacterName>/`.
  4. Change Rig to **Humanoid** — it instantly inherits all runner animations!

### 3. Quaternius & Kenney (CC0 Public Domain 3D Models)
- **URL**: [https://quaternius.com](https://quaternius.com) / [https://kenney.nl/assets](https://kenney.nl/assets)
- **What it provides**:
  - 100% Free CC0 modular city packs (skyscrapers, asphalt roads, streetlights, trees, subway trains, roadblocks).
  - Rigged modular animated characters.

---

## 2. 📁 Character Placement in Unity
Place your downloaded Mixamo FBX models directly into the corresponding folders:

```text
Assets/Dreamers/Art/Characters/
├── Lingaraj/       --> Place Lingaraj.fbx & textures here (Entrepreneur)
├── Bhuvanesh/      --> Place Bhuvanesh.fbx & textures here (Game Dev)
├── Meeha/          --> Place Meeha.fbx & textures here (Full-Stack)
├── Vedika/         --> Place Vedika.fbx & textures here (Cloud Eng)
└── Lakashna/       --> Place Lakashna.fbx & textures here (AI Eng)
```

---

## 3. ⚙️ Unity Rig Setup (Humanoid Avatar)
1. Select the character `.fbx` in Unity's Project window.
2. In the Inspector, switch to the **Rig** tab.
3. Change **Animation Type** to **`Humanoid`**.
4. Set **Avatar Definition** to **`Create From This Model`** $\rightarrow$ Click **Apply**.
5. All 5 characters can now share the exact same Universal Animator Controller (`AC_UniversalRunner`) via Humanoid Retargeting!

---

## 4. 🏙️ Realistic Real-World Environment Setup
To achieve a realistic look in Unity 6:
1. **Universal Render Pipeline (URP)**:
   - Enable **HDR Lighting** and **Soft Shadows**.
   - Use PBR Materials with **Albedo**, **Normal Maps**, and **Smoothness/Roughness**.
2. **Post-Processing Volume**:
   - **Tonemapping**: ACES (Film-grade color grading).
   - **Bloom**: Threshold 1.1, Intensity 0.35 (glowing streetlights & neon).
   - **Ambient Occlusion**: Screen Space Ambient Occlusion (SSAO) for realistic ground contact shadows.
