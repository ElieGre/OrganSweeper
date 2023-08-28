using UnityEngine;

public class Garbage : InteractableObject {
	private AudioManager am;
    
	private void Update() {
		PreventInteraction(PlayerManager.Instance.OrganInHand == null);
	}
	
	protected override void Interact() {
		am = FindObjectOfType<AudioManager>();
        am.Play("TrashInteract");
        PlayerManager.Instance.SetOrganInHand(null);
	}
}
