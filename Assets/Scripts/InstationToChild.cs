using UnityEngine;
using System.Collections;

public class InstationToChild : MonoBehaviour {
	public Transform Parent;
	public Transform Child;
	Vector3 ChildOldPos;
	private float x = 0.0f;
	private float y = 0.0f;
	private float xSpeed = 10.0f;
	private float ySpeed = 10.0f;
	public Transform mCamTransform;		// 摄像机Transform
	public LayerMask mLayer;		// 射击时，射线能碰到的碰撞层
	private bool mIsMove;
	private int MoveIndex;		// 移动坐标轴的索引；1：X；2：Y；3：Z；
	private GameObject  mCamera;
	// Use this for initialization
	void Start () {
		ChildOldPos = Child.position;
		mCamTransform = Camera.main.transform;
		mCamera = GameObject.Find("Main Camera");
		mIsMove = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0)) {
			x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
			RaycastHit info;
			// 从muzzlepoint的位置，向摄像机面向的正方向射出一根射线
			// 射线只能与mPlayer所指向的层碰撞
			Ray ray = mCamera.camera.ScreenPointToRay(Input.mousePosition);
			bool hit = Physics.Raycast(ray, out info, 100,mLayer);
			if (hit && !mIsMove) {
				Debug.Log(info.transform.tag);
				// 更新物体Transform
				ChildOldPos = Child.position;
				// 允许移动，同时禁用射线检测
				mIsMove = true;
				// 如果射中了指定层指定标签的对象，设置对应的移动坐标轴索引
				if (info.transform.tag.CompareTo ("TransX") == 0) {
					MoveIndex = 1;
				} else if (info.transform.tag.CompareTo ("TransY") == 0) {
					MoveIndex = 2;
				} else if (info.transform.tag.CompareTo ("TransZ") == 0) {
					MoveIndex = 3;
				} else {
					mIsMove = false;
				}
				
				
			} 
			// 执行移动任务
			if (mIsMove) {
				switch (MoveIndex) {
				case 1: 
					Child.position = new Vector3(ChildOldPos.x+x,ChildOldPos.y,ChildOldPos.z); 
					break;
				case 2:
					Child.position = new Vector3(ChildOldPos.x,ChildOldPos.y - y,ChildOldPos.z); 
					break;
				case 3:
					Child.position = new Vector3(ChildOldPos.x ,ChildOldPos.y,ChildOldPos.z + x); 
					break;
				}
			}
			
		} else if (Input.GetMouseButtonUp(0)) {
			mIsMove = false;
			x=0.0f;
			y=0.0f;
		}

	}

	void OnGUI () {
		if (GUI.Button (new Rect (0,0,80,40)," GO ")) {
			Child.parent = Parent;
		}
	}
}
