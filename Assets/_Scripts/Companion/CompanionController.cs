using SonicBloom.Koreo;
using System.Collections;
using UnityEngine;

public class CompanionController : MonoBehaviour

{
    public GameObject player;
    public float followSpeed = 0.1f;
    public Vector3 offset = new Vector3(-2f, 1f, 0f);  // offset of the follower relative to the player
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.3F;


    public string bassDrumEventID = "BassDrum";

    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public Transform bulletSpawnPoint;

    private Vector3 lastPosition;
    private bool isFacingRight = true;
    // private bool allowFlip = false;

    private IEnumerator Start()
    {
        //make sure familiar is facing right when loading in 
        yield return new WaitForSeconds(1f);
        lastPosition = transform.position;
        // allowFlip = true;

        //Call Koreographer to register koreography event id 
        Koreographer.Instance.RegisterForEvents(bassDrumEventID, OnbassDrumEventTriggered);
        lastPosition = transform.position;
    }

    void OnDisable()
    {
        if (Koreographer.Instance != null)
        {
            Koreographer.Instance.UnregisterForEvents(bassDrumEventID, OnbassDrumEventTriggered);
        }
    }

    private void OnbassDrumEventTriggered(KoreographyEvent koreographyEvent)
    {
        Shoot();
    }

    void Update()
    {
        //Follow player
        Vector3 targetPosition = player.transform.position + offset;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // Check for change in direction
        CheckForDirectionChange();

    }

    void CheckForDirectionChange()
    {
        Vector3 playerMovement = player.transform.position - lastPosition;
        if (playerMovement.x > 0 && !isFacingRight)
        {
            // Player has moved right and companion is facing left
            Flip();
        }
        else if (playerMovement.x < 0 && isFacingRight)
        {
            // Player has moved left and companion is facing right
            Flip();
        }
        lastPosition = player.transform.position;
    }

    void Flip()
    {
        // Rotate the companion by 90 degrees around the Y axis
        isFacingRight = !isFacingRight;
        float yRotation = isFacingRight ? 90 : 270;  // assuming 0 is right and 180 is left
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }


    public void Shoot()
    {
        //Shoot bullets method
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        float direction = isFacingRight ? 1 : -1;
        rb.velocity = new Vector2(direction * bulletSpeed, 0f);



    }
}