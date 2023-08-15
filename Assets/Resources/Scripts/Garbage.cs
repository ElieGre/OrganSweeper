using UnityEngine;

public class Garbage : InteractableObject 
{
	
	private void Update() 
    {
		PreventInteraction(PlayerManager.Instance.GetInv() == "empty");
	}
	
	protected override void Interact() 
    {
		PlayerManager.Instance.SetInv("empty", false);
	}
}
