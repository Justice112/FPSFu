/// <summary>
/// Author: Fu
/// CreateDate: 2015 -01 -19 20:26
/// Function: 鼠标左键按下拖动，旋转摄像机，滚轮缩放
/// </summary>
using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	//摄像机的观察点
	public Transform Target;
	//摄像机Y轴的旋转系数（调整旋转）
	public float minimumY ;
	public float maximumY ;
	//摄像机X轴的旋转系数（调整俯仰）
	public float minimumX ;
	public float maximumX ;
	// 暂存鼠标增量
	public double temX;
	public double temY;
	// 移动旋转速度控制
	public float xSpeed;		
	public float ySpeed;
	public float wheelSpeed;
	//
	static float x;
	static float y;
	//摄像机与观察点的距离
	public float distance;
	public float initDistance ;
	public float minDistance ;
	public float maxDistance ;
	public float temDistance ;
	//
	public Transform mTransform;
	private Vector3 mPosition;
	private Quaternion mRotation;
	// Use this for initialization
	void Start () {
		Common.IS_CAMERA_CONTROL =true;
		mTransform = transform; 		// 挂载到摄像机上
		// 初始距离
		temDistance = initDistance =Vector3.Distance(Target.position,mTransform.position);	
		minDistance=3f;
		maxDistance=10.0f;
		// 旋转速度城初始值
		xSpeed=100.0f;
		ySpeed=100.0f;
		wheelSpeed=2f;
		temX=0.0f;
		temY=0.0f;
		// 俯仰角度
		minimumY = 1;
		maximumY = 100;
		// 
		x = -130;
		y = -30;
		if (rigidbody) {
			rigidbody.freezeRotation = true;
		}
	}
	
	void LateUpdate () {
		if (Target) {
			if (Common.IS_CAMERA_CONTROL) {
				//目标与摄像机之间的距离
				distance = Vector3.Distance(Target.position,mTransform.position);
				// 获取鼠标增量
				if(Input.GetMouseButton(0)) {
					temX += Input.GetAxis("Mouse X") * xSpeed * 0.04;
					temY -= Input.GetAxis("Mouse Y") * ySpeed * 0.04;
					Debug.Log(temY);
					temY = ClampAngleY((float)temY, minimumY, maximumY);
				}
				// Mathf.Abs 绝对值
				if(Mathf.Abs((float)temX-x)>0.01) {
					x=Mathf.Lerp(x,(float)temX,Time.deltaTime*6);		// Mathf.Lerp 插值
				}
				// 
				if(Mathf.Abs((float)temY-y)>0.01) {
					y=Mathf.Lerp(y,(float)temY,Time.deltaTime*6);
				}
				// 鼠标滚轮控制摄像机与观察点的距离
				temDistance-= Input.GetAxis("Mouse ScrollWheel")*wheelSpeed;
				temDistance = Mathf.Clamp(temDistance,minDistance,maxDistance);
				if(Mathf.Abs(distance-temDistance)>0.01) {
					distance=Mathf.Lerp(distance,temDistance,Time.deltaTime*4);
				}
				mRotation =  Quaternion.Euler(y, x, 0);
				mPosition = mRotation * new Vector3(0.0f, 0.0f, -distance) + Target.position;
				mTransform.rotation = mRotation;
				mTransform.position = mPosition;
			} else {
				return;
			}
		} else {
			Debug.Log("Wrong Time: No Target!!");
		}

	}
	// 
	private float ClampAngleY(float angle,float min, float max)
	{
		if (angle < min) {
			angle =min;
		}
		if (angle > max) {
			angle = max;
		}
		// 限制value的值在min和max之间
		return Mathf.Clamp(angle, min, max);
	}
}
