using UnityEngine;

public interface I_Perchable
{
     void StartPerch();
     void StopPerch();
     void UpdatePerch();
     void MovePosition(float x);

    void SetPlayerRef(GameObject player);

}
