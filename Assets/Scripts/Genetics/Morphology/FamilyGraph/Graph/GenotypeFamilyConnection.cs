using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * Connects two GenotypeFamilyNodes
 * @author Barry Becker
 */
public class GenotypeFamilyConnection : GenotypeFamilyComponent, IComparable<GenotypeFamilyConnection>
{
	// if true, then this connection will be applied before others
	public bool isFirst;

	// If true, this connction applies only at the recursive limit of the parent.
	public bool terminalOnly;

	// If greater than 1, then it represents a set of "symmetry"" symmetrical connections to child appendages.
	// For example, 2 means bilateral symmetry.
	public int symmetry = 1;

	// transform from parent (position, orientation, scale)
	public Vector3 translateFromParent;
	public Vector3 rotateFromParent;  // eulerAngles

	private GenotypeFamilyNode childNode;

	/** Constructor */
	public GenotypeFamilyConnection(GenotypeFamilyNode child) : base()
	{
		childNode = child;
	}

	public GenotypeFamilyNode GetChild()
	{
		return childNode;
	}

	/**
	 * We want connections sorted so that isFirst nodes are processed first (in no particular order), 
	 * followed by all nonfirst and nonTerminal nodes (sorted decreasing by symmetry), followed by 
	 * all terminal nodes (in no particular order).
	 * @return -1, 0, or 1 depending on if this is less, equal, or greater than other 
	 */
	public int CompareTo(GenotypeFamilyConnection other)
	{
		if (this.isFirst != other.isFirst)
		{
			return this.isFirst.CompareTo(other.isFirst);
		}
		else if(this.terminalOnly != other.terminalOnly)
		{
			return other.terminalOnly.CompareTo(this.terminalOnly);
		}
		return other.symmetry.CompareTo(this.symmetry);
	}

	public override string ToString()
	{
		return ToString("") + "\n";
	}

	public override string ToString(string indent)
	{
		string s = indent + "{\n";
		s += indent + "  isFirst: " + isFirst + ",\n";
		s += indent + "  terminalOnly: " + terminalOnly + ",\n";
		s += indent + "  symmetry: " + symmetry + "\n";
		s += indent + "  child: \n" + childNode.ToString(indent + "  ");
		s += indent + "}";

		return s;
	}
}

