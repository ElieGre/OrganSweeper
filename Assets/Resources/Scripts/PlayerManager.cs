using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager> {
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer organSprite;
	
    public Organ OrganInHand {get; private set;}
    private Vector2 savedDirection;
	

    public void SetOrganInHand(Organ organ) {
        OrganInHand = organ;
        organSprite.sprite = OrganHelper.GetOrganIcon(organ);
    }
	
    public void RemoveOrganInHand() {
        OrganInHand = null;
		organSprite.sprite = null;
    }

    private void FixedUpdate() {
        Vector2 currentDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 movementVector = currentDirection.normalized;

        if(currentDirection.x != 0 || currentDirection.y != 0)
			savedDirection = new Vector2(currentDirection.x, currentDirection.y);
		
		animator.SetFloat("Horizontal", savedDirection.x);
		animator.SetFloat("Vertical", savedDirection.y);

		if(SurgeryUI.Instance.IsPatientInfoOpen())
			movementVector = Vector2.zero;
		
		animator.speed = (movementVector == Vector2.zero) ? 0 : 1;
		
		rb.velocity = movementVector * 5;
    }
}
