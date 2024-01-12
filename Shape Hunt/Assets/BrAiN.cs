using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class BrAiN : MonoBehaviour
{
    [SerializeField] public int state;
    [SerializeField] private float timeToWait;
    [SerializeField] private float eventRot;
    [SerializeField] private float randomRot;
    [SerializeField] private Quaternion rotNeeded;
    [SerializeField] private float timer;
    [SerializeField] private float fear;
    [SerializeField] private Vector2 targetPos;
    [SerializeField] private Vector2 actualPos2D;
    [SerializeField] private Vector3 dir;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 noisePos;
    [SerializeField] private Quaternion check;
    [SerializeField] private float detectionAngle = 45;
    [SerializeField] private Material skin;
    [SerializeField] private CapsuleCollider body;
    [SerializeField] private Rigidbody bodyDead;
    [SerializeField] private GameObject collectible;
    // Start is called before the first frame update
    void Start()
    {
        bodyDead.useGravity=false;
        skin.color = new Color(255, 255, 255, 1);
        state = 0;
        timer = 0;
        fear = 0;
        timeToWait = Random.Range(1f, 15f);
        eventRot = Random.Range(1f, timeToWait - 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (fear >= 100)
        {
            state = 3;
        }
        if (Mathf.Abs(Vector3.Distance(transform.position, player.transform.position)) < 35)
        {
            
            if (Vector3.Dot(Vector3.Normalize(transform.position - player.transform.position), transform.forward) > Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad))
            {
                state = 3;
            }
        }
        switch (state)
        {
            case 0://idle
                idle();
                break;
            case 1: //walking
                moving();
                break;
            case 2://intrigate
                intrigate();
                break;
            case 3://fleeing
                fleeing();
            break;
            case 4://dead
                death();
            break;
        }

    }
    private void idle()
    {
        if (Mathf.Abs(timer - eventRot) < 0.5f || eventRot == 50)
        {
            randomRot = Random.Range(1f, 90f);
            eventRot = 100;
        }
        if (timer < timeToWait)
        {
            if (eventRot == 100)
            {
                if (Mathf.Abs(transform.rotation.y - randomRot) > 2)
                {
                    if (Mathf.Abs(transform.rotation.y - randomRot) < 45)
                    {
                        transform.Rotate(new Vector3(0, -Time.fixedDeltaTime * 50, 0));
                    }
                    else
                    {
                        transform.Rotate(new Vector3(0, Time.fixedDeltaTime * 50, 0));
                    }
                }
                else
                {
                    eventRot = Random.Range(1f, timeToWait + 1);
                }
            }
            timer += Time.fixedDeltaTime;
        }
        else
        {

            state = 1;
            targetPos = new Vector2(transform.position.x, transform.position.z) + Random.insideUnitCircle * 50;
            eventRot = 50;
        }
    }
    private void moving() {

        actualPos2D = new Vector2(transform.position.x, transform.position.z);
        if (Vector2.Distance(actualPos2D, targetPos) >= 1f)
        {

            dir = new Vector3(targetPos.x - transform.position.x, 0, targetPos.y - transform.position.z);
            dir = Vector3.Normalize(dir);
            rotNeeded = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z), new Vector3(0, dir.y, 0));
            //rotNeeded.x = 90;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotNeeded, 1);
            transform.position += dir * Time.fixedDeltaTime*1.5f;
        }
        else
        {
            state = 0;
            timer = 0;
            timeToWait = Random.Range(1f, 10f);
            eventRot = Random.Range(1f, timeToWait - 1);
        }
    }
    private void intrigate() {
        check = Quaternion.LookRotation(new Vector3(noisePos.x, 0, noisePos.z), new Vector3(0, noisePos.y, 0));
        transform.rotation = Quaternion.Slerp(transform.rotation, check, Time.fixedDeltaTime*2);
        skin.color = new Color(0, 200, 242, 1);
    }
    private void fleeing()
    {
        if (fear == 100)
        {
            dir = player.transform.position - transform.position;
            dir = -Vector3.Normalize(dir);
            skin.color = new Color(25, 170, 15, 1);
        }
        if (fear > 0)
        {

            transform.position += dir * 30 * Time.fixedDeltaTime;
            fear -= 10 * Time.fixedDeltaTime;

        }
        else
        {
            fear = 0;
            state = 0;
        }
    }
    
    public void detectNoise()
    {
        if (state < 3)
        {
            switch (player.GetComponent<Movements>().noise)
            {
                case 1:
                    if (Vector3.Distance(player.transform.position, transform.position) <= 15)
                    {
                        fear += 5;
                        if (state < 2)
                        {
                            state = 2;
                            noisePos = player.transform.position;
                        }
                    }
                    break;
                case 2:
                    if (Vector3.Distance(player.transform.position, transform.position) <= 25)
                    {
                        fear += 10;
                        if (state < 2)
                        {
                            state = 2;
                            noisePos = player.transform.position;
                        }
                    }
                    break;
                case 3:
                    if (Vector3.Distance(player.transform.position, transform.position) <= 50)
                    {
                        fear += 20;
                        if (state < 2)
                        {
                            state = 2;
                            noisePos = player.transform.position;
                        }
                    }
                    break;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetType() == typeof(SphereCollider))
        {
            state = 4;
            body.isTrigger = false;
            bodyDead.useGravity = true;
            skin.color = new Color(255, 0, 0, 1);
        }
    }
}
