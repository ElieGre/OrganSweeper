using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : InteractableObject
{
	private enum State {Alive, Harvestable, Dead}
	
    //variables
    [SerializeField] private List<string> organs = new List<string> {"heart", "lungs", "kidney", "liver", "appendix", "pancreas", "intestines", "bladder"};
    [SerializeField] private List<string> organsHealthy = new List<string>();
    [SerializeField] private List<string> organsBad = new List<string>();
    [SerializeField] private List<string> organsMissing = new List<string>();
    [SerializeField] private Sprite[] patients;
    [SerializeField] private Sprite harvestablePatient;
    [SerializeField] private Sprite deadPatient;
    [SerializeField] private GameObject warningImage;
    private bool isStable;
    private float blood;
    private int age;
    private int sprite;
	private State currentState = State.Alive;
	
	private static List<int> usedIndexes = new List<int>();

    private void Start() {
        SpriteSetup();
        AgeSetup();
        OrganSetup();
        BloodSetup();
    }

    private void Update()
    {
		if((currentState == State.Alive) && (organsBad.Count > 0 || organsMissing.Count > 0) && blood < 50) 
        { 
			float healthPercentage = blood / 100f;
			if (healthPercentage < 0.1f)
            {
				warningImage.SetActive(Time.time % 0.5f < 0.25f);
            }
			else if (healthPercentage < 0.25f)
            {
				warningImage.SetActive(Time.time % 1f < 0.5f);
            }
			else
            {
				warningImage.SetActive(Time.time % 2f < 1f);
            }
		} 
        else
        {
            warningImage.SetActive(false);
        }
			
		if(currentState == State.Dead)
        {
			return;
        }
		LoseBlood((organsBad.Count * 0.16f + organsMissing.Count * 0.5f) * Time.deltaTime);
    }

    protected override void Interact() {
		
		// If we already have the patient info menu open, pressing E closes it.
		if(SurgeryUI.Instance.IsPatientInfoOpen()) {
			if(!SkillCheckRemoveOrgan.Instance.IsSkillCheckInProgress() && 
				!SkillCheckAddOrgan.Instance.IsSkillCheckInProgress()) {
				SurgeryUI.Instance.ClosePatientInfo();
			}
		} else {
			SurgeryUI.Instance.LoadPatient(this);
		}
    }

    private void SpriteSetup()
    {
		while(true) 
		{
	        sprite = Random.Range(0,patients.Length);
		  
		    // If we did not use that number, marked it as used and break out of loop.
		    if(!usedIndexes.Contains(sprite))
		    {
			    usedIndexes.Add(sprite);
			    break;
		    }
		}
        GetComponent<SpriteRenderer>().sprite = patients[sprite];
    }

    private void AgeSetup()
    {
        age = Random.Range(1,101);
    }

	private void OnDisable() 
    {
		usedIndexes = new List<int>();
	}
	
    //generates no. from 1-3, randomly assigns that amount of organs to organsBad and assigns remaining organs to organsHealthy 
    private void OrganSetup()
    {
        int problems = Random.Range(1,4);
        for(int x=0;x<problems;x++)
        {
            int randomOrgan = Random.Range(0, organs.Count);
            organsBad.Add(organs[randomOrgan]);
            organs.RemoveAt(randomOrgan);
        }
        organsHealthy = organs;
    }

    // gives patient 80-100 blood (health) based on age
    private void BloodSetup()
    {
        if(age < 60)
        {
            blood = 100;
        }
        else if(age < 80)
        {
            blood = 90;
        }         
        else
        {
            blood = 80;
        }

    }

    public float GetBlood()
    {
        return blood;
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
        if(blood>20)
        {
            LoseBlood(10);
            PlayerManager.Instance.SetInv("bloodPack", true);
        } 
    }
	
	public void LoseBlood(float amount)
	{
		if(blood > amount)
		{
			blood -= amount;
		} 
        else
		{
			blood -= blood;
		}
		
		if(blood == 0 && currentState == State.Alive) 
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

    public List<string> GetOrgansHealthy()
    {
        return organsHealthy;
    }

    public List<string> GetOrgansBad()
    {
        return organsBad;
    }

    public List<string> GetOrgansMissing()
    {
        return organsMissing;
    }
	
	public bool IsAlive() => currentState == State.Alive;
	public bool IsDead() => currentState == State.Dead;
	public bool IsHarvestable() => currentState == State.Harvestable;
}
