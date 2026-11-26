using UnityEngine;
using TMPro;
using System.Collections;

public class HintTrigger : MonoBehaviour
{
    [SerializeField] private TMP_Text hintText;
    [TextArea]
    [SerializeField] private string message;
    [SerializeField] private float duration = 4f;

    private bool used;

    void Start()
    {
        hintText.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (used || other.GetComponentInParent<PlayerHealth>() == null) return;
        used = true;
        StartCoroutine(Show());
    }

    IEnumerator Show()
    {
        hintText.text = message;
        hintText.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        hintText.gameObject.SetActive(false);
    }
}