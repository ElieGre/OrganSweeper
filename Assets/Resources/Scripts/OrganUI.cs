using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class OrganUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
	private bool mouseOver;
	private bool organMissing;
    private string organName = "";
	
	private Patient relatedPatient;
	private SurgeryUI surgeryUI;
	private SkillCheckRemoveOrgan skillcheck;
	private LetterPopUp skillcheckDDR;
	private PlayerManager playerManager;
	
	void Start() 
	{
		surgeryUI = GameObject.FindWithTag("GameController").GetComponent<SurgeryUI>();
		skillcheck = GameObject.FindWithTag("GameController").GetComponent<SkillCheckRemoveOrgan>();
		skillcheckDDR = GameObject.FindWithTag("GameController").GetComponent<LetterPopUp>();
		playerManager = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
	}
	
	void Update() 
	{
		if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			ClickOnItem();
		}
	}
	
    public void OnPointerEnter(PointerEventData eventData) 
	{
		mouseOver = true;
    }
	
    public void OnPointerExit(PointerEventData eventData) 
	{
		mouseOver = false;
    }
	
	void ClickOnItem () {
		if(!mouseOver)
		{
			return;
		}
		
		if(organName == "")
		{
			return;
		}
		
		if(relatedPatient.IsDead())
		{
			return;
		}

		if(organMissing && playerManager.GetInv() != organName)
		{
			return;
		}

		if((playerManager.GetInv() == "empty" && organMissing) || (playerManager.GetInv() != "empty" && !organMissing))
		{
			return;
		}
			
		if(!organMissing) 
		{
			relatedPatient.RemoveOrgan(organName);
			skillcheck.StartSkillcheck(relatedPatient);
		} 
		else if(relatedPatient.IsAlive()) 
		{
			relatedPatient.ReplaceOrgan(organName);	
			skillcheckDDR.StartSkillcheck(relatedPatient);
		}
		
		surgeryUI.ResetPatientInfo();
	}
	
	public void Set(string organName, bool organMissing, Patient relatedPatient) 
	{
		this.organName = organName;
		this.organMissing = organMissing;
		this.relatedPatient = relatedPatient;
		GameObject.Find("PatientSprite").GetComponent<Image>().sprite = relatedPatient.GetComponent<SpriteRenderer>().sprite;
	}
	
	public void Reset() 
	{
		mouseOver = false;
		organName = "";
		relatedPatient = null;
		organMissing = true;
	}
}
