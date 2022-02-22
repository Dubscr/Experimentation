using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBob : MonoBehaviour
{
    public Animator anim;
    void Update()
    {
        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(input.magnitude > 0.1f && !Input.GetButton("Fire2"))
        {
            anim.SetBool("Bob", true);
        } else
        {
            anim.SetBool("Bob", false);
        }
    }
}
