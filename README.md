# Magical Girl: HARMONY

**Solo stealth-action prototype focused on enemy AI behavior, infiltration gameplay, and player feedback systems.**

This project demonstrates core gameplay programming skills through the implementation of modular AI, stealth mechanics, and responsive combat systems.

---

## 🧠 Gameplay Systems Implemented

- Enemy AI patrol, search, chase, and attack states using a **finite state machine architecture**  
- Line-of-sight and auditory detection with awareness escalation  
- Stealth-to-combat transition logic with scoring and feedback  
- Weapon handling, reload logic, and combat cooldown systems  
- Health, mana, and consumable item management  
- Modular UI feedback architecture for player awareness

---

## 🏗 Architecture Notes

- **State-driven AI design** for extensibility and behavior isolation  
- Clear separation between:
  - Player input  
  - Combat resolution  
  - UI feedback  
- **ScriptableObject-driven configuration** for tuning gameplay parameters

---

## 🛠 Tech Stack

- Unity (C#)  
- NavMeshAgent (2.5D navigation)  
- Object pooling for projectiles  
- Custom Gizmo debugging tools


