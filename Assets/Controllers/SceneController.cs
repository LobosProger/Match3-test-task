using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
	private const string sceneGame = "Game";
	private const string sceneMenu = "Menu";
	private const string sceneRecordsMenu = "Records menu";
	private const string sceneAboutProgram = "About program";

	public static void LoadGame() => SceneManager.LoadScene(sceneGame);
	public static void LoadMenu() => SceneManager.LoadScene(sceneMenu);
	public static void LoadRecordsMenu() => SceneManager.LoadScene(sceneRecordsMenu);
	public static void LoadAboutProgram() => SceneManager.LoadScene(sceneAboutProgram);
	public static void ExitFromGame() => Application.Quit();
	public static void OpenProfile() => Application.OpenURL("https://github.com/LobosProger");
}
