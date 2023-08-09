using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SurgeryUI : MonoBehaviour 
{
	[SerializeField] private GameObject parentObj;
	[SerializeField] private Image[] organSprites;
	[SerializeField] private TMP_Text[] organTexts;
	[SerializeField] private Image[] organConditions;
	[SerializeField] private OrganUI[] organScripts;
	private SkillCheckRemoveOrgan skillcheck;
	private LetterPopUp skillcheckDDR;
	
	[Space(10)]
	
	[SerializeField] private Image healthbarFillImage;
	[SerializeField] private TMP_Text healthbarText;
	
	[SerializeField] private Sprite[] conditionSprites;
	
	private static List<string> organs = new List<string> {"heart", "lungs", "kidney", "liver", "appendix", "pancreas", "intestines", "bladder"};

	private Patient patientReference;
	
	void Start() 
	{
		skillcheck = GameObject.FindWithTag("GameController").GetComponent<SkillCheckRemoveOrgan>();
		skillcheckDDR = GameObject.FindWithTag("GameController").GetComponent<LetterPopUp>();
	}
	
	void Update() 
	{
		if(Input.GetKeyDown(KeyCode.Escape) && IsPatientInfoOpen() && !skillcheck.IsSkillCheckInProgress())
		{
			ClosePatientInfo();
		}
		if(patientReference != null) 
		{
			healthbarFillImage.fillAmount = patientReference.GetBlood() / 100f;
			healthbarText.text = (int)patientReference.GetBlood() + "/" + 100;
		}
	}
	
    public void LoadPatient(Patient newPatient) 
	{
		patientReference = newPatient;
		parentObj.SetActive(true);
		
		ResetPatientInfo();
	}
	
	public void ResetPatientInfo() 
	{	
		List<string> organsHealthy = patientReference.GetOrgansHealthy();
		List<string> organsBad = patientReference.GetOrgansBad();
		List<string> organsMissing = patientReference.GetOrgansMissing();
		
		int i = 0;
		foreach(string organName in organs) 
		{
			bool organMissing = organsMissing.Contains(organName);
			organScripts[i].Set(organName, organMissing, patientReference);
			
			organTexts[i].text = organName.Substring(0, 1).ToUpper() + organName.Substring(1);
			if(organsHealthy.Contains(organName)) 
			{
				organSprites[i].sprite = OrganHelper.GetOrganIconByName(organName, true);
				organConditions[i].sprite = conditionSprites[0];
			} 
			else if(organsBad.Contains(organName)) 
			{
				organSprites[i].sprite = OrganHelper.GetOrganIconByName(organName, false);
				organConditions[i].sprite = conditionSprites[1];
			} 
			else 
			{
				organSprites[i].sprite = null;
				organConditions[i].sprite = conditionSprites[2];
			}
			i++;
		}
	}
	
	public bool IsPatientInfoOpen() 
	{
		return parentObj.activeSelf;
	}
	
	public void ClosePatientInfo() 
	{
		parentObj.SetActive(false);
		
		foreach(OrganUI organScript in organScripts)
		{
			organScript.Reset();
		}
	}
}