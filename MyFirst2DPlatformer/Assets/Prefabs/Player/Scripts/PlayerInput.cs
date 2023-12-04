using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public IMoveble moveble;
    // Update is called once per frame
    void Start()
    {
        moveble = GetComponent<PlayerController>();
    }
    void Update()
    {
        moveble.MoveHorizontal(Input.GetAxis("Horizontal"));
        moveble.MoveVertical(Input.GetAxis("Vertical"));
        moveble.Atack(Input.GetKeyDown(KeyCode.E));
        if(Input.GetButton("Jump"))
        {
            moveble.Jump();
        }
    }
}
