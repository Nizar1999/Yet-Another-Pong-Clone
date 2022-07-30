using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] side representingSide; 
    private Text score;
    
    private void Awake()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
        GameManager.OnSideScored += OnSideScored;
        score = GetComponent<Text>();
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
            score.text = 0.ToString();
        }
    }

    private void OnSideScored(side scoringSide, int scoreVal, bool won)
    {
        if (representingSide == scoringSide)
        {
            score.text = scoreVal.ToString();
        }
    }
}