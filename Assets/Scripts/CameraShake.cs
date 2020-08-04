using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 _origPos;

    // Start is called before the first frame update
    void Start()
    {
        _origPos = transform.position;
    }

    public IEnumerator ShakeCamera(float length)
    {
        while (length > 0)
        {
            float xPos = Random.Range(-0.15f, 0.15f);
            float yPos = Random.Range(-0.15f, 0.15f);

            transform.position = new Vector3(xPos, yPos, transform.position.z);

            length -= Time.deltaTime;
            yield return null;
        }

        transform.position = _origPos;
    }
}
