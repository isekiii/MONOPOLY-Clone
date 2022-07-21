using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    Rigidbody rb;
    
    [SerializeField]
    List<Transform> sides;
    public int result;
    public bool isDoneRolling = false;

    public void RollDice()
    {
        isDoneRolling = false;
        rb = GetComponent<Rigidbody>();
        Vector3 randomRotation = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        
        transform.rotation = Quaternion.Euler(randomRotation);
        transform.position = new Vector3(Random.Range(1f, 15f), 4f, Random.Range(-4f, -17f));
        
        rb.AddForce(Random.Range(-3f,3f), Random.Range(-3f,3f), Random.Range(-3f,3f), ForceMode.Impulse);
        StartCoroutine(WaitForResult());
    }


    IEnumerator WaitForResult()
    {
        yield return new WaitForSeconds(4f);
        result = GetResult();
        isDoneRolling = true;
    }

    int GetResult()
    {
        int result = 0;
        float max = -5f;
        for (int i = 0; i < 6; i++)
        {
            if (sides[i].position.y >= max)
            {
                result = i + 1;
                max = sides[i].position.y;
            }
        }
        Debug.Log(result);
        return result;
    }

    
}
