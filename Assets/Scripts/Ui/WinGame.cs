using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGame : MonoBehaviour
{
   
    public Cube[] cubesArray;

    public GameObject winUI;
    public void winGame()
    {
        cubesArray = GetComponentsInChildren<Cube>();

        
        for (int i = 0; i < cubesArray.Length; i++)
        {
           if( cubesArray[i].CubeNumber >= 1000000000)
            {
                Time.timeScale = 0f;
                winUI.SetActive(true);
            }

        }
    }

    private void Update()
    {
        winGame();
    }

}
