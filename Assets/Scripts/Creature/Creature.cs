using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


/**
 * Creatures swim around, mate and eat. 
 * The ones that survive the longest, reproduce and pass on their genes.
 *	Author: Craig Lomax
 *	Author: Barry Becker
 */
public class Creature : MonoBehaviour
{
	Transform _t;

	Settings settings;
	Ether eth;

	public GameObject body;
	public Body body_script;

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
	int age_sexual_maturity;

	public int num_offspring;
	public int food_eaten;

	private ArrayList limbs;
	private ArrayList all_limbs;

	// These should be part of each joint
	private float joint_frequency;
	private float joint_amplitude;
	private float joint_phase;
	private float force_scalar = 1F;

	public delegate void CreatureState(Creature c);
	public static event CreatureState CreatureDead;

	// TODO: Fix this "state machine"
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
		_t = transform;
		name = "creature" + gameObject.GetInstanceID();

		eth = Ether.getInstance();
		settings = Settings.getInstance();

		joint_frequency = chromosome.base_joint_frequency;
		joint_amplitude = chromosome.base_joint_amplitude;
		joint_phase = chromosome.base_joint_phase;

		body = CreateBody();
		eye = CreateEye();
		mouth = CreateMouth();

		line_of_sight = settings.line_of_sight;
		metabolic_rate = settings.metabolic_rate;
		age_sexual_maturity = settings.age_sexual_maturity;

		all_limbs = new ArrayList();
		setupLimbs();

		age = 0.0;
		ChangeState(State.searching_for_food);
		food_eaten = 0;
		num_offspring = 0;
		low_energy_threshold = settings.low_energy_threshold;

		// timers
		InvokeRepeating("updateState", 0, 0.1f);

		ms = GetComponentsInChildren<MeshRenderer>();
	}

	GameObject CreateBody()
	{
		GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
		body.name = "body";
		body.transform.parent = _t;
		body.transform.position = _t.position;
		body.transform.eulerAngles = _t.eulerAngles;
		body.AddComponent<Rigidbody>();
		body_script = body.AddComponent<Body>();
		body_script.setColour(chromosome.getBodyColour());
		body_script.setScale(chromosome.getBodyScale());
		//body.rigidbody.mass = 15F;
		body.GetComponent<Rigidbody>().angularDrag = settings.angular_drag;
		// If drag is too high, creatures go nowhere; too low, and they fly off
		body.GetComponent<Rigidbody>().drag = settings.drag;
		// Are creatures made of lead (40) or styrophoam (0.4)
		body.GetComponent<Rigidbody>().SetDensity(4F);
		return body;
	}

	GameObject CreateEye()
	{
		GameObject eye = new GameObject();
		eye.name = "Eye";
		eye.transform.parent = body.transform;
		eye.transform.eulerAngles = body.transform.eulerAngles;
		eye.transform.position = body.transform.position;
		eye_script = eye.AddComponent<Eye>();
		return eye;
	}

	GameObject CreateMouth()
	{
		GameObject mouth = new GameObject();
		mouth.name = "Mouth";
		mouth.transform.parent = body.transform;
		mouth.transform.eulerAngles = body.transform.eulerAngles;
		mouth.transform.localPosition = new Vector3(0, 0, .5F);
		mouth.AddComponent<Mouth>();
		return mouth;
	}

	/**
	 * Physics update
	 * // TODO: Find a better way of controlling the joints with wave functions
	 *				the current way needs some sort of magic scalar
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
			target_direction = (eye_script.goal.transform.position - body.transform.position).normalized;
		}

		if (target_direction != Vector3.zero) {
			// turn toward the goal. This is faking it. It needs to be emergent.
			lookRotation = Quaternion.LookRotation(target_direction);
		}

		float abs_sine = Mathf.Abs(sine);
		float pos_sine = System.Math.Max(sine,0);
		// interpolates toward where it is looking
		body.transform.rotation = Quaternion.Slerp(body.transform.rotation, lookRotation, Time.deltaTime * abs_sine * 3F);

		if (pos_sine == 0) {
			direction = body.transform.forward;
		}

		// toally fake. Needs fixing.
		body.GetComponent<Rigidbody>().AddForce(Mathf.Abs(force_scalar) * direction * pos_sine * chromosome.getBranchCount());
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
		{
			throw new System.InvalidOperationException("Chromosome is null");
		}
			
		if (energy < chromosome.hunger_threshold) {
			ChangeState((eye_script.targetFbit != null) ? State.persuing_food : State.searching_for_food);
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

	// TODO: Limbs should be made into a better tree structure, not this
	// 	list of lists rubbish. Also, add symmetry
	private void setupLimbs () {
		int num_branches = chromosome.getBranchCount();
		chromosome.setNumBranches(num_branches);

		for (int i=0; i<num_branches; i++)
		{
			limbs = chromosome.getLimbs(i);
			List<GameObject> actual_limbs = new List<GameObject>();

			for (int j=0; j<limbs.Count; j++) {
				GameObject limb = GameObject.CreatePrimitive(PrimitiveType.Cube);
				limb.layer = LayerMask.NameToLayer("Creature");
				limb.name = "limb_" + i + "_" + j;
				limb.transform.parent = _t;
				actual_limbs.Add(limb);
				Limb limb_script = limb.AddComponent<Limb>();

				ArrayList attributes = (ArrayList) limbs[j];
				limb_script.setScale( (Vector3) attributes[1] );
				limb_script.setColour( (Color) chromosome.getLimbColour());

				if (j == 0) {
					limb_script.setPosition( (Vector3) attributes[0] );
					limb.transform.LookAt(body.transform);
				} else {
					limb_script.setPosition( actual_limbs[j-1].transform.localPosition );
					limb.transform.LookAt(body.transform);
					limb.transform.Translate(0,0,-actual_limbs[j-1].transform.localScale.z);
				}

				limb.AddComponent<Rigidbody>();
				limb.AddComponent<BoxCollider>();
				limb.GetComponent<Collider>().material = (PhysicMaterial)Resources.Load("Physics Materials/Creature");

				ConfigurableJoint joint = limb.AddComponent<ConfigurableJoint>();
				joint.axis = new Vector3(0.5F, 0F, 0F);
				joint.anchor = new Vector3(0F, 0F, 0.5F);
				joint.breakForce = 1000.0f;  // lower this to make limbs break off
				//joint.breakTorque = 10.0f;
				if (j == 0) {
					joint.connectedBody = body.GetComponent<Rigidbody>();
				} else {
					joint.connectedBody = actual_limbs[j-1].GetComponent<Rigidbody>();
				}
				limb.GetComponent<Rigidbody>().drag = 1F;

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

				limb.GetComponent<Rigidbody>().SetDensity(1F);

				all_limbs.Add(limb_script);
			}
		}
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
		body.GetComponent<MeshRenderer>().material.color = body_script.original_colour;
		foreach (Limb l in all_limbs)
			l.GetComponent<MeshRenderer>().material.color = l.original_colour;
	}

	private void ResetSpeed()
	{
		joint_frequency = chromosome.base_joint_frequency;
		force_scalar = 1F;
	}
}
