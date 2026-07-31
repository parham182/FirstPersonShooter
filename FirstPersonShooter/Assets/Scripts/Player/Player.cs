using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] CameraRotation cameraRotation;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] GameObject uiCanvas;
    [SerializeField] UiFader uiFader;
    
    public bool hasPills = false;
    public static Player instance;
    
    private void Awake() { instance = this; }

    private void Start()
    {
        uiFader.FadeOut();
    }
    
    public void Sleep()
    {
        uiFader.FadeIn(1);
        LevelManager.instance.ChangeLevel();
    }

    public void DisableControl() {
        playerMovement.canMove = false;
        cameraRotation.enable = false;
        uiCanvas.SetActive(false);
    }

    public void EnableControl() {
        playerMovement.canMove = true;
        cameraRotation.enable = true;
        uiCanvas.SetActive(true);
    }
}
