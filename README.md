# It's Just Like Hot Potato... Only Deadly
A turn based combat game where you throw grenades at and back at each other.
## Overall Design
* Final Fantasy-like battle system.
* Two teams of three (or four?) characters
* Each grenade thrown has a timer, skills change the timer as well as the skills of the enemy
* 1 hit kill (?)
* Throw can knock down characters
  * Knockdown makes it so you cant catch grenades
___
## Milestones
[X] check off when done
___
## Fourth Milestone: Skills
* [X] Change to round based timer ticks for skill variation
* [X] Implement Skill inheritance model
  * Execute overriding
  * Basic variables: Name, Description, Source, Mode, Cooldown, MaxCooldown, CharacterTargets, GrenadeTargets
* [X] Implement some skills
  * Knockdown
  * Hold it... you just dont throw it (Cook)
___
## Unsorted Ideas
* Skill: Grenademancy, revive a dead grenade? I don't know but I like the name
* Skill: Throwing more than 1 grenade
* Skill: something to let you take more than 1 hit
* Skill: counter, like you quickly throw it back
* Skill: dodge. I was thinking the grenade just goes away, like a miss
* Skill: Intercept. So you jump in front
* Skill: A Russian roulette throw... you throw it in the air and next turn it falls on someone random and explodes
* Skill: Heavy grenade, takes 2 turns to throw. And it makes its scale * 2
* Skill: Deflect... that one sends it to a party member... its cheap in points but no one likes you. Description: who needs friends when youve got grenades
* Skill: The Big One... kills everyone on the one side... maybe OP. Or maybe it's the only skill you can have.
* Skill: Quick throw, throw 2 of your grenades but cant throw next turn
* Skill: Incendiary grenades, when it explodes it catches the guy next to you on fire
  * What does being on fire do? Ticks all your nades down 1
* Skill: Spread em out - throws all of your grenades to everyone else... including your tream
* hover text on skills to display description variables
* Different types of grenades look different
* Make all nades have a 1% chance to be a dud?
* start to shake at either 1 or 2 ticks... 3 might be too much?
* I'm also thinking what if it took at least 1 round for the grenade to get to its target, so grenades could potentially explode on the way there
___
## Completed Milestones
## First Milestone: Two guys throwing grenades with timers
* [X] Grenade class
  * Has a timer which can be modified by skills
  * Timer tick when thrown
  * Can be basically unplayable, but a proof of concept
* [X] Character class
  * Sprite name
  * Is health necessary? If 1 hit kill, nope
* [X] Turn System
  * Singleton
  * ChangeTurn event for when turns change
	* Menu system can use this to display options
  * CurrentCharacter to return whos turn it is
  * Turn counter
## Second Milestone: Clean up a bit
* [X] Dead people shouldn't throw grenades
* [X] Don't target dead people... I mean come on. They aren't going to get more dead.
* [X] Grenade blocks the person. Probably should appear in their hand and smaller instead of over them.
* [X] Remove expended grenades after you can see the BOOM!
* [X] Some kind of timer, maybe not numbers... like the idea of not knowing when it'll explode
## Third Milestone: Multiple options - Done
* [X] Menu system for attacking: throw and grenade types
  * Take a List of IMenuItem (interface to be attached to throw, skill or grenade types?)
  * Menu has a selected index (wrap between 0-List.Count?)
* [X] Default list of IMenuItem to display options
  * Basic Throw
  * Skills
* [X] UI functionality
  * Put a workflow in so you can select your target and skill
___
Used Sprites:
http://gaurav.munjal.us/Universal-LPC-Spritesheet-Character-Generator/#?jacket=none&armor=chest_leather&mail=chain&legs=robe_skirt&hair=bedhead_white
https://openclipart.org/detail/284551/boom
https://openclipart.org/detail/172791/grenade

Used Libraries:
https://github.com/prime31/GoKit