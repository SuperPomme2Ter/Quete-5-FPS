using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class tir : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Camera view;
    [SerializeField] private GameObject bulletHolder;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject rifle;
    [SerializeField] private bool support;
    
    void Start()
    {
        support = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }
    public void Tir(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (!support)
            {
                support = true;
                rifle.transform.localPosition = new Vector3(0, -0.01f, 0.4f);
                rifle.transform.localRotation = Quaternion.Euler(0, -90, 0);
            }
            else
            {

                bullet = Instantiate(bulletHolder, view.transform.position + new Vector3(view.transform.forward.x * 0.4f, view.transform.forward.y * 0.42f, view.transform.forward.z * 0.4f), transform.rotation);
                bullet.GetComponent<decay>().dir = view.transform.forward;
                bullet.GetComponent<Rigidbody>().velocity = view.transform.forward * 50;
            }
        }
    }
    public void StopTir(InputAction.CallbackContext context)
    {
        support = false;
        rifle.transform.localPosition = new Vector3(-1, 0.25f, 1);
        rifle.transform.localRotation = Quaternion.Euler(14.5f, 180, 90);
    }
}
