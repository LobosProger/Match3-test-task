using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreShower : MonoBehaviour
{
	[SerializeField]
	private Text dateText;
	[SerializeField]
	private Text scoreText;

	public void ShowData(string date, int score)
	{
		dateText.text = date;
		scoreText.text = score.ToString();
	}

	public void LightTheBlock()
	{
		GetComponent<Image>().color = Color.yellow;
	}
}
