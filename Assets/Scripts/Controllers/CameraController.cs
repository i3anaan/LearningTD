using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	//WorldSpaceBoundraries
	public float minX;
	public float maxX;
	public float minY;
	public float maxY;

	public float cameraMoveSpeedX = 1;
	public float cameraMoveSpeedY = 1;

	private Camera cam;
	private float worldViewHeight;
	private float worldViewWidth;


	public void Start ()
	{
		this.cam = Camera.main;

		if (minX > maxX || minY > maxY) {
			Debug.LogError ("Invalid worldspace bounds!");
		}
	}


	public void FixedUpdate ()
	{
		worldViewHeight = cam.orthographicSize * 2;
		worldViewWidth = worldViewHeight * cam.aspect;

		if (Input.anyKey) {
			Vector3 pos = cam.transform.position;
			if (Input.GetKey (KeyCode.UpArrow)) {
				pos.y = pos.y + (cameraMoveSpeedY * Time.fixedDeltaTime);
			}
			if (Input.GetKey (KeyCode.DownArrow)) {
				pos.y = pos.y - (cameraMoveSpeedY * Time.fixedDeltaTime);
			}
			if (Input.GetKey (KeyCode.RightArrow)) {
				pos.x = pos.x + (cameraMoveSpeedX * Time.fixedDeltaTime);
			}
			if (Input.GetKey (KeyCode.LeftArrow)) {
				pos.x = pos.x - (cameraMoveSpeedX * Time.fixedDeltaTime);
			}

			cam.transform.position = limitToWorldSpaceBoundraries (pos);
		}
	}

	public Vector3 limitToWorldSpaceBoundraries (Vector3 pos)
	{
		if (minX + worldViewWidth / 2f < maxX - worldViewWidth / 2f) {
			pos.x = Mathf.Clamp (pos.x, minX + worldViewWidth / 2f, maxX - worldViewWidth / 2f);
		} else {
			pos.x = (minX + maxX) / 2f;
		}
		if (minY + worldViewHeight / 2f < maxY - worldViewHeight / 2f) {
			pos.y = Mathf.Clamp (pos.y, minY + worldViewHeight / 2f, maxY - worldViewHeight / 2f);
		} else {
			pos.y = (minY + maxY) / 2f;
		}
		return pos;
	}
}
