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
	 Data d;
	 Logger lg;
	 Settings settings;
	 Selection selectionManager;
	 Spawner spawner;
	 CollisionMediator co;

	 GameObject aperatus;
	 GameObject cam;
	 Ether ether;
#pragma warning restore 0414

	void Start()
	{
		createFolders();
		d = Data.getInstance();
		lg = Logger.getInstance();
		settings = Settings.getInstance();
		selectionManager = Selection.getInstance();
		aperatus = (GameObject)Instantiate(Resources.Load("Prefabs/Aperatus"));
		cam = GameObject.Find("Main Camera");
		cam.AddComponent<CameraCtl>();
		ether = Ether.getInstance();
		spawner = Spawner.getInstance();
		co = CollisionMediator.getInstance();
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
