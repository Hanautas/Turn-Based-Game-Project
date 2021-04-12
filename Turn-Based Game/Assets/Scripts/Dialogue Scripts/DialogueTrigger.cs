using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public bool canInteract;

    public Dialogue dialogue;

    private GameObject player;
    private Collider2D interactCollider;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        interactCollider = GetComponent<Collider2D>();

        canInteract = false;
    }

    void Update()
    {
        if (canInteract == true && DialogueManager.isInteracting == false && PauseMenu.isPaused == false)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                TriggerDialogue();
                canInteract = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            canInteract = false;
        }
    }
}