using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * Take the term text and create an appropriate corresponding GenotypeTerm.
 * A GenotypeTerm is in turn capable of producing a GenotypeNode.
 * A GenotypeNode can produce a PhenotypeNode. The PhenotypeNode can produce the actual creature morphology.
 */
public class GenotypeTermParser
{
	private Dictionary<string, List<string>> productions;

	private static char PARAM_SEP = ':';


	/** Constructor */
	public GenotypeTerm ParseTerm(string termText)
	{

		string[] termParts = termText.Split(PARAM_SEP);
		GenotypeTerm term;
		string txt = termParts[0];

		switch (txt)
		{
			case "L": term = new GenotypeTerm(txt); break;
			case "head": term = new GenotypeTerm(txt); break;
			case "F": term = new GenotypeTerm(txt); break;
			case "torso": term = new GenotypeTerm(txt); break;
			case "limb": term = new GenotypeTerm(txt); break;
			case "C": term = new GenotypeTerm(txt); break;
			default: term = new VariableTerm(txt); break;
			//default: throw new ArgumentException("Unexpected term name: " + txt);
		}
		MonoBehaviour.print("grammar dictionary = \n" + this.ToString());
		return term;
	}

}
