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

	public GameObject torso;
	public Torso torso_script;

	public GameObject eye;
	public GameObject mouth;

	List<ConfigurableJoint> joints = new List<ConfigurableJoint>();

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

	public Eye eye_script;
	public Vector3 target_direction;

	// Directional movement should be emergent rather than explicit
	private Quaternion lookRotation;
	private float sine;
	private Vector3 direction;

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

		torso = CreateTorso();
		eye = CreateEye();
		mouth = CreateMouth();

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

	GameObject CreateTorso()
	{
		GameObject torso = CreateMorphology(chromosome.getGraph(), null, 0, 0);

		torso.name = "torso";
		torso.transform.parent = transform;
		torso.transform.position = transform.position;
		torso.transform.eulerAngles = transform.eulerAngles;

		//torso.AddComponent<Rigidbody>();
		torso_script = torso.AddComponent<Torso>();
		torso_script.setColour(chromosome.getBodyColour());
		torso_script.setScale(chromosome.getBodyScale());
		//torso.rigidbody.mass = 15F;
		torso.GetComponent<Rigidbody>().angularDrag = settings.angular_drag;
		// If drag is too high, creatures go nowhere; too low, and they fly off
		torso.GetComponent<Rigidbody>().drag = settings.drag;
		// Are creatures made of lead (40) or styrophoam (0.4)?
		torso.GetComponent<Rigidbody>().SetDensity(4F);
		return torso;
	}

	/**
	 * Traverse the graph and create the morphology.
	 */
	GameObject CreateMorphology(GenotypeNode node, GameObject parent, int depth, int childIndex)
	{
		GameObject segment =  GameObject.CreatePrimitive(PrimitiveType.Cube);
		segment.layer = LayerMask.NameToLayer("Creature");
		segment.name = "part_" + depth + "_" + childIndex;
		segment.transform.parent = transform;  // incorporate transformFromParent

		Limb segment_script = segment.AddComponent<Limb>();
		segment_script.setScale((Vector3) node.dimensions);
		segment_script.setColour((Color) chromosome.getLimbColour());

		if (parent)
		{
			segment.transform.LookAt(parent.transform);
			segment_script.setPosition(parent.transform.localPosition);
			segment.transform.Translate(0, 0, -parent.transform.localPosition.z);
		}
		else
		{
			segment_script.setPosition((Vector3)node.translateFromParent); // ?
		}
	
		int idx = 0;
		foreach (GenotypeNode child in node.children)
		{
			//GameObject childSegment = 
			CreateMorphology(child, segment, depth + 1, idx++);
		}

		segment.AddComponent<Rigidbody>();
		segment.AddComponent<BoxCollider>();
		segment.GetComponent<Collider>().material = (PhysicMaterial)Resources.Load("Physics Materials/Creature");

		
		if (parent)
		{
			ConfigurableJoint joint = segment.AddComponent<ConfigurableJoint>();
			joint.axis = new Vector3(0.5F, 0F, 0F);
			joint.anchor = new Vector3(0F, 0F, 0.5F);
			joint.breakForce = 1000.0f;  // lower this to make limbs break off; joint.breakTorque = 10.0f;

			joint.connectedBody = parent.GetComponent<Rigidbody>();

			joints.Add(joint);

			joint.xMotion = ConfigurableJointMotion.Locked;
			joint.yMotion = ConfigurableJointMotion.Locked;
			joint.zMotion = ConfigurableJointMotion.Locked;

			joint.angularXMotion = ConfigurableJointMotion.Free;
			joint.angularYMotion = ConfigurableJointMotion.Free;
			joint.angularZMotion = ConfigurableJointMotion.Free;

			JointDrive angXDrive = new JointDrive();
			//angXDrive.mode = JointDriveMode.Position;
			angXDrive.positionSpring = 7F;
			// If this gets down around 0.001, they look sleepy. Two high and population declines.
			angXDrive.maximumForce = 10.0F;

			joint.angularXDrive = angXDrive;
			joint.angularYZDrive = angXDrive;
		}
		else
		{
			//joint.connectedBody = segment.GetComponent<Rigidbody>();
		}
		segment.GetComponent<Rigidbody>().drag = 1F;


		segment.GetComponent<Rigidbody>().SetDensity(1F);

		return segment;
	}

	GameObject CreateEye()
	{
		GameObject eye = new GameObject();
		eye.name = "Eye";
		eye.transform.parent = torso.transform;
		eye.transform.eulerAngles = torso.transform.eulerAngles;
		eye.transform.position = torso.transform.position;
		eye_script = eye.AddComponent<Eye>();
		return eye;
	}

	GameObject CreateMouth()
	{
		GameObject mouth = new GameObject();
		mouth.name = "Mouth";
		mouth.transform.parent = torso.transform;
		mouth.transform.eulerAngles = torso.transform.eulerAngles;
		mouth.transform.localPosition = new Vector3(0, 0, .5F);
		mouth.AddComponent<Mouth>();
		return mouth;
	}

	/**
	 * Physics update
	 * // TODO: Find a better way of controlling the joints with wave functions
	 *				the current way needs some sort of magic scalar.
	 * The physics should really be purely dynamic, instead of this kinematic stuff.
	 * The creature movement should be a result of water resistance when paddling. 
	 */
	void FixedUpdate () {
		sine = Sine(joint_frequency, joint_amplitude, joint_phase);
		for (int i=0; i<joints.Count; i++) {
			if (joints[i] != null)
				joints[i].targetRotation = Quaternion.Euler (sine * new Vector3(5F, 0F, 0F));
		}

		if (eye_script.goal) {
			// The unit vector toward the object in sight should only be used as input to neural network.
			target_direction = (eye_script.goal.transform.position - torso.transform.position).normalized;
		}

		if (target_direction != Vector3.zero) {
			// turn toward the goal. This is faking it. It needs to be emergent.
			lookRotation = Quaternion.LookRotation(target_direction);
		}

		float abs_sine = Mathf.Abs(sine);
		float pos_sine = System.Math.Max(sine,0);
		// interpolates toward where it is looking
		torso.transform.rotation = Quaternion.Slerp(torso.transform.rotation, lookRotation, Time.deltaTime * abs_sine * 3F);

		if (pos_sine == 0) {
			direction = torso.transform.forward;
		}

		// totally fake. Needs fixing.
		torso.GetComponent<Rigidbody>().AddForce(Mathf.Abs(force_scalar) * direction * pos_sine * 2 /*chromosome.getBranchCount()*/);
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
		Lighten();
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

		State proposedNewState = (eye_script.targetFbit != null) ? State.persuing_food : State.searching_for_food;
		if (proposedNewState != state) {
			ChangeState(proposedNewState);
		}
	}

	public void SetChromosome (Chromosome gs) {
		this.chromosome = gs;
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

	private void Lighten()
	{
		torso.GetComponent<MeshRenderer>().material.color = torso_script.original_colour;
		//foreach (Limb l in all_limbs)
		//	l.GetComponent<MeshRenderer>().material.color = l.original_colour;
	}

	private void ResetSpeed()
	{
		joint_frequency = chromosome.base_joint_frequency;
		force_scalar = 1F;
	}
}
