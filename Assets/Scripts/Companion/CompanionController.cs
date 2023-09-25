using System.Collections;
using UnityEngine;
using SonicBloom.Koreo;

public class CompanionController : MonoBehaviour

{
    public GameObject player;
    public float followSpeed = 0.1f;
    public Vector3 offset = new Vector3(-2f, 1f, 0f);  // offset of the follower relative to the player

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

        // Lerp towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

       
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