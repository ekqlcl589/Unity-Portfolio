using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotation : MonoBehaviour
{
    private Vector3 OpenRotation = new Vector3(0, 90, 0);
    private Vector3 CloseRotation = new Vector3(0, 0, 0);

    private float rotSpeed = 1f;

    private bool doorBool;


    // Start is called before the first frame update
    void Start()
    {
        doorBool = false;

    }

    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag == ("Player") && Input.GetKeyDown(KeyCode.E))
        {
            if (!doorBool)
                doorBool = true;
            else
                doorBool = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (doorBool)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0f, 90f, 0f)), rotSpeed * Time.deltaTime);
        else
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(CloseRotation), rotSpeed * Time.deltaTime);

    }
}
