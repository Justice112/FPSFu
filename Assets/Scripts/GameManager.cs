/// <summary>
/// Author: Fu
/// CreateDate: 2015-01-04
/// Function:
/// </summary>
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static GameManager Instance = null;
	int mScore = 0;
	static int mHiscore = 0;
	int mAmmo = 100;
	Player mPlayer;
	GUIText txt_ammo;
	GUIText txt_hiscore;
	GUIText txt_life;
	GUIText txt_score;

	// Use this for initialization
	void Start () {
		Instance = this;
		// 获得主角
		mPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		foreach (Transform t in this.transform.GetComponentsInChildren<Transform>()) {
			if (t.name.CompareTo("txt_ammo") == 0) {
				txt_ammo = t.GetComponent<GUIText>();
			} else if (t.name.CompareTo("txt_hiscore") == 0) {
				txt_hiscore = t.GetComponent<GUIText>();
				txt_hiscore.text = "High Score:" + mHiscore;
			} else if (t.name.CompareTo("txt_life") == 0) {
				txt_life = t.GetComponent<GUIText>();
			} else if (t.name.CompareTo("txt_score") == 0) {
				txt_score = t.GetComponent<GUIText>();
			}
		}

	}
	/// <summary>
	/// 更新分数
	/// </summary>
	/// <param name="score">Score.</param>
	public void SetScore(int score) {
		mScore += score;
		if (mScore > mHiscore) {
			mHiscore = mScore;
		}
		txt_score.text = "Score <color=yellow>" + mScore + "</color>";
		txt_hiscore.text = "High Score" + mHiscore;
	}
	/// <summary>
	/// 更新弹药
	/// </summary>
	/// <param name="ammo">Ammo.</param>
	public void SetAmmo (int ammo) {
		mAmmo -= ammo;
		if(mAmmo <= 0) {
			mAmmo = 100 - mAmmo;
		}
		txt_ammo.text = mAmmo.ToString()+"/100";
	}
	/// <summary>
	/// 更新生命
	/// </summary>
	/// <param name="life">Life.</param>
	public void SetLife (int life) {
		txt_life.text = life.ToString();
	}
	void OnGUI () {
		if(mPlayer.mLife <= 0) {
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.skin.label.fontSize = 40;
			GUI.Label(new Rect(0,0,Screen.width,Screen.height),"Game Over");
			GUI.skin.label.fontSize = 30;
			if (GUI.Button(new Rect(Screen.width * 0.5f-150,Screen.height * 0.75f,300,40),"Try Again")) {
				Application.LoadLevel(Application.loadedLevelName);
			}
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
