using UnityEngine;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    [SerializeField] side representingSide;
    private Text win;

    private void Awake()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
        GameManager.OnSideScored += OnSideScored;
        win = GetComponent<Text>();
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
        GameManager.OnSideScored -= OnSideScored;
    }

    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.Play)
        {
            win.gameObject.SetActive(false);
        }
    }

    private void OnSideScored(side scoringSide, int scoreVal, bool won)
    {
        if (!won)
            return;

        if (representingSide == scoringSide)
        {
            win.text = "Win";
            win.gameObject.SetActive(true);
        }
        else
        {
            win.text = "Lose";
        }
    }
}
