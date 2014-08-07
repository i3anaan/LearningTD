using UnityEngine;
using System.Collections;

public class GameGUI : MonoBehaviour
{
	public Camera camera;
	// ( W / H );
	public float guiAspectRatio = 0.5f;
	private float guiWidth;
	private float guiPercentage;
	private GameController gc;

	public BasicTower[] towers;


	public void Start ()
	{
		gc = GameController.getInstance ();
	}


	//Temporary rescale every frame.
	public void FixedUpdate ()
	{
		//print ("W" + Screen.width + "  H" + Screen.height);
		guiWidth = (float)Screen.height * (float)guiAspectRatio;
		guiPercentage = guiWidth / Screen.width;
		camera.rect = new Rect (guiPercentage, 0, 1 - guiPercentage, 1);
	}

	public void OnGUI ()
	{
		//GUI.Box (new Rect (0, 0, guiWidth, Screen.height), "NIET TEST GUI");

		for (int i=0; i<towers.Length; i++) {
			BasicTower tower = towers [i];
			if (GUI.Button (new Rect (0, i * 50, guiWidth, 50), tower.name)) {
				gc.towerSelected = tower;
			}
		}
	}


}
