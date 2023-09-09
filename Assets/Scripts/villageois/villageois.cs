using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Villageois : MonoBehaviour
{

    //informations
    private string nom;
    private int sexe;
    private int age = 0; //mois
    private float ageTime = 0;
    private float mortAgeTime = 0;

    private Etat state;
    private int timeur;
    private float onActionTime;

    //actions
    private float heureDodo = 22;
    private float tempsDodoMin = 8;
    private float tempsDodoMax = 12;

    //stats
    private int vieMax = 100;
    private int vie;

    private int faim = 100;
    private int faimMax = 100;
    private float faimTime = 0;
    private float cdFaim = 6*Config_world.getHeure_conf();
    private float histoFaim = 0;

    private int soif = 100;
    private int soifMax = 100;
    private float soifTime = 0;
    private float cdSoif = Config_world.getHeure_conf();
    private float histoSoif = 0;

    private Dictionary<Villageois, float> affinites = new Dictionary<Villageois, float>();


    //components
    private NavMeshAgent navigation;
    private GameObject target;

    
    //objets
    [SerializeField] private GameObject lit;
    private List<Objet> inventaire = new List<Objet>();

    private enum Etat
    {
        Endormi,
        Reveille,
        EnAction,
        AuTravaille,
        Mort
    }

    private void Awake()
    {
        navigation = GetComponent<NavMeshAgent>();
        if (!navigation)
        {
            gameObject.AddComponent<NavMeshAgent>();
            navigation = GetComponent<NavMeshAgent>();
        }

        if(Random.value >= 0.5)
        {
            sexe = 1;
            nom = "Gwen";
        }
        else
        {
            sexe = 0;
            nom = "Fabre";
        }

        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    // Start is called before the first frame update
    void Start()
    {
        vie = vieMax;

        state = Etat.Reveille;

        inventaire.Add(new Boisson(50, "Gourde fraiche"));
    }

    // Update is called once per frame
    void Update()
    {

        //MORT
        if (vie == 0)
        {
            die();
        }

        //MORT AGE
        if (age / 12 > 75 && (Mathf.Round((Time.time - mortAgeTime) * 1000) / 1000 / Config_world.getJour_conf() > 1))
        {
            mortAgeTime = Time.time;

            switch (sexe)
            {
                case 1:
                    {

                        if (age < 95)
                        {
                            if (Random.value < age / 12 * 100 / 110 - 45)
                            {
                                die();
                            }
                        }
                        else
                        {
                            if (Random.value < age / 12 * 100 / 110)
                            {
                                die();
                            }
                        }
                        break;
                    }
                case 0:
                    {
                        if (age < 90)
                        {
                            if (Random.value < age / 12 * 100 / 110 - 35)
                            {
                                die();
                            }
                        }
                        else
                        {
                            if (Random.value < age / 12 * 100 / 110)
                            {
                                die();
                            }
                        }
                        break;
                    }
            }

        }

        //GESTION AGE
        if (Mathf.Round((Time.time - ageTime) * 1000) / 1000 / Config_world.getMois_conf() > 1)
        {
            age++;
            ageTime = Time.time;
        }

        //GESTION FAIM
        //GESTION SOIF
        if (state == Etat.Endormi)
        {
            if (Mathf.Round((Time.time - faimTime) * 1000) / 1000 / Config_world.getMinute_conf() > 30 && faim > 0)
            {
                addFaim(-1);
                faimTime = Time.time;
            }

            if (Mathf.Round((Time.time - soifTime) * 1000) / 1000 / (Config_world.getMinute_conf()) > 10 && soif > 0)
            {
                addSoif(-1);
                soifTime = Time.time;
            }
        }
        else
        {
            if (Mathf.Round((Time.time - faimTime) * 1000) / 1000 / Config_world.getMinute_conf() > 15 && faim > 0)
            {
                addFaim(-1);
                faimTime = Time.time;
            }

            if (Mathf.Round((Time.time - soifTime) * 1000) / 1000 / (Config_world.getMinute_conf()) > 5 && soif > 0)
            {
                addSoif(-1);
                soifTime = Time.time;
            }
        }

        //PERTE DE VIE -> FAIM
        if (faim == 0 && Mathf.Round((Time.time - faimTime) * 1000) / 1000 / Config_world.getHeure_conf() > 1)
        {
            addVie(-1);
            faimTime = Time.time;
        }
        //PERTE DE VIE -> SOIF
        if (soif == 0 && Mathf.Round((Time.time - soifTime) * 1000) / 1000 / Config_world.getHeure_conf() > 1)
        {
            addVie(-1);
            soifTime = Time.time;
        }

        actions();

    }

    private void actions()
    {
        if (state != Etat.EnAction && state != Etat.AuTravaille)
        {

            //VA DORMIR
            if (World.getHeures() == heureDodo && state == Etat.Reveille)
            {
                navigation.SetDestination(lit.transform.position);
                setTarget(lit);
            }

            //REVEILLE
            if (state == Etat.Endormi && World.getHeures() == heureDodo + Mathf.Round(Random.Range(tempsDodoMin, tempsDodoMax)))
            {
                state = Etat.Reveille;
            }

            //MANGER
            if (state == Etat.Reveille && Mathf.Abs(histoFaim - Time.time) >= cdFaim && faimMax - faim >= 30)
            {
                manger();
            }

            //BOIRE
            if (state == Etat.Reveille && Mathf.Abs(histoSoif - Time.time) >= cdSoif && soifMax - soif >= 30)
            {
                boire();
            }

            if (target)
            {
                if (Vector3.Distance(transform.position, target.transform.position) < 2)
                {
                    //S'ENDORT
                    if (target == lit)
                    {
                        state = Etat.Endormi;
                    }
                    setTarget(null);
                    navigation.SetDestination(transform.position);
                }
            }

        }
        else
        {
            if (Mathf.Round(Time.time) - onActionTime == timeur)
            {
                finAction();
            }
        }

    }

    protected void die()
    {
        Debug.Log("mort");
        state = Etat.Mort;
    }

    public GameObject getTarget() { return target; }
    public void setTarget(GameObject target) { this.target = target; }

    public int getAge() { return age; }
    public int getFaim() { return faim; }
    public int getSoif() { return soif; }
    public int getVie() { return vie; }

    public void addFaim(int f) { faim += f; }
    public void addSoif(int s) { soif += s; }
    public void addVie(int v) { vie += v; }

    protected void enAction(int a, int m, int j, int h, int min, int s) 
    { 
        state = Etat.EnAction;
        timeur = s + min * 60 + h * 60 * 60 + j * 60 * 60 * 24 + m * 60 * 60 * 24 * (365 / 12) + a * 60 * 60 * 24 * 12;
        onActionTime = Mathf.Round(Time.time);
    }
    protected void finAction() { state = Etat.Reveille; }

    public void donnerA(Villageois a, Utilisable obj)
    {
        if (inventaire.Contains(obj))
        {
            inventaire.Remove(obj);
            a.recevoir(obj);
        }
        
    }

    public void recevoir(Utilisable obj)
    {
        inventaire.Add(obj);
    }

    protected void manger()
    {
        Nourriture nourriture = null;
        foreach (Nourriture item in inventaire)
        {
            if (item.GetType() == typeof(Nourriture))
            {
                if (nourriture == null)
                {
                    nourriture = item;
                }
                else
                {
                    if (item.getValeur() > nourriture.getValeur())
                    {
                        nourriture = item;
                    }
                    else if (item.getValeur() == faimMax - faim)
                    {
                        nourriture = item;
                        break;
                    }
                }
            }
        }

        if(nourriture != null)
        {
            inventaire.Remove(nourriture);
            faim += nourriture.consommation();
            if(faim > 100) { faim = 100; }
            histoFaim = Time.time;
            enAction(0, 0, 0, 0, 0, 1);

            Debug.Log(this + ", a manger " + nourriture);
        }
        else
        {
            Debug.Log(this + ", n'a pas pu manger");
        }
        
    }

    protected void boire()
    {
        Boisson boisson = null;
        foreach(Boisson item in inventaire)
        {
            if(item.GetType() == typeof(Boisson))
            {
                if(boisson == null)
                {
                    boisson = item;
                }
                else
                {
                    if(item.getValeur() > boisson.getValeur())
                    {
                        boisson = item;
                    }
                    else if (item.getValeur() == soifMax - soif)
                    {
                        boisson = item;
                        break;
                    }
                }
            }
        }

        if(boisson != null)
        {
            inventaire.Remove(boisson);
            soif += boisson.consommation();
            if(soif > 100) { soif = 100; }
            histoSoif = Time.time;
            enAction(0, 0, 0, 0, 0, 1);

            Debug.Log(this + ", a bu " + boisson);
        }
        else
        {
            Debug.Log(this + ", n'a pas pu boire");
        }
        
    }

    public void parlerA(Villageois interlocuteur)
    {
        interlocuteur.parlerA(this);
        enAction(0, 0, 0, 0, 1, 0);

        if (affinites.ContainsKey(interlocuteur))
        {
            affinites[interlocuteur] += 1;
        }
    }

    public override string ToString()
    {
        return nom;
    }
}
