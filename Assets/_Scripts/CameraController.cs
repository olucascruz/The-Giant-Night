using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{   
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform skinTransform;
    [SerializeField] private Transform VfxTransform;

    

    private float rotationX;
    private float rotationY;

    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Transform cam;
    [SerializeField] private float velCam;
    private GameController gc;
    // Start is called before the first frame update
    void Start()
    {   
        gc = GameController.gc;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    { 
        if(gc){
            if(gc.gameState == GameController.GameState.GAMEOVER) return;
        }
        if (playerTransform){
            rotationX = Mathf.Lerp(rotationX, Input.GetAxisRaw("Mouse X")*2, 100 * Time.deltaTime);
            rotationY = Mathf.Clamp(
                rotationY - (Input.GetAxisRaw("Mouse Y")* 2 * 100 * Time.deltaTime),
                -30, 30);
            
            VfxTransform.Rotate(0, rotationX/2f, 0, Space.World);
            playerTransform.Rotate(0, rotationX, 0, Space.World);
            skinTransform.Rotate(0, rotationX, 0, Space.World);
            
            cam.rotation = Quaternion.Lerp(
                cam.rotation,
                Quaternion.Euler(rotationY * 2, playerTransform.eulerAngles.y, 0),
                100 * Time.deltaTime);
            
            cameraPivot.position = Vector3.Lerp(cameraPivot.position, playerTransform.position, velCam*Time.deltaTime);
            
        }
    }
}