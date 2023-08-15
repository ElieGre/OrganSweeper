using UnityEngine;

public abstract class InteractableObject : MonoBehaviour {
    [SerializeField] private GameObject interactImage;
	private bool inRange, interactionPrevented;
	
	private void OnTriggerEnter2D() => ShowTooltip(true);
	private void OnTriggerStay2D() => ShowTooltip(true);
    private void OnTriggerExit2D() => ShowTooltip(false);
	
	private void ShowTooltip(bool value) {
		if(interactionPrevented && value)
			return;
		
        inRange = value;
        interactImage.SetActive(inRange);
	}
	
	protected void PreventInteraction() => PreventInteraction(true);
	protected void UnpreventInteraction() => PreventInteraction(false);
	
	protected void PreventInteraction(bool value) {
		if(value)
			ShowTooltip(false);
		
		interactionPrevented = value;
	}
	
	// We use LateUpdate() so that we don't have to override Update() in child classes.
	private void LateUpdate() {
        if(!inRange || interactionPrevented || !Input.GetKeyDown(KeyCode.E)) 
			return;
		
		Interact();
		ShowTooltip(false);
    }
	
    protected abstract void Interact();
}
