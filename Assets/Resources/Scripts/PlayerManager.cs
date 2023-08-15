using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager> {
    public Organ organInHand {get; private set;}
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 savedDirection;
	
	// Matt - replaced gameobject instantiation with sprite swapping. 
    [SerializeField] private SpriteRenderer organSprite;
	// Matt - added a way to store whether an organ we're carrying right now is good or bad.
	private bool organHealthy;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // puts organ in the players hand (inventory)
    public void SetOrganInHand(string organ, bool healthy)
    {
        inv = organ;
		organHealthy = healthy;
		
        organSprite.sprite = OrganHelper.GetOrganIconByName(organ, healthy);
    } 
	
	public bool GetHealthy()
	{
		return organHealthy;
	}

    public void RemoveOrganInHand() {
        organInHand
		organSprite.sprite = null;
    }

    private void FixedUpdate()
    {
        Vector2 currentDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 movementVector = currentDirection.normalized;

        if(currentDirection.x != 0 || currentDirection.y != 0)
		{
			savedDirection = new Vector2(currentDirection.x, currentDirection.y);
		}
		animator.SetFloat("Horizontal", savedDirection.x);
		animator.SetFloat("Vertical", savedDirection.y);

		// Matt - added this so that you can't move if you got patient info open. Change it if you want as long as necessary functions are called!
		// Find the UI script on tag by finding a gameobject with tag "GameController" and accessing its SurgeryUI script.
		SurgeryUI uiScript = GameObject.FindWithTag("GameController").GetComponent<SurgeryUI>();
		// Replace movement vector with zero if you are viewing patient info right now.
		if(uiScript.IsPatientInfoOpen())
        {
			movementVector = Vector2.zero;
        }
		
		if(movementVector == Vector2.zero)
        {
			animator.speed = 0;
        }
		else
        {
			animator.speed = 1;
        }
		
        rb.velocity = movementVector * 5;
    }
}
