using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;

    private enum Callers { player, enemy };
    private Callers _caller;

    // Update is called once per frame
    private void Update()
    {
        switch (_caller)
        {
            case Callers.player:
            {
                transform.Translate(Vector3.up * _speed * Time.deltaTime);
                break;
            }

            case Callers.enemy:
            {
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
                break;
            }
        }

        if (transform.position.y > 8 || transform.position.y < -10)
        {
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);

            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Asteroid")
            Destroy(this.gameObject);
    }

    public void setCaller(string caller)
    {
        switch (caller)
        {
            case "Player":
                _caller = Callers.player;
                break;
            case "Enemy":
                _caller = Callers.enemy;
                break;
            default:
                Debug.LogError("Caller incorrect");
                break;
        }
    }

    public string getCaller()
    {
        switch (_caller)
        {
            case Callers.player:
                return "Player";
            case Callers.enemy:
                return "Enemy";
            default:
                return "error";
        }
    }
}