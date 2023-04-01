using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemy : MonoBehaviour
{
    [SerializeField] GameObject[] SmallCar;
    [SerializeField] GameObject[] MediumCar;
    [SerializeField] GameObject[] BigCar;

    [SerializeField] Material[] ChangeMat;

    [SerializeField] Transform[] generatePos;
    private float currentPos;

    private bool sppedUp1 = false;
    private bool sppedUp2 = false;

    private bool isSpecialGenerate = false;
    private int SpecialProbability = 50;

    private bool isGenerate = false;

    private int check = 0;

    private float minusPosX;

    void Start()
    {
        StartCoroutine(generate());
        StartCoroutine(FirstGenerate());
    }

    private void Update()
    {
        if (check % 60 != 0)
        {
            check++; return;
        }
        currentPos = generatePos[0].position.x;
        if (currentPos < 3000f) return;
        if (!sppedUp1)
            sppedUp1 = true;
        if (currentPos < 6000f) return;
        if (!isSpecialGenerate)
            isSpecialGenerate = true;
        if (currentPos < 12000f) return;
        if (!sppedUp2)
            sppedUp2 = true;
        if (currentPos < 15000f) return;
        if (SpecialProbability != 25)
            SpecialProbability = 25;
        if (currentPos < 24000f) return;
        if (!isGenerate)
        {
            isGenerate = true;
            StartCoroutine(generate2());
        }
    }

    private IEnumerator generate()
    {
        yield return null;
        var wait = new WaitForSeconds(0.5f);
        int ranPos, ranSize, ranGene;
        while (true)
        {
            ranPos = Random.Range(0, 10);
            if (ranPos >= generatePos.Length)
                ranPos = Random.Range(0, generatePos.Length / 2);

            ranSize = Random.Range(0, 10);
            if (ranSize < 5)
                ranSize = 0;
            else if (ranSize < 8)
                ranSize = 1;
            else
                ranSize = 2;

            generateEnemyCar(ranPos, ranSize);

            //特殊生成
            if (isSpecialGenerate)
            {
                ranGene = Random.Range(0, SpecialProbability);
                if (ranGene == 0)
                {
                    int type = Random.Range(0, 2);
                    if (type == 0)
                        StartCoroutine(SpecialGenerate1());
                    else
                        StartCoroutine(SpecialGenerate2());
                }
            }

            yield return wait;
        }        
    }

    private IEnumerator generate2()
    {
        yield return null;
        var wait = new WaitForSeconds(1.1f);
        int ranPos, ranSize, ranGene;
        while (true)
        {
            ranPos = Random.Range(0, 10);
            if (ranPos >= generatePos.Length)
                ranPos = Random.Range(0, generatePos.Length / 2);

            ranSize = Random.Range(0, 10);
            if (ranSize < 5)
                ranSize = 0;
            else if (ranSize < 8)
                ranSize = 1;
            else
                ranSize = 2;

            generateEnemyCar(ranPos, ranSize);

            //特殊生成
            if (isSpecialGenerate)
            {
                ranGene = Random.Range(0, SpecialProbability);
                if (ranGene == 0)
                {
                    int type = Random.Range(0, 2);
                    if (type == 0)
                        StartCoroutine(SpecialGenerate1());
                    else
                        StartCoroutine(SpecialGenerate2());
                }
            }

            yield return wait;
        }
    }

    private void generateEnemyCar(int pos,int size)
    {
        int ranLook, ranMat;
        Vector3 rotation = new Vector3(1, 0, 0);
        float angle = 0;
        Vector3 position = generatePos[pos].localPosition;
        position.x -= minusPosX;
        GameObject ins = gameObject;
        MeshRenderer renderer;
        Material[] materials;

        if (pos < 4)
            angle = 0;
        else
            angle = 180;

        switch (size)
        {
            case 0:
                ranLook = Random.Range(0, 71);
                if (ranLook < 6)
                    ranLook = 0;
                else if (ranLook < 12)
                    ranLook = 1;
                else if (ranLook < 18)
                    ranLook = 2;
                else if (ranLook < 24)
                    ranLook = 3;
                else if (ranLook < 34)
                    ranLook = 4;
                else if (ranLook < 44)
                    ranLook = 5;
                else if (ranLook < 45)
                    ranLook = 6;
                else if (ranLook < 58)
                    ranLook = 7;
                else
                    ranLook = 8;

                ins = Instantiate(SmallCar[ranLook], position, Quaternion.AngleAxis(angle, rotation), transform);
            
                if (ranLook < 4)
                    ranMat = Random.Range(0, 6);
                else if (ranLook < 6)
                    ranMat = Random.Range(6, 16);
                else if (ranLook == 6)
                    ranMat = 0;
                else
                    ranMat = Random.Range(16, 29);
                renderer = ins.GetComponentInChildren<MeshRenderer>();
                materials = renderer.sharedMaterials;
                materials[0] = ChangeMat[ranMat];
                renderer.sharedMaterials = materials;
                break;
            case 1:
                ranLook = Random.Range(0, 27);
                if (ranLook < 6)
                    ranLook = 0;
                else if (ranLook < 12)
                    ranLook = 1;
                else if (ranLook < 25)
                    ranLook = 2;
                else if (ranLook < 26)
                    ranLook = 3;
                else
                    ranLook = 4;

                ins = Instantiate(MediumCar[ranLook], position, Quaternion.AngleAxis(angle, rotation), transform);

                if (ranLook < 2)
                    ranMat = Random.Range(0, 6);
                else if (ranLook < 3)
                    ranMat = Random.Range(6, 16);
                else if (ranLook < 4)
                    ranMat = 29;
                else
                    ranMat = 24;
                renderer = ins.GetComponentInChildren<MeshRenderer>();
                materials = renderer.sharedMaterials;
                materials[0] = ChangeMat[ranMat];
                renderer.sharedMaterials = materials;
                break;
            case 2:
                ranLook = Random.Range(0, 49);
                if (ranLook < 6)
                    ranLook = 0;
                else if (ranLook < 12)
                    ranLook = 1;
                else if (ranLook < 18)
                    ranLook = 2;
                else if (ranLook < 31)
                    ranLook = 3;
                else if (ranLook < 37)
                    ranLook = 4;
                else if (ranLook < 43)
                    ranLook = 5;
                else
                    ranLook = 6;

                ins = Instantiate(BigCar[ranLook], position, Quaternion.AngleAxis(angle, rotation), transform);

                if (ranLook < 3)
                    ranMat = Random.Range(0, 6);
                else if (ranLook < 4)
                    ranMat = Random.Range(16, 29);
                else
                    ranMat = Random.Range(0, 6);
                renderer = ins.GetComponentInChildren<MeshRenderer>();
                materials = renderer.sharedMaterials;
                materials[0] = ChangeMat[ranMat];
                renderer.sharedMaterials = materials;
                break;
        }

        if (sppedUp1)
            ins.GetComponent<EnemyMove>().speed += 0.05f;
        if (sppedUp2)
            ins.GetComponent<EnemyMove>().speed += 0.1f;
    }

    private IEnumerator SpecialGenerate1()
    {
        var wait = new WaitForSeconds(0.4f);
        int ran = Random.Range(3, 6);
        int size, pos;

        pos = Random.Range(0, 10);
        if (pos >= generatePos.Length)
            pos = Random.Range(0, generatePos.Length / 2);

        yield return null;
        for (int i = 0; i < ran; i++)
        {
            if (i == 0)
                size = 2;
            else
                size = Random.Range(0, 2);

            generateEnemyCar(pos, size);
            yield return wait;
        }

        yield break;
    }

    private IEnumerator SpecialGenerate2()
    {
        var wait = new WaitForSeconds(0.4f);
        int ran = Random.Range(3, 4);
        int size, pos;

        pos = Random.Range(0, 4);
        if (pos == 1)
            pos = 3;
        else if (pos == 2)
            pos = 4;
        else if (pos == 3)
            pos = 7;

        yield return null;
        size = Random.Range(0, 3);
        for (int i = 0; i < ran; i++)
        {
            if (pos == 0 || pos == 4)
                pos++;
            else
                pos--;
            generateEnemyCar(pos, size);
            yield return wait;
        }

        yield break;
    }

    private IEnumerator FirstGenerate()
    {
        int ranPos, ranSize;
        minusPosX = 125f;
        for(int i = 0; i < 9; i++)
        {
            ranPos = Random.Range(0, 10);
            if (ranPos >= generatePos.Length)
                ranPos = Random.Range(0, generatePos.Length / 2);

            ranSize = Random.Range(0, 10);
            if (ranSize < 5)
                ranSize = 0;
            else if (ranSize < 8)
                ranSize = 1;
            else
                ranSize = 2;

            generateEnemyCar(ranPos, ranSize);

            minusPosX -= 12.5f;
            yield return null;
        }
        yield return null;
        minusPosX = 0;

        yield break;
    }

    public void GenerateEnemyCar(int pos, int size)
    {
        generateEnemyCar(pos, size);
    }
}
