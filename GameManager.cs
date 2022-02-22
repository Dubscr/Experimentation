using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TMP_Text wave;
    public int health;
    public int money;
    public int waveNum;
    [SerializeField] GameObject[] zombies;
    [SerializeField] GameObject[] spawns;
    [SerializeField] private float waitTime;

    [SerializeField] private GameObject rifle;
    [SerializeField] private GameObject pistol;
    [SerializeField] public bool rifleAble;
    public TMP_Text money_;
    public Slider health_;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckForZombies());
    }

    // Update is called once per frame
    void Update()
    {
        wave.text = "Wave: " + (waveNum - 1);
        health_.value = health;
        money_.text = "$" + money;
        if (Input.GetKeyDown(KeyCode.Alpha1) && rifleAble)
        {
            pistol.SetActive(false);
            rifle.SetActive(true);
        } else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            pistol.SetActive(true);
            rifle.SetActive(false);
        }
    }
    void Spawn()
    {
        var zombieAmount = waveNum * 2;

        for (int i = 0; i < zombieAmount; i++)
        {
            var zombie = Instantiate(zombies[0], spawns[Random.Range(0, spawns.Length)].transform);
            zombie.transform.parent = null;
        }

    }

    IEnumerator CheckForZombies()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            if (GameObject.FindGameObjectsWithTag("Enemy").Length < 1)
            {
                Spawn();
                waveNum++;
            }
        }
    }
}
