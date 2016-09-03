using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/**
 * Creatures swim around and eat food. The ones that survive the longest, reproduce and pass on their genes.
 *	Author: Craig Lomax
 *	Author: Barry Becker
 */
public class Creature : MonoBehaviour
{
	Settings settings;
	Ether eth;

	public Phenotype phenotype;

	public double age;
	public float energy;
	// The creature dies if its energy gets below this for too long.
	public float low_energy_threshold;

	public Chromosome chromosome;

	public double line_of_sight;
	// Instead a fixed rate of burning energy, it should be dependent on forces in physical movement
	float metabolic_rate;

	public int num_offspring;
	public int food_eaten;

	// These should be part of each joint
	private float joint_frequency;
	private float joint_amplitude;
	private float joint_phase;
	private float force_scalar = 1F;

	public delegate void CreatureState(Creature c);
	public static event CreatureState CreatureDead;

	// Possible creature states.
	public enum State {
		persuing_food,
		eating,
		searching_for_food,
		dead
	};
	public State state;
	private bool state_lock = false;

	private bool low_energy_lock = false;
	private MeshRenderer[] ms;
	private float metabolise_timer = 1F;

	void Start()
	{
		name = "creature" + gameObject.GetInstanceID();

		eth = Ether.getInstance();
		settings = Settings.getInstance();

		joint_frequency = chromosome.base_joint_frequency;
		joint_amplitude = chromosome.base_joint_amplitude;
		joint_phase = chromosome.base_joint_phase;

		phenotype = new Phenotype(chromosome, transform);

		line_of_sight = settings.line_of_sight;
		metabolic_rate = settings.metabolic_rate;

		age = 0.0;
		ChangeState(State.searching_for_food);
		food_eaten = 0;
		num_offspring = 0;
		low_energy_threshold = settings.low_energy_threshold;

		// timers
		InvokeRepeating("updateState", 0, 0.1f);

		ms = GetComponentsInChildren<MeshRenderer>();
	}

	/** Physics update  */
	void FixedUpdate()
	{
		float sine = Sine(joint_frequency, joint_amplitude, joint_phase);
		phenotype.FixedUpdate(sine, force_scalar);
	}

	/**
	 * Sinusoidal frequency slows as the creature ages. Should it?
	 */
	float Sine(float freq, float amplitude, float phase_shift) {
		return Mathf.Sin((float)age * freq + phase_shift) * amplitude;
	}

	/**
	 * Animation update
	 */
	void Update()
	{
		age += Time.deltaTime;

		metabolise_timer -= Time.deltaTime;
		if (metabolise_timer <= 0 && state != State.dead)
		{
			metabolise();
			metabolise_timer = 1F;
		}

		if (energy <= low_energy_threshold && !low_energy_lock)
			StartShuttingDown();

		if (energy > low_energy_threshold)
			AbortShutDown();

		float original_force = force_scalar;
		float original_joint_frequency = joint_frequency;
		if (state == State.eating)
		{
			joint_frequency = 0F;
			force_scalar = 0F;
		}
		else
		{
			joint_frequency = original_joint_frequency;
			force_scalar = original_force;
		}

		if (state == State.dead)
			kill();
	}

	private void StartShuttingDown()
	{
		low_energy_lock = true;
		StartCoroutine(SlowDown());
		StartCoroutine(Darken());
	}

	private void AbortShutDown()
	{
		state_lock = false;
		low_energy_lock = false;
		StopCoroutine(SlowDown());
		StopCoroutine(Darken());
		phenotype.Lighten();
		ResetSpeed();
	}

	public void setEnergy(float n) {
		energy = n;
	}

	/**
	 * Searching for food if not pursuing food.
	 */
	void updateState() {
		if (chromosome == null)
			throw new System.InvalidOperationException("Chromosome is null");

		State proposedNewState = phenotype.HasTargetFood() ? State.persuing_food : State.searching_for_food;
		if (proposedNewState != state) {
			ChangeState(proposedNewState);
		}
	}

	public void SetChromosome (Chromosome gs) {
		this.chromosome = gs;
	}

	public Eye getEye()
	{
		return phenotype.eye.GetComponent<Eye>();
	}

	public GameObject getTorsoObject()
	{
		return phenotype.torso;
	}

	public void ChangeState(State s)
	{
		if (!state_lock)
			state = s;
	}

	/*
	 * Return the current energy value for the creature
	 */
	public float getEnergy () {
		return energy;
	}

	/*
	 * Remove a specified amount of energy from the creature,
	 * kill it if the creature's energy reaches zero.
	 *
	 * Return: true if energy is now below zero
	 */
	public bool subtractEnergy(float n)
	{
		bool equal_or_below_zero = false;
		
		energy -= n;
		if (energy <= 0.0)
		{
			eth.addEnergy(energy + n);
			energy = 0;
			equal_or_below_zero = true;
			state = State.dead;
			state_lock = true;
		}
		else
		{
			eth.addEnergy(n);
		}

		return (equal_or_below_zero);
	}

	/*
	 * Remove energy from the creature for merely existing,
	 * return it to the ether.
	 */
	private void metabolise () {
		subtractEnergy(metabolic_rate);
	}

	/*
	 * Remove the creature from existence and return
	 * the creature's energy.
	 */
	public void kill () {
		subtractEnergy(energy);
		CreatureDead(this);
		Destroy(gameObject);
	}

	float d_col = 0.01F;
	private IEnumerator Darken()
	{
		float col = 1F;
		while (col > 0.15F && energy < low_energy_threshold)
		{
			foreach (var m in ms)
			{
				m.material.color -= new Color(d_col, d_col, d_col);
			}
			col -= d_col;
			yield return new WaitForSeconds(0.025F);
		}
	}

	float d_freq = 0.01F;
	float d_force = 0.01F;

	private IEnumerator SlowDown()
	{
		float freq = joint_frequency;
		while (freq > 0.15F && energy < low_energy_threshold)
		{
			freq -= d_freq;
			joint_frequency = freq;
			if(force_scalar > 0F)
				force_scalar -= d_force;
			yield return new WaitForSeconds(0.025F);
		}
	}

	private void ResetSpeed()
	{
		joint_frequency = chromosome.base_joint_frequency;
		force_scalar = 1F;
	}
}
