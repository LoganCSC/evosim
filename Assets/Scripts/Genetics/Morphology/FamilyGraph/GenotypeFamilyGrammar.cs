using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * Generate a graph representing a certain family of creatures.
 * The graph that is created can create the phonotypes for specific creatures.
 * It's those phenotype graphs that will be part of the creature, mutated, and persisted as json.
 * @author Barry Becker
 */
public class GenotypeFamilyGrammar
{
	// context free grammar production rules
	private Dictionary<string, List<List<GenotypeTerm>>> productions;

	private static char PROD_SEP = ';';
	private static string[] RULE_SEP = {"=>"};
	private static char OPTION_SEP = '|';
	private static char TERM_SEP = ',';

	public string initialNonTerminal;


	/** 
	 * Constructor 
	 * @param grammar:  a grammar of this form
	 * "<LHS1>=><term1A1>,<term1A2>,...|<term1B1>,<term1B2>,...|...;<LHS2>=><term2A1>,<term2A2>,...|<term2B1>,<term2B2>,...|...;..."
	 * For example:
	 * "creature=>body,First,head;body=>torso:1:8|torso:1:5,Connect:1:4,limbs;limbs=>limb:1:3"
	 * Where "First" indicates a connection that needs to be instantiated first,
	 * "Last" indicates a connection that needs to be instantiated last,
	 * And m:n after a term indicates the number of times that it can recur. Probabilistically, m is much more likely than n.
	 * 
	 * Other examples:
	 * 1) "creature=>body,First,head;body=>torso:1:8|torso:0:5,Connect:1:2,limbs|torso:1:4,Connect:2:4,limbs;limbs=>limb:0:3"
	 * 2) "creature=>body,First,head;body=>torso:0:0,Connect:2:2,limbs;limbs=>limb:3:3"
	 * 
	 * Example 2 will produce a creature that has a body, a head, a single block torso, 
	 * 2 limbs sprouting from the torso, and each limb has 3 segments.
	 * 
	 * The family graph for 2) can be represented like this:
	 * {
	 *    name: "torso",
	 *    selfRecursion: "0",
	 *    connections: [
	 *      {
	 *        isFirst: False,
	 *        terminalOnly: False,
	 *        symmetry: 2
	 *        child: 
	 *        {
	 *          name: "limb",
	 *          selfRecursion: "3"
	 *        }
	 *      },
	 *      {
	 *        isFirst: True,
	 *        terminalOnly: False,
	 *        symmetry: 1
	 *        child: 
	 *        {
	 *          name: "head",
	 *          selfRecursion: "0"
	 *        }
	 *      }
	 *    ]
	 *  }
	 */
	public GenotypeFamilyGrammar(string grammar)
	{
		productions = new Dictionary<string, List<List<GenotypeTerm>>>();
		GenotypeTermParser parser = new GenotypeTermParser();

		string[] rules = grammar.Split(PROD_SEP);
		initialNonTerminal = rules[0].Split(RULE_SEP, StringSplitOptions.None)[0];

		foreach (string rule in rules)
		{
			string[] lhsRhs = rule.Split(RULE_SEP, StringSplitOptions.None);
			List<string> optionsList = lhsRhs[1].Split(OPTION_SEP).ToList();
			List<List<GenotypeTerm>> options = new List<List<GenotypeTerm>>();
			foreach (string opt in optionsList)
			{
				string[] termList = opt.Split(TERM_SEP);
				List<GenotypeTerm> terms = new List<GenotypeTerm>();
				foreach (string termText in termList)
				{
					terms.Add(parser.ParseTerm(termText));
				}
				options.Add(terms);
			}
			productions.Add(lhsRhs[0], options);
		}

		MonoBehaviour.print("grammar dictionary = \n" + this.ToString());
	}

	public bool IsNonTerminal(string key)
	{
		return productions.ContainsKey(key);
	}

	public List<List<GenotypeTerm>> GetRHS(String nonTerminal)
	{
		if (!productions.ContainsKey(nonTerminal))
			throw new ArgumentException("Unexpected nonTerminal: " + nonTerminal);
		return productions[nonTerminal];
	}

	/** Serialize the productions dictionary. Useful for debugging */
	public override string ToString()
	{
		StringBuilder bldr = new StringBuilder();
		foreach (KeyValuePair<string, List<List<GenotypeTerm>>> rule in productions)
		{
			bldr.Append(rule.Key + " => ");
			List<string> optionTerms = new List<string>();

			foreach (List<GenotypeTerm> terms in rule.Value)
			{
				List<string> termsList = new List<string>();
				foreach (GenotypeTerm term in terms)
				{
					termsList.Add(term.ToString());
					//MonoBehaviour.print("term " + term.ToString());
				}
				optionTerms.Add(string.Join(", ", termsList.ToArray()));
			}
			bldr.Append(string.Join(" | ", optionTerms.ToArray()));
			bldr.Append("   ");
		}
		return bldr.ToString();
	}
}
