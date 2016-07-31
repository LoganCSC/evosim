using System;
using System.Collections.Generic;
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
			case "Last": term = new LastConnectionTerm(txt); break;
			case "First": term = new FirstConnectionTerm(txt); break;
			case "head": term = new HeadTerm(txt); break;
			case "torso": term = new TorsoTerm(txt, (int)float.Parse(termParts[1]), (int)float.Parse(termParts[2])); break;
			case "limb": term = new LimbTerm(txt, (int)float.Parse(termParts[1]), (int)float.Parse(termParts[2])); break;
			case "Connect": term = new MultiConnectionTerm(txt, (int)float.Parse(termParts[1]), (int)float.Parse(termParts[2])); break;
			default: term = new VariableTerm(txt); break;
			//default: throw new ArgumentException("Unexpected term name: " + txt);
		}
		MonoBehaviour.print("grammar dictionary = \n" + this.ToString());
		return term;
	}

}
