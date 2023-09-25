using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    int numGarrisoned = 0;
    void Start()
    {
        EventManager.current.onGarrisonEnter += onGarrison;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void onGarrison(GameObject garrison, Animal a)
    {
        numGarrisoned++;
    }
}
