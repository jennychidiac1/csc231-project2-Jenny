using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;

public class Cauldron : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text orderText;
    [SerializeField] private TMP_Text selectionText;
    [SerializeField] private TMP_Text[] countLabels;
    [SerializeField] private GameObject promptText;
    [SerializeField] private PlayerController player;
    [SerializeField] private int maxIngredients = 2;

    private bool playerNear;
    private bool open;
    private List<IngredientType> selected = new List<IngredientType>();

    void Awake()
    {
        panel.SetActive(false);
        if (promptText != null) promptText.SetActive(false);
    }

    void Update()
    {
        Keyboard kb = Keyboard.current;
        if (kb == null || !kb.eKey.wasPressedThisFrame) return;

        if (open)
        {
            Close();
        }
        else if (playerNear
                 && !DialogueManager.Instance.IsActive
                 && GameFlow.Instance.CurrentCustomer() != null)
        {
            Open();
        }
    }

    void Open()
    {
        open = true;
        selected.Clear();
        player.canMove = false;
        player.GetComponent<Animator>().SetFloat("Speed", 0f);
        panel.SetActive(true);
        Refresh();
    }

    public void Close()
    {
        open = false;
        panel.SetActive(false);
        player.canMove = true;
    }

    public void AddIngredient(int index)
    {
        IngredientType type = (IngredientType)index;
        if (selected.Count >= maxIngredients) return;
        if (IngredientInventory.Instance.Get(type) - CountSelected(type) <= 0) return;
        selected.Add(type);
        Refresh();
    }

    public void Clear()
    {
        selected.Clear();
        Refresh();
    }

    public void Brew()
    {
        Customer c = GameFlow.Instance.CurrentCustomer();
        if (c == null || selected.Count == 0) return;

        if (!c.HasOrdered)
        {
            orderText.text = "Hear " + c.CustomerName + "'s order first!";
            return;
        }

        bool correct = Matches(c.Recipe);
        AudioManager.Instance.PlayBrew(correct);

        foreach (IngredientType t in selected)
        {
            IngredientInventory.Instance.Remove(t);
        }
        selected.Clear();

        Close();
        GameFlow.Instance.ReportResult(correct);
        c.React(correct, GameFlow.Instance.CheckEnd);
    }

    bool Matches(IngredientType[] recipe)
    {
        if (recipe.Length != selected.Count) return false;
        foreach (IngredientType t in recipe)
        {
            if (CountIn(recipe, t) != CountSelected(t)) return false;
        }
        return true;
    }

    int CountSelected(IngredientType type)
    {
        int n = 0;
        foreach (IngredientType s in selected)
        {
            if (s == type) n++;
        }
        return n;
    }

    int CountIn(IngredientType[] list, IngredientType type)
    {
        int n = 0;
        foreach (IngredientType t in list)
        {
            if (t == type) n++;
        }
        return n;
    }

    void Refresh()
    {
        Customer c = GameFlow.Instance.CurrentCustomer();
        orderText.text = c == null ? "" : c.CustomerName + " wants: " + c.PotionName;

        for (int i = 0; i < countLabels.Length; i++)
        {
            IngredientType type = (IngredientType)i;
            int left = IngredientInventory.Instance.Get(type) - CountSelected(type);
            countLabels[i].text = type + " x" + left;
        }

        selectionText.text = selected.Count == 0
            ? "Cauldron: empty"
            : "Cauldron: " + string.Join(", ", selected);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponentInParent<PlayerHealth>() == null) return;
        playerNear = true;
        if (promptText != null) promptText.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponentInParent<PlayerHealth>() == null) return;
        playerNear = false;
        if (promptText != null) promptText.SetActive(false);
    }
}