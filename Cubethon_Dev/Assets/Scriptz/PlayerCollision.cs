using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovement movement; //reference to player movement script

    public delegate void HitObstacle(Collision collisionInfo);
    public static event HitObstacle OnHitObstacle;

    void OnCollisionEnter(Collision collisionInfo) //function is called when we hit another object
                                                   //we get info about the colllision & call it "collisionInfo".
    {
        if(collisionInfo.collider.tag == "Obstacle") //check if the object we collided with is called "Obstacle".
        {
            if(OnHitObstacle != null)
            {
                OnHitObstacle(collisionInfo);
            }
            // movement.enabled = false; //Disable the player's movement
            // FindAnyObjectByType<GameManager>().EndGame();

        }
    }

    public static void TriggerHitObstacle(Collision collisionInfo)
    {
        OnHitObstacle?.Invoke(collisionInfo);
    }
}
