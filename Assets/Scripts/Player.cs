/// <summary>
/// Author: Fu
/// CreateDate: 2015-01 - 04 !$:#&
/// Function:
/// </summary>
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public Transform mTransform;
	protected  CharacterController mChController;
	public float mMoveSpeed = 3.0f;
	public float mGravity = 2.0f;
	public int mLife = 5;
	public Transform mCamTransform;		// 摄像机Transform
	protected Vector3 mCamRotation;		// 摄像机Rotation
	protected float mCamHeight = 1.4f;		// 摄像机高度
	protected Transform mMuzzlePoint;		// 枪口Transform
	public LayerMask mLayer;		// 射击时，射线能碰到的碰撞层
	public Transform mFx;		// 射中目标后的粒子效果
	public AudioClip mAudio;		// 射击音效
	protected float mShootTimer = 0;		// 射击间隔时间计时器
	// Use this for initialization
	void Start () {
		mTransform = this.transform;
		mChController = this.GetComponent<CharacterController>();
		mCamTransform = Camera.main.transform;
		Vector3 pos = mTransform.position;
		pos.y +=mCamHeight;
		mCamTransform.position = pos;
		mCamTransform.rotation = mTransform.rotation;
		mCamRotation = mTransform.eulerAngles;
		Screen.lockCursor = true;
		mMuzzlePoint = mCamTransform.FindChild("M16/weapon/muzzlepoint").transform;

	}
	
	// Update is called once per frame
	void Update () {
		if (mLife <=0) {
			return;
		}
		Control();
		// 更新射击间隔时间
		mShootTimer -= Time.deltaTime;
		// 鼠标左键射击
		if (Input .GetMouseButton(0) && mShootTimer <= 0) {
			mShootTimer = 0.1f;
			// 射击音效
			this.audio.PlayOneShot(mAudio);
			// 减少弹药，更新弹药UI
			GameManager.Instance.SetAmmo(1);
			// RaycastHit用来保存射线的探测结果
			RaycastHit info;

			// 从muzzlepoint的位置，向摄像机面向的正方向射出一根射线
			// 射线只能与mPlayer所指向的层碰撞
			bool hit = Physics.Raycast(mMuzzlePoint.position,mCamTransform.TransformDirection(Vector3.forward),out info, 100,mLayer);
			if (hit ) {
				// 如果射中了tag为enemy的游戏体
				if (info.transform.tag.CompareTo ("Enemy") == 0) {
					Enemy enemy = info.transform.GetComponent<Enemy>();
					// 减少敌人生命
					enemy.OnDamage(1);
				}
				// 射中的地方释放一个粒子效果
				Instantiate(mFx,info.point,info.transform.rotation);
			}
		}

	}
	void Control () {
		// 	获取鼠标移动距离
		float rh = Input.GetAxis("Mouse X");
		float rv = Input.GetAxis("Mouse Y");
		
		// 旋转摄像机
		mCamRotation.x -= rv;
		mCamRotation.y += rh;
		mCamTransform.eulerAngles = mCamRotation;
		
		// 使主角的方向与摄像机一致
		Vector3 camrot = mCamTransform .eulerAngles;
		camrot .x = 0 ;
		camrot.z = 0;
		mTransform.eulerAngles = mCamRotation;
		// 定义三个值控制移动
		float xm = 0, ym = 0,zm = 0;
		// 重力运动
		ym -= mGravity *Time.deltaTime;
		// 上下左右运动
		if (Input.GetKey(KeyCode.W)) {
			zm += mMoveSpeed *Time.deltaTime;

		} else if (Input.GetKey(KeyCode.S)) {
			zm -= mMoveSpeed*Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.A)) {
			xm -= mMoveSpeed * Time .deltaTime;

		} else if ( Input.GetKey(KeyCode.D)) {
			xm += mMoveSpeed  * Time.deltaTime;
		}
		// 使用角色控制器提供的move函数进行移动，他会自动检测碰撞
		mChController.Move(mTransform.TransformDirection(new Vector3(xm,ym,zm)));

		// 使摄像机的位置与主角一致
		Vector3 pos = mTransform.position;
		pos.y += mCamHeight;
		mCamTransform.position = pos;

	}
	// 在编辑器中为主角显示一个图标
	void OnDrawGizmos () {
		Gizmos.DrawIcon(this.transform.position,"Spawn.tif");
	}
	public void OnDamage (int damage) {
		mLife -= damage;
		GameManager.Instance.SetLife(mLife);
		// 如果生命为零，取消鼠标锁定
		if ( mLife <= 0) {
			Screen.lockCursor = false;
		}
	}
}
