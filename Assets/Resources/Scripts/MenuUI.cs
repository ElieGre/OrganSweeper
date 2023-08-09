using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
  [SerializeField]private AudioSource amogusPlayer, molePlayer;
  
    public void StartGame()
  {
        SceneManager.LoadScene("Lvl1new");
  }
  public void ShowTutorial()
  {
    SceneManager.LoadScene("Tutorial");
  }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
      SceneManager.LoadScene("StartMenu");
    }
  
  public void Mole() {
		if(!molePlayer.isPlaying)
		  molePlayer.Play();
  }      
}