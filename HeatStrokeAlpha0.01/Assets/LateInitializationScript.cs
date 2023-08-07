using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateInitializationScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        yield return new WaitForSeconds(3f);

        GameEventSystem.current.generatedGrid();
    }
}
