using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlterInteractionScript : MonoBehaviour
{
    [SerializeField]
    private GameObject dialoguePanel, interactionLabel;
    [SerializeField]
    private Sprite rewardSprite;
    [SerializeField]
    private string[] noRewardDialogues;

    private PlayerController pc;
    private RewardController rc;

    private bool canInteract = false;
    private int noRewardDialoguesIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        interactionLabel.SetActive(false);
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        rc = GameObject.Find("RewardPopup").GetComponent<RewardController>();
    }

    void Update()
    {
        CheckInput();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        interactionLabel.SetActive(true);
        canInteract = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        dialoguePanel.SetActive(false);
        interactionLabel.SetActive(false);
        canInteract = false;
        noRewardDialoguesIndex = 0;
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.F) && canInteract) {
            Interact();
        }
    }

    private void ClaimReward()
    {
        rc.createRewardPopup(
            rewardImageSprite: rewardSprite,
            rewardTextString: "Congratulation! You have repaired the legendary treasure!\nPlease LOOK BACK to claim it!! (YES IRL)",
            rewardType: RewardController.RewardType.REWARD
        );
    }

    private void Interact()
    {
        interactionLabel.SetActive(false);
        if (pc.hasClaimedShards) {
            ClaimReward();
        } else if (noRewardDialoguesIndex >= noRewardDialogues.Length) {
            noRewardDialoguesIndex = 0;
            dialoguePanel.SetActive(false);
            interactionLabel.SetActive(true);
        } else {
            dialoguePanel.SetActive(true);
            TextMeshProUGUI interactionTextComponent = dialoguePanel.GetComponentInChildren<TextMeshProUGUI>();
            interactionTextComponent.text = noRewardDialogues[noRewardDialoguesIndex++];
        }
    }
}
