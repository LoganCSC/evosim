﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * Generate a graph representing a certain family of creatures.
 * The graph that is created can create the phonotypes for specific creatures.
 * @author Barry Becker
 */
public class GenotypeFamilyGrammar : MonoBehaviour
{
	// context free grammar production rules
	private Dictionary<string, List<List<GenotypeTerm>>> productions;

	private static char PROD_SEP = ';';
	private static string[] RULE_SEP = {"=>"};
	private static char OPTION_SEP = '|';
	private static char TERM_SEP = ',';


	/** 
	 * Constructor 
	 * @param grammar:  a grammar of this form
	 * "<LHS1>=><term1A1>,<term1A2>,...|<term1B1>,<term1B2>,...|...;<LHS2>=><term2A1>,<term2A2>,...|<term2B1>,<term2B2>,...|...;..."
	 * For example:
	 * "creature=>body,F,head;body=>torso:1:8|torso:1:5,C:1:4,limbs;limbs=>limb:1:3"
	 * Where F indicates a connection that needs to be instantiated first,
	 * L indicates a connection that needs to be instantiated last,
	 * And m:n after a term indicates the number of times that it can recur. Typically m is more likely than n.
	 */
	public GenotypeFamilyGrammar(string grammar)
	{
		productions = new Dictionary<string, List<List<GenotypeTerm>>>();
		GenotypeTermParser parser = new GenotypeTermParser();

		string[] rules = grammar.Split(PROD_SEP);

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

	/** 
	 * @return a sentence in the language defined by the grammar.
	 * This sentence will be turned into genotype-family graph.
	 * This graph will produce the genotype-graph. The genotypy-graph is what will be persisted and mutated,
	 * and is what produces the phenotype (actual physical morphology).
	 */
	public String createSentence()
	{
		// start with "creature" and make the sentance by applying production rules.
		return "torso:2,Connect:2,limb:2,First,head";
	}

	/** Serialize the productions dictionary. Useful for debugging */
	public override String ToString()
	{
		StringBuilder bldr = new StringBuilder();
		foreach (KeyValuePair<string, List<List<GenotypeTerm>>> rule in productions)
		{
			bldr.Append(rule.Key + " => ");
			List<string> optionTerms = new List<string>();

			foreach (List<GenotypeTerm> terms in rule.Value)
			{
				List<string> termsList = new List<string>();
				MonoBehaviour.print("terms len " + terms.Count());
				foreach (GenotypeTerm term in terms)
				{
					termsList.Add(term.ToString());
					MonoBehaviour.print("term " + term.ToString());
				}
				optionTerms.Add(String.Join(", ", termsList.ToArray()));
			}
			bldr.Append(String.Join(" | ", optionTerms.ToArray()));
			bldr.Append("   ");
		}
		return bldr.ToString();
	}
}
