using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class OptionsTrigger : MonoBehaviour
{
    public bool canInteract;
    public GameObject canvasDialogue;

    private Animator animator;

    private GameObject optionsBox;
    private GameObject options;

    private GameObject buttonPrefab;

    public GameObject[] dialogueOption;
    public string[] buttonText;

    public UnityEvent buttonFunctions;

    void Start()
    {
        canInteract = false;

        animator = GetComponent<Animator>();

        optionsBox = canvasDialogue.transform.Find("Options Box").gameObject;
        options = canvasDialogue.transform.Find("Options Box/Options").gameObject;

        buttonPrefab = Resources.Load("DialogueButton") as GameObject;
    }

    public void ShowOptions()
    {
        animator.SetBool("IsOpen", true);
        DialogueManager.instance.blockPanel.SetActive(true);

        for (int i = 0; i < options.transform.childCount; i++)
        {
            Transform child = options.transform.GetChild(i);
            Destroy(child.gameObject);
        }

        for (int i = 0; i < dialogueOption.Length; i++)
        {
            int count = i;

            GameObject optionButton = Instantiate(buttonPrefab, transform.position, Quaternion.identity) as GameObject;

            optionButton.transform.SetParent(options.transform, false);
            optionButton.gameObject.transform.Find("Text").GetComponent<Text>().text = buttonText[i];

            optionButton.GetComponent<Button>().onClick.AddListener(() => OnClickTriggerDialogue(count));
            optionButton.GetComponent<Button>().onClick.AddListener(() => OnClickHideOptions());
        }
    }

    public void OnClickCallFunction()
    {
        buttonFunctions.Invoke();
    }

    public void OnClickHideOptions()
    {
        animator.SetBool("IsOpen", false);
        DialogueManager.instance.blockPanel.SetActive(false);
    }

    public void OnClickTriggerDialogue(int iCount)
    {
        dialogueOption[iCount].GetComponent<DialogueTrigger>().TriggerDialogue();

        FindObjectOfType<DialogueManager>().DisplayNextSentence();
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
