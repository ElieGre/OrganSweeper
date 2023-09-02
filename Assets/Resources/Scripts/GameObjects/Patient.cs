using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Patient : InteractableObject {
	public enum State {Alive, Harvestable, Dead}
	
	public List<Organ> Organs {get; protected set;} = new List<Organ>();
	
    [SerializeField] private GameObject warningImage;
    public float Blood {get; protected set;} 
	public Patient.State currentState {get; private set; } = State.Alive;
	
	private static List<int> usedSpriteIndexes = new List<int>();

    private void Start() {
		SetupSprite();
		SetupBlood();
		SetupOrgans();
    }
	
	protected virtual void SetupSprite() {
	    int sprite;
		while (usedSpriteIndexes.Contains(sprite = Random.Range(1, 18)));
		usedSpriteIndexes.Add(sprite);
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/People/Patients/Patient" + sprite);
	}
	
	protected virtual void SetupBlood() =>
		Blood = Random.Range(80f, 100f);
		
	protected virtual void SetupOrgans() {
		List<int> organIndices = Enumerable.Range(0, (int)Organ.Type.Length)
			.OrderBy(_ => Random.Range(0, (int)Organ.Type.Length)).Take((int)Organ.Type.Length).ToList();
        int problems = Random.Range(1, 4);
		organIndices.RemoveRange(problems, organIndices.Count - problems);
		
		for(int i = 0; i < (int)Organ.Type.Length; i++)
			Organs.Add(new Organ((Organ.Type)i, !organIndices.Contains(i)));
	}
	
    private void Update() {
		warningImage.SetActive(
			Organs.All(organ => organ.Healthy) && Organs.Count == (int)Organ.Type.Length ? false :
			currentState == State.Dead ? false :
			Blood < 10 ? Time.time % 0.5f < 0.25f :
			Blood < 25 ? Time.time % 1f < 0.5f :
			Blood < 50 ? Time.time % 2f < 1f :
			false);
			
		Bloodloss();
    }
	
	protected virtual void Bloodloss() {
		if(currentState != State.Dead)
			LoseHealth((Organs.Count(organ => !organ.Healthy) * 0.16f + 
				((int)Organ.Type.Length - Organs.Count) * 0.5f) * Time.deltaTime);
	}

    protected override void Interact() {
		if(!SurgeryUI.Instance.IsPatientInfoOpen())
			SurgeryUI.Instance.LoadPatient(this);
		
		PreventInteraction(true);
    }

	private void OnDisable() => usedSpriteIndexes = new List<int>();

    // Finds an organ by type and transfers it from inventory to patient.
    public void RemoveOrgan(Organ organ) {
		PlayerManager.Instance.SetOrganInHand(organ);
		Organs.Remove(organ);
    }

    // Transfers organ from patient to inventory.
    public void AddOrgan(Organ organ) {
		Organs.Add(organ);
		PlayerManager.Instance.RemoveOrganInHand();
    }
	
	public virtual void SkillcheckLoseHealth() => LoseHealth(30);
	
	public void LoseHealth(float amount) {
		Blood = Mathf.Max(0, Blood - amount);
		
		if(Blood == 0 && currentState == State.Alive) {
			currentState = State.Harvestable;
			GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/People/Patients/PatientHarvestable");
			HarvestableDelay();
		}
	}
	
	protected virtual void HarvestableDelay() => Invoke("Die", 30);
	
	private void Die() {
		currentState = State.Dead;
		GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/People/Patients/PatientDead");
		PreventInteraction(true);
	}
}