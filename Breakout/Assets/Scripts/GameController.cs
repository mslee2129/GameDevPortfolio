using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameController instance;
    [SerializeField] private float timeRemaining = 60.0f;
    private bool started = false;
    private int lives = 3;
    
    [SerializeField] private int rows = 4;
    [SerializeField] private int columns = 12;
    [SerializeField] private float brickSpacing = 0.3f;
    private int totalBricks;
    private int itemCount = 0;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI introText;
    private List<GameObject> bricks = new List<GameObject>();
    private float itemInterval = 5.0f;
    private float itemTimer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        GenerateBricks();
        gameOverText.gameObject.SetActive(false);
        winText.gameObject.SetActive(false);
        introText.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(started) {
            DisplayLives();
            DisplayTime();
            introText.gameObject.SetActive(false);
            Countdown();
            itemTimer += Time.deltaTime;
            if (itemTimer >= itemInterval && totalBricks > 0 && itemCount < totalBricks/2) {
                itemTimer = 0f;
                TurnItem();
            }
            if(totalBricks == 0 || lives <= 0 || timeRemaining <= 0f) {
                EndGame();
            }
            
        }
    }

    private void Countdown() {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            Debug.Log("Time has run out!");
            timeRemaining = 0;
            EndGame();
        }
    }

    public void StartGame() {
        started = true;
        timeRemaining = 60f;
    }

    public void LoseLife() {
        lives--;
    }

    public void AddLife() {
        lives++;
    }

    public void AddTime() {
        timeRemaining += 10.0f;
    }

    private void GenerateBricks() {
        float brickWidth = brickPrefab.GetComponent<Renderer>().bounds.size.x;
        float brickHeight = brickPrefab.GetComponent<Renderer>().bounds.size.y;

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                Vector3 brickPosition = new Vector3(
                    column * (brickWidth + brickSpacing) - 9f,
                    row * (brickHeight + brickSpacing) + 3,
                    0
                );
                GameObject newBrick = Instantiate(brickPrefab, brickPosition, Quaternion.identity);
                bricks.Add(newBrick);
                totalBricks++;
            }
        }
    }

    public void DestroyBrick(GameObject gameObject) {
        Brick brick = gameObject.GetComponent<Brick>();
        ItemType itemType = brick.GetItemType();
        Debug.Log("Itemtype: " + itemType);
        switch(itemType) {
            case (ItemType) 0:
                bricks.Remove(gameObject);
                totalBricks--;
                Debug.Log("Bricks left: " + totalBricks);
                break;
            case (ItemType) 1:
                Debug.Log("Life Added");
                AddLife();
                break;
            case (ItemType) 2:
                AddTime();
                Debug.Log("Time Added");
                break;
        }
        Destroy(gameObject);
    }

    private void TurnItem() {
        int randIndex = Random.Range(0, bricks.Count);
        GameObject randomBrick = bricks[randIndex];
        Brick brick = randomBrick.GetComponent<Brick>();
        brick.TurnIntoItem();
        totalBricks--;
        itemCount++;
        bricks.Remove(randomBrick);  // Remove the brick from the list after turning it into an item
        Debug.Log("Bricks left: " + totalBricks);
    }

    private void DisplayTime() {
        float seconds = Mathf.FloorToInt(timeRemaining);
        countdownText.text = string.Format("{0}: {1:000}", "Timer", seconds);
    }

    private void DisplayLives() {
        livesText.text = string.Format("{0}: {1:0}", "Lives", lives);
    }

    public void EndGame()
    {
        started = false;
        if(totalBricks == 0 && timeRemaining > 0f) { // win condition
            winText.gameObject.SetActive(true);
            Debug.Log("You Win!");
        } 
        if (lives <= 0 || timeRemaining <= 0f) { // lose condition
            gameOverText.gameObject.SetActive(true);
            Debug.Log("Game Over!");
        }
        Time.timeScale = 0f;
    }

    // getters & setters
    public int GetLife() {
        return lives;
    }

    public bool GetStarted() {
        return started;
    }
}
