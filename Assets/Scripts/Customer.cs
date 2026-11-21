using UnityEngine;

public class Customer : MonoBehaviour
{
    [SerializeField] private string customerName = "Customer";
    [SerializeField] private string potionName = "Potion";
    [SerializeField] private IngredientType[] recipe;

    [TextArea(2, 4)]
    [SerializeField] private string[] orderLines;
    [TextArea(2, 4)]
    [SerializeField] private string[] happyLines;
    [TextArea(2, 4)]
    [SerializeField] private string[] angryLines;

    public string CustomerName => customerName;
    public string PotionName => potionName;
    public IngredientType[] Recipe => recipe;
    public bool HasOrdered { get; private set; }
    public bool IsServed { get; private set; }

    void Start()
    {
        Debug.Log("Customer script running on: " + name);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Customer trigger touched by: " + other.name);
        if (DialogueManager.Instance == null)
        {
            Debug.LogError("No DialogueManager in the scene");
            return;
        }

        if (HasOrdered) return;
        if (other.GetComponentInParent<PlayerHealth>() == null) return;
        if (DialogueManager.Instance.IsActive) return;

        HasOrdered = true;
        DialogueManager.Instance.Show(customerName, orderLines);
    }

    public void React(bool correctPotion, System.Action finished = null)
    {
        IsServed = true;
        DialogueManager.Instance.Show(
            customerName,
            correctPotion ? happyLines : angryLines,
            finished);
    }
}