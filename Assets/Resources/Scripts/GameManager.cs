using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private int patientsHealthy, patientsSick, patientsDead;

	private Patient[] patients = new Patient[12];
	private bool gameOver;
	
	[SerializeField] private GameObject endScreen, checkpoint, checkpointDDR, patientInfo;
	[SerializeField] private TMP_Text saved, died;
	
	
    IEnumerator Start()
    {
        patients = FindObjectsOfType<Patient>();
		
		while(true) 
        {
			patientsDead = 0;
			patientsHealthy = 0;
			patientsSick = 0;
			
			foreach(Patient patient in patients) 
            {
				if(patient.IsDead() || patient.IsHarvestable())
                {
					patientsDead++;
                }
				else if(patient.GetOrgansHealthy().Count == 8)
                {
					patientsHealthy++;
                }
				else
                {
					patientsSick++;
                }
			}
			
			if(patientsSick == 0)
            {
				GameOver();
            }
			yield return new WaitForSeconds(1f);
		}
    }
	
	void Update() 
    {
		if(gameOver && Input.GetKeyDown(KeyCode.Escape)) 
        {
			Time.timeScale = 1;
			SceneManager.LoadScene("StartMenu");
		}
	}
	
	private void GameOver() 
    {
		endScreen.SetActive(true);
		checkpoint.SetActive(false);
		checkpointDDR.SetActive(false);
		patientInfo.SetActive(false);
		
		Time.timeScale = 0;
		gameOver = true;
		
		saved.text = patientsHealthy.ToString()	;
		died.text = patientsDead.ToString();	
	}
	
	public bool IsGameOver() 
    {
		return gameOver;
	}
}
