using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameController : MonoBehaviour
{
    public enum SpawnState {SPAWNING, COUNTING, WAITING};
    public enum GameState {PLAY, PAUSE, GAMEOVER};
    public int lvl = 0;
    public int enemiesDied = 0;

    public static GameController gc;

    private SpawnSystem ss;
    public SpawnState spawnState = SpawnState.WAITING;
    public GameState gameState = GameState.PLAY;

    [SerializeField] private GameObject obejctTextInitial;
    [SerializeField] private TextMeshProUGUI textInitial;
    private bool textInitialRunning = true;
    private bool callInformation = false;
    private bool firstGiant = false;
    [SerializeField] private GameObject objGameOver;
    [SerializeField] private TextMeshProUGUI textGameOver;
    private GameObject objPlayer;
    private Player scriptPlayer;

    [SerializeField] private GameObject waveInformation;
    [SerializeField] private TextMeshProUGUI textWaveInformation;

    

    void Awake(){
        if (gc == null){
            gc = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {   
        spawnState = SpawnState.WAITING;
        ss = SpawnSystem.ss;
        objPlayer = GameObject.FindGameObjectWithTag("Player");
        scriptPlayer = objPlayer.GetComponent<Player>();
    }
    
    void Pause(){
        Time.timeScale = 0;
        gameState = GameState.PAUSE;

    }

    void Resume(){
        Time.timeScale = 1f;
        gameState = GameState.PLAY;
    }

    void GameOver(){
        gameState = GameState.GAMEOVER;
        objGameOver.SetActive(true);
        textGameOver.text = "Você morreu";
        Cursor.visible = true;
    }


    void Update()
    {   
        if(gameState == GameState.GAMEOVER) return;

        if(scriptPlayer.PlayerLife <= 0){
            GameOver();
        }

        LevelController();
        if(Input.GetKeyDown(KeyCode.P)){
            if(gameState == GameState.PLAY){
                Pause();
            }else if(gameState == GameState.PAUSE){
                Resume();
            }

        } 
    }


    void Spawner(){
        if(spawnState == SpawnState.SPAWNING){
             switch (lvl)
            {
                case 0:
                    ss.Spawner(1, 0.1f);
                    break;
                case 1:
                    ss.Spawner(3, 2f);
                    break;
                case 2:
                    ss.Spawner(6, 3f);
                    break;
                case 3:
                    ss.Spawner(9, 4f);
                    break;
                default:
                    break;
            }

            spawnState = SpawnState.WAITING;
        }
    }

    void Information(){
        callInformation = true;
        string[] texts = new string[4];;
        texts[0] = "Os orbes protetores estão sem energia máxima";
        texts[1] = "E como você é o único mago eletrico da vila";
        texts[2] = "Sua missão é proteger a vila dos gigantes";
        texts[3] = "Você ainda pode recarregar com o pouco de energia que resta das pedras protetoras (botão direito mouse)";
        int index = 0;
        StartCoroutine(ShowText(texts,index));    
    }

    void WaveInformation(int n){
        string[] texts = new string[4];;
        texts[0] = "Wave 1: Apenas um.";
        texts[1] = "Wave 2: 3 é demais.";
        texts[2] = "Wave 3: Horda de gigantes CUIDADO.";
        texts[3] = "Wave 4: Agora veio todo mundo.";

        textWaveInformation.text = "";
        textWaveInformation.text = texts[n];

        waveInformation.SetActive(true);
    }

    IEnumerator ShowText(string[] t, int i){
      while(i < t.Length){
        textInitial.text = "";
        textInitial.text = t[i];
        yield return new WaitForSeconds(3f);
        i++;
      }
      textInitialRunning = false;
      obejctTextInitial.SetActive(false);
    } 

    IEnumerator WaitNextWave(){
        spawnState = SpawnState.COUNTING;
        WaveInformation(lvl);
        yield return new WaitForSeconds(5f);
        waveInformation.SetActive(false);
        spawnState = SpawnState.SPAWNING;
        Spawner();
    }

    void LevelController(){
        if(spawnState != SpawnState.WAITING) return;
    
        if(lvl == 0 && enemiesDied == 0){
            if(!callInformation){
                Information();
            }
            if(!textInitialRunning && !firstGiant){
                spawnState = SpawnState.SPAWNING;
                StartCoroutine(WaitNextWave());
                firstGiant = true;
            }
        }

        if(lvl == 0 && enemiesDied == 1){
            lvl++;
            StartCoroutine(WaitNextWave());
            enemiesDied = 0;
        }
        if(lvl == 1 && enemiesDied == 3){
            lvl++;
            StartCoroutine(WaitNextWave());
            enemiesDied = 0;

        }
        if(lvl == 2 && enemiesDied == 6){
            lvl++;
            StartCoroutine(WaitNextWave());
            enemiesDied = 0;

        }
        if(lvl == 3 && enemiesDied == 9){
            enemiesDied = 0;
        }
    }

}


    