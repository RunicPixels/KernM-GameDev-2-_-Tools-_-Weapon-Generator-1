# KernM-GameDev-2-_-Tools-_-Weapon-Generator-1
Credits Reserved to
https://github.com/CDeWijs (Not really used, more for debugging and testing her part) and
https://github.com/RubicalMe (Made the wonderful Editor Window preview my base class extends.)
for their respective parts in the Editor Scripts.

Kernmodule Game Development HKU, By Ronald van Egdom, studentnummer 3018119.

# Goal
I wanted to make a Unity tool that generates different results based on procedural generation and user input. I wanted it to have a smaller scope with a lot of extensibility.

# Idea
The idea is to make an Sprite based Weapon Generator. Which puts various "part" sprites on top of each other.
In the current version I have a sword generator that uses "Blades" on top, "Guards" in the middle and "Handles" on the bottom.

# Tool Structure and Scene Flow

A variety of things are customizable in the current version.
## Part Based :
- Randomize Shape of a part.
This will look in the supplemented resources folder in order to find all sprites in the respective part folder. Put those in an array and a random one.
- Randomize Hue of a part.
This will take the supplemented shape and put them through a still extendable color modification method. For now it randomizes the hue of the blade if it is Selected.
- Shape form
Manually select which shape you want to use, toggling down randomized shape will use that shape in your next generation.

## Weapon Based :
- Weapon name
Will save the file in the root asset folder with this variable als image name.
- Seed
 Parses a string into a seed that will make Unity uses that seed with it's random generation and thus output results based on the seed.

# Learning Goals
I want to learn about the following aspects of game development.
- Procedural Generation
- Unity Tools implementation
- Unity Editor Extension.
- Sharing tools to help artist and designers.
- Make tools that help generate game content.


