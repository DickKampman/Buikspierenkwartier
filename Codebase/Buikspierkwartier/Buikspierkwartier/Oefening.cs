using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buikspierkwartier
{
    public class Oefening
    {
        public string Naam;
        public string Houding;
        public string oefening;
        public string Link;
        public Spieren Spieren;
        public string Categorie;
        public string Reps_Easy, Reps_Medium, Reps_Hard;

        public Oefening(string naam, string houding, string oefening, string link, string Categorie, string spieren, string reps_easy, string reps_medium, string reps_hard)
        {
            this.Naam = naam;
            this.Houding = houding;
            this.oefening = oefening;
            this.Link = link;
            this.Categorie = Categorie;
            Enum.TryParse(spieren, out this.Spieren);
            this.Reps_Easy = reps_easy;
            this.Reps_Medium = reps_medium;
            this.Reps_Hard = reps_hard;
        }

        public string GetReps(Niveau niveau)
        {
            switch (niveau)
            {
                case Niveau.Easy: return Reps_Easy;
                case Niveau.Medium: return Reps_Medium;
                case Niveau.Hard: return Reps_Hard;
                default:
                    var random = new Random();
                    var r = random.Next(0, 3);
                    if (r == 0) return Reps_Easy;
                    if (r == 1) return Reps_Medium;
                    else return Reps_Hard;
            }
        }

        public override string ToString()
        {
            return String.Format("Naam: \t\t{0}\nOmschrijving:\t{1}\nSpieren:\t{2}\nMakkelijk:\t{3}\nGemiddeld:\t{4}\nZwaar:\t\t{5}\n", Naam, Houding, Spieren, Reps_Easy, Reps_Medium, Reps_Hard);
        }

    }

    public enum Spieren { Buik, Rug}
}
