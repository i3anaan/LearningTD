using UnityEngine;
using System.Collections;

public class FloatingText : MonoBehaviour {

    public float duration;
    public bool turnTransparent;
    public float transparencyDelay;
    public float movementPercentPower = 1;
    public float transparancyPercentPower = 1;
    public Vector3 position;
    public Vector3 offset;
    public Color color;
    private GUIText guiText;
    private float progress;
    

	// Use this for initialization
	void Awake () {
        guiText = this.gameObject.GetComponent<GUIText>();
        progress = 0;

        position = this.transform.position;
        guiText.color = color;
	}

    void Start()
    {
        foreach(GUIText gt in this.GetComponentsInChildren<GUIText>()){
            gt.text = this.guiText.text;
            gt.fontSize = this.guiText.fontSize;
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        this.progress = progress + Time.fixedDeltaTime;
        float percent = Mathf.Pow((progress / duration), movementPercentPower);
        this.transform.position = this.position + offset * percent;

        if (turnTransparent)
        {
            float transparencyPercent = Mathf.Pow(Mathf.Max(((progress - transparencyDelay) / (duration - transparencyDelay)), 0), transparancyPercentPower);
            Color color = guiText.color;
            color.a = 1f * (1f - transparencyPercent);
            guiText.color = color;
            
            //Duplicate GUITexts for outline
            foreach (GUIText gt in this.GetComponentsInChildren<GUIText>())
            {
                color = gt.color;
                color.a = 1f * (1f - transparencyPercent);
                gt.color = color;
            }
            
        }


        if (progress >= duration)
        {
            GameObject.Destroy(this.gameObject);
        }
	}
}
