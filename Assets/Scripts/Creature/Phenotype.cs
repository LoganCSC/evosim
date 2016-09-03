using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/**
 * The creature phenotype is the physical manifestation of its genotype.
 * It is the body, limps, head, tail, and joints (etc).
 *	Author: Barry Becker
 */
public class Phenotype
{
	private Settings settings;
	private Chromosome chromosome;

	public GameObject torso;
	public GameObject eye;

	private Torso torso_script;
	private Eye eye_script;
	private GameObject mouth;
	private List<ConfigurableJoint> joints = new List<ConfigurableJoint>();
	private Transform transform;
	private Vector3 target_direction;
	private Vector3 direction;
	// Directional movement should be emergent rather than explicit
	private Quaternion lookRotation;

	/**
	 * Constructor
	 */
	public Phenotype(Chromosome chromo, Transform rootTrans)
	{
		settings = Settings.getInstance();
		chromosome = chromo;
		transform = rootTrans;
		torso = CreateTorso();
		eye = CreateEye(torso);
		mouth = CreateMouth(torso);
	}


	public void Lighten()
	{
		torso.GetComponent<MeshRenderer>().material.color = torso_script.original_colour;
		//foreach (Limb l in all_limbs)
		//	l.GetComponent<MeshRenderer>().material.color = l.original_colour;
	}

	public Boolean HasTargetFood()
	{
		return eye_script.targetFbit != null;
	}

	private GameObject CreateTorso()
	{
		GameObject torso = GameObject.CreatePrimitive(PrimitiveType.Cube);

		torso.name = "torso";
		torso.transform.parent = transform;
		torso.transform.position = transform.position;
		torso.transform.eulerAngles = transform.eulerAngles;

		torso.AddComponent<Rigidbody>();
		torso_script = torso.AddComponent<Torso>();
		torso_script.setColour(chromosome.getBodyColour());
		torso_script.setScale(chromosome.getBodyScale());

		//torso.rigidbody.mass = 15F;
		torso.GetComponent<Rigidbody>().angularDrag = settings.angular_drag;
		// If drag is too high, creatures go nowhere; too low, and they fly off
		torso.GetComponent<Rigidbody>().drag = settings.drag;
		// Are creatures made of lead (40) or styrophoam (0.4)?
		torso.GetComponent<Rigidbody>().SetDensity(4F);

		CreateMorphology(chromosome.getGraph(), torso, 0, 0);

		return torso;
	}

	private GameObject CreateEye(GameObject torso)
	{
		GameObject eye = new GameObject();
		eye.name = "Eye";
		eye.transform.parent = torso.transform;
		eye.transform.eulerAngles = torso.transform.eulerAngles;
		eye.transform.position = torso.transform.position;
		eye_script = eye.AddComponent<Eye>();
		return eye;
	}

	private GameObject CreateMouth(GameObject torso)
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
	 * Traverse the graph and create the morphology.
	 */
	private void CreateMorphology(GenotypeNode node, GameObject parent, int depth, int childIndex)
	{
		GameObject segment = GameObject.CreatePrimitive(PrimitiveType.Cube);
		segment.layer = LayerMask.NameToLayer("Creature");
		segment.name = "part_" + depth + "_" + childIndex;
		segment.transform.parent = parent.transform;  // incorporate transformFromParent

		Limb segment_script = segment.AddComponent<Limb>();
		segment_script.setScale((Vector3) node.dimensions);
		segment_script.setColour((Color) chromosome.getLimbColour());

		segment.transform.LookAt(parent.transform);
		segment_script.setPosition(parent.transform.localPosition);
		segment.transform.Translate(0, 0, -parent.transform.localPosition.z);
		
		int idx = 0;
		foreach (GenotypeNode child in node.children)
		{
			//GameObject childSegment = 
			CreateMorphology(child, segment, depth + 1, idx++);
		}

		segment.AddComponent<Rigidbody>();
		segment.AddComponent<BoxCollider>();
		segment.GetComponent<Collider>().material = (PhysicMaterial)Resources.Load("Physics Materials/Creature");

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

		segment.GetComponent<Rigidbody>().drag = 1F;
		segment.GetComponent<Rigidbody>().SetDensity(1F);
	}

	/**
	 * Physics update
	 * // TODO: Find a better way of controlling the joints with wave functions
	 *				the current way needs some sort of magic scalar.
	 * The physics should really be purely dynamic, instead of this kinematic stuff.
	 * The creature movement should be a result of water resistance when paddling. 
	 */
	public void FixedUpdate(float sine, float force_scalar)
	{
		for (int i = 0; i < joints.Count; i++)
		{
			if (joints[i] != null)
				joints[i].targetRotation = Quaternion.Euler(sine * new Vector3(5F, 0F, 0F));
		}

		if (eye_script.goal)
		{
			// The unit vector toward the object in sight should only be used as input to neural network.
			target_direction = (eye_script.goal.transform.position -torso.transform.position).normalized;
		}

		if (target_direction != Vector3.zero)
		{
			// turn toward the goal. This is faking it. It needs to be emergent.
			lookRotation = Quaternion.LookRotation(target_direction);
		}

		float abs_sine = Mathf.Abs(sine);
		float pos_sine = System.Math.Max(sine, 0);
		// interpolates toward where it is looking
		torso.transform.rotation = Quaternion.Slerp(torso.transform.rotation, lookRotation, Time.deltaTime * abs_sine * 3F);

		if (pos_sine == 0)
		{
			direction = torso.transform.forward;
		}

		// totally fake. Needs fixing.
		torso.GetComponent<Rigidbody>().AddForce(Mathf.Abs(force_scalar) * direction * pos_sine * 2 /*chromosome.getBranchCount()*/);
	}
}
