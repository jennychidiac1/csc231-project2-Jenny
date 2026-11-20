using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class SpellCaster : MonoBehaviour
{
    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private Transform spellPoint;
    [SerializeField] private float cooldown = 0.8f;
    [SerializeField] private float castDelay = 0.2f;

    private Animator animator;
    private PlayerController controller;
    private float nextCastTime;

    void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!controller.canMove || Time.time < nextCastTime) return;

        Keyboard kb = Keyboard.current;
        Mouse mouse = Mouse.current;
        bool pressed = (kb != null && kb.kKey.wasPressedThisFrame)
                    || (mouse != null && mouse.rightButton.wasPressedThisFrame);

        if (pressed)
        {
            nextCastTime = Time.time + cooldown;
            animator.SetTrigger("Special");
            StartCoroutine(Cast());
        }
    }

    IEnumerator Cast()
    {
        yield return new WaitForSeconds(castDelay);

        Vector3 offset = spellPoint.position - transform.position;
        offset.x = Mathf.Abs(offset.x) * controller.Facing;

        GameObject spell = Instantiate(
            spellPrefab, transform.position + offset, Quaternion.identity);
        spell.GetComponent<Spell>().Launch(controller.Facing);
    }
}