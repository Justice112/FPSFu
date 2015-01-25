/// <summary>
/// Author: Fu
/// CreateDate: 2015-01 -20
/// Function: 拖拽旋转模型
/// </summary>
using UnityEngine;
using System.Collections;

public class DragRotateFree : MonoBehaviour {
	private bool onDrag = false;  // 是否被拖拽     
	public float speed = 6f;   // 旋转速度     
	private float tempSpeed;   // 阻尼速度      
	private float axisX;    // 鼠标沿水平方向移动的增量     
	private float axisY;    // 鼠标沿竖直方向移动的增量  
	private float cXY;     // 鼠标移动的距离
	private float temX;
	private float temY;
	// Use this for initialization
	void Start () {
	
	}
	// 接受鼠标按下的事件
	void OnMouseDown(){    
		Common.IS_CAMERA_CONTROL = false;

		axisX=0f;
		axisY=0f;     
	} 
	//鼠标拖拽时的操作//  
	void OnMouseDrag () {  
		onDrag = true;  
		axisX = -Input.GetAxis ("Mouse X");  //获得鼠标增量  
		axisY = Input.GetAxis ("Mouse Y");  
		temX +=Mathf.Abs(Input.GetAxis("Mouse X"));
		temY += Mathf.Abs(Input.GetAxis("Mouse Y"));
		cXY = Mathf.Sqrt (axisX * axisX + axisY * axisY); //计算鼠标移动的长度 
		if(cXY ==0f){ 
			cXY=1f;        
		}     
	}
	// 计算阻尼速度    
	float Rigid () { 
		if (onDrag) {  
			tempSpeed = speed;         
		} else { 
			if (tempSpeed> 0) {
				tempSpeed -=speed *2 * Time.deltaTime / cXY;
			}else {
				tempSpeed =0;
			}
		}
		return tempSpeed;
	}
	// Update is called once per frame
	void Update () {
		Debug.Log("TempX:"+ temX);
		Debug.Log("TempY:"+ temY);
		if (temX > temY) {
			gameObject.transform.Rotate(new Vector3(0,axisX,0) * Rigid(),Space.World);
		}else {
			gameObject.transform.Rotate(new Vector3(axisY,0,0) * Rigid(),Space.World);
		}

		if (!Input.GetMouseButton(0)) {
			onDrag = false;
			temX = 0f;
			temY = 0f;
			Common.IS_CAMERA_CONTROL = true;
		}
	}
}
