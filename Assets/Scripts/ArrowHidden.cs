/// <summary>
/// Author: Fu
/// CreateDate: 2015 -01 -19 19:34
/// Function: 控制坐标轴的显示跟隐藏
/// </summary>
using UnityEngine;
using System.Collections;

public class ArrowHidden : MonoBehaviour {
	private GameObject mArrow;
	private GameObject mCamera;
	private LayerMask mLayer;
	// Use this for initialization
	void Start () {

		mCamera = GameObject.Find("Main Camera");
		mLayer = LayerMask.GetMask("transform");
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit info;
		Ray ray = mCamera.camera.ScreenPointToRay(Input.mousePosition);
		bool hit = Physics.Raycast(ray,out info,100,mLayer);
		if (hit) {
			if (!mArrow) {
				mArrow = info.transform.gameObject;
				mArrow.renderer.material.color = Color.red;
			}
		} else {
			if (mArrow ) {
				if ( Input.GetMouseButton(0)) {
					mArrow.renderer.material.color = Color.red;
				} else {
					mArrow.renderer.material.color = Color.green;
					mArrow = null;
				}
			}
		} 


	}
}
