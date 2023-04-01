using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetClothing : MonoBehaviour
{
    //頭と体
    [SerializeField] GameObject skin_head;
    [SerializeField] GameObject skin_body;

    //髭
    [SerializeField] GameObject[] beards;

    //髪型
    [SerializeField] GameObject[] hairs;

    //帽子とネックレス
    [SerializeField] GameObject[] caps;
    [SerializeField] GameObject[] chains;

    //各種職業の服と帽子
    [SerializeField] GameObject banker_suit;

    [SerializeField] GameObject cock_suit;
    [SerializeField] GameObject cock_suit_hat;

    [SerializeField] GameObject farmer_suit;
    [SerializeField] GameObject farmer_suit_hat;

    [SerializeField] GameObject fireman_suit;
    [SerializeField] GameObject fireman_suit_hat;

    [SerializeField] GameObject mechanic_suit;
    [SerializeField] GameObject mechanic_suit_hat;

    [SerializeField] GameObject nurse_suit;

    [SerializeField] GameObject police_suit;
    [SerializeField] GameObject police_suit_hat;

    [SerializeField] GameObject roober_suit;
    [SerializeField] GameObject roober_suit_hat;

    [SerializeField] GameObject security_guard_suit;
    [SerializeField] GameObject security_guard_suit_hat;

    [SerializeField] GameObject seller_suit;

    [SerializeField] GameObject worker_suit;
    [SerializeField] GameObject worker_suit_hat;

    [SerializeField] GameObject glasses;
    [SerializeField] GameObject jacket;
    [SerializeField] GameObject pullover;
    [SerializeField] GameObject scarf;
    [SerializeField] GameObject shirt;

    //靴
    [SerializeField] GameObject[] pairShoes;

    [SerializeField] GameObject shortpants;
    [SerializeField] GameObject t_shirt;
    [SerializeField] GameObject tank_top;
    [SerializeField] GameObject trousers;

    //それぞれの物の色
    [SerializeField] Texture[] skin_textures;

    [SerializeField] Texture[] beard_textures;

    [SerializeField] Texture[] hair_a_textures;
    [SerializeField] Texture[] hair_b_textures;
    [SerializeField] Texture[] hair_c_textures;
    [SerializeField] Texture[] hair_d_textures;
    [SerializeField] Texture[] hair_e_textures;

    [SerializeField] Texture[] cap_textures;
    [SerializeField] Texture[] cap2_textures;
    [SerializeField] Texture[] cap3_textures;
    [SerializeField] Texture[] chain1_textures;
    [SerializeField] Texture[] chain2_textures;
    [SerializeField] Texture[] chain3_textures;

    [SerializeField] Texture[] banker_suit_texture;

    [SerializeField] Texture cock_suit_texture;

    [SerializeField] Texture farmer_suit_texture;

    [SerializeField] Texture fireman_suit_texture;

    [SerializeField] Texture mechanic_suit_texture;

    [SerializeField] Texture nurse_suit_texture;

    [SerializeField] Texture police_suit_texture;

    [SerializeField] Texture roober_suit_texture;

    [SerializeField] Texture security_guard_suit_texture;

    [SerializeField] Texture seller_suit_texture;

    [SerializeField] Texture worker_suit_texture;

    [SerializeField] Texture[] glasses_texture;
    [SerializeField] Texture[] jacket_textures;
    [SerializeField] Texture[] pullover_textures;
    [SerializeField] Texture[] scarf_textures;
    [SerializeField] Texture[] shirt_textures;

    [SerializeField] Texture[] shoes1_textures;
    [SerializeField] Texture[] shoes2_textures;
    [SerializeField] Texture[] shoes3_textures;

    [SerializeField] Texture[] shortpants_textures;
    [SerializeField] Texture[] t_shirt_textures;
    [SerializeField] Texture[] tank_top_textures;
    [SerializeField] Texture[] trousers_textures;

    bool hat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(int num)
    {
        //服をセット　渡す引数で役割が変わる　0で一般人,1で警察,2で職業持ち,3で泥棒,4でお金持ち
        StartCoroutine(SetCloth(num));
    }

    private IEnumerator SetCloth(int role)
    {
        hat = true;

        for(int i = 0; i < 5; i++)
        {
            hairs[i].SetActive(false);
        }

        for (int i = 0; i < 4; i++)
        {
            beards[i].SetActive(false);
        }

        for (int i = 0; i < 3; i++)
        {
            caps[i].SetActive(false);
            chains[i].SetActive(false);
        }

        banker_suit.SetActive(false);

        cock_suit.SetActive(false);
        cock_suit_hat.SetActive(false);

        farmer_suit.SetActive(false);
        farmer_suit_hat.SetActive(false);

        fireman_suit.SetActive(false);
        fireman_suit_hat.SetActive(false);

        mechanic_suit.SetActive(false);
        mechanic_suit_hat.SetActive(false);

        nurse_suit.SetActive(false);

        police_suit.SetActive(false);
        police_suit_hat.SetActive(false);

        roober_suit.SetActive(false);
        roober_suit_hat.SetActive(false);

        security_guard_suit.SetActive(false);
        security_guard_suit_hat.SetActive(false);

        seller_suit.SetActive(false);

        worker_suit.SetActive(false);
        worker_suit_hat.SetActive(false);

        glasses.SetActive(false);

        jacket.SetActive(false);

        pullover.SetActive(false);

        scarf.SetActive(false);

        shirt.SetActive(false);

        for (int i = 0; i < 3; i++)
        {
            pairShoes[i].SetActive(false);
        }

        shortpants.SetActive(false);

        t_shirt.SetActive(false);

        tank_top.SetActive(false);

        trousers.SetActive(false);
        

        // 肌の色を設定
        int skin_color = UnityEngine.Random.Range(0, 6);

        skin_head.GetComponent<Renderer>().materials[0].mainTexture = skin_textures[skin_color];
        skin_body.GetComponent<Renderer>().materials[0].mainTexture = skin_textures[skin_color];

        // 男女を設定
        int male_female = UnityEngine.Random.Range(0, 2);

        // 髪の色を設定
        int hairColor = UnityEngine.Random.Range(0, 3);    // 0 = dark  1 = brown  2 = blonde
        

        // 男
        if (male_female == 0)
        {
            hat = true;

            // choose hair type   hair_a , hair_b  , hair_e
            int hair = UnityEngine.Random.Range(0, 3);
            hairs[hair].SetActive(true);

            int hair_cut = 0;
            switch (hair)
            {
                case 0:
                    // 0 = full hair    1 = under cut
                    hair_cut = UnityEngine.Random.Range(0, 2);
                    hat = true;
                    break;
                case 1:
                    // 0 = full hair    1 = under cut
                    hair_cut = UnityEngine.Random.Range(0, 2);
                    hat = false;
                    break;
                case 2:
                    hat = false;
                    break;
                default:
                    Debug.Log("髪の設定ミス");
                    break;
            }
            
            if (hair == 0)
            {
                if (hairColor == 0)
                {
                    hairs[0].GetComponent<Renderer>().materials[0].mainTexture = hair_a_textures[hair_cut];
                }
                if (hairColor == 1)
                {
                    hairs[0].GetComponent<Renderer>().materials[0].mainTexture = hair_a_textures[hair_cut + 2];
                }
                if (hairColor == 2)
                {
                    hairs[0].GetComponent<Renderer>().materials[0].mainTexture = hair_a_textures[hair_cut + 4];
                }
            }

            if (hair == 1)
            {
                if (hairColor == 0)
                {
                    if (hair_cut == 0)
                    {
                        hairs[1].GetComponent<Renderer>().materials[0].mainTexture = hair_b_textures[0];
                    }
                    if (hair_cut == 1)
                    {
                        hairs[1].GetComponent<Renderer>().materials[0].mainTexture = hair_b_textures[5];
                    }
                }
                if (hairColor == 1)
                {
                    if (hair_cut == 0)
                    {
                        hairs[1].GetComponent<Renderer>().materials[0].mainTexture = hair_b_textures[1];
                    }
                    if (hair_cut == 1)
                    {
                        hairs[1].GetComponent<Renderer>().materials[0].mainTexture = hair_b_textures[3];
                    }
                }
                if (hairColor == 2)
                {
                    if (hair_cut == 0)
                    {
                        hairs[1].GetComponent<Renderer>().materials[0].mainTexture = hair_b_textures[2];
                    }
                    if (hair_cut == 1)
                    {
                        hairs[1].GetComponent<Renderer>().materials[0].mainTexture = hair_b_textures[4];
                    }
                }
            }

            if (hair == 2)
            {
                hairs[4].GetComponent<Renderer>().materials[0].mainTexture = hair_e_textures[hairColor];
            }
        }
        // 女性
        if (male_female == 1)
        {
            hat = false;

            // 髪の設定   hair_c , hair_d
            int hair = UnityEngine.Random.Range(2, 4);
            hairs[hair].SetActive(true);
            switch (hair)
            {
                case 2:
                    hairs[hair].GetComponent<Renderer>().materials[0].mainTexture = hair_c_textures[hairColor];
                    break;
                case 3:
                    hairs[hair].GetComponent<Renderer>().materials[0].mainTexture = hair_d_textures[hairColor];
                    break;
                default:
                    Debug.Log("髪の設定ミス");
                    break;
            }
        }

        // 髭の設定
        if (male_female == 0)
        {
            int percent = UnityEngine.Random.Range(0, 100);

            if (percent > 0 && percent < 50)
            {
                // 髭無し
            }
            else
            {
                int beardNum = 0;
                if (percent > 50 && percent < 70)
                {
                    // beard a
                    beardNum = 0;
                }
                if (percent > 70 && percent < 80)
                {
                    // beard b
                    beardNum = 1;
                }
                if (percent > 80 && percent < 90)
                {
                    // beard c
                    beardNum = 2;
                }
                if (percent > 90 && percent < 100)
                {
                    // beard d
                    beardNum = 3;
                }
                beards[beardNum].SetActive(true);
                beards[beardNum].GetComponent<Renderer>().materials[0].mainTexture = beard_textures[hairColor];
            }
        }

        // 役割に応じた服の設定
        switch (role)
        {
            case 0:
                //一般人
                int shoes = UnityEngine.Random.Range(0, 3);

                pairShoes[shoes].SetActive(true);

                int shoes_texture = UnityEngine.Random.Range(0, 8 - shoes);
                switch (shoes)
                {
                    case 0:
                        pairShoes[shoes].GetComponent<Renderer>().materials[0].mainTexture = shoes1_textures[shoes_texture];
                        break;
                    case 1:
                        pairShoes[shoes].GetComponent<Renderer>().materials[0].mainTexture = shoes2_textures[shoes_texture];
                        break;
                    case 2:
                        pairShoes[shoes].GetComponent<Renderer>().materials[0].mainTexture = shoes3_textures[shoes_texture];
                        break;
                    default:
                        Debug.Log("靴の設定ミス");
                        break;
                }

                int glasses_percentage = UnityEngine.Random.Range(0, 100);

                if (glasses_percentage < 20)
                {
                    glasses.SetActive(true);

                    int texture_choose = UnityEngine.Random.Range(0, 6);

                    glasses.GetComponent<Renderer>().materials[0].mainTexture = glasses_texture[texture_choose];
                }

                int chain = UnityEngine.Random.Range(0, 3);
                chains[chain].SetActive(true);

                if (chain == 0)
                {
                    int textures = UnityEngine.Random.Range(0, 4);
                    chains[chain].GetComponent<Renderer>().materials[0].mainTexture = chain1_textures[textures];
                }
                if (chain == 1)
                {
                    int textures = UnityEngine.Random.Range(0, 3);
                    chains[chain].GetComponent<Renderer>().materials[0].mainTexture = chain2_textures[textures];
                }
                if (chain == 2)
                {
                    int textures = UnityEngine.Random.Range(0, 3);
                    chains[chain].GetComponent<Renderer>().materials[0].mainTexture = chain3_textures[textures];
                }

                int scarfPercentage = UnityEngine.Random.Range(0, 100);

                if (scarfPercentage < 20)
                {
                    scarf.SetActive(true);

                    int textures = UnityEngine.Random.Range(0, 11);

                    scarf.GetComponent<Renderer>().materials[0].mainTexture = scarf_textures[textures];
                }

                int which_trouser = UnityEngine.Random.Range(0, 2);

                // trousers
                if (which_trouser == 0)
                {
                    trousers.SetActive(true);

                    int texture = UnityEngine.Random.Range(0, 15);

                    trousers.GetComponent<Renderer>().materials[0].mainTexture = trousers_textures[texture];
                }
                // short pants
                if (which_trouser == 1)
                {
                    shortpants.SetActive(true);

                    int texture = UnityEngine.Random.Range(0, 11);

                    shortpants.GetComponent<Renderer>().materials[0].mainTexture = shortpants_textures[texture];
                }

                // upper bosy cloth :   0 = pullover  1 = shirt    2 = t_shirt    3 = tanktop
                int upper_cloth = UnityEngine.Random.Range(0, 4);

                if (upper_cloth == 0)
                {
                    pullover.SetActive(true);

                    int texture = UnityEngine.Random.Range(0, 17);

                    pullover.GetComponent<Renderer>().materials[0].mainTexture = pullover_textures[texture];
                }

                if (upper_cloth == 1)
                {
                    shirt.SetActive(true);

                    int texture = UnityEngine.Random.Range(0, 14);

                    shirt.GetComponent<Renderer>().materials[0].mainTexture = shirt_textures[texture];
                }
                if (upper_cloth == 2)
                {
                    t_shirt.SetActive(true);

                    int texture = UnityEngine.Random.Range(0, 21);

                    t_shirt.GetComponent<Renderer>().materials[0].mainTexture = t_shirt_textures[texture];
                }
                if (upper_cloth == 3)
                {
                    tank_top.SetActive(true);

                    int texture = UnityEngine.Random.Range(0, 11);

                    tank_top.GetComponent<Renderer>().materials[0].mainTexture = tank_top_textures[texture];
                }
                break;
            case 1:
                //警察
                police_suit.SetActive(true);

                police_suit.GetComponent<Renderer>().materials[0].mainTexture = police_suit_texture;

                if (hat)
                {
                    police_suit_hat.SetActive(true);

                    police_suit_hat.GetComponent<Renderer>().materials[0].mainTexture = police_suit_texture;
                }
                break;
            case 2:
                //職業持ち
                int which_suit = UnityEngine.Random.Range(1, 8);

                // cocksuit      1
                // farmersuit    2
                // firemansuit   3
                // mechanicsuit  4
                // nursesuit     5
                // workersuit    6
                // sellersuit    7

                // cock suit
                if (which_suit == 1)
                {
                    cock_suit.SetActive(true);

                    cock_suit.GetComponent<Renderer>().materials[0].mainTexture = cock_suit_texture;

                    if (hat)
                    {
                        cock_suit_hat.SetActive(true);
                        cock_suit_hat.GetComponent<Renderer>().materials[0].mainTexture = cock_suit_texture;
                    }
                }
                // farmer suit
                if (which_suit == 2)
                {
                    farmer_suit.SetActive(true);

                    farmer_suit.GetComponent<Renderer>().materials[0].mainTexture = farmer_suit_texture;

                    if (hat)
                    {
                        farmer_suit_hat.SetActive(true);
                        farmer_suit_hat.GetComponent<Renderer>().materials[0].mainTexture = farmer_suit_texture;
                    }
                }
                // fireman suit
                if (which_suit == 3)
                {
                    fireman_suit.SetActive(true);

                    fireman_suit.GetComponent<Renderer>().materials[0].mainTexture = fireman_suit_texture;

                    if (hat)
                    {
                        fireman_suit_hat.SetActive(true);

                        fireman_suit_hat.GetComponent<Renderer>().materials[0].mainTexture = fireman_suit_texture;
                    }
                }
                // mechanic suit
                if (which_suit == 4)
                {
                    mechanic_suit.SetActive(true);

                    mechanic_suit.GetComponent<Renderer>().materials[0].mainTexture = mechanic_suit_texture;

                    if (hat)
                    {
                        mechanic_suit_hat.SetActive(true);

                        mechanic_suit_hat.GetComponent<Renderer>().materials[0].mainTexture = mechanic_suit_texture;
                    }
                }
                // nurse suit
                if (which_suit == 5)
                {
                    nurse_suit.SetActive(true);

                    nurse_suit.GetComponent<Renderer>().materials[0].mainTexture = nurse_suit_texture;


                }
                // worker suit
                if (which_suit == 6)
                {
                    worker_suit.SetActive(true);

                    worker_suit.GetComponent<Renderer>().materials[0].mainTexture = worker_suit_texture;

                    if (hat)
                    {
                        worker_suit_hat.SetActive(true);

                        worker_suit_hat.GetComponent<Renderer>().materials[0].mainTexture = worker_suit_texture;
                    }
                }
                // seller suit
                if (which_suit == 7)
                {
                    seller_suit.SetActive(true);

                    seller_suit.GetComponent<Renderer>().materials[0].mainTexture = seller_suit_texture;
                }
                break;
            case 3:
                //泥棒
                roober_suit.SetActive(true);
                roober_suit.GetComponent<Renderer>().materials[0].mainTexture = roober_suit_texture;
                roober_suit_hat.SetActive(true);
                roober_suit_hat.GetComponent<Renderer>().materials[0].mainTexture = roober_suit_texture;

                for (int i = 0; i < 5; i++)
                {
                    hairs[i].SetActive(false);
                }
                for (int i = 0; i < 4; i++)
                {
                    beards[i].SetActive(false);
                }
                break;
            case 4:
                //お金持ち
                int which_suits = UnityEngine.Random.Range(0, 2);

                // bankersuit    0
                // securitysuit  1

                // banker suit
                if (which_suits == 0)
                {
                    banker_suit.SetActive(true);

                    int which_texture = UnityEngine.Random.Range(0, 7);


                    banker_suit.GetComponent<Renderer>().materials[0].mainTexture = banker_suit_texture[which_texture];
                }
                // security guard suit
                if (which_suits == 1)
                {
                    security_guard_suit.SetActive(true);

                    security_guard_suit.GetComponent<Renderer>().materials[0].mainTexture = security_guard_suit_texture;

                    if (hat)
                    {
                        security_guard_suit_hat.SetActive(true);

                        security_guard_suit_hat.GetComponent<Renderer>().materials[0].mainTexture = security_guard_suit_texture;
                    }
                }
                break;
            default:
                Debug.Log("役割の数字が違います");
                break;
        }
        yield break;
    }
}
