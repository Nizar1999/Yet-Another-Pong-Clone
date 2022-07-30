using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState state;
    public static event System.Action<GameState> OnGameStateChanged;
    public static event System.Action<side, int, bool> OnSideScored;

    [SerializeField] GameObject wall;
    [SerializeField] GameObject paddle;
    public GameObject ball;

    [SerializeField] const int scoreToWin = 5;
    private int rightScore = 0;
    private int leftScore = 0;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        Cursor.visible = false;
        ball = Instantiate(ball);

        PaddleController rPaddle = Instantiate(paddle, new Vector2(4.75f, 0), Quaternion.identity).GetComponent<PaddleController>();
        PaddleController lPaddle = Instantiate(paddle, new Vector2(-4.75f, 0), Quaternion.identity).GetComponent<PaddleController>();

        rPaddle.isAI = true;

        GameObject topWall = Instantiate(wall, new Vector2(0, -3.3f), Quaternion.identity);
        GameObject bottomWall = Instantiate(wall, new Vector2(0, 3.3f), Quaternion.identity);
        topWall.transform.localScale = new Vector2(13, 1);
        bottomWall.transform.localScale = new Vector2(13, 1);

        UpdateGameState(GameState.Play);
    }

    private void Update()
    {
        HandleInput();
        CheckBallStatus();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("Main Menu");
    }

    private void CheckBallStatus()
    {
        Vector3 ballPos = Camera.main.WorldToViewportPoint(ball.transform.position);

        if (ballPos.x < 0.0)
        {
            rightScore++;
            HandleScoring(side.right, rightScore);
        }
        if (ballPos.x > 1.0)
        {
            leftScore++;
            HandleScoring(side.left, leftScore);
        }
    }

    private IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(3f);
        rightScore = 0;
        leftScore = 0;
        UpdateGameState(GameState.Play);
    }

    public void UpdateGameState(GameState newState)
    {
        state = newState;
        switch (newState)
        {
            case GameState.Play:
                break;
            case GameState.Reset:
                StartCoroutine(ResetGame());
                break;
            default:
                break;
        }
        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleScoring(side scoringSide, int score)
    {
        Debug.Log(state);
        ball.GetComponent<BallController>().InitBall();
        if (score == scoreToWin)
        {
            OnSideScored?.Invoke(scoringSide, score, true);
            UpdateGameState(GameState.Reset);
            return;
        }
        
        OnSideScored?.Invoke(scoringSide, score, false);
    }
}
public enum GameState
{
    Play,
    Reset
}

public enum side
{
    right,
    left
}