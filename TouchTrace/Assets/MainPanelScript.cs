using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainPanelScript : AbstractOnSizeChangeBehaviour {

	public GameObject childBase;
	public GameObject childListGO;

	private RectTransform childListGORt;
	private List<RectTransform> childRtList = new List<RectTransform>();

	private ChildScript widthCS;
	private ChildScript heightCS;
	private ChildScript xCS;
	private ChildScript yCS;
	private ChildScript downCS;
	private ChildScript[] timeCSList;

	protected override void Start(){
		base.Start ();

		childListGORt = childListGO.GetComponent<RectTransform> ();

		if (Application.isPlaying) {
			widthCS = MakeChild ("Width");
			heightCS = MakeChild ("Height");
			xCS = MakeChild ("X");
			yCS = MakeChild ("Y");
			downCS = MakeChild ("Down");
			timeCSList = new ChildScript[4];
			for (int i = 0; i < 4; ++i) {
				timeCSList[i] = MakeChild ("");
			}

			childBase.SetActive (false);

			EventTrigger trigger = GetComponent<EventTrigger>();

			EventTrigger.Entry entry;

			entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener((data) => { OnDown((PointerEventData)data); });
			trigger.triggers.Add(entry);

			entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.BeginDrag;
			entry.callback.AddListener((data) => { OnDown((PointerEventData)data); });
			trigger.triggers.Add(entry);

			entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.Drag;
			entry.callback.AddListener((data) => { OnDown((PointerEventData)data); });
			trigger.triggers.Add(entry);

			entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.EndDrag;
			entry.callback.AddListener((data) => { OnUp((PointerEventData)data); });
			trigger.triggers.Add(entry);

			entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerUp;
			entry.callback.AddListener((data) => { OnUp((PointerEventData)data); });
			trigger.triggers.Add(entry);

		}
	}

	private ChildScript MakeChild(string title){
		GameObject go = Instantiate (childBase);
		go.SetActive (true);
		RectTransform goRt = go.GetComponent<RectTransform> ();
		goRt.SetParent (childListGORt);
		childRtList.Add (goRt);
		ChildScript cs = go.GetComponent<ChildScript> ();
		cs.title = title;
		return cs;
	}

	public override void OnSizeChange(Rect nowRect) {
		float itemHeight = nowRect.width / Const.BIT_SIZE;
		if (childBase != null) {
			RectTransform childBaseRectTransform = childBase.GetComponent<RectTransform> ();
			childBaseRectTransform.anchorMin = new Vector2 (0, 1);
			childBaseRectTransform.offsetMin = new Vector2 (0, -itemHeight);
			childBaseRectTransform.anchorMax = new Vector2 (1, 1);
			childBaseRectTransform.offsetMax = new Vector2 (0, 0);
		}

		if (childRtList != null) {
			int i = 0;
			foreach (RectTransform rt in childRtList) {
				rt.anchorMin = new Vector2 (0, 1);
				rt.offsetMin = new Vector2 (0, (i+1)*(-itemHeight));
				rt.anchorMax = new Vector2 (1, 1);
				rt.offsetMax = new Vector2 (0, (i)*(-itemHeight));
				++i;
			}
		}
	}

	protected override void Update(){
		base.Update ();
		if (Application.isPlaying) {
			widthCS.value = Screen.width;
			heightCS.value = Screen.height;
			long t = System.DateTime.UtcNow.Ticks / 10000;
			for (int i = 0; i < 4; ++i) {
				long v = t >> (i * 16);
				v &= (1 << 16) - 1;
				timeCSList [i].value = (int)v;
			}
		}
	}

	public void OnDown(PointerEventData ped){
		downCS.value = (1<<Const.BIT_SIZE)-1;
		xCS.value = (int) ped.position.x;
		yCS.value = Screen.height - ((int) ped.position.y);
	}
	public void OnUp(PointerEventData ped){
		downCS.value = 0;
		xCS.value = (int) ped.position.x;
		yCS.value = Screen.height - ((int) ped.position.y);
	}

}
