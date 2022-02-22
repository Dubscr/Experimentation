using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Buy : MonoBehaviour
{
    public GameManager gm;
    public Rifle rifle;
    public TMP_Text text;
    [SerializeField] private Transform Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(Player.position, transform.position) < 1.5f)
        {
            text.text = "Press E to Buy for 3500";
            if (Input.GetButtonDown("Interact") && !gm.rifleAble && gm.money >= 3500)
            {
                gm.rifleAble = true;
                gm.money -= 3500;
            }
            if (Input.GetButtonDown("Interact") && gm.rifleAble && gm.money >= 3500)
            {
                rifle.curAmmo += 250;
                gm.money -= 3500;
            }
        } 
        else
        {
            text.text = "";
        }
    }
}
