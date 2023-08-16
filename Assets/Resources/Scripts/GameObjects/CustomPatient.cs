using UnityEngine;
using System.Linq;

public class CustomPatient : Patient {
	private enum OrganState {Healthy, Unhealthy, Missing}
	
	[SerializeField] private Sprite patientSprite;

	[Header("Health Properties")]
	[Range(0, 100f)]
	[SerializeField] private float startBloodAmount = 100f;
	[Range(0, 5f)]
	[SerializeField] private float bloodlossPerMissingOrgan = 0.5f,
								   bloodlossPerUnhealthyOrgan = 0.16f;
	[Range(0, 100)]
	[SerializeField] private int skillcheckMissBloodPenalty = 30;
	[Range(0, 120)]
	[SerializeField] private float deathHarvestableDelay = 30;
				  
	[Header("Organ Properties")]
	[SerializeField] private OrganState heartState;
	[SerializeField] private OrganState lungsState;
	[SerializeField] private OrganState kidneyState;
	[SerializeField] private OrganState liverState;
	[SerializeField] private OrganState appendixState;
	[SerializeField] private OrganState pancreasState;
	[SerializeField] private OrganState intestinesState;
	[SerializeField] private OrganState bladderState;
	
	protected override void SetupSprite() =>
		GetComponent<SpriteRenderer>().sprite = patientSprite;
	
	protected override void SetupBlood() =>
		Blood = startBloodAmount;
		
	protected override void SetupOrgans() {
		SetupOrganByState(Organ.Type.Heart, heartState);
		SetupOrganByState(Organ.Type.Lungs, lungsState);
		SetupOrganByState(Organ.Type.Kidney, kidneyState);
		SetupOrganByState(Organ.Type.Liver, liverState);
		SetupOrganByState(Organ.Type.Appendix, appendixState);
		SetupOrganByState(Organ.Type.Pancreas, pancreasState);
		SetupOrganByState(Organ.Type.Intestines, intestinesState);
		SetupOrganByState(Organ.Type.Bladder, bladderState);
	}
	
	private void SetupOrganByState(Organ.Type type, OrganState state) {
		if(state == OrganState.Missing)
			return;
		
		Organs.Add(new Organ(type, state == OrganState.Healthy));
	}
	
	protected override void Bloodloss() {
		if(currentState != State.Dead)
			LoseHealth((Organs.Count(organ => !organ.Healthy) * bloodlossPerUnhealthyOrgan + 
				((int)Organ.Type.Length - Organs.Count) * bloodlossPerMissingOrgan) * Time.deltaTime);
	}
	
	public override void SkillcheckLoseHealth() => LoseHealth(skillcheckMissBloodPenalty);
	
	protected override void HarvestableDelay() => Invoke("Die", deathHarvestableDelay);
}
