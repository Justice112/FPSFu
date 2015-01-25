/// <summary>
/// Author: Fu
/// CreateDate: 2015-01-06
/// Function:
/// </summary>
using UnityEngine;
using System.Collections;

public class AutoDesroy : MonoBehaviour {
	public float mTimer = 1.0f;

	// Use this for initialization
	void Start () {
		Destroy (this.gameObject,mTimer);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
