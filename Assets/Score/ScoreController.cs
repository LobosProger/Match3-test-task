using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ScoreController : MonoBehaviour
{
	[SerializeField]
	private ScoreShower prefabOfScoreShower;
	[SerializeField]
	private Transform contentOfScrollRect;
	private static int maxScoreOfPlayer = 0;
	private static bool isPlayerAchievedNewRecord;
	private void Start()
	{
		LoadScore();

		List<ScoreLoader.ScoreData> sortedListWithRecords = new List<ScoreLoader.ScoreData>(ScoreLoader.tableWithScore);
		sortedListWithRecords = sortedListWithRecords.OrderByDescending(x => x.score).ToList();

		foreach (ScoreLoader.ScoreData every in sortedListWithRecords)
		{
			ScoreShower createdScoreShower = Instantiate(prefabOfScoreShower);
			createdScoreShower.transform.SetParent(contentOfScrollRect);
			if (isPlayerAchievedNewRecord)
			{
				isPlayerAchievedNewRecord = false;
				createdScoreShower.LightTheBlock();
			}

			createdScoreShower.ShowData(every.time, every.score);
		}
	}

	public static bool IsNewRecord(int record)
	{
		if (record > maxScoreOfPlayer)
		{
			maxScoreOfPlayer = record;
			ScoreLoader.WriteScore(record);
			isPlayerAchievedNewRecord = true;
			return true;
		}
		else
		{
			return false;
		}
	}

	private static void GetMaxScoreOfPlayer()
	{
		foreach (ScoreLoader.ScoreData everyScoreData in ScoreLoader.tableWithScore)
		{
			if (everyScoreData.score > maxScoreOfPlayer)
				maxScoreOfPlayer = everyScoreData.score;
		}
	}

	public static void LoadScore()
	{
		if (!ScoreLoader.IsScoreLoaded())
		{
			ScoreLoader.LoadScoreInGame();
			GetMaxScoreOfPlayer();
		}
	}
}
