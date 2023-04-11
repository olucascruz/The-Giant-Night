using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{

    public string nextScene;

   public void EnterGame(){
    nextScene = "Maguin";
    SceneManager.LoadScene("ScreenLoading");
   }
   public void Retry(){
    SceneManager.LoadScene("Maguin");
   }

   public void QuitSceneGame(){
    nextScene = "Menu";
    SceneManager.LoadScene("ScreenLoading");
   }

   public void QuitGame(){
    Application.Quit();
   }
}
