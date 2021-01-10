using System.Collections;
using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{

    private AudioSource LovesHang;
    
    
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;
    
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;

    public int maxAmmo;

    public float currentAmmo;

    float reloadSpeed = 3f;

    public Animator animator;

    public Text ammoDisplay;

    void Start()
    {
        LovesHang = GetComponent<AudioSource>();

        currentAmmo = maxAmmo;

    }

    // Update is called once per frame
    void Update()
    {
        ammoDisplay.text = currentAmmo.ToString();
        
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }


        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        {

            currentAmmo--;

            LovesHang.Play();
            
            nextTimeToFire = Time.time + 1f / fireRate;

            Shoot();

            muzzleFlash.Play();

        }


        if (Input.GetKey("r") == true)
        {
            StartCoroutine(Reload());
            return;
        }
    }
    
    void Shoot()
    {
        
        muzzleFlash.Play();
        
        currentAmmo --;

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }
    }

IEnumerator Reload()
    {   
        yield return new WaitForSeconds (reloadSpeed);

        currentAmmo = maxAmmo;
    }
    
    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}
