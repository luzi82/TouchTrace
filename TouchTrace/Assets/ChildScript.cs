using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildScript : MonoBehaviour {

	public GameObject imageList;
	public GameObject imageBase;
	public GameObject text;

	public string title;
	public int value;

	public bool initDone;
	public GameObject[] imageAry;
	public UnityEngine.UI.Image[] imageImageAry;
	public UnityEngine.UI.Text textText;

	void Start(){
		if (!initDone) {
			if (Application.isPlaying) {
				RectTransform imageListRt = imageList.GetComponent<RectTransform> ();

				imageAry = new GameObject[Const.BIT_SIZE];
				imageImageAry = new UnityEngine.UI.Image[Const.BIT_SIZE];
				for (int i = 0; i < Const.BIT_SIZE; ++i) {
					GameObject ia = imageAry [i] = Instantiate (imageBase);
					RectTransform iaRt = ia.GetComponent<RectTransform> ();
					iaRt.SetParent (imageListRt);
					iaRt.anchorMin = new Vector2 ((i * 1f) / Const.BIT_SIZE, 0);
					iaRt.offsetMin = new Vector2 (0, 0);
					iaRt.anchorMax = new Vector2 (((i + 1) * 1f) / Const.BIT_SIZE, 1);
					iaRt.offsetMax = new Vector2 (0, 0);
					ia.SetActive (true);

					imageImageAry [i] = ia.GetComponent<UnityEngine.UI.Image> ();
				}

				imageBase.SetActive (false);
			}

			if (imageBase != null) {
				RectTransform imageBaseRt = imageBase.GetComponent<RectTransform> ();
				imageBaseRt.anchorMin = new Vector2 (0, 0);
				imageBaseRt.offsetMin = new Vector2 (0, 0);
				imageBaseRt.anchorMax = new Vector2 (1f / Const.BIT_SIZE, 1);
				imageBaseRt.offsetMax = new Vector2 (0, 0);
			}

			textText = text.GetComponent<UnityEngine.UI.Text> ();

			initDone = true;
		}

	}

	void Update(){
		for (int i = 0; i < Const.BIT_SIZE; ++i) {
			int v = (value >> i) & 1;
			imageImageAry [i].color = (v != 0) ? Color.white : Color.black;
		}
		textText.text = string.Format ("{0}: {1}", title, value);
	}

}
