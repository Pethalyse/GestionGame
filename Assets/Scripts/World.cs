using TMPro;
using UnityEngine;

public class World : MonoBehaviour
{

    //Date depuis le debut du monde
    private static float annees;
    private static float mois;
    private static float jours;
    private float heure_en_jour; //0.1 - 0.99;
    private static float heures;
    private static float minutes;
    private static float secondes;
    private static float secondesModulo60;

    //components
    [SerializeField] private TextMeshProUGUI horaire;

    private void Start()
    {
        annees = 0;
        mois = 0;
        jours = 0;
        heures = 0;
        minutes = 0;
        secondes = 0;

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            Config_world.acceleration(1);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            Config_world.acceleration(2);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            Config_world.acceleration(3);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            Config_world.acceleration(4);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            Config_world.acceleration(5);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            Config_world.acceleration(6);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            Config_world.acceleration(7);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            Config_world.acceleration(8);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            Config_world.acceleration(9);
        }

        secondes = Mathf.Floor(Time.time / (Config_world.getMinute_conf() / 60f));
        secondesModulo60 = secondes % 60;

        float min, h, j, m;

        min = Mathf.Floor(secondes / 60);
        h = Mathf.Floor(secondes / (60 * 60));
        j = Mathf.Floor(secondes / (60 * 60 * 24));
        m = Mathf.Floor(secondes / (60 * 60 * 24 * (365 / 12)));

        minutes = Mathf.Floor((secondes - 3600 * h) / 60);
        heures = Mathf.Floor((secondes - 86400 * j) / (60*60));
        heure_en_jour = heures / 24;
        jours = Mathf.Floor((secondes - (60 * 60 * 24 * (365 / 12)) * m) / (60 * 60 * 24));
        mois = Mathf.Floor((secondes - 31536000 * annees) / (60 * 60 * 24 * (365/12)));

        annees = Mathf.Floor(secondes / (60 * 60 * 24 * 365));

        horaire.text = "" + annees + ":" + mois + ":" + jours + ":" + heures + ":" + minutes + ":" + secondesModulo60;


    }

    public static float getSecondes() => secondes;
    public static float getMinutes() => minutes;
    public static float getHeures() => heures;
    public static float getJours() => jours;
    public static float getMois() => mois;
    public static float getAnnee() => annees;

}
