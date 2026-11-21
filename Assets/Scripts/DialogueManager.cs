using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text bodyText;
    [SerializeField] private PlayerController player;

    private string speaker;
    private string[] lines;
    private int index;
    private int startFrame;
    private bool active;
    private Action onFinished;

    public bool IsActive => active;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show(string speakerName, string[] dialogueLines, Action finished = null)
    {
        if (active || dialogueLines == null || dialogueLines.Length == 0) return;

        speaker = speakerName;
        lines = dialogueLines;
        index = 0;
        onFinished = finished;
        active = true;
        startFrame = Time.frameCount;

        player.canMove = false;
        player.GetComponent<Animator>().SetFloat("Speed", 0f);

        panel.SetActive(true);
        ShowLine();
    }

    void Update()
    {
        if (!active || Time.frameCount == startFrame) return;

        Keyboard kb = Keyboard.current;
        if (kb != null && kb.spaceKey.wasPressedThisFrame)
        {
            index++;
            if (index >= lines.Length) End();
            else ShowLine();
        }
    }

    void ShowLine()
    {
        bodyText.text = "<b>" + speaker + "</b>\n" + lines[index]
            + "\n\n<size=60%><color=#aaaaaa>Press SPACE</color></size>";
    }

    void End()
    {
        active = false;
        panel.SetActive(false);
        StartCoroutine(Unfreeze(onFinished));
        onFinished = null;
    }

    IEnumerator Unfreeze(Action callback)
    {
        yield return null;
        player.canMove = true;
        if (callback != null) callback();
    }
}