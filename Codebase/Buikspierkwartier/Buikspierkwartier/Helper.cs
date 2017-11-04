using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buikspierkwartier
{
    public static class Helper
    {
        public static Niveau GetNiveau(String niveau)
        {
            switch (niveau)
            {
                case "Easy": return Niveau.Easy;
                case "Medium": return Niveau.Medium;
                case "Hard": return Niveau.Hard;
                default: return Niveau.Random;
            }
        }
    }

    public enum Niveau { Easy, Medium, Hard, Random }
}
