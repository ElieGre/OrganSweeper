using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCheckRemoveOrgan : Singleton<SkillCheckRemoveOrgan>  {
	[SerializeField] private GameObject skillcheckParent;
	[SerializeField] private RectTransform skillcheckArrow;
	[SerializeField] private RectTransform skillcheckSuccessArea;
	
	private bool skillcheckEnabled;
	private float skillcheckSpeed;
	private bool rotating;
	private float z, successZ;
	private Patient operatingPatient;
	

	void Start() =>
		skillcheckParent.SetActive(false);
	
    void Update() {
		if(!skillcheckEnabled)
			return;
		
		if(rotating) {
			z += skillcheckSpeed * Time.deltaTime;
			skillcheckArrow.eulerAngles = new Vector3(0, 0, z);
		}
		
		if(Input.GetKeyDown(KeyCode.Space) && rotating) {
			rotating = false;
			
			if(Mathf.Abs(Mathf.DeltaAngle(z, successZ)) > 20)
				operatingPatient.SkillcheckLoseHealth();
			
			SurgeryUI.Instance.ResetPatientInfo();
			Invoke("DisableSkillcheck", 0.5f);
		}
    }

	void EnableSkillcheck() {
		z = Random.Range(0, 360f);
		successZ = Random.Range(0, 360f);
        skillcheckSpeed = Random.Range(300, 500);
		
		skillcheckParent.SetActive(true);
		skillcheckEnabled = true;
		rotating = true;
		skillcheckSuccessArea.eulerAngles = new Vector3(0, 0, successZ);
	}
	
	void DisableSkillcheck() {
		skillcheckParent.SetActive(false);
		skillcheckEnabled = false;
	}
	
	public void StartSkillcheck(Patient operatingPatient) {
		this.operatingPatient = operatingPatient;
		EnableSkillcheck();
	}
	
	public bool IsSkillCheckInProgress() => skillcheckEnabled;
}
