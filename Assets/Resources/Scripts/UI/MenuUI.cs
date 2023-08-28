using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : Singleton<MenuUI> {
	private AudioManager am; 
	 
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
        am.Play("MainMenuMusic");
    } 

	public void StartGame() => 
		SceneManager.LoadScene("Lvl1new"); 
	 
	public void ShowTutorial() => 
		SceneManager.LoadScene("Tutorial"); 
	 
	public void QuitGame() => 
		Application.Quit(); 
	 
	public void MainMenu() => 
		SceneManager.LoadScene("StartMenu"); 
	 
} 
