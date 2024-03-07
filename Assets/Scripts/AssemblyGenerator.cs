using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyGenerator : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] float delay = 1f;

    void Awake()
    {
        StartCoroutine(Assemble());
    }

    IEnumerator Assemble()
    {
        while(true)
        {
            Instantiate(prefab, transform.position, transform.rotation);
            yield return new WaitForSeconds(delay);
        }
    }
}
