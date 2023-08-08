using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garbage : MonoBehaviour {
    [SerializeField] private GameObject interactImage;
	private PlayerManager playerManager;
    private bool inRange = false;
	
	private void Start() {
		playerManager = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
	}

    private void Update() {
        if(inRange && Input.GetKeyDown(KeyCode.E)) {
			playerManager.SetInv("empty", false);
			interactImage.SetActive(false);
			inRange = false;
        }
    }
	
    private void OnTriggerEnter2D(Collider2D collision) {
		if(playerManager.GetInv() == "empty")
			return;
		
        interactImage.SetActive(true);
        inRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        interactImage.SetActive(false);
        inRange = false;
    }
}
