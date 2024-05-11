using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.GetInt("First");
        PlayerPrefs.GetInt("Second");
        PlayerPrefs.GetInt("Third");
        PlayerPrefs.GetInt("Fourth");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
