using UnityEngine;
using System.Collections;

public class GameGUI : MonoBehaviour
{
	public Camera camera;
	// ( W / H );
	public float guiAspectRatio = 0.5f;
	private float guiWidth;
	private float guiPercentage;


	//Temporary rescale every frame.
	public void FixedUpdate ()
	{
		print ("W" + Screen.width + "  H" + Screen.height);
		guiWidth = (float)Screen.height * (float)guiAspectRatio;
		guiPercentage = guiWidth / Screen.width;
		camera.rect = new Rect (guiPercentage, 0, 1 - guiPercentage, 1);
	}

	public void OnGUI ()
	{
		GUI.Box (new Rect (0, 0, guiWidth, Screen.height), "Test GUI");

	}


}
