/// <summary>
/// Author: Fu
/// CreateDate: 2015-01-20
/// Function:碰撞分析
/// </summary>
using UnityEngine;
using System.Collections;

public class ColliderAnalysis : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider other) {
		Debug.Log(other.gameObject.name);
	}
}
