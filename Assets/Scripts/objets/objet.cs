public abstract class Objet
{
    protected int valeur;
    protected string nom;

    public int getValeur() { return valeur; }
    public string getNom() { return nom; }

    public override string ToString()
    {
        return nom + " : " + valeur;
    }

}