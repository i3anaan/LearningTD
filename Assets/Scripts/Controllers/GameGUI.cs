using UnityEngine;
using System.Collections;

public class GameGUI : MonoBehaviour
{
	public new Camera camera;
    public GUIStyle GUIbackground;
    public GUIStyle GUIText;
	// ( W / H );
	public float guiAspectRatio = 0.5f;
	private float guiWidth;
	private float guiPercentage;
	private GameController gc;

    public PassiveTower[] towers;
	public Renderer selectionHighlight;
    public MonoBehaviour floatingText_Gold;


	public void Start ()
	{
		gc = GameController.getInstance ();
        GUIbackground.normal.textColor = Color.red;
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
        GUI.Box(new Rect(0, 0, guiWidth, Screen.height), "", GUIbackground);

		for (int i=0; i<towers.Length; i++) {
			PassiveTower tower = towers [i];
			if (GUI.Button (new Rect (0, i * 50, guiWidth, 50), tower.name +" ("+tower.goldCost+")")) {
				gc.towerSelected = tower;
			}
		}

        GUI.Box(new Rect(0f * Screen.width, 0.8f * Screen.height, guiWidth, 0.1f * Screen.height), "Gold: " + gc.currentGold, GUIText);
        GUI.Box(new Rect(0f * Screen.width, 0.9f * Screen.height, guiWidth, 0.1f * Screen.height), "Lives: " + gc.livesLeft, GUIText);
	}
}
