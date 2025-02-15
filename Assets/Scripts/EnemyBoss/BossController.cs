using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossController : MonoBehaviour
{
    [SerializeField]
    private float health = 20, damage = 5, edgeRadius = 4f;
    [HideInInspector]
    public bool isDead = false;
    [SerializeField]
    private GameObject hitParticles;
    [SerializeField]
    private Transform leftEdge, rightEdge;
    [SerializeField]
    private Sprite bossDefeatRewardSprite;

    private BossRoomCameraController bossRoomCamera;
    private RewardController rc;
    private PlayerController pc;

    // Start is called before the first frame update
    void Start()
    {
        bossRoomCamera = GetComponentInChildren<BossRoomCameraController>();
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        rc = GameObject.Find("RewardPopup").GetComponent<RewardController>();
    }

    // Update is called once per frame
    void Update()
    {
        Kill();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(leftEdge.position, edgeRadius);
        Gizmos.DrawWireSphere(rightEdge.position, edgeRadius);
    }

    private void Kill()
    {
        if (health <= 0) {
            gameObject.SetActive(false);
            isDead = true;
            bossRoomCamera.ResetCamera();
            rc.createRewardPopup(
                rewardImageSprite: bossDefeatRewardSprite,
                rewardTextString: "ENEMY FELLED!\nYou have obtained the broken treasure.\nPress E to continue!",
                rewardType: RewardController.RewardType.SHARDS
            );
        }
    }

    private void TakeDamage(float damage)
    {
        Instantiate(
            hitParticles,
            pc.isFacingRight ? leftEdge.position : rightEdge.position,
            Quaternion.Euler(-10.0f, 0f, Random.Range(0f, 360f))
        );
        health -= damage;
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        other.gameObject.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        other.gameObject.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
    }    
}
