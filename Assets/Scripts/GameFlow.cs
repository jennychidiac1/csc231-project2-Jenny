using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    public static GameFlow Instance;

    [SerializeField] private Customer[] customers;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private PlayerController player;

    private int served;
    private int angry;

    public bool Ended { get; private set; }

    void Awake()
    {
        Instance = this;
        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public Customer CurrentCustomer()
    {
        foreach (Customer c in customers)
        {
            if (!c.IsServed) return c;
        }
        return null;
    }

    public void ReportResult(bool correct)
    {
        served++;
        if (!correct) angry++;
    }

    public void CheckEnd()
    {
        if (angry >= 2)
        {
            AudioManager.Instance.PlayGameOver();
            Ended = true;
            player.canMove = false;
            gameOverPanel.SetActive(true);
        }
        else if (served >= customers.Length)
        {
            AudioManager.Instance.PlayWin();
            Ended = true;
            player.canMove = false;
            winPanel.SetActive(true);
        }
    }

    public void Restart()
    {
        MenuManager.skipTitle = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}