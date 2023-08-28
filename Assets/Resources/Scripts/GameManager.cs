using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : Singleton<GameManager> {
	private Patient[] patients = new Patient[12];
	public bool GameOver {private set; get;}
	
	[SerializeField] private GameObject endScreen, checkpoint, checkpointDDR, patientInfo;
	[SerializeField] private TMPro.TMP_Text saved, died;
    private AudioManager am;

    IEnumerator Start() {
		am = FindObjectOfType<AudioManager>();
        am.Play("MainTheme");
        patients = FindObjectsOfType<Patient>();
		
		while(true) {
			yield return new WaitForSeconds(1f);
			int patientsHealthy = 0, patientsSick = 0, patientsDead = 0;
			
			foreach(Patient patient in patients) {
				if(patient.currentState == Patient.State.Dead || patient.currentState == Patient.State.Harvestable)
					patientsDead++;
				else if(!patient.Organs.Any(organ => !organ.Healthy) && patient.Organs.Count == (int)Organ.Type.Length)
					patientsHealthy++;
				else
					patientsSick++;
			}
			
			if(patientsSick == 0)
				DisplayResults(patientsHealthy, patientsDead); 
		}
    }
	
	private void DisplayResults(int patientsSaved, int patientsDied) {
		endScreen.SetActive(true);
		checkpoint.SetActive(false);
		checkpointDDR.SetActive(false);
		patientInfo.SetActive(false);
		
		Time.timeScale = 0;
		GameOver = true;
		
		saved.text = patientsSaved.ToString();
		died.text = patientsDied.ToString();	
	}
	
	void Update() {
		if(GameOver && Input.GetKeyDown(KeyCode.Escape))
			SceneManager.LoadScene("StartMenu");
	}
	
	void OnDisable() => Time.timeScale = 1;
}