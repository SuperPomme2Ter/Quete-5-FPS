using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generation : MonoBehaviour
{
    [SerializeField] private GameObject prey;
    [SerializeField] private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            
            GameObject newPrey= Instantiate(prey,new Vector3(0,1,0),prey.transform.rotation);
            while (Vector3.Distance(newPrey.transform.position, player.transform.position) < 80)
            {
                newPrey.transform.position = new Vector3(Random.Range(-500, 500), 1, Random.Range(-500, 500));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
