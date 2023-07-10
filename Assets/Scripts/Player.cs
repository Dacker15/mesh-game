using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xVariation = Input.GetAxisRaw("Horizontal");
        float yVariation = Input.GetAxisRaw("Vertical");
        
        gameObject.transform.position += new Vector3(speed * xVariation * Time.deltaTime, 0, speed * yVariation * Time.deltaTime);
    }
}
