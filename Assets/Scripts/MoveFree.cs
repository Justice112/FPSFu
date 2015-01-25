/// <summary>
/// Author: Fu
/// CreateDate:  2015- 01-19
/// Function: 自由移动模型
/// </summary>
using UnityEngine;
using System.Collections;

public class MoveFree : MonoBehaviour {
	private Transform mTransform;		// 该物体的变换
	private Vector3 mOldPos;		// 旧坐标
	public Transform  mArrowPrefab;		// 用作坐标轴的箭头预制体
	private Vector3 [] mAxisPosArry = new Vector3[3];
	private Vector3 [] mAxisRotArry = new Vector3[3];
	private string [] mAxisTag = new string[3];
	private float x = 0.0f;
	private float y = 0.0f;
	private float xSpeed = 10.0f;
	private float ySpeed = 10.0f;
	public Transform mCamTransform;		// 摄像机Transform
	public LayerMask mLayer;		// 射击时，射线能碰到的碰撞层
	private bool mIsMove;
	private int MoveIndex;		// 移动坐标轴的索引；1：X；2：Y；3：Z；
	private GameObject  mCamera;
	private Vector3 OriginPosition;		// 模型原始坐标
	private Quaternion OriginRotation;	// 模型原始旋转
	// Use this for initialization
	void Start () {
		mTransform = this.transform;
		OriginPosition = this.transform.position;
		OriginRotation = this.transform.rotation;
		mOldPos = mTransform.position;
		mArrowPrefab = Resources.Load("Prefab/Arrow",typeof (Transform)) as Transform;
		// X
		mAxisPosArry [0] = new Vector3(2,0,0);		
		mAxisRotArry[0] = new Vector3(0,0,0);
		mAxisTag[0] = "TransX";
		// Y
		mAxisPosArry [1] = new Vector3(0,2,0);		
		mAxisRotArry[1] = new Vector3(0,90,90);
		mAxisTag[1] = "TransY";
		// Z
		mAxisPosArry [2] = new Vector3(0,0,-2);		
		mAxisRotArry[2] = new Vector3(0,90,0);
		mAxisTag[2] = "TransZ";
		mLayer = LayerMask.GetMask("transform");
		// 创建坐标轴图标
		CreateMoveModelAxis();
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
				mOldPos = mTransform.position;
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
				Common.IS_CAMERA_CONTROL =false;
				switch (MoveIndex) {
				case 1: 
					mTransform.position = new Vector3(mOldPos.x+x,mOldPos.y,mOldPos.z); 
					break;
				case 2:
					mTransform.position = new Vector3(mOldPos.x,mOldPos.y - y,mOldPos.z); 
					break;
				case 3:
					mTransform.position = new Vector3(mOldPos.x ,mOldPos.y,mOldPos.z + x); 
					break;
				}
			}
			
			
		} else if (Input.GetMouseButtonUp(0)) {
			mIsMove = false;
			Common.IS_CAMERA_CONTROL =true;
			x=0.0f;
			y=0.0f;
		}

	}

	/// <summary>
	/// 创建移动模型的移动坐标轴
	/// </summary>
	void CreateMoveModelAxis () {
		if (mTransform.childCount ==0) {
			for (int i =0; i < 3;i++) {
				// 实例化X坐标轴对象
				Transform axisX=(Transform) Instantiate(mArrowPrefab,new Vector3(0,0,0),Quaternion.identity) ;
				// 挂载为子对象
				axisX.parent = mTransform;
				// 设置相对坐标
				axisX.position = mAxisPosArry[i] +mTransform.position;
				// 设置相对旋转
				axisX.eulerAngles = mAxisRotArry[i];
				// 设置标签
				axisX.tag = mAxisTag[i];
				// 或许效果控制脚本
				axisX.gameObject.AddComponent<ArrowHidden>();
			}
		}
	}
	/// <summary>
	/// 销毁箭头模型
	/// </summary>
	void DestoryMoveModelAxis () {
		foreach (Transform child in mTransform) {
			if(child) {
				Destroy(child.gameObject);
			}
		}
	}
	/// <summary>
	/// 重置模型
	/// </summary>
	void ResetMoveModel() {
		transform.position = OriginPosition;
		transform.rotation = OriginRotation;
	}

	/// <summary>
	/// Raises the GU event.
	/// </summary>
	void OnGUI () {
		if (GUI.Button (new Rect(0,0,80,40),"Destory")) {
			DestoryMoveModelAxis();
		}
		if (GUI.Button (new Rect(0,50,80,40),"Create")) {
			CreateMoveModelAxis();
		}
		if (GUI.Button (new Rect(0,100,80,40),"Reset")) {
			ResetMoveModel();
		}
	}
}
