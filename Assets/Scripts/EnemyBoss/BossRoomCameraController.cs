using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BossRoomCameraController : MonoBehaviour
{
    private int CAMERA_PRIORITY_BEFORE = 1, CAMERA_PRIORITY_AFTER = 100;

    [SerializeField]
    private CinemachineVirtualCamera bossRoomCamera;
    [SerializeField]
    private GameObject doorsClosed, doorsOpen;

    private EnemyBossController bossEnemyBossController;

    // Start is called before the first frame update
    void Start()
    {
        bossRoomCamera.Priority = CAMERA_PRIORITY_BEFORE;
        bossEnemyBossController = GetComponentInParent<EnemyBossController>();
        doorsClosed.SetActive(false);
        doorsOpen.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !bossEnemyBossController.isDead) {
            bossRoomCamera.Priority = CAMERA_PRIORITY_AFTER;
            doorsClosed.SetActive(true);
            doorsOpen.SetActive(false);
        }
    }

    public void ResetCamera()
    {
        bossRoomCamera.Priority = CAMERA_PRIORITY_BEFORE;
        doorsClosed.SetActive(false);
        doorsOpen.SetActive(true);
    }

}
