using UnityEngine;
public interface IAttack
{
    void Hit(int type, Vector3 pos, float speed = 0f, OVRInput.Controller controller = OVRInput.Controller.Active);
    void HitEnemyAttack(int type = 0, OVRInput.Controller controller = OVRInput.Controller.Active);
}