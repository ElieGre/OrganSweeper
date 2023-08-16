using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillCheckAddOrgan : Singleton<SkillCheckAddOrgan> 
{
	[SerializeField] private GameObject skillcheckParent;
	[SerializeField] private TMP_Text letterText;

    private int correctLetterCount;
	
	static char[] validLetters = { 'A', 'B', 'C', 'D', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
	
	private Patient operatingPatient;
	private bool skillcheckEnabled;

    private void Update() {
		if(!skillcheckEnabled)
			return;

        // If there is no user input, return.
        if (!Input.anyKeyDown || string.IsNullOrEmpty(letterText.text)) 
			return;
		
        string input = GetInputFromKeyCode();

        // Check if the pressed key matches the current letter
        if (input.Equals(letterText.text)) {
            correctLetterCount++;

            if (correctLetterCount >= 5)
                DisableSkillcheck();
            else
                GenerateLetter();
        } else {
			operatingPatient.SkillcheckLoseHealth();
			DisableSkillcheck();
        }
    }

    private void GenerateLetter() {
		letterText.text = validLetters[Random.Range(0, validLetters.Length)].ToString();
    }

    private string GetInputFromKeyCode() {
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            if (Input.GetKeyDown(keyCode) && (keyCode >= KeyCode.A && keyCode <= KeyCode.Z))
                return keyCode.ToString();

        return string.Empty;
    }
	
	void DisableSkillcheck() {
		skillcheckParent.SetActive(false);
		skillcheckEnabled = false;
	}
	
	public void StartSkillcheck(Patient operatingPatient) {
		this.operatingPatient = operatingPatient;	
		
		GenerateLetter();
		correctLetterCount = 0;
		
		skillcheckParent.SetActive(true);
		skillcheckEnabled = true;
	}
	
	public bool IsSkillCheckInProgress() => skillcheckEnabled;
	
}