using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : Singleton<MenuUI> {
	[SerializeField] private AudioSource amogusPlayer, molePlayer; 
	 
	public void StartGame() => 
	SceneManager.LoadScene("Lvl1new"); 
	 
	public void ShowTutorial() => 
	SceneManager.LoadScene("Tutorial"); 
	 
	public void QuitGame() => 
	Application.Quit(); 
	 
	public void MainMenu() => 
	SceneManager.LoadScene("StartMenu"); 
	 
	public void Mole()  {
		if(!molePlayer.isPlaying) 
		molePlayer.Play(); 
	} 
} 
