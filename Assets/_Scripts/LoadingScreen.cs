using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Image loadingBar; 
    [SerializeField] private TextMeshProUGUI textLoading;
    private string[] texts = new string(3);
    
    private SceneChange sc;

    void Start()
    {
        sc = SceneChange.sc;
        texts[0] = "Use Espaço para pula";
        texts[1] = "Use AWSD ou Setas para se mover";
        texts[2] = "Use o botão esquerdo do mouse para atirar raios";

        textLoading.text = texts[0];
        loadingBar.fillAmount = 0;
        StartCoroutine();
    }

    private IEnumerator Loading(){
        for (int i = 1; i < 3; i++)
        {
            yield return new WaitForSeconds(2f);
            textLoading.text = texts[i];
        }
        SceneManager.LoadScene(sc.nextScene);

    }
}
