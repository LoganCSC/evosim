using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * Serialize to json
 */
public class GenotypeGraphSerializer
{



	public string writeAsJson(GenotypeNode root, string indent)
	{
		string graph_json =
		@"""genotype_graph"": ";

		graph_json += writeAsJsonAux(root, indent);
		graph_json +=
		@""; // close graph

		return graph_json;
	}

	/**
	 * write the current node and all its children
	 */
	private string writeAsJsonAux(GenotypeNode root, string indent)
	{
		string node_json =
		@"{
		";

		string node_string = 
		@"	{{
			""dimensions"" : {{ ""x"": {0}, ""y"": {1}, ""z"": {2}}},
			""rotateFromParent"" : {{ ""x"": {3}, ""y"": {4}, ""z"": {5}}},
			""translateFromParent"" : {{ ""x"": {6}, ""y"": {7}, ""z"": {8}}}
		}}";

		string[] l_args = {
			root.dimensions.x.ToString(), root.dimensions.y.ToString(), root.dimensions.z.ToString(),
			root.rotateFromParent.x.ToString(), root.rotateFromParent.y.ToString(), root.rotateFromParent.z.ToString(),
			root.translateFromParent.x.ToString(), root.translateFromParent.y.ToString(), root.translateFromParent.z.ToString(),
		};
		node_json += String.Format(node_string, l_args);

		// print the node attributes
		// print the child nodes
		// print "childre" : ....
		foreach (GenotypeNode child in root.children)
		{
			node_json += writeAsJsonAux(child, indent + "  ");
		}

		node_json +=
		@"
		}"; // close node
		return node_json;
	}

}
