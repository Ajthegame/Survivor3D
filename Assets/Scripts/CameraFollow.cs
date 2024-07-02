using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]GameObject followThis;
    [SerializeField] Vector3 _offset;
    Vector3 offset
    {
        get
        {
            return _offset;
        }

        set
        {
            if (offset != value)
            {
                _offset = value;
                transform.position = followThis.transform.position - offset;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = followThis.transform.position + offset;
    }

    // Update is called once per frame
    void LateUpdate()
    {
         Vector3 difference = transform.position - followThis.transform.position;
         transform.position = Vector3.Lerp(transform.position, followThis.transform.position + offset,.2f);
    }
}
