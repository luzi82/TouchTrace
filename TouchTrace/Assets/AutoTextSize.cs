using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTextSize : AbstractOnSizeChangeBehaviour {

	public float ratio;

	public AutoTextSize(){
		onChangeDetectorList.Add (new OnChangeDetector<float> (
			()=>{return ratio;},
			(float a,float b)=>{return a==b;}
		));
	}

	public override void OnSizeChange(Rect nowRect){
		UnityEngine.UI.Text text = GetComponent<UnityEngine.UI.Text> ();
//		UnityEngine.Debug.Log (text.fontSize);
		text.fontSize = (int)(nowRect.height * ratio);
	}
}
