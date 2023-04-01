using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FootPrintGenerator : MonoBehaviour
{
    public GameObject footPrintPrefab;
    float time = 0;
    private EnemyStatus _status;
    private NavMeshAgent _agent;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>(); // NavMeshAgent‚ğ•Û‚µ‚Ä‚¨‚­
        _status = GetComponent<EnemyStatus>();
    }

    void Update()
    {
        //UŒ‚’†‚â“®‚¢‚Ä‚¢‚È‚¢‚Í‘«Õ‚ğ¶¬‚µ‚È‚¢
        if (!_status.IsMovable)
        {
            return;
        }
        if (_agent.velocity.magnitude == 0)
        {
            return;
        }

            this.time += Time.deltaTime;
        if (this.time > 0.15f)
        {
            this.time = 0;
            Instantiate(footPrintPrefab, transform.position, transform.rotation);
        }
    }
}