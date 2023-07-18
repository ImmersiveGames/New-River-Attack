using UnityEngine;
namespace RiverAttack
{
   
public class PlayerSkinTrail : MonoBehaviour
{
    private PlayerMaster playerMaster;
    private GamePlayManager gamePlay;
    private TrailRenderer[] trailRenderer;

    private void OnEnable()
    {
        SetInitialReferences();
        if (playerMaster)
            playerMaster.EventControllerMovement += SetTrail;
        trailRenderer = GetComponentsInChildren<TrailRenderer>();
        gamePlay.EventCompletePath += EnableTrails;
        SetTrails(false);
    }
    private void SetInitialReferences()
    {
        playerMaster = GetComponentInParent<PlayerMaster>();
        gamePlay = GamePlayManager.instance;
    }
    private void SetTrail(Vector2 dir)
    {
        if (dir.y > 0)
            SetTrails(true);
        if (dir.y <= 0)
            SetTrails(false);
    }

    private void SetTrails(bool setting)
    {
        for (int i = 0; i < trailRenderer.Length; i++)
        {
            trailRenderer[i].enabled = setting;
        }
    }
    private void EnableTrails()
    {
        for (int i = 0; i < trailRenderer.Length; i++)
        {
            trailRenderer[i].enabled = true;
        }
    }

    private void OnDisable()
    {
        if (playerMaster)
            playerMaster.EventControllerMovement -= SetTrail;
        gamePlay.EventCompletePath -= EnableTrails;
    }
} 
}

