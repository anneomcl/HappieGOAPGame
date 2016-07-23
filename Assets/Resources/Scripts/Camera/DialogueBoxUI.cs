using UnityEngine;
using System.Collections;

public class DialogueBoxUI : MonoBehaviour {

	Canvas canvas;
	Rect rect;
	float scaleWidth;
	float scaleHeight;
	float posWidth;
	float posHeight;

	// Use this for initialization
	void Start () {
		canvas = GameObject.Find ("Canvas").GetComponent<Canvas>();
		rect = GetComponentInParent<RectTransform> ().rect;
		scaleWidth = canvas.GetComponent<RectTransform> ().rect.width / rect.width;
		scaleHeight = (canvas.GetComponent<RectTransform> ().rect.height/3)/ rect.height;

		posWidth = 0f;
		posHeight = -1 * scaleHeight * rect.height ;

		GetComponent<RectTransform> ().localPosition = new Vector3 (posWidth, posHeight);
		GetComponent<RectTransform> ().transform.localScale = new Vector3 (scaleWidth, scaleHeight);
	}

	// Update is called once per frame
	void Update () {
		
	}

	void SnapToWindowDimensions(){


	}
}
