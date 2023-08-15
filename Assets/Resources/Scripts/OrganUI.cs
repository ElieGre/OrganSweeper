using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class OrganUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	private bool mouseOver;
	
	private Organ organ;
	private Patient relatedPatient;
	
	void Update() {
		if(Input.GetKeyDown(KeyCode.Mouse0))
			ClickOnItem();
	}
	
    public void OnPointerEnter(PointerEventData eventData) => mouseOver = true;
    public void OnPointerExit(PointerEventData eventData) => mouseOver = false;
	
	private void ClickOnItem () {
		if(!mouseOver || relatedPatient.currentState == Patient.State.Dead || (organ == null) == (PlayerManager.Instance.OrganInHand == null))
			return;

		if(organ != null) {
			relatedPatient.RemoveOrgan(organ);	
			SkillCheckRemoveOrgan.Instance.StartSkillcheck(relatedPatient);
		} else if(relatedPatient.currentState == Patient.State.Alive) {
			relatedPatient.AddOrgan(PlayerManager.Instance.OrganInHand);
			SkillCheckAddOrgan.Instance.StartSkillcheck(relatedPatient);
		}
		
		SurgeryUI.Instance.ResetPatientInfo();
	}
	
	public void Set(Organ organ, Patient relatedPatient) {
		this.organ = organ;
		this.relatedPatient = relatedPatient;
		mouseOver = false;
	}
	
	public void Reset() => Set(null, null);
}
