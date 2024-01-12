using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class Movements : MonoBehaviour
{
    // Start is called before the first frame update
    private float rotationX=0;
    private float rotationY=0;
    [SerializeField] private Camera view;
    [SerializeField] private GameObject player;
    private Vector3 HorizontalVelocity;
    private Vector3 VerticalVelocity;
    private float gravity = 9.81f;
    private float movespeed = 10f;
    public int noise;
    [SerializeField] public UnityEvent noiseManager;
    [SerializeField] private bool isMov;

    private IEnumerator MakeNoise()
    {
        if (isMov)
        {
            noiseManager.Invoke();
        }
        yield return new WaitForSeconds(1);
        
        StartCoroutine(MakeNoise());
    }

    void Start()
    {
        noise = 3;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        isMov = false;
        StartCoroutine(MakeNoise());
    }
    // Update is called once per frame
    public void Lok(InputAction.CallbackContext context)
    {
        Vector2 angle = context.ReadValue<Vector2>() / 10;
        rotationX -= angle.y;
        rotationY += angle.x;

        rotationX = Mathf.Clamp(rotationX, -80f, 80f);

        view.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        player.transform.rotation = Quaternion.Euler(0, rotationY, 0);
    }
    void FixedUpdate()
    {
        transform.GetComponent<CharacterController>().Move((HorizontalVelocity.x * player.transform.right + HorizontalVelocity.y * player.transform.forward) * Time.fixedDeltaTime);

        VerticalVelocity += Vector3.down * gravity * Time.fixedDeltaTime;
        transform.GetComponent<CharacterController>().Move(VerticalVelocity * Time.fixedDeltaTime);

    }

    public void Mov(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isMov = true;
            
        }
        HorizontalVelocity = new Vector3(context.ReadValue<Vector2>().x * movespeed, context.ReadValue<Vector2>().y * movespeed, 0);
        if (context.canceled)
        {
            isMov = false;
        }
    }
    public void crutch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (noise > 1)
            {
                noise -= 1;
                switch (noise)
                {
                    case 1:
                        movespeed = 1;
                        view.transform.position -= new Vector3(0, 0.25f, 0);
                        break;
                    case 2:
                        movespeed = 5;
                        view.transform.position -= new Vector3(0, 0.5f, 0);
                        break;
                }
            }
            
        }
    }
    public void stand_u(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (noise < 3)
            {
                noise += 1;
                switch (noise)
                {
                    case 2:
                        movespeed = 5;
                        view.transform.position += new Vector3(0, 0.25f, 0);
                        break;
                    case 3:
                        movespeed = 10;
                        view.transform.position += new Vector3(0, 0.5f, 0);
                        break;
                }
            }
            
        }
    }
    public void collect()
    {

    }
    private void collect(GameObject text)
    {
        if (text.activeSelf)
        {
            Destroy(gameObject);
        }
    }

}
