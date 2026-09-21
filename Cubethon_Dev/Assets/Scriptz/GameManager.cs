using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;
    public float restartDelay = 2f;
    public GameObject completeLevelUI;
    [SerializeField] GameObject player;

    private static List<ReplaySnapshot> recordedFrames = new List<ReplaySnapshot>();
    private static bool shouldPlayReplay = false;
    private bool isReplaying = false;
    private int replayIndex = 0;
    
    private void OnEnable()
    {
        PlayerCollision.OnHitObstacle += EndGame;
        PlayerMovement.OnRecordFrame += RecordPlayerFrame;
    }

    private void OnDisable()
    {
        PlayerCollision.OnHitObstacle -= EndGame;
        PlayerMovement.OnRecordFrame -= RecordPlayerFrame;
    }

    void Start()
    {
        if(player != null)
        {
            PlayerMovement targetMovement = Object.FindFirstObjectByType<PlayerMovement>();
            if(targetMovement != null)
            {
                player = targetMovement.gameObject;
            }
        }
        if(shouldPlayReplay && recordedFrames.Count > 0)
        {
            StartReplay();
        }
    }

    private void FixedUpdate()
    {
        if(isReplaying)
        {
            RunReplay();
        }
    }

    void RecordPlayerFrame(ReplaySnapshot snapshot)
    {
        if(!isReplaying && !gameHasEnded)
        {
            recordedFrames.Add(snapshot);
        }
    }

    public void CompleteLevel()
    {
        completeLevelUI.SetActive(true);
        recordedFrames.Clear();
        shouldPlayReplay = false;

    }
    public void EndGame(Collision collisionInfo)
    {
        player.GetComponent<PlayerMovement>().enabled = false;

        if(collisionInfo != null)
        {
            Debug.Log("Hit: " + collisionInfo.collider.name);
        }

        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            shouldPlayReplay = true;
            Invoke("Restart", restartDelay); //delay executing Restart() for however long (seconds) restartDelay is set to 
        }
        
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //loads the name of whatever scene is active
    }

    void StartReplay()
    {
        isReplaying = true;
        replayIndex = 0;

        player.GetComponent<PlayerMovement>().enabled = false; //turn off player controls and physics during replay
        if(player.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }
    }

    void RunReplay()
    {
        if(replayIndex < recordedFrames.Count)
        {                                                           //read data frame by frame and force player to coordinates
            ReplaySnapshot snapshot = recordedFrames[replayIndex];
            player.transform.position = snapshot.position;
            player.transform.rotation = snapshot.rotation;
            replayIndex++;
        }
        else
        {
            isReplaying = false;
            shouldPlayReplay = false;
            recordedFrames.Clear();

            if(player.TryGetComponent<Rigidbody>(out Rigidbody rb)) //allow physics to work again
            {
                rb.isKinematic = false;
            }
            player.GetComponent<PlayerMovement>().enabled = true;
            gameHasEnded = false;
        }
    }
}

public struct ReplaySnapshot
{
    public Vector3 position;
    public Quaternion rotation;

    public ReplaySnapshot(Vector3 _position, Quaternion _rotation)
    {
        position = _position;
        rotation = _rotation;
    }
}
