
public sealed class Config_world
{
    private static Config_world instance;

    //Time.time en seconde
    private float annee_conf;
    private float mois_conf;
    private float jour_conf; //10 minutes
    private float demi_jour_conf; //5 minutes
    private float minute_conf; //0.41 secondes
    private float heure_conf; //24.6 secondes

    private float _jour;

    private Config_world(float j)
    {
        jour_conf = j;
        demi_jour_conf = jour_conf / 2f;
        heure_conf = jour_conf / 24f;
        annee_conf = jour_conf * 365f;
        mois_conf = annee_conf / 12f;
        minute_conf = heure_conf / 60f;

        _jour = jour_conf;
    }

    private static Config_world getInstance()
    {
        if (instance == null)
        {
            instance = new(600);
        }

        return instance;
    }

    public static float getJour_conf() => getInstance().jour_conf;
    public static float getDemi_jour_conf() => getInstance().demi_jour_conf;
    public static float getHeure_conf() => getInstance().heure_conf;
    public static float getMinute_conf() => getInstance().minute_conf;
    public static float getAnnee_conf() => getInstance().annee_conf;
    public static float getMois_conf() => getInstance().mois_conf;
    public static float get_jour() => getInstance()._jour;

    public void newInstance(float j) { instance = new(j); }

    public static void acceleration(int i) => getInstance().newInstance(getInstance()._jour / i);
}
