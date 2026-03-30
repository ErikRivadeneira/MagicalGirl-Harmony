# Magical Girl: HARMONY

Magical Girl: HARMONY is a stealth-action prototype focused on **enemy AI systems**, **state-driven behavior**, and **player feedback mechanics**.

The project explores how modular AI and perception systems can support dynamic gameplay through **clear state transitions**, **predictable rules**, and **responsive combat interactions**.

## Project Status
This prototype is complete and maintained as a **portfolio project** demonstrating AI system design and real-time gameplay logic.

## Overview
The core gameplay is driven by an **enemy AI system** that reacts to player actions through perception and state transitions.

Enemies operate using a structured behavior model, moving between patrol, search, chase, and attack states depending on player visibility and noise.

The system is **state-driven and deterministic**, ensuring consistent behavior while allowing dynamic responses to player input.

## Core Design Goals
* Build a **modular AI system** using state-driven architecture  
* Implement **perception systems** for vision and sound detection  
* Support smooth **stealth-to-combat transitions**  
* Provide **clear feedback** for player awareness and enemy state  
* Maintain **extensible and maintainable behavior logic**  

## Systems Overview

### Enemy AI System
The AI is structured around a **finite state machine (FSM)** controlling behavior.

* Patrol, search, chase, and attack states  
* State transitions driven by perception and player interaction  
* Behavior logic encapsulated within individual state implementations  

### Perception System
Handles how enemies detect the player.

* **Line-of-sight detection** for visibility  
* **Auditory detection** through noise events  
* **Alert escalation system** controlling AI awareness levels  

### Combat System
Manages enemy engagement and player interaction.

* Weapon handling and shooting logic  
* Reload and cooldown systems  
* Resource-based interactions tied to gameplay flow  

### Player Systems
Supports interaction with AI and the environment.

* Health and resource management  
* Feedback systems for player awareness  
* Integration with stealth and combat mechanics  

### Feedback Systems
Communicates game state to the player.

* Visual indicators for alert levels  
* UI feedback tied to AI awareness  
* Debug visualization using custom Gizmos  

## Architecture

The project follows a **state-driven architecture** centered around AI behavior and real-time system interaction.

* **Finite State Machine (FSM)** controls enemy behavior and transitions  
* **Centralized AI controller** manages perception, combat, and state updates  
* **State encapsulation** separates behavior logic into individual components  
* **Supporting systems** (perception, combat, feedback) interact through shared state data  

This structure enables flexible behavior extension while maintaining clear control flow.

## Implementation Notes

* AI behavior is implemented using a **state interface pattern**  
* Perception combines **vision and audio inputs** for detection logic  
* Navigation is handled using **NavMeshAgent**  
* **Object pooling** is used to optimize projectile performance  
* Custom **Gizmos** are used for debugging AI behavior and detection systems  

## Constraints
The project was developed within a **limited timeframe** with a focus on system implementation over content volume.

This resulted in:

* Emphasis on **core AI systems**  
* Simplified gameplay scenarios to highlight behavior logic  
* Focus on **functionality and responsiveness** over polish  

## Key Takeaways

* **State-driven AI systems** enable scalable and maintainable behavior design  
* Combining perception inputs improves **responsiveness and realism**  
* Clear feedback systems enhance **player understanding of AI behavior**  
* Performance optimizations such as **object pooling** are essential in real-time systems  
