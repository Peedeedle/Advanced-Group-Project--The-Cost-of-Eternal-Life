using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movecamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKey(KeyCode.Z)) {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(-20.41336f, 2.69f, -10), .6f);
		}

        //0.479506f
    }
}
