using UnityEngine;

public class GameOverZone : MonoBehaviour
{
    [SerializeField] private GameSceneManager gameSceneManager;
    [SerializeField] private LayerMask monsterLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject currentCollision = collision.gameObject;

        if (((1 << currentCollision.layer) & monsterLayer) != 0)
        {
            gameSceneManager.ShowGameOverPopup();
        }
    }
}
