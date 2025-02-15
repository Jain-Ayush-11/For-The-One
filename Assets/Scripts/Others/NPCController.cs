using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    private bool isFacingRight = true;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        CheckFacingDirection();
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180f, 0);

        TextMeshProUGUI interactionPrompt = GetComponentInChildren<TextMeshProUGUI>(true);
        interactionPrompt.gameObject.transform.Rotate(0, 180f, 0);
    }

    private void CheckFacingDirection()
    {
        if (
            (isFacingRight && player.transform.position.x < transform.position.x) ||
            (!isFacingRight && player.transform.position.x > transform.position.x)
        ) {
            Flip();
        }
    }


}
