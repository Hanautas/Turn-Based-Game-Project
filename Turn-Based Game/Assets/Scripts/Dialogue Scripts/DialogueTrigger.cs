using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public bool canInteract;

    public Dialogue dialogue;

    void Start()
    {
        canInteract = false;
    }

    void Update()
    {
        if (canInteract && !DialogueManager.isInteracting && !PauseMenu.isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
                FindObjectOfType<DialogueManager>().DisplayNextSentence();
                canInteract = false;
            }
        }
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
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