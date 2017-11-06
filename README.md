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
## First Milestone: Two guys throwing grenades with timers
[X] check off when done
* [ ] Menu system for attacking: throw and grenade types
  * Take a List of IMenuItem (interface to be attached to throw, skill or grenade types?)
  * Menu has a selected index (wrap between 0-List.Count?)
  * Select event to message to UI a selection has been made
  * ChangeSelection event to message to UI what is selected
* [ ] Grenade class
  * Has a timer which can be modified by skills
  * Timer ticks every round
  * First milestone has timers at 1 round, basically unplayable, but a proof of concept
* [ ] Character class
  * Default list of IMenuItem to display options
    * Throw Grenade (for this milestone)
  * Sprite name
  * Is health necessary? If 1 hit kill, nope
* [ ] Turn System
  * Singleton
  * ChangeTurn event for when turns change
    * Grenades can hook into this to tick down
	* Menu system can use this to display options
  * CurrentCharacter to return whos turn it is
  * Only need 2 teams
  * Turn counter
___