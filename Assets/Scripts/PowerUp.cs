using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // enum to hold all the types of powerUps we can have
    private enum PowerUpType { TripleShot, Speed, Sheild, Ammo, Health, ExtraFire, StealAmmo };

    [SerializeField]
    private PowerUpType _powerUpType;

    [SerializeField]
    private float _speed = 3.0f;

    [SerializeField]
    private AudioClip _powerUpSound;

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_powerUpSound, transform.position);
            Player player = other.transform.GetComponent<Player>();

            if (player == null)
                Debug.LogError("Player is null");
            switch (_powerUpType)
            {
                case PowerUpType.TripleShot:
                    player.EnableTripleShot();
                    player.ResetShotsLeft(15);
                    break;

                case PowerUpType.Speed:
                    player.EnableSpeedBoost();
                    break;

                case PowerUpType.Sheild:
                    player.EnableSheild();
                    player.SetSheildHits();
                    break;

                case PowerUpType.Ammo:
                    player.ResetShotsLeft(15);
                    break;

                case PowerUpType.Health:
                    player.IncreaseLives();
                    break;

                case PowerUpType.ExtraFire:
                    player.EnableExtraFire();
                    player.ResetShotsLeft(15);
                    break;

                case PowerUpType.StealAmmo:
                    player.DecreaseAmmo(5);
                    break;
            }
            Destroy(this.gameObject);
        }
    }
}