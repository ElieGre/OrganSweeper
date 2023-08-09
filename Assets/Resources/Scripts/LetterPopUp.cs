using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LetterPopUp : MonoBehaviour 
{
	[SerializeField] private GameObject skillcheckParent;
	[SerializeField] private TMP_Text letterText;

    private string currentLetter;
    private int correctLetterCount;
	
	static char[] validLetters = { 'B', 'C', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'T', 'U', 'V', 'X', 'Y', 'Z' };
	
	private Patient operatingPatient;
	private bool skillcheckEnabled;

    private void Update() 
    {
		if(!skillcheckEnabled)
        {
			return;
        }

        // Check for user input
        if (Input.anyKeyDown && !string.IsNullOrEmpty(currentLetter)) 
        {
            string input = GetInputFromKeyCode();

            // Check if the pressed key matches the current letter
            if (input.Equals(currentLetter)) 
            {
                correctLetterCount++;

                if (correctLetterCount >= 5)
                {
                    DisableSkillcheck();
                }
                else
                {
                    GenerateLetter();
                }
            } 
            else 
            {
				operatingPatient.LoseBlood(30);
				DisableSkillcheck();
			}
        }

    }

    private void GenerateLetter() 
    {
		currentLetter = validLetters[Random.Range(0, validLetters.Length)].ToString();
        letterText.text = currentLetter;
    }

    private string GetInputFromKeyCode() 
    {
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode))) 
        {
            if (Input.GetKeyDown(keyCode) && (keyCode >= KeyCode.A && keyCode <= KeyCode.Z))
            {
                return keyCode.ToString();
            }
        }

        return string.Empty;
    }

	void EnableSkillcheck() 
    {
		GenerateLetter();
		correctLetterCount = 0;
		
		skillcheckParent.SetActive(true);
		skillcheckEnabled = true;
	}
	
	void DisableSkillcheck() 
    {
		skillcheckParent.SetActive(false);
		skillcheckEnabled = false;
	}
	
	public void StartSkillcheck(Patient operatingPatient) 
    {
		this.operatingPatient = operatingPatient;		
		EnableSkillcheck();
	}
	
	public bool IsSkillCheckInProgress() 
    {
		return skillcheckEnabled;
	}
}
