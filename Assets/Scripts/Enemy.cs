/// <summary>
/// Author: Fu
/// CreateDate: 2015-01-04 !%:!(
/// Function:
/// </summary>
using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	Transform mTransform;		// 
	Player mPlayer;		// 主角
	NavMeshAgent mAgent;		// 寻路组件
	float mMoveSpeed = 0.5f;
	Animator mAnimator;		// 动画组件
	float mRotSpeed = 30;
	float mTimer = 2;
	int mLife = 3;
	protected EnemySpawn mSpawn;		// 生成点

	// Use this for initialization
	void Start () {
		mTransform = this.transform;
		// 获取动画组件
		mAnimator = this.GetComponent<Animator>();
		mPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		// 获得寻路组件
		mAgent = GetComponent<NavMeshAgent>();
		mAgent.speed = mMoveSpeed;

		mAgent.SetDestination(mPlayer.mTransform.position);
	}
	void RotateTo () {
		Vector3 targetDir = mPlayer.mTransform.position - mTransform.position;
		// 计算方向
		Vector3 newDir = Vector3.RotateTowards(transform.forward,targetDir,mRotSpeed * Time.deltaTime, 0.0f);
		// 旋转至方向
		mTransform.rotation = Quaternion.LookRotation(newDir);
	}
	// Update is called once per frame
	void Update () {
		if (mPlayer.mLife <= 0) {
			return;
		}
		AnimatorStateInfo stateInfo = mAnimator.GetCurrentAnimatorStateInfo(0);
		if (stateInfo.nameHash == Animator.StringToHash("Base Layer.idle") && !mAnimator.IsInTransition(0) ) {
			mAnimator.SetBool("idle",false);
			mTimer -= Time.deltaTime;
			if (mTimer > 0) {
				return;
			}
			if (Vector3.Distance(mTransform.position,mPlayer.mTransform.position) < 1.5f) {
				mAnimator.SetBool("attack",true);
			} else {
				mTimer = 1;		// 重置计时器
				mAgent.SetDestination(mPlayer.mTransform.position);		// 设置寻路目标
				// 进入跑步动画状态
				mAnimator.SetBool("run",true);
			}
		}
		// 如果处于跑步状态
		if (stateInfo.nameHash == Animator.StringToHash("Base Layer.run") && !mAnimator.IsInTransition(0)) {
			mAnimator.SetBool("run",false);

			// 每隔1秒重新定位主角的位置
			mTimer -= Time.deltaTime;
			if (mTimer < 0) {
				mAgent.SetDestination(mPlayer.mTransform.position);
				mTimer =1;
			}

			// 如果距离主角小于1.5米，向主角攻击
			if (Vector3.Distance(mTransform.position,mPlayer.mTransform.position) <=1.5f) {
				// 停止寻路
				mAgent.Stop();
				// 进入攻击状态
				mAnimator.SetBool("attack",true);
			}
		} 
		// 如果处于攻击状态
		if (stateInfo.nameHash == Animator.StringToHash("Base Layer.attack") && !mAnimator.IsInTransition(0)) {
			// 面向主角
			RotateTo();
			mAnimator.SetBool("attack",false);

			// 如果动画播完，重新进入待机状态
			if (stateInfo.normalizedTime >= 1.0f) {
				// 进入待机状态
				mAnimator.SetBool("idle",true);
				// 重置计时器
				mTimer = 2;
				// 更新主角的生命
				mPlayer.OnDamage(1);
			}
		}
		// 死亡状态
		if (stateInfo.nameHash == Animator.StringToHash ("Base Layer.death") && !mAnimator.IsInTransition(0)) {
			// 当播放完死亡动画
			if (stateInfo.normalizedTime >= 1.0f) {
				// 加分
				GameManager.Instance.SetScore(100);
				// 销毁自身
				Destroy(this.gameObject);
				// 更新敌人计数
				mSpawn.mEnemyCount --;
			}
		}
	}
	public void OnDamage(int damage) {
		mLife -= damage ;
		// 如果生命为零进入死亡状态
		if (mLife <= 0) {
			mAnimator.SetBool ("death",true);
		}
	}
	public void Init(EnemySpawn spawn) {
		mSpawn = spawn;
		// 该敌人的出生点增加计数
		mSpawn.mEnemyCount++;
	}

}
