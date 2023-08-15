using UnityEngine;

public class Garbage : InteractableObject {
	
	private void Update() {
		PreventInteraction(PlayerManager.Instance.OrganInHand == null);
	}
	
	protected override void Interact() {
		PlayerManager.Instance.SetOrganInHand(null);
	}
}
