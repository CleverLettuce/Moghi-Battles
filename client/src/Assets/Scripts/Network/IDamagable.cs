using UnityEngine;
using System.Collections;

public interface IDamagable {

    int getTeamId();
    bool isDead();
    void takeDamage(float opponentAttack, string playerName);
    float getHealth();
    float getMaxHealth();
    PhotonView getView();

}
