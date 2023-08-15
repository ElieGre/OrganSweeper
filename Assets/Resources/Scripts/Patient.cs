
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Patient : InteractableObject {
	public enum State {Alive, Harvestable, Dead}
	
	public List<Organ> Organs {get; private set;} = new List<Organ>();
	
    [SerializeField] private Sprite[] patients;
    [SerializeField] private Sprite harvestablePatient, deadPatient;
    [SerializeField] private GameObject warningImage;
    public float Blood {get; private set;} 
	public Patient.State currentState {get; private set; } = State.Alive;
	
	private static List<int> usedSpriteIndexes = new List<int>();

    private void Start() {
		// Setup sprite.
        int sprite;
		while (usedSpriteIndexes.Contains(sprite = Random.Range(0, patients.Length)));
		usedSpriteIndexes.Add(sprite);
        GetComponent<SpriteRenderer>().sprite = patients[sprite];
		
		// Setup age.
        int age = Random.Range(1, 100 + 1);
		
		// Setup blood amount.
		Blood = (age < 60) ? 100 : (age < 80) ? 90 : 80;
      
		// Setup organs. TODO: Rewrite organ generation so that it consistently allows to save x patients.
        int organAmount = System.Enum.GetNames(typeof(Organ.Type)).Length;
		
		List<int> organIndices = Enumerable.Range(0, organAmount)
			.OrderBy(_ => Random.Range(0, organAmount)).Take(organAmount).ToList();
        int problems = Random.Range(1, 4);
		organIndices.RemoveRange(problems, organIndices.Count - problems);
		
		for(int i = 0; i < organAmount; i++)
			Organs.Add(new Organ((Organ.Type)i, !organIndices.Contains(i)));
    }
	
    private void Update() {
		if((currentState == State.Alive) && (Organs.Any(organ => !organ.Healthy) || Organs.Count < System.Enum.GetNames(typeof(Organ.Type)).Length))
			warningImage.SetActive(
				Blood < 10 ? Time.time % 0.5f < 0.25f :
				Blood < 25 ? Time.time % 1f < 0.5f :
				Blood < 50 ? Time.time % 2f < 1f :
				false
			);
			
		int organAmount = System.Enum.GetNames(typeof(Organ.Type)).Length;
		if(currentState != State.Dead)
			LoseBlood((Organs.Count(organ => organ != null && !organ.Healthy) * 0.16f + 
				(organAmount - Organs.Count) * 0.5f) * Time.deltaTime);
    }

    protected override void Interact() {
		if(!SurgeryUI.Instance.IsPatientInfoOpen())
			SurgeryUI.Instance.LoadPatient(this);
		
		PreventInteraction(true);
    }

	private void OnDisable() => usedSpriteIndexes = new List<int>();

    // Finds an organ by type and transfers it from inventory to patient.
    public void RemoveOrgan(Organ organ) {
		if(currentState == State.Dead)
			return;
	
		PlayerManager.Instance.SetOrganInHand(organ);
		
		Organs.Remove(organ);
    }

    // Transfers organ from patient to inventory.
    public void AddOrgan(Organ organ) {
		if(currentState != State.Alive)
			return;
		
		Organs.Add(organ);
		PlayerManager.Instance.RemoveOrganInHand();
    }
	
	public void LoseBlood(float amount) {
		Blood = Mathf.Max(0, Blood - amount);
		
		if(Blood == 0 && currentState == State.Alive) {
			currentState = State.Harvestable;
			GetComponent<SpriteRenderer>().sprite = harvestablePatient;
			Invoke("Die", 30);
		}
	}
	
	public void Die() {
		currentState = State.Dead;
		GetComponent<SpriteRenderer>().sprite = deadPatient;
		PreventInteraction(true);
	}
}