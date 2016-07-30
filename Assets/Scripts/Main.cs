using UnityEngine;
using System.Collections;
using System.IO;

/**
 * Entry point for evosim.
 *		Author: 	Craig Lomax
 */
public class Main : MonoBehaviour
{

#pragma warning disable 0414
	 Settings settings;
	 Selection selectionManager;
	 Spawner spawner;
	 CollisionObserver co;

	 GameObject aperatus;
	 GameObject cam;
	 Ether ether;
#pragma warning restore 0414

	void Start()
	{
		createFolders();
		settings = Settings.getInstance();
		selectionManager = Selection.getInstance();
		aperatus = (GameObject)Instantiate(Resources.Load("Prefabs/Aperatus"));
		cam = GameObject.Find("Main Camera");
		cam.AddComponent<CameraCtl>();
		ether = Ether.getInstance();
		spawner = Spawner.getInstance();
		co = CollisionObserver.getInstance();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#else
			Application.Quit();
			#endif
		}
	}

	private void createFolders()
	{
		if (!Directory.Exists(Application.dataPath + "/data"))
			Directory.CreateDirectory(Application.dataPath + "/data");

		if (!Directory.Exists(Application.dataPath + "/data/saved_creatures"))
			Directory.CreateDirectory(Application.dataPath + "/data/saved_creatures");
	}
	
}
