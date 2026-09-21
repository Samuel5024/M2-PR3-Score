using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb; //reference to the Player's rigidbody coomponent
    public float forwardForce = 2000f;
    public float sidewaysForce = 500f;
    public delegate void RecordFrameHandler(ReplaySnapshot snapshot);
    public static event RecordFrameHandler OnRecordFrame;

void FixedUpdate() //use fixed update for physics stuff
    {
        if(OnRecordFrame != null)  //broadcast position/rotation data to observers like the GameManager
        {
            ReplaySnapshot currentFrame = new ReplaySnapshot(transform.position, transform.rotation);
            OnRecordFrame(currentFrame);
        }
        rb.AddForce(0, 0, forwardForce * Time.deltaTime); //Time.deltaTime is the amount of time since computer drew the last frame

        if(Input.GetKey("d")) //if the player is pressing the "D" key
        {
            rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange); //add a force to the right
        }

        if(Input.GetKey("a")) //if the player is pressing the "A" key 
        {
            rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange); //add a force to the left (inverse (-) sidewaysForce)
        }

        if(rb.position.y < -1f)
        {
            PlayerCollision.TriggerHitObstacle(null);
            //FindObjectOfType<GameManager>().EndGame(null);
        }
    }
}
