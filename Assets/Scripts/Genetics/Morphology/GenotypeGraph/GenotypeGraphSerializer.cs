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
		//Debug.Log(JsonUtility.ToJson(root, true));

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
		indent + @"{
		";

		string node_string =
		indent + @"{{
			""dimensions"" : {{ ""x"": {0}, ""y"": {1}, ""z"": {2}}},
			""rotateFromParent"" : {{ ""x"": {3}, ""y"": {4}, ""z"": {5}}},
			""translateFromParent"" : {{ ""x"": {6}, ""y"": {7}, ""z"": {8}}}
		";
		//}}";

		string[] l_args = {
			root.dimensions.x.ToString(), root.dimensions.y.ToString(), root.dimensions.z.ToString(),
			root.rotateFromParent.x.ToString(), root.rotateFromParent.y.ToString(), root.rotateFromParent.z.ToString(),
			root.translateFromParent.x.ToString(), root.translateFromParent.y.ToString(), root.translateFromParent.z.ToString(),
		};
		node_json += String.Format(node_string, l_args);

		Boolean hasChildren = root.children.Count > 0;
		GenotypeNode lastChild = null;
		if (hasChildren)
		{
			node_string +=
			indent + @"""children"": [";
			lastChild = root.children[root.children.Count - 1];
		}
		// print the node attributes
		// print the child nodes
		// print "children" : ....
		foreach (GenotypeNode child in root.children)
		{
			node_json += writeAsJsonAux(child, indent + "  ");
			if (child == lastChild)
			{
				node_json += ",\n";
			}
		}
		if (hasChildren)
		{
			node_string +=
			indent + @"
			" + indent + "]"; // close child list
		}
		node_json +=
		indent + @"
		" + indent + "}"; // close node
		return node_json;
	}

}
