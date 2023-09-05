using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class villageois : MonoBehaviour
{

    private int vieMax = 100;
    private int vie;

    private NavMeshAgent navigation;
    private GameObject target;

    [SerializeField] private GameObject lit;

    private void Awake()
    {
        navigation = GetComponent<NavMeshAgent>();
        if (!navigation)
        {
            gameObject.AddComponent<NavMeshAgent>();
            navigation = GetComponent<NavMeshAgent>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        vie = vieMax;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            navigation.SetDestination(lit.transform.position);
            SetTarget(lit);
        }

        if (target)
        {
            if(Vector3.Distance(transform.position, target.transform.position) < 2)
            {
                navigation.SetDestination(transform.position);
            }
        }
    }

    public GameObject getTarget() { return target; }
    public void SetTarget(GameObject target) {  this.target = target; }
}
