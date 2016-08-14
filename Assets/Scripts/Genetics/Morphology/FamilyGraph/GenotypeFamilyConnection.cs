using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * Connects two GenotypeFamilyNodes
 * @author Barry Becker
 */
public class GenotypeFamilyConnection
{
	/* not sure if I prefer enums to separate booleans
	public enum ConnectionType
	{
		first,
		normal,
		terminal
	};*/

	// if true, then this connection will be applied before others
	public bool isFirst;

	// If true, this connction applies only at the recursive limit of the parent.
	public bool terminalOnly;

	// If greater than 1, then it represents a set of "symmetry"" symmetrical connections to child appendages.
	// For example, 2 means bilateral symmetry.
	public int symmetry = 0;

	// transform from parent (position, orientation, scale)
	public Vector3 translateFromParent;
	public Vector3 rotateFromParent;  // eulerAngles
}

