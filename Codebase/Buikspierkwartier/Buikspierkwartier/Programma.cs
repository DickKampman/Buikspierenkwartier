using iTextSharp.text;
using iTextSharp.text.pdf;
using Landoefeningen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buikspierkwartier
{
    public class Programma
    {
        List<Programma_item> Oefeningen;

        public Programma()
        {
            Oefeningen = new List<Programma_item>();
        }

        public void AddOefening(string spieren, string niveau)
        {
            Spieren spierencat;
            Enum.TryParse(spieren, out spierencat);
            Niveau Niveau = Helper.GetNiveau(niveau);
            List<Oefening> templist;

            switch (Niveau)
            {
                case Niveau.Easy: templist = Program.Oefeningen.FindAll(x => x.Spieren == spierencat && x.Reps_Easy != "-" && !this.Oefeningen.Select(y => y.Oefening).Contains(x) && this.Oefeningen.Count(y => y.Oefening.Categorie.Equals(x.Categorie)) <=1);
                    break;
                case Niveau.Medium: templist = Program.Oefeningen.FindAll(x => x.Spieren == spierencat && x.Reps_Medium != "-" && !this.Oefeningen.Select(y => y.Oefening).Contains(x) && this.Oefeningen.Count(y => y.Oefening.Categorie.Equals(x.Categorie)) <= 1);
                    break;
                case Niveau.Hard: templist = Program.Oefeningen.FindAll(x => x.Spieren == spierencat && x.Reps_Hard != "-" && !this.Oefeningen.Select(y => y.Oefening).Contains(x) && this.Oefeningen.Count(y => y.Oefening.Categorie.Equals(x.Categorie)) <= 1);
                    break;
                default: templist = Program.Oefeningen.FindAll(x => x.Spieren == spierencat);
                    break;
            }
            var random = new Random().Next(0, templist.Count);
            
            this.Oefeningen.Add(new Programma_item(templist[random], Niveau));
        }

        public void ProgrammaToPdf()
        {
            FileStream fs = new FileStream("Chapter1_Example1.pdf", FileMode.Create, FileAccess.Write, FileShare.None);
            Document doc = new Document(new Rectangle(PageSize.A4.Rotate()));
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            doc.Open();

            doc.Add(new Paragraph("Buikspierenkwartier"));
            iTextSharp.text.Image myImage = iTextSharp.text.Image.GetInstance("ZVH.jpg");
            myImage.ScaleAbsolute(120f, 120f);
            PdfPTable table = new PdfPTable(5);
            PdfPCell cell = new PdfPCell(myImage);
            cell.Colspan = 5;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.AddCell(cell);
            foreach(var oefening in this.Oefeningen)
            {
                table.AddCell(oefening.Oefening.Naam);
                table.AddCell(oefening.Oefening.Houding);
                table.AddCell(oefening.Oefening.oefening);
                table.AddCell(oefening.Oefening.GetReps(oefening.Niveau));
                table.AddCell(oefening.Oefening.Link);

            }
            doc.Add(table);

            doc.Close();
        }

        public override string ToString()
        {
            string result = "";
            foreach(var oefening in Oefeningen)
            {
                result += oefening.ToString();
            }
            return result;
        }

    }

    public class Programma_item
    {
        public Oefening Oefening;
        public Niveau Niveau;

        public Programma_item(Oefening oefening, Niveau niveau)
        {
            this.Niveau = niveau;
            this.Oefening = oefening;
        }

        public override string ToString()
        {
            return String.Format("Naam: \t\t{0}\nOmschrijving:\t{1}\nReps:\t\t{2}\n", Oefening.Naam, Oefening.Houding, Oefening.GetReps(Niveau));
        }
    }


}
