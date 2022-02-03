using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class BanditPlayerController : MonoBehaviour
{
    private bool isMovement = true;
    [SerializeField] private LayerMask raycastMask;
    [SerializeField] private LayerMask explosionMask;
    public GameObject prefBomb;
    public GameObject prefExplosion;
    public GameObject Player;

    // Update is called once per frame
    void Update()
    {
        if(isMovement)
        {
            
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) MovePlayerTo(Vector2.left);
        if (Input.GetKeyDown(KeyCode.RightArrow)) MovePlayerTo(Vector2.right);
        if (Input.GetKeyDown(KeyCode.UpArrow)) MovePlayerTo(Vector2.up);
        if (Input.GetKeyDown(KeyCode.DownArrow)) MovePlayerTo(Vector2.down);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject Bomb = Instantiate(prefBomb, transform.position, transform.rotation);
            StartCoroutine(BombDestroy(Bomb));
        }
    }

    private IEnumerator BombDestroy(GameObject Bomb)
    {
        yield return new WaitForSeconds(1f);
        var colliders = Physics2D.OverlapCircleAll(Bomb.transform.position, 1f, explosionMask);

        foreach (var item in colliders)
        {
            if(item.gameObject == Player) ReloadGame();
            Destroy(item.gameObject);
        }

        GameObject Explosion = Instantiate(prefExplosion, Bomb.transform.position, Bomb.transform.rotation);
        Destroy(Bomb);
    }

    private void ReloadGame()
    {  
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    private void MovePlayerTo(Vector2 dir)
    {
        if(Raycast(dir))
        {
            return;
        }

        isMovement = true;

        var pos = (Vector2)transform.position + dir;
        transform.DOMove(pos, 0.1f).OnComplete(() =>
        {
            isMovement = false;
        });
    }

    private bool Raycast(Vector2 dir)
    {
        var hit = Physics2D.Raycast(transform.position, dir, 1f, raycastMask);
        return hit.collider != null;
    }

    private GameObject RaycastFromCamera()
    {
        var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        return hit.collider != null ? hit.collider.gameObject : null;
    }
}
