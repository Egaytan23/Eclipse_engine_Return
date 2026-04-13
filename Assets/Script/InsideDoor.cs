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
    private Quaternion closedRotation;
    private Quaternion openRotation;

    public Transform door;

    void Start()
    {
       closedRotation = door.rotation;
        openRotation = Quaternion.Euler(door.eulerAngles.x, door.eulerAngles.y + doorOpenAngle, door.eulerAngles.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isBusy)
        {
           StartCoroutine(OpenThenClosedoor());
        }

    }

   IEnumerator OpenThenClosedoor()
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
