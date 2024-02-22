using System.Collections.Generic;

public class SpeciesDescriptions
{
    public string CommunName;
    public string ScientificName;
    public List<string> descriptionsText;

    public SpeciesDescriptions(string name)
    {
        CommunName = name;
        ScientificName = "";
        descriptionsText = new List<string>();

    }

    public static SpeciesDescriptions GenerateCarrots()
    {

        SpeciesDescriptions carrots = new SpeciesDescriptions("Carrote")
        {
            ScientificName = "Carotus carotus"
        };
        carrots.descriptionsText.Add("Une carrote qui pousse sur un rocher, étrange");
        carrots.descriptionsText.Add("Cette espèce à l'air adapté à la vie très proche de l'océan");
        carrots.descriptionsText.Add("Il semble qu'il faille que plusieurs rocher soient proche pour que les carottes poussent");

        return carrots;
    }

    public static SpeciesDescriptions GenerateShell()
    {

        SpeciesDescriptions shell = new SpeciesDescriptions("Coquillage")
        {
            ScientificName = "Coquillatus coquillus"
        };
        shell.descriptionsText.Add("");
        shell.descriptionsText.Add("");

        return shell;
    }

}