using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    public GameObject projectilePrefab; //bullet or rocket prefab
    public int damage = 40;
    public int fireRate = 15;
    public int bulletSpeed = 5;

    public AudioClip shootSound;
    public float Shootvolume = 1f;

    public AudioClip reloadSound; //WHERE THE SOUND IS LOCATED AND PULLED FROM IN SHOOT SCRIPT
    public float reloadVolume = 1f;

    public Vector3 holdLocalPosition;
    public Vector3 holdlocalEuler;

    public int magSize = 30;
    public int reserveAmmo = 90;
    public float reloadTime = 2f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
