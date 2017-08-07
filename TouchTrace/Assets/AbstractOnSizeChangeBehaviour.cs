using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public abstract class AbstractOnSizeChangeBehaviour : MonoBehaviour
{
	private RectTransform rectTransform{
		get{
			if (_rectTransform == null)
				_rectTransform = GetComponent<RectTransform> ();
			return _rectTransform;
		}
	}
	private RectTransform _rectTransform;
	protected List<IOnChangeDetector> onChangeDetectorList=new List<IOnChangeDetector>();

	public AbstractOnSizeChangeBehaviour (){
		onChangeDetectorList.Add(new OnChangeDetector<Rect>(
			()=>{return rectTransform.rect;},
			(Rect a,Rect b)=>{return a==b;}
		));
	}

	// Use this for initialization
	protected virtual void Start () {
	}

	// Update is called once per frame
	protected virtual void Update () {
//		KickOnSizeChange ();
		bool changeExist = false;
		foreach(IOnChangeDetector ocd in onChangeDetectorList){
//			UnityEngine.Debug.Log ("ffff");
			changeExist = ocd.CheckChangeAndUpdate () || changeExist;
		}

		if (changeExist) {
			Rect nowRect = rectTransform.rect;
			OnSizeChange (nowRect);
		}
	}

//	public void KickOnSizeChange(){
//		Rect nowRect = rectTransform.rect;
//		if (lastRect == nowRect)
//			return;
//
//		OnSizeChange (nowRect);
//		lastRect = nowRect;
//	}

	public abstract void OnSizeChange(Rect nowRect);

	protected interface IOnChangeDetector{
		bool CheckChangeAndUpdate();
//		void Update();
	}

	protected class OnChangeDetector<T>:IOnChangeDetector{
		public delegate T GetT();
		public delegate bool EqualT(T a,T b);

		private GetT getT;
		private EqualT equalT;
		private T oldValue;

		public OnChangeDetector(GetT getT,EqualT equalT){
			this.getT = getT;
			this.equalT = equalT;
		}

		public bool CheckChangeAndUpdate(){
			T newValue = getT();
			bool ret = !equalT(newValue,oldValue);
			oldValue = newValue;
			return ret;
		}

//		public void Update(){
//			oldValue = getT ();
//		}
	}
}

