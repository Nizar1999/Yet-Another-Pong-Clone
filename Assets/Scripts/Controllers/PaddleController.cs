using UnityEngine;

public class PaddleController : MonoBehaviour
{
    [SerializeField] float paddleSpeed = 5.0f;
    public bool isAI = false;
    public side representingSide;

    private void Awake()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void FixedUpdate()
    {
        if(isAI)
        {
            UpdateAIPos(GameManager.instance.ball.transform.position.y);
        } else
        {
            UpdatePlayerPos();
        }  
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float x = isAI? -1 : 1;
        float y = (collision.transform.position.y - transform.position.y);

        Vector2 newDir = new Vector2(x,y);
        collision.collider.GetComponent<BallController>().ChangeDirection(newDir);
    }

    private void UpdateAIPos(float y)
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, Mathf.Clamp(y, -2.15f, 2.15f)), paddleSpeed * Time.deltaTime);
    }

    private void UpdatePlayerPos()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 lPaddlePos = transform.position;
        Vector3 targetPos = new Vector3(lPaddlePos.x, Camera.main.ScreenToWorldPoint(mousePos).y, 0);
        Vector3 newPaddlePos = Vector3.MoveTowards(lPaddlePos, targetPos, paddleSpeed * Time.deltaTime);
        transform.position = new Vector3(newPaddlePos.x, Mathf.Clamp(newPaddlePos.y, -2.15f, 2.15f), newPaddlePos.z);
    }

    private void OnGameStateChanged(GameState state)
    {
        if(state == GameState.Play)
        {
            gameObject.SetActive(true);
        } else
        {
            gameObject.SetActive(false);
        }
    }
}
