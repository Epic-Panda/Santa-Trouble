using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighScoreController : MonoBehaviour
{

	[SerializeField]
	private Text nameText;
	[SerializeField]
	private Text scoreText;

	void Start ()
	{
		string highScoreName = "Name       ";
		string scores = "Score";
		for (int i = 0; i < 10; i++) {
			if (PlayerPrefs.HasKey (i + "HiScore")) {
				highScoreName += "\n" + PlayerPrefs.GetString (i + "HiScoreName");
				scores += "\n" + PlayerPrefs.GetInt (i + "HiScore");
			} else
				break;
		}
		nameText.text = highScoreName;
		scoreText.text = scores;
	}

	public void toMainMenu ()
	{
		SceneManager.LoadScene ("MainMenu");
	}
}
