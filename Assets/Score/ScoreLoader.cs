using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreLoader : MonoBehaviour
{
	public static List<ScoreData> tableWithScore = new List<ScoreData>();
	public struct ScoreData
	{
		public string time;
		public int score;
	}
	private const string pathOfTableOfScore = "Table of score/Score";
	private const string _nameOfFieldTimeToWriteInMemory = "Time";
	private const string _nameOfFieldScoreToWriteInMemory = "Score";
	private const string _nameOfFieldAmountScoreToWriteInMemory = "Amount score";
	private static bool isScoreLoadedIntoGame;

	public static void LoadScoreInGame()
	{
		if (!PlayerPrefs.HasKey(_nameOfFieldAmountScoreToWriteInMemory))
		{
			TextAsset table = Resources.Load<TextAsset>(pathOfTableOfScore);
			string[] data = table.text.Split(new string[] { ";", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
			for (int row = 2; row < data.Length; row += 2)
			{
				ScoreData score = new ScoreData
				{
					time = data[row],
					score = int.Parse(data[row + 1])
				};
				WriteScore(score);
			}
		}
		else
		{
			for (int scoreIndex = 0; scoreIndex < PlayerPrefs.GetInt(_nameOfFieldAmountScoreToWriteInMemory); scoreIndex++)
			{
				ScoreData score = ReadScore(scoreIndex);
				tableWithScore.Add(score);
			}
		}

		isScoreLoadedIntoGame = true;
	}

	private static ScoreData ReadScore(int scoreIndex)
	{
		return new ScoreData
		{
			time = PlayerPrefs.GetString(_nameOfFieldTimeToWriteInMemory + scoreIndex.ToString()),
			score = PlayerPrefs.GetInt(_nameOfFieldScoreToWriteInMemory + scoreIndex.ToString())
		};
	}

	public static void WriteScore(ScoreData scoreData)
	{
		tableWithScore.Add(scoreData);
		PlayerPrefs.SetString(_nameOfFieldTimeToWriteInMemory + (tableWithScore.Count - 1).ToString(), scoreData.time);
		PlayerPrefs.SetInt(_nameOfFieldScoreToWriteInMemory + (tableWithScore.Count - 1).ToString(), scoreData.score);
		PlayerPrefs.SetInt(_nameOfFieldAmountScoreToWriteInMemory, tableWithScore.Count);
	}

	public static void WriteScore(int score)
	{
		ScoreData scoreData = new ScoreData
		{
			time = System.DateTime.Now.ToString(),
			score = score
		};
		tableWithScore.Add(scoreData);
		PlayerPrefs.SetString(_nameOfFieldTimeToWriteInMemory + (tableWithScore.Count - 1).ToString(), scoreData.time);
		PlayerPrefs.SetInt(_nameOfFieldScoreToWriteInMemory + (tableWithScore.Count - 1).ToString(), scoreData.score);
		PlayerPrefs.SetInt(_nameOfFieldAmountScoreToWriteInMemory, tableWithScore.Count);
	}

	public static bool IsScoreLoaded() => isScoreLoadedIntoGame;
}
