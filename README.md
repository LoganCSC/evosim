# Evosim
A three-dimensional, real time simulation of natural selection.

[Download executable from itch.io](http://rlomax.itch.io/evosim)

This is a proof of concept with the intention of testing how capable today's consumer computing power is simulating artificial genepools, which are likely to contain many 3D rigidbodies.

#### Usage
It is recommended to make sure that you have checked out to the `develop` branch since `master` is likely to be out of date.

Clone this repository and open the directory as an existing project in Unity3D, open the scene named 'Main' and make sure that the script named 'Main' is attached to the GameObject named 'Main' in the hierarchy.




## Documentation

The program is currently a 'toy'. It is not a scientific tool or a true reflection of evolution in the natural world, but will become so eventually.

#####You can:
See the internal state of each creature by clicking on it
Save/import creatures
Kill creatures (works better than a stress ball)

#####Usage suggestions:
Turn it on, do something else, come back in ~30 minutes to see the results of evolution.
Watch evolution unfold in real time. Have you ever caught yourself staring at a fish tank for hours? It's kind of like that.
Use it as a screensaver.

Movement is currently fake in that their are no forces aplied by the joints and movement of the articulated arme. There is just a single forcen that moves the body toawrd its current goal. This will be fixed.

### Settings:

All settings are contained in a JSON file in "evosim_Data/settings.json". All non-integer values must be written in the form "x.x", e.g. 3.0
There is no validation performed on these values (but there should be). 

##### Config:
* population_logging	- Whether you want the creature count over time to be logged to a data file
* foodbit_logging		- Whether you want the foodbit count over time to be logged to a data file
* log_time			- How often, in seconds, you want the data to be written to the data file
* Camera:
..* sensitivity		- Sensitivity of camera movement controls
..* invert			- 1 = invert camera, 0 = don't invert camera
* Environment:
..* tile_scale		- Controls the size of the tiles on the plane the creatures live on; purely cosmetic

##### Ether:
* total_energy				- How much energy is put into the system
* starting_number_foodbits	- How many foodbits the simulation starts with
* starting_creatures			- How many creatures the simulation starts with
* creature_spread				- The size of the area that the initial creatures are spawned in
* Foodbit:
..* init_energy					- How many units of energy a foodbit starts with
..* destroy_at					- How many units of energy at which a foodbit is destroyed
..* spore_time					- How often, in seconds, is a new foodbit spawned (if enough energy)
..* spore_range					- How far from the parent foodbit can a child spawn
..* wide_spread					- The size of the area that the initial foodbits are spawned in
..* decay_amount				- How many units of energy a foodbit decays by every
..* decay_time 					- How often, in seconds, does the foodbit decay
..* decay_rate					- The probability of a foodbit decaying at decay_time

#### Creature:
* MAX_ENERGY					- The absolute maximum units of energy a creature can have
* init_energy					- The amount of units of energy given to the initial creatures
		hunger_threshold			- The energy level at which the creature becomes hungry and looks for food
		line_of_sight				- How far a creature can see
		energy_to_offspring			- The percentage of energy each creature gives to its offspring
		metabolic_rate				- The amount of energy subtracted from a creature every second just for existing
		mate_range					- The range at which one creature can mate with another
		eat_range					- The range at which a creature can eat a foodbit
		eye_refresh_rate			- How often, in seconds, a creature looks around and updates what they can see
		age_sexual_maturity			- The age of a creature, in seconds, at which they can mate with another creature
		branch_limit				- How many limbs can a creature have branching from its main body part
		recurrence_limit			- How many limb segments each branch can have

		root:
			max_root_scale {x,y,z}	- The maximum size of the root body part when the initial creatures are generated
			min_root_scale {x,y,z}  - The minimum size of the root body part when the initial creatures are generated

		limb:
			max_limb_scale {x,y,z}	- The maximum size of a limb segment when the initial creatures are generated
			min_limb_scale {x,y,z}	- The minimum size of a limb segment when the initial creatures are generated

	Genitalia:
		"line_length"				- The length of the line when no mate is in line of sight

	Genetics:
		crossover_rate				- The probability of each gene being crossed over with a gene of a partner
		mutation_rate				- The probability of each gene being mutated
		mutation_factor				- By how much is a gene mutated




### License
Evosim by Craig Lomax is licensed under a [Creative Commons
Attribution-NonCommercial-ShareAlike 4.0 International
License](http://creativecommons.org/licenses/by-nc-sa/4.0/).
Modifications and extensions by Barry Becker
