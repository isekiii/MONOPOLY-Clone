using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    Transform[] position;

    int activeIndex;

    public float speed = 8f;
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, position[activeIndex].position, speed * Time.deltaTime);
        //transform.SetPositionAndRotation(position[activeIndex].position, position[activeIndex].rotation );
        Quaternion r = transform.rotation;
        r.eulerAngles = Vector3.Lerp(r.eulerAngles, position[activeIndex].rotation.eulerAngles, speed * Time.deltaTime);
        transform.rotation = r;
    }

    public void SetActivePosition(int pos)
    {
        activeIndex = pos;
    }
}
