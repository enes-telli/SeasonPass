using UnityEngine;
using BhorGames;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float zSpeed = 2f;
    [SerializeField] private float xSpeed = 2f;

    private float deltaX;

    private void Update()
    {
        if (GameManager.Instance.isGameStarted)
        {
            if (Input.GetMouseButtonDown(0))
            {
                deltaX = 0;
            }
            else if (Input.GetMouseButton(0))
            {
                deltaX = Input.GetAxis("Mouse X");
            }
            else
            {
                deltaX = 0;
            }
            
            transform.Translate(deltaX * xSpeed * Time.deltaTime, 0, zSpeed * Time.deltaTime);

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -2f, 2f), transform.position.y, transform.position.z);
        }
    }
    
}
