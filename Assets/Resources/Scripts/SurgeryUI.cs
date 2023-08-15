using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SurgeryUI : Singleton<SurgeryUI> {
	[SerializeField] private GameObject parentObj;
	[SerializeField] private Image[] organSprites;
	[SerializeField] private TMP_Text[] organTexts;
	[SerializeField] private Image[] organConditions;
	[SerializeField] private OrganUI[] organScripts;
	
	[Space(10)]
	
	[SerializeField] private Image healthbarFillImage, patientSprite;
	[SerializeField] private TMP_Text healthbarText;
	
	[SerializeField] private Sprite[] conditionSprites;
	
	private Patient patientReference;
	
	void Update() {
		if((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
		 && !SkillCheckAddOrgan.Instance.IsSkillCheckInProgress() && !SkillCheckRemoveOrgan.Instance.IsSkillCheckInProgress()) {
			ClosePatientInfo();
		}
		
		if(patientReference != null) {
			healthbarFillImage.fillAmount = patientReference.Blood / 100f;
			healthbarText.text = (int)patientReference.Blood + "/" + 100;
		}
	}
	
    public void LoadPatient(Patient newPatient) {
		patientReference = newPatient;
		parentObj.SetActive(true);
		ResetPatientInfo();
	}
	
	public void ResetPatientInfo() {	
		// Set all organs to missing by default
		for(int i = 0; i < organSprites.Length; i++) {
			organScripts[i].Set(null, patientReference);
			organSprites[i].sprite = null;
			organConditions[i].sprite = conditionSprites[2];
		}
		
		// Then check all organs we have and overwrite data where needed.
		foreach(Organ organ in patientReference.Organs) {
			if(organ == null)
				continue;
			
			organScripts[(int)organ.OrganType].Set(organ, patientReference);
			
			organTexts[(int)organ.OrganType].text = organ.OrganType.ToString().Substring(0, 1).ToUpper() + organ.OrganType.ToString().Substring(1);
			organSprites[(int)organ.OrganType].sprite = OrganHelper.GetOrganIcon(organ);
			organConditions[(int)organ.OrganType].sprite = organ.Healthy ? conditionSprites[0] : conditionSprites[1];
		}
		
		// Load patient image.
		patientSprite.sprite = patientReference?.GetComponent<SpriteRenderer>()?.sprite;
	}
	
	public bool IsPatientInfoOpen() => parentObj.activeSelf;
	
	public void ClosePatientInfo() {
		if(!IsPatientInfoOpen())
			return;
		
		parentObj.SetActive(false);
		
		foreach(OrganUI organScript in organScripts)
			organScript.Reset();
			
		patientReference.Invoke("UnpreventInteraction", 0);
	}
}