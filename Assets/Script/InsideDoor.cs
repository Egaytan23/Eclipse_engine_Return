using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideDoor : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip doorOpenSound;
    public AudioClip doorCloseSound;
    public bool doorOpen = false;
    public float doorOpenAngle = 90f;
    public float openTime = 3f;

    private bool isBusy = false;
    private Quaternion closedRotation; // Store the initial rotation of the door as the closed rotation
    private Quaternion openRotation; // Calculate the open rotation based on the closed rotation and the specified open angle

    public Transform door;

    void Start()
    {
       closedRotation = door.rotation; //   Store the initial rotation of the door as the closed rotation
        openRotation = Quaternion.Euler(door.eulerAngles.x, door.eulerAngles.y + doorOpenAngle, door.eulerAngles.z); // Calculate the open rotation based on the closed rotation and the specified open angle
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isBusy)
        {
           StartCoroutine(OpenThenClosedoor());
        }

    }

   IEnumerator OpenThenClosedoor() // Coroutine to handle opening the door, waiting for a specified time, and then closing it again while playing the appropriate sounds
    {
        isBusy = true;
        door.rotation = openRotation;
        audioSource.PlayOneShot(doorOpenSound);

        yield return new WaitForSeconds(openTime);
        

        door.rotation = closedRotation;
        audioSource.PlayOneShot(doorCloseSound);
        isBusy = false;
    }
    void Update()
    {
        
    }
}
