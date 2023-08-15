using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Patient : InteractableObject {
	public enum State {Alive, Harvestable, Dead}
	
    //variables
	private List<Organ> organs = new List<Organ>();
	
    [SerializeField] private Sprite[] patients;
    [SerializeField] private Sprite harvestablePatient, deadPatient;
    [SerializeField] private GameObject warningImage;
    private bool isStable;
    private float Blood {get; private set;} 
    private int age;
	public Patient.State currentState {get; private set; } = State.Alive;
	
	private static List<int> usedIndexes = new List<int>();

    private void Start() {
        SpriteSetup();
        AgeSetup();
        OrganSetup();
        BloodSetup();
    }
	
	private void SpriteSetup() {
		int sprite;
		while (usedIndexes.Contains(sprite = Random.Range(0, patients.Length)));
		usedIndexes.Add(sprite);
        GetComponent<SpriteRenderer>().sprite = patients[sprite];
    }

    private void AgeSetup() {
        age = Random.Range(1,101);
    }
	
	// TODO: Rewrite organ generation so that it consistently allows to save x patients.
    private void OrganSetup() {
		int organAmount = Enum.GetNames(Organ.Type).Length;
		
        int problems = Random.Range(1, 4);
		List<int> organIndices = Enumerable.Range(0, organAmount)
			.OrderBy(_ => Random.Range(0, organAmount)).Take(organAmount).ToList();

		// Setup some organs as good, others as problematic.
		for(int i = 0; i < Enum.GetNames(Organ.Type).Length)
			organs.Add(new Organ((Organ.Type)i, organIndices.Contains(i)));
    }

    private void BloodSetup() {
		Blood = (age < 60) ? 100 : (age < 80) ? 90 : 80;
    }

    private void Update() {
		if((currentState == State.Alive) && (organsBad.Count > 0 || organsMissing.Count > 0) && Blood < 50) { 
			float healthPercentage = Blood / 100f;
			if (healthPercentage < 0.1f)
				warningImage.SetActive(Time.time % 0.5f < 0.25f);
			else if (healthPercentage < 0.25f)
				warningImage.SetActive(Time.time % 1f < 0.5f);
			else
				warningImage.SetActive(Time.time % 2f < 1f);
		} 
        else
            warningImage.SetActive(false);
			
		if(currentState != State.Dead)
			LoseBlood((organsBad.Count * 0.16f + organsMissing.Count * 0.5f) * Time.deltaTime);
    }

    protected override void Interact() {
		if(!SurgeryUI.Instance.IsPatientInfoOpen())
			SurgeryUI.Instance.LoadPatient(this);
    }

	private void OnDisable() {
		usedIndexes = new List<int>();
	}

    // finds organ parameter in organsHealthy and moves it to organsMissing
    public void RemoveOrgan(string organ) {
		if(currentState == State.Dead)
			return;
		
        organsMissing.Add(organ);

		bool organHealthy;
		if(organsBad.Contains(organ))
		{
			organHealthy = false;
			organsBad.RemoveAt(organsBad.IndexOf(organ));
		}
		else
		{
			organHealthy = true;
			organsHealthy.RemoveAt(organsHealthy.IndexOf(organ));
		}
        PlayerManager.Instance.SetInv(organ, organHealthy);
    }

    // finds organ parameter in organsBad and moves it to Healthy
    public void ReplaceOrgan(string organ)
    {
		if(currentState != State.Alive)
			return;
		
		// Matt - added check whether inventory item is healthy.
		if(PlayerManager.Instance.GetHealthy()) 
		{
			organsHealthy.Add(organ);
		}
		else
		{
			organsBad.Add(organ);
		}
        organsMissing.RemoveAt(organsMissing.IndexOf(organ));
		
        PlayerManager.Instance.RemoveInv(organ);
    }

    // removes 10 blood and gives the player a blood pack
    public void DonateBlood()
    {
        if(Blood>20)
        {
            LoseBlood(10);
            PlayerManager.Instance.SetInv("bloodPack", true);
        } 
    }
	
	public void LoseBlood(float amount)
	{
		if(Blood > amount)
		{
			Blood -= amount;
		} 
        else
		{
			Blood -= Blood;
		}
		
		if(Blood == 0 && currentState == State.Alive) 
        {
			currentState = State.Harvestable;
			GetComponent<SpriteRenderer>().sprite = harvestablePatient;
			Invoke("Die", 30);
		}
	}
	
	public void Die() 
    {
		currentState = State.Dead;
		PreventInteraction(true);
		GetComponent<SpriteRenderer>().sprite = deadPatient;
	}
}
