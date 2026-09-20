using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;
    public float restartDelay = 2f;
    public GameObject completeLevelUI;
    [SerializeField] GameObject player;
    
    private void OnEnable()
    {
        PlayerCollision.OnHitObstacle += EndGame;
    }

    private void OnDisable()
    {
        PlayerCollision.OnHitObstacle -= EndGame;
    }
    
    public void CompleteLevel()
    {
        completeLevelUI.SetActive(true);
    }
    public void EndGame(Collision collisionInfo)
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        PlayerCollision.OnHitObstacle -= EndGame;

        if(collisionInfo != null)
        {
            Debug.Log("Hit: " + collisionInfo.collider.name);
        }

        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("Game Over!");
            Invoke("Restart", restartDelay); //delay executing Restart() for however long (seconds) restartDelay is set to 
        }
        
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //loads the name of whatever scene is active
    }
}
