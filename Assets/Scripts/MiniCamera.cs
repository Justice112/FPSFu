/// <summary>
/// Author: Fu
/// CreateDae:2015-1-6
/// Function:
/// </summary>
using UnityEngine;
using System.Collections;
[AddComponentMenu("Game/MiniMap")]
public class MiniCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// 获得屏幕分辨率比例
		float ratio = (float) Screen.width / (float) Screen.height;
		// 试摄像机视图永远是个正方形，rect的前两个参数表示XY位置，后两个参数是XY大小；
		this.camera.rect = new Rect((1-0.2f),(1 - 0.2f* ratio),0.2f,0.2f*ratio);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
