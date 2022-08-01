using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	private int amountOfRemainedSteps = 5;
	public static GameController singletonGameController;

	[SerializeField]
	private Text textToShowSteps;
	private Animator animatorOfText;
	private readonly int animatorState_Zoom = Animator.StringToHash("Zoom");

	[SerializeField]
	private Text textToShowScore;
	private int score = 0;

	[SerializeField]
	private GameObject gameOverPanel;
	[SerializeField]
	private GameObject pausePanel;
	private void Start()
	{
		animatorOfText = textToShowSteps.gameObject.GetComponent<Animator>();
		singletonGameController = this;
		ShowAmountStepsOnText();
		ScoreController.LoadScore();
	}

	public void DestroyBubbles(int amountOfDestroyingBubbles)
	{
		score += amountOfDestroyingBubbles;
		if (amountOfDestroyingBubbles == 1)
		{
			amountOfRemainedSteps--;
		}
		else
		{
			//* Если число лопнутых шаров более одного, то добавляем число ходов и воспроизводим анимацию на тексте
			amountOfRemainedSteps += amountOfDestroyingBubbles - 1;
			animatorOfText.SetTrigger(animatorState_Zoom);
		}
		ShowScoreOnText();
		ShowAmountStepsOnText();

		if (amountOfRemainedSteps == 0)
		{
			BoardController.singletonBoardController.SetBoardClickable(false);
			if (ScoreController.IsNewRecord(score))
			{
				StartCoroutine(LoadRecordsMenu());
			}
			else
			{
				StartCoroutine(ShowGameOverPanel());
			}
		}

	}

	private IEnumerator LoadRecordsMenu()
	{
		yield return new WaitForSecondsRealtime(1.5f);
		SceneController.LoadRecordsMenu();
	}

	private IEnumerator ShowGameOverPanel()
	{
		yield return new WaitForSecondsRealtime(1.5f);
		gameOverPanel.SetActive(true);
	}

	public void ShowPausePanel()
	{
		if (amountOfRemainedSteps != 0)
			pausePanel.SetActive(true);
	}

	private void ShowAmountStepsOnText() => textToShowSteps.text = amountOfRemainedSteps.ToString();
	private void ShowScoreOnText() => textToShowScore.text = score.ToString();
}
