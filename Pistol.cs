using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pistol : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoTotal;
    [SerializeField] private TMP_Text ammoGun;
    [SerializeField] private Animator arms;
    [SerializeField] private Animator pistola;
    public GameObject Hit;
    public GameObject Hit2;

    [SerializeField] private AudioClip inject;
    [SerializeField] private AudioClip eject;
    [SerializeField] private AudioClip shoot;

    [SerializeField] private int curAmmo;
    [SerializeField] private int ammo;
    [SerializeField] private int maxAmmo;
    [SerializeField] private float reloadTime;
    [SerializeField] private bool isReloading;
    [SerializeField] private ParticleSystem PS;
    [SerializeField] private AudioSource AS;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate;
    // Start is called before the first frame update
    void Start()
    {
        ammo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1)
        {
            ammoGun.text = ammo.ToString();
            ammoTotal.text = "Reserve Ammo: " + curAmmo.ToString();

            if (Input.GetButton("Fire2") && !isReloading)
            {
                Camera.main.fieldOfView = 50;
                pistola.SetBool("Aim", true);
            }
            else
            {
                pistola.SetBool("Aim", false);
                Camera.main.fieldOfView = 60;
            }

            if (Input.GetButtonDown("Reload") && !isReloading && ammo <= maxAmmo && curAmmo > 0)
            {
                StartCoroutine(Reload());
            }

            Debug.DrawRay(transform.parent.position, transform.TransformDirection(Vector3.forward), Color.green);
            if (Input.GetButtonDown("Fire1") && !isReloading && ammo > 0)
            {
                Shoot();
                AS.Play();
            }
            else
            {
                pistola.SetBool("Shooting", false);
            }
        }

    }
    private void OnEnable()
    {
        if (isReloading)
        {
            StopCoroutine(Reload());
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        AS.volume = 0.4f;
        pistola.SetBool("Reloading", true);
        arms.SetBool("Reloading", true);
        AS.clip = eject;
        AS.Play();
        isReloading = true;
        yield return new WaitForSeconds(reloadTime - 0.3f);
        AS.clip = inject;
        AS.Play();
        yield return new WaitForSeconds(0.3f);
        AS.clip = shoot;

        if (curAmmo < maxAmmo - ammo)
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
        pistola.SetBool("Reloading", false);
        AS.volume = 0.2f;
    }

    private void Shoot()
    {
        ammo--;
        if (!pistola.GetBool("Shooting"))
        {
            pistola.SetBool("Shooting", true);
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.parent.position, transform.TransformDirection(Vector3.forward), out hit, 100))
        {
            if (hit.collider.name == "Head")
            {
                Instantiate(Hit2, hit.point, Quaternion.identity);
                hit.transform.GetComponent<Zombie>().health -= 3f * Random.Range(3.5f, 4.5f);
            }
            if (hit.collider.name == "Body")
            {
                Instantiate(Hit2, hit.point, Quaternion.identity);
                hit.transform.GetComponent<Zombie>().health -= 2.75f;
            }
            if (hit.collider.name == "Legs")
            {
                Instantiate(Hit2, hit.point, Quaternion.identity);
                hit.transform.GetComponent<Zombie>().health -= 1f;
            }
            if (hit.collider.name != "Head" && hit.collider.name != "Body" && hit.collider.name != "Legs")
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
