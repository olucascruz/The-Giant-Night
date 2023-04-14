using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    private SceneChange sc;
    
    void Start()
    {
        sc = SceneChange.sc;
    }

    public void Retry()
    {
        print("retry");
        sc.Retry();
    }

    public void QuitSceneGame()
    {
        sc.QuitSceneGame();
    }
}
