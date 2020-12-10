# Turn-Based-RPG-Combat-System

This is a basic turn based combat system made with C# and Unity 2019.1. Most of the work on this project was done back in August 2020. This is my first (personal) programming project which I made as a way to learn programming, using online resources such as documentation, forum posts and articles to find ways to implement certain bits of functionality.

Each side can have up to 3 combatants. Turns are sorted in order of the combatants' speed stat - faster combatants will have their turns earlier than slower ones. The battle is won if all of the enemies are killed, while the battle will be lost if all of the current combatants on the player's side are killed.

**Features**
- Switching - while only 3 combatants can be out on the player side, the party can contain up to 8 combatants, which can be switched into battle provided they aren't already in battle or dead.
- Types - each type has a list of weaknesses, resistances and immunities. Each combatant can have up to 2 elemental types. This affects what attacks they are weak, resistant or immune to.
- Analysis panels - the stats of each combatant in the fight can be viewed, including any stat changes.
- Auto attack mode - A mode in which the player combatants use regular attacks automatically. The game speed is also sped up to resolve battles quicker. In a full game, this would be useful for dealing with enemies much weaker (lower level) than the party.
