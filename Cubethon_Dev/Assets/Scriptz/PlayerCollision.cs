using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovement movement; //reference to player movement script

    void OnCollisionEnter(Collision CollisionInfo) //function is called when we hit another object
                                                   //we get info about the colllision & call it "collisionInfo".
    {
        if(CollisionInfo.collider.tag == "Obstacle") //check if the object we collided with is called "Obstacle".
        {
            movement.enabled = false; //Disable the player's movement
            FindAnyObjectByType<GameManager>().EndGame();

        }
    }
}
