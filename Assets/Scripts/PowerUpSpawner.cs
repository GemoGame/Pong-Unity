using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public PowerUp powerUp;
    public void spawnPowerUp()
    {
        PowerUp newPowerUp = Instantiate(powerUp);
        newPowerUp.gameObject.SetActive(true);
        newPowerUp.pushPowerUp();
    }

}
