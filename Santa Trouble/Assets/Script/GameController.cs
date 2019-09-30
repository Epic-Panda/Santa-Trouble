using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	[SerializeField]
	private GameObject gameStatusCanvas;
	[SerializeField]
	private Text lifeTxt;
	[SerializeField]
	private Text timeTxt;
	[SerializeField]
	private Text scoreTxt;
	[SerializeField]
	private Text gameEndScoreTxt;
	[SerializeField]
	private Text playerName;

	[SerializeField]
	private GameObject gameEndCanvas;

	private int score = 0;
	private int time = 300;
	private int life = 3;
	private bool pause = false;
	private bool gameEnd = false;
	private bool callOnce = true;

	private static GameController scoreController;

	public static GameController Controller {
		get {
			if (scoreController == null)
				scoreController = GameObject.FindObjectOfType<GameController> ();
			return scoreController;
		}
	}

	// Use this for initialization
	void Start ()
	{
		Cursor.visible = false;	
		StartCoroutine (timeUpdate ());
	}

	// Update is called once per frame
	void Update ()
	{
		if (!gameEnd) {
			pauseCheck ();
			endOfGameCheck ();
		} else if (gameEnd && callOnce) {
			finishGame ();
			callOnce = false;
			finishScore ();
			gameStatusCanvas.SetActive (false);
			gameEndCanvas.SetActive (true);
			Cursor.visible = true;
		}
	}

	public void addScore (int s)
	{
		score += s;
		scoreTxt.text = "Score: " + score;
	}

	public void addLife (int l)
	{
		life += l;
		lifeTxt.text = "Life: " + life;
	}

	public int getLife ()
	{
		return life;
	}

	private IEnumerator timeUpdate ()
	{
		while (time > 0 && !gameEnd) {
			if (!gameEnd && !pause) {
				timeTxt.text = "Time: " + time / 60 + ':' + time % 60;
				time--;
				yield return new WaitForSeconds (1);
			} else
				yield return new WaitUntil (() => pause == false);
		}
	}

	public bool GameEnd {
		set{ gameEnd = value; }
	}

	private void pauseGame ()
	{
		PlayerController.Controller.Stop = pause;
		foreach (PlatformController p in PlatformController.Controller)
			p.Stop = pause;
		foreach (CollectableController c in CollectableController.Controller)
			c.Stop = pause;
	}

	private void finishGame ()
	{
		PlayerController.Controller.Stop = gameEnd;
		foreach (PlatformController p in PlatformController.Controller)
			p.Stop = gameEnd;
		foreach (CollectableController c in CollectableController.Controller)
			c.Stop = gameEnd;
	}

	private void pauseCheck ()
	{
		if (Input.GetKeyDown ("p") || Input.GetKeyDown (KeyCode.Escape)) {
			if (pause == false) {
				pause = true;
				Cursor.visible = true;
			} else {
				pause = false;
				Cursor.visible = false;
			}
			pauseGame ();
		}
	}

	private void endOfGameCheck ()
	{
		if (life == 0)
			gameEnd = true;
	}

	private void finishScore ()
	{
		score += life * time;
		gameEndScoreTxt.text = "Your score: " + score;
	}

	public void goToMainMenu ()
	{
		int currScore = score, oldScore;
		string currName = playerName.text, oldName;

		if (currName.Length > 11)
			currName = currName.Substring (0, 11);

		for (int i = 0; i < 10; i++) {
			if (PlayerPrefs.HasKey (i + "HiScore")) {
				if (PlayerPrefs.GetInt (i + "HiScore") < currScore) {
					oldScore = PlayerPrefs.GetInt (i + "HiScore");
					oldName = PlayerPrefs.GetString (i + "HiScoreName");
					PlayerPrefs.SetInt (i + "HiScore", currScore);
					PlayerPrefs.SetString (i + "HiScoreName", currName);
					currName = oldName;
					currScore = oldScore;
				}
			} else {
				PlayerPrefs.SetInt (i + "HiScore", currScore);
				PlayerPrefs.SetString (i + "HiScoreName", currName);
				currName = "";
				currScore = 0;
			}
		}

		SceneManager.LoadScene ("MainMenu");
	}
}
