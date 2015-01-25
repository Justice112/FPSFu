/// <summary>
/// Author: Fu
/// CreateDate: 2015-1-6
/// Function:
/// </summary>
using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour {
	public Transform mEnemy;		// 	敌人的prefab 
	public int mEnemyCount = 0;		// 生成的敌人数量
	public int mMaxEnemyNum = 3;		// 敌人的最大生成数量
	public float mTimer= 0;		// 生成敌人的时间间隔
	protected Transform mTransform;		// 

	// Use this for initialization
	void Start () {
		mTransform = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
		// 如果生成的敌人数量达到最大值，停止生成敌人
		if(mEnemyCount >= mMaxEnemyNum) {
			return;
		}
		// 每间隔一定的时间
		mTimer -= Time.deltaTime;
		if (mTimer <= 0) {
			// 得到下一轮生成敌人的间隔时间，最大值15秒，最小5秒
			mTimer = Random.value * 15.0f;
			if (mTimer < 5) {
				mTimer = 5;
			}
			// 生成敌人
			Transform obj = (Transform) Instantiate (mEnemy,mTransform.position,Quaternion.identity);
			// 获取敌人的脚本
			Enemy enemy = obj.GetComponent<Enemy>();
			// 初始化敌人
			enemy.Init(this);
		}
	}
	void OnDrawGizmos () {
		Gizmos.DrawIcon(transform.position,"Item.png",true);
	}
}
