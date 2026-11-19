using UnityEngine;
using TMPro;

public class IngredientInventory : MonoBehaviour
{
    public static IngredientInventory Instance;

    [SerializeField] private TMP_Text counterText;

    private int[] counts;

    void Awake()
    {
        Instance = this;
        counts = new int[System.Enum.GetValues(typeof(IngredientType)).Length];
    }

    void Start()
    {
        UpdateText();
    }

    public void Add(IngredientType type)
    {
        counts[(int)type]++;
        UpdateText();
    }

    public int Get(IngredientType type)
    {
        return counts[(int)type];
    }

    public bool Remove(IngredientType type)
    {
        if (counts[(int)type] <= 0) return false;
        counts[(int)type]--;
        UpdateText();
        return true;
    }

    public int Total()
    {
        int total = 0;
        foreach (int c in counts) total += c;
        return total;
    }

    void UpdateText()
    {
        counterText.text = "Ingredients: " + Total();
    }
}