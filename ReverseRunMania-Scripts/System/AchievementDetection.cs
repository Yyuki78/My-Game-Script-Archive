using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementDetection : MonoBehaviour
{
    //一部の実績はItemManager,SwampGenerateでも判定してます

    public static float freezingTime = 0f;
    public static float wallStayTime = 0f;
    public static int DestroyCarNum = 0;
    public static int TeleportNum = 0;
    public static bool VanishCar = false;

    private float posX;
    private int checkNum;

    private float speed;
    private float maxSpeed;

    private GameObject Player;
    private PlayerMove _move;

    [SerializeField] GameObject Notification;
    private AchieveNotification _notify;

    void Start()
    {
        freezingTime = 0f;
        wallStayTime = 0f;
        DestroyCarNum = 0;
        TeleportNum = 0;
        VanishCar = false;
        Player = GameObject.FindGameObjectWithTag("Player");
        _move = Player.GetComponent<PlayerMove>();
        maxSpeed = _move.MaxSpeed;
        _notify = Notification.GetComponent<AchieveNotification>();
    }
    
    void Update()
    {
        if (checkNum % 10 != 0)
        {
            checkNum++;
            return;
        }
        if (freezingTime >= 21f && !PlayerPrefs.HasKey("Freeze"))
        {
            PlayerPrefs.SetInt("Freeze", 1);
            _notify.PlayNotification("アイススケーター");
        }
        if (wallStayTime >= 100f && !PlayerPrefs.HasKey("Wall"))
        {
            PlayerPrefs.SetInt("Wall", 1);
            _notify.PlayNotification("壁走りマスター");
        }
        if (DestroyCarNum >= 15 && !PlayerPrefs.HasKey("DestroyCar1"))
        {
            PlayerPrefs.SetInt("DestroyCar1", 1);
            _notify.PlayNotification("犯罪を重ねて");
        }
        if (DestroyCarNum >= 100 && !PlayerPrefs.HasKey("DestroyCar2"))
        {
            PlayerPrefs.SetInt("DestroyCar2", 1);
            _notify.PlayNotification("クラッシャー");
        }
        if (TeleportNum >= 20 && !PlayerPrefs.HasKey("Teleport"))
        {
            PlayerPrefs.SetInt("Teleport", 1);
            _notify.PlayNotification("テレポーター");
        }
        if(VanishCar && !PlayerPrefs.HasKey("Vanish"))
        {
            PlayerPrefs.SetInt("Vanish", 1);
            _notify.PlayNotification("バニッシュメント");
        }

        posX = Player.transform.position.x;
        speed = _move.CurrentSpeed;
        if (speed >= maxSpeed && !PlayerPrefs.HasKey("MaxSpeed"))
        {
            PlayerPrefs.SetInt("MaxSpeed", 1);
            _notify.PlayNotification("スピードスター");
        }
        if (posX < 2000f) return;
        if (!PlayerPrefs.HasKey("MeterAchieve1"))
        {
            PlayerPrefs.SetInt("MeterAchieve1", 1);
            _notify.PlayNotification("駆け出しのゴール");
        }
        if (posX < 5000f) return;
        if (!PlayerPrefs.HasKey("MeterAchieve2"))
        {
            PlayerPrefs.SetInt("MeterAchieve2", 1);
            _notify.PlayNotification("逆走常習犯");
        }
        if (posX < 10000f) return;
        if (!PlayerPrefs.HasKey("MeterAchieve3"))
        {
            PlayerPrefs.SetInt("MeterAchieve3", 1);
            _notify.PlayNotification("よくぞ辿り着いた");
        }
        if (posX < 20000f) return;
        if (!PlayerPrefs.HasKey("MeterAchieve4"))
        {
            PlayerPrefs.SetInt("MeterAchieve4", 1);
            _notify.PlayNotification("ゲームクリアの証");
        }
    }
}
