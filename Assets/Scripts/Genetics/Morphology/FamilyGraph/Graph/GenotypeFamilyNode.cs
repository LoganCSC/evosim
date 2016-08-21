using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * The genotype-family-graph is an abstract representation of a whole family of creatures.
 * When this graph is run it will produce specific genotype-graphs (a different one each time run).
 */
public class GenotypeFamilyNode : GenotypeFamilyComponent
{
	public string name;

	public Vector3 minDimension;
	public Vector3 maxDimension;

	public Vector3 translateFromParent;
	public Vector3 rotateFromParent;  // eulerAngles

	/** The number of times to recurse. If 0, then no recursion */
	public int selfRecursion;

	// Configurable joint parameters (there are a lot)
	// set target angle and use max force for slerp mode
	// set angle limits here.
	//public TODO

	// TODO - define how input should be translated into join actuator 
	// public Neuron[] neurons; 

	List<GenotypeFamilyConnection> connections;

	public GenotypeFamilyNode()
	{
		connections = new List<GenotypeFamilyConnection>();
	}

	public void AddConnection(GenotypeFamilyConnection connection)
	{
		connections.Add(connection);
	}

	public override string ToString()
	{
		return ToString("");
	}

	public override string ToString(string indent)
	{
		string s = indent + "{\n";
		s += indent + "  name: \"" + name + "\",\n";
		int numConnections = connections.Count;
		s += indent + "  selfRecursion: \"" + selfRecursion + "\"" + (numConnections > 0 ? ",\n" : "\n");
		if (numConnections > 0)
		{
			s += indent + "  connections: [\n";
			for (int i = 0; i < numConnections; i++)
			{
				s += connections[i].ToString(indent + "    ");
				if (i < numConnections - 1)
				{
					s += ",\n";
				}
			}
			s += indent + "\n  ]\n";
		}
		s += indent + "}\n";

		return s;
	}
}
