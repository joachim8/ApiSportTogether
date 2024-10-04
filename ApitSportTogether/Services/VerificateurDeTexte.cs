namespace ApiSportTogether.Services
{
    public class VerificateurDeTexte
    {
        private readonly List<string> motsSexistes = new List<string>
        {
            "femmelette", "ménagère", "hystérique", "poufiasse", "gonzesse", "salope", "boniche",
            "pute", "nymphomane", "bimbo", "poule", "machiste", "péripatéticienne", "chauvin",
            "pisseuse", "cruche", "frustrée", "dinde", "vieux garçon", "pédé"
        };

        private readonly List<string> motsRacistes = new List<string>
        {
            "nègre", "bougnoule", "youpin", "chinetoque", "jaune", "raton", "toubab", "niafou",
            "blédard", "gitane", "macaque", "métèque", "feuj", "reubeu", "chleuh", "bicot",
            "zoulou", "peau-rouge", "nippon", "tching-tchong", "gueule noire", "roumi","pd","lesbienne"
        };

        public (bool isClean, List<string> motsTrouves) VerifierTexte(string texte)
        {
            var motsDansLeTexte = texte.ToLower().Split(new[] { ' ', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

            var motsTrouves = new List<string>();

            // Vérifier chaque liste
            motsTrouves.AddRange(motsDansLeTexte.Where(m => motsSexistes.Contains(m)));
            motsTrouves.AddRange(motsDansLeTexte.Where(m => motsRacistes.Contains(m)));

            bool isClean = motsTrouves.Count == 0;

            return (isClean, motsTrouves);
        }
    }
}
