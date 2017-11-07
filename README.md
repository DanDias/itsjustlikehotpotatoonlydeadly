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
## Second Milestone: Clean up a bit
* [X] Dead people shouldn't throw grenades
* [X] Don't target dead people... I mean come on. They aren't going to get more dead.
* [ ] Grenade blocks the person. Probably should appear in their hand and smaller instead of over them.
* [X] Remove expended grenades after you can see the BOOM!
___
## Third Milestone: Multiple options
* [ ] Menu system for attacking: throw and grenade types
  * Take a List of IMenuItem (interface to be attached to throw, skill or grenade types?)
  * Menu has a selected index (wrap between 0-List.Count?)
  * Select event to message to UI a selection has been made
  * ChangeSelection event to message to UI what is selected
* [ ] Default list of IMenuItem to display options
  * Basic Throw
  * Skills
___
## Unsorted Ideas
* Some kind of timer, maybe not numbers... like the idea of not knowing when it'll explode
* Juggling skill, maybe hold more than one grenade. Maybe create an additional grenade when you already have one?
* Grenademancy, revive a dead grenade? I don't know but I like the name
* Change to round based timer ticks
* Knockdown
___
## Completed Milestones
## First Milestone: Two guys throwing grenades with timers (Completed)
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
___
Used Sprites:
http://gaurav.munjal.us/Universal-LPC-Spritesheet-Character-Generator/#?jacket=none&armor=chest_leather&mail=chain&legs=robe_skirt&hair=bedhead_white
https://openclipart.org/detail/284551/boom
https://openclipart.org/detail/172791/grenade