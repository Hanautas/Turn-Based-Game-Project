using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public Text nameText;
    public Text dialogueText;

    public RectTransform transformSprite;
    public Image characterSprite;

    public GameObject blockPanel;

    public Animator animator;

    private Queue<string> names;
    private Queue<string> sentences;
    private Queue<Color> textColors;
    private Queue<Font> textFonts;
    private Queue<float> delays;
    private Queue<Sprite> sprites;
    private Queue<AudioClip> sounds;

    public static bool isInteracting;

    private GameObject optionsObject;

    void Start()
    {
        instance = this;

        names = new Queue<string>();
        sentences = new Queue<string>();
        textColors = new Queue<Color>();
        textFonts = new Queue<Font>();
        delays = new Queue<float>();
        sprites = new Queue<Sprite>();
        sounds = new Queue<AudioClip>();

        isInteracting = false;
    }

    void Update()
    {
        if (isInteracting && !PauseMenu.isPaused)
        {
            if (Input.GetMouseButtonDown(0))
            {
                DisplayNextSentence();
            }
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        characterSprite.enabled = true;
        isInteracting = true;
        blockPanel.SetActive(true);

        names.Clear();
        sentences.Clear();
        textColors.Clear();
        textFonts.Clear();
        delays.Clear();
        sprites.Clear();
        sounds.Clear();
        
        foreach (string name in dialogue.names)
        {
            names.Enqueue(name);
        }

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        foreach (Color color in dialogue.textColors)
        {
            textColors.Enqueue(color);
        }

        foreach (Font font in dialogue.textFonts)
        {
            textFonts.Enqueue(font);
        }

        foreach (float delay in dialogue.delays)
        {
            delays.Enqueue(delay);
        }

        foreach (Sprite sprite in dialogue.sprites)
        {
            sprites.Enqueue(sprite);
        }

        foreach (AudioClip sound in dialogue.sounds)
        {
            sounds.Enqueue(sound);
        }

        //DisplayNextSentence();

        optionsObject = dialogue.optionsObject;
    }

    IEnumerator TypeSentence(string sentence, string name, Color color, Font font, float delay, Sprite sprite, AudioClip sound)
    {
        nameText.text = "";
        dialogueText.text = "";

        foreach (char letter in name.ToCharArray())
        {
            nameText.text += letter;
            yield return null;
        }

        /*
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
        */
        
        for (int i = 0; i < sentence.Length; i++)
        {
            dialogueText.text += sentence[i];
            SoundManager.instance.PlaySound(sound);
            yield return new WaitForSeconds(delay);
        }
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string name = names.Dequeue();

        string sentence = sentences.Dequeue();

        Color color = textColors.Dequeue();
        nameText.color = color;
        dialogueText.color = color;

        Font font = textFonts.Dequeue();
        nameText.font = font;
        dialogueText.font = font;

        float delay = delays.Dequeue();

        Sprite sprite = sprites.Dequeue();
        characterSprite.sprite = sprite;
        characterSprite.SetNativeSize();

        Vector3 spritePosition = new Vector3(0, transformSprite.rect.height / 2, 0);
        transformSprite.anchoredPosition = Vector3.zero + spritePosition;

        AudioClip sound = sounds.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence, name, color, font, delay, sprite, sound));
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        characterSprite.enabled = false;
        isInteracting = false;
        blockPanel.SetActive(false);

        StopAllCoroutines();

        if (optionsObject != null)
        {
            optionsObject.GetComponent<OptionsTrigger>().ShowOptions();
        }
    }
}