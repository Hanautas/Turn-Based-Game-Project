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

    public string[] buttonText;

    public UnityEvent[] buttonFunctions;

    void Start()
    {
        canInteract = false;

        buttonPrefab = Resources.Load("DialogueButton") as GameObject;

        optionsBox = canvasDialogue.transform.Find("Options Box").gameObject;
        options = canvasDialogue.transform.Find("Options Box/Background/Options").gameObject;

        animator = optionsBox.GetComponent<Animator>();
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

        for (int i = 0; i < buttonFunctions.Length; i++)
        {
            int count = i;

            GameObject optionButton = Instantiate(buttonPrefab, transform.position, Quaternion.identity) as GameObject;

            optionButton.transform.SetParent(options.transform, false);
            optionButton.transform.Find("Text").GetComponent<Text>().text = buttonText[i];

            optionButton.GetComponent<Button>().onClick.AddListener(() => buttonFunctions[count].Invoke());
            optionButton.GetComponent<Button>().onClick.AddListener(() => OnClickHideOptions());
        }
    }

    public void OnClickHideOptions()
    {
        animator.SetBool("IsOpen", false);
        DialogueManager.instance.blockPanel.SetActive(false);
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
