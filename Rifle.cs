using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Rifle : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoTotal;
    [SerializeField] private TMP_Text ammoGun;
    [SerializeField] private Animator arms;
    [SerializeField] private Animator rifle;
    public GameObject Hit;
    public GameObject Hit2;

    [SerializeField] public int curAmmo;
    [SerializeField] private int ammo;
    [SerializeField] private int maxAmmo;
    [SerializeField] private float reloadTime;
    [SerializeField] private bool isReloading;
    [SerializeField] private ParticleSystem PS;
    [SerializeField] private AudioSource AS;
    [SerializeField] private Transform firePoint;
    private float lastfired;
    [SerializeField] private float fireRate;
    // Start is called before the first frame update
    void Start()
    {
        ammo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 1)
        {
            ammoGun.text = ammo.ToString();
            ammoTotal.text = curAmmo.ToString();

            if (Input.GetButton("Fire2"))
            {
                Camera.main.fieldOfView = 50;
                rifle.SetBool("Aim", true);
            }
            else
            {
                rifle.SetBool("Aim", false);
                Camera.main.fieldOfView = 60;
            }

            if (Input.GetButtonDown("Reload") && !isReloading && ammo <= maxAmmo && curAmmo > 0)
            {
                StartCoroutine(Reload());
            }

            Debug.DrawRay(transform.parent.position, transform.TransformDirection(Vector3.forward), Color.green);
            if (Input.GetButton("Fire1") && !isReloading && ammo > 0)
            {
                AS.volume = 0.2f;
                if (Time.time - lastfired > 1 / fireRate)
                {
                    lastfired = Time.time;
                    Shoot();
                    AS.Play();
                    rifle.SetBool("Shooting", true);
                }
            }
            else
            {
                rifle.SetBool("Shooting", false);
            }
        }
    }

    private IEnumerator Reload()
    {
        AS.volume = 0.6f;
        rifle.SetBool("Reloading", true);
        arms.SetBool("Reloading", true);
        AS.clip = (AudioClip)Resources.Load("Eject");
        AS.Play();
        isReloading = true;
        yield return new WaitForSeconds(reloadTime - 0.3f);
        AS.clip = (AudioClip)Resources.Load("Inject");
        AS.Play();
        yield return new WaitForSeconds(0.3f);
        AS.clip = (AudioClip)Resources.Load("Shoot");

        if(curAmmo < maxAmmo - ammo)
        {
            ammo += curAmmo;
            curAmmo -= curAmmo;
        } 
        else
        {
            curAmmo -= maxAmmo - ammo;
            if (ammo < maxAmmo)
            {
                ammo = maxAmmo;
            }
            else
            {
                curAmmo -= 1;
                ammo = maxAmmo + 1;
            }
        }

        isReloading = false;
        arms.SetBool("Reloading", false);
        rifle.SetBool("Reloading", false);
        AS.volume = 0.2f;
    }
    private void OnEnable()
    {
        if (isReloading)
        {
            StartCoroutine(Reload());
        } else
        {
            arms.SetBool("Reloading", false);
            rifle.SetBool("Reloading", false);
        }
    }

    private void Shoot()
    {
        ammo--;
        RaycastHit hit;
        if(Physics.Raycast(transform.parent.position, transform.TransformDirection(Vector3.forward), out hit, 100))
        {
            if (hit.collider.name == "Head")
            {
                Instantiate(Hit2, hit.point, Quaternion.identity);
                hit.transform.GetComponent<Zombie>().health -= 3 * Random.Range(4f, 5f);
            } 
            if (hit.collider.name == "Body")
            {
                Instantiate(Hit2, hit.point, Quaternion.identity);
                hit.transform.GetComponent<Zombie>().health -= 4;
            }
            if (hit.collider.name == "Legs")
            {
                Instantiate(Hit2, hit.point, Quaternion.identity);
                hit.transform.GetComponent<Zombie>().health -= 2.5f;
            }
            if(hit.collider.name != "Head" && hit.collider.name != "Body" && hit.collider.name != "Legs")
            {
                {
                    Instantiate(Hit, hit.point, Quaternion.identity);
                }
            }


        }

        PS.Play();
        AS.Play();
    }
}
