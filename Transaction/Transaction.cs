/*
        Programmeur(s):      Mohamed ESSANHAJI,
                             Alioune sarr,
                             Mbengue El Hadji Cisse,
                             Franck Gildas M. K.

        Date:                Novembre

        Solution:            Restauration.sln
        Projet:              Transaction.csproj
        Classe:              Transaction.cs
        
        Namespace:           {TransactionNS}

        But:                 •	Créer et manipuler les tableaux à une et à deux dimensions sous .Net. 
                             •	Créer et documenter les membres de la classe de l'objet métier Transaction (Couche Métier / Business Tier - Bibliothèque de classes Transaction.Dll).
                             •	Créer un objet métier de la classe Transaction et s'en servir à partir du formulaire initial de la couche Présentation (VentesPneus.Exe).
                             •	Utiliser le diagramme hiérarchique remis pour écrire le code.
                             •	Insérer le diagramme de classe pour chaque projet.
                             •  Représenter une transaction avec un identifiant, un nom, une date de livraison et un prix, et etc.
                             •  Créer une propriété pour chaque attribut de la transaction de facturation
                             •  Valider les données 


        Info:                Couche de métier.    
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using TransactionNS;

namespace TransactionNS
{
    /// <summary>
    /// Objet métier : Classe de transaction (commande)
    /// La classe Transaction encapsule toutes les règles métier et opérations traitant de la transaction.
    /// Représente une transaction avec un identifiant, un nom, une date de livraison et un prix, et etc.
    /// </summary>
    /// <remarks>Tous droits réservés : Franky Gildas Inc. 2035</remarks>
    public class Transaction
    {
        #region Déclaration des tableaux

        private string[] tNomRepas;
        private string[] tAccompagnement;
        private decimal[,] tPrix;

        #endregion

        #region Variables Privées

        private int idInt;
        private string nomStr;
        private string prenomStr;
        private string adresseStr;
        private string codePostalStr;
        private string telephoneStr;
        private string typeCuisineStr;
        private string typePlatStr;
        private DateTime dateLivraisonDateTime;
        private decimal prixDecimal;
        private string repasStr;
        private string accompagnementStr;
        private DateTime datePaiementDateTime;

        #endregion

        #region Variable publique pour le numero de la transacion

        public static int NumeroTransaction { get; private set; } = 0;

        #endregion

        #region Constantes pour les expressions regulières 

        // Code postal Canadien
        private const string codePostalModel = @"^([A-Za-z]\d[A-Za-z][-]?\d[A-Za-z]\d)$";

        // Numéro de téléphone Nord-Américain
        private const string telephoneModel = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";

        #endregion

        #region Expressions régulières pour la validation

        private static Regex codePostalRegex = new Regex(codePostalModel);
        private static Regex telephoneRegex = new Regex(telephoneModel);

        #endregion

        #region Déclaration des énumérations, tableaux d’erreurs

        private enum CodesErreurs
        {
            NomObligatoire,
            PrenomObligatoire,
            AdresseObligatoire, 
            CodePostalObligatoire,
            CodePostalInvalid,
            TelephoneObligatoire,
            TelephoneInvalid,
            TypeCuisineObligatoire,
            TypePlatObligatoire,
            DateLivraisonInvalide,
            RepasObligatoire,
            RepasInvalid,
            AccompagnementObligatoire,
            AccompagnementInvalid,
            PrixCorrespondant,
            PrixPositive,
            ErreurInconnue
        }

        #endregion

        #region Messages d'erreurs

        private string[] tMessagesErreurs = new string[17];

        private void InitMessagesErreurs()
        {
            tMessagesErreurs[(int)CodesErreurs.NomObligatoire] = "Le nom est obligatoire.";
            tMessagesErreurs[(int)CodesErreurs.PrenomObligatoire] = "Le prenom est obligatoire.";
            tMessagesErreurs[(int)CodesErreurs.AdresseObligatoire] = "L'adresse est obligatoire.";
            tMessagesErreurs[(int)CodesErreurs.CodePostalObligatoire] = "Le code postal est obligatoire.";
            tMessagesErreurs[(int)CodesErreurs.CodePostalInvalid] = "Le code postal est invalid. Veuillez entrer un autre.";
            tMessagesErreurs[(int)CodesErreurs.TelephoneObligatoire] = "Le cellulaire est obligatoire.";
            tMessagesErreurs[(int)CodesErreurs.TelephoneInvalid] = "Le numéro de téléphone est invalid. Veuillez entrer un autre.";
            tMessagesErreurs[(int)CodesErreurs.TypeCuisineObligatoire] = "Le type de cuisine est obligatoire.";
            tMessagesErreurs[(int)CodesErreurs.TypePlatObligatoire] = "Le type de plat est obligatoire.";
            tMessagesErreurs[(int)CodesErreurs.DateLivraisonInvalide] = "La date de livraison est invalide.";
            tMessagesErreurs[(int)CodesErreurs.RepasObligatoire] = "Le repas est obligatoire.";
            tMessagesErreurs[(int)CodesErreurs.RepasInvalid] = "Le repas est invalide.";
            tMessagesErreurs[(int)CodesErreurs.AccompagnementObligatoire] = "L'accompagnement est obligatoire.";
            tMessagesErreurs[(int)CodesErreurs.AccompagnementInvalid] = "L'accompagnement est invalide.";
            tMessagesErreurs[(int)CodesErreurs.PrixCorrespondant] = "Le prix ne correspond pas aux repas et l'accompagnement.";
            tMessagesErreurs[(int)CodesErreurs.PrixPositive] = "Le prix doit être un nombre positif.";
            tMessagesErreurs[(int)CodesErreurs.ErreurInconnue] = "Une erreur inconnue est subvenue lor du chargement. Veillez contactez RestoManager.";
        }

        #endregion

        #region Propriétés

        /// <summary>
        /// Obtient l'ID de la transaction.
        /// </summary>
        public int ID
        {
            get { return idInt; }
            private set { idInt = value; } //L'identifiant est en lecture seule
        }

        /// <summary>
        /// Obtient ou définit le nom associé à la transaction.
        /// </summary>
        public string Nom
        {
            get { return nomStr; }
            set 
            {
                if (value != null)
                {
                    value = value.Trim();

                    if (value != string.Empty)
                        nomStr = value;
                    else
                        throw new ArgumentException(tMessagesErreurs[(int)CodesErreurs.NomObligatoire]);
                }
                else
                    throw new ArgumentNullException(tMessagesErreurs[(int)CodesErreurs.NomObligatoire]);

            }
        }

        /// <summary>
        /// Obtient ou définit le prénom associé à la transaction.
        /// </summary>
        public string Prenom
        {
            get { return prenomStr; }
            set 
            {
                if (value != null)
                {
                    value = value.Trim();

                    if (value != string.Empty)
                        prenomStr = value;
                    else
                        throw new ArgumentException(tMessagesErreurs[(int)CodesErreurs.PrenomObligatoire]);
                }
                else
                    throw new ArgumentNullException(tMessagesErreurs[(int)CodesErreurs.PrenomObligatoire]);
            }
        }

        /// <summary>
        /// Obtient ou définit l'adresse associé à la transaction.
        /// </summary>
        public string Adresse
        {
            get { return adresseStr; }
            set 
            { 
                if (value != null)
                {
                    value = value.Trim();

                    if (value != string.Empty)
                        adresseStr = value;
                    else
                        throw new ArgumentException(tMessagesErreurs[(int)CodesErreurs.AdresseObligatoire]);
                }
                else
                {
                    throw new ArgumentNullException(tMessagesErreurs[(int)CodesErreurs.AdresseObligatoire]);
                }
            }
        }

        /// <summary>
        /// Obtient ou définit le code postal associé à la transaction.
        /// </summary>
        public string CodePostal
        {
            get { return codePostalStr; }
            set 
            { 
                if (value != null)
                {
                    value = value.Trim();

                    if (value != string.Empty)
                    {
                        if (codePostalRegex.IsMatch(value))
                            codePostalStr = value;
                        else
                            throw new ArgumentException(tMessagesErreurs[(int)CodesErreurs.CodePostalInvalid]);
                    }
                    else
                        throw new ArgumentException(tMessagesErreurs[(int)CodesErreurs.CodePostalObligatoire]);
                }
                else
                {
                    throw new ArgumentNullException(tMessagesErreurs[(int)CodesErreurs.CodePostalObligatoire]);
                }
            }
        }

        /// <summary>
        /// Obtient ou définit l'adresse téléphonique associé à la transaction.
        /// </summary>
        public string Telephone
        {
            get { return telephoneStr; }
            set 
            {
                if (value != null)
                {
                    value = value.Trim();

                    if (value != string.Empty)
                    {
                        if (telephoneRegex.IsMatch(value))
                            telephoneStr = value;
                        else
                            throw new ArgumentException(tMessagesErreurs[(int)CodesErreurs.TelephoneInvalid]);
                    }
                    else
                        throw new ArgumentException(tMessagesErreurs[(int)CodesErreurs.TelephoneObligatoire]);
                }
                else
                {
                    throw new ArgumentNullException(tMessagesErreurs[(int)CodesErreurs.TelephoneObligatoire]);
                }
            }
        }

        /// <summary>
        /// Obtient ou définit le type de cuisine associé à la transaction.
        /// </summary>
        public string TypeCuisine
        {
            get { return typeCuisineStr; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();

                    if (value != string.Empty)
                        typeCuisineStr = value;
                    else
                        throw new ArgumentException(tMessagesErreurs[(int)CodesErreurs.TypeCuisineObligatoire]);
                }
                else
                {
                    throw new ArgumentNullException(tMessagesErreurs[(int)CodesErreurs.TypeCuisineObligatoire]);
                }
            }
        }

        /// <summary>
        /// Obtient ou définit le type de plat associé à la transaction.
        /// </summary>
        public string TypePlat
        {
            get { return typePlatStr; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();

                    if (value != string.Empty)
                        typePlatStr = value;
                    else
                        throw new ArgumentException(tMessagesErreurs[(int)CodesErreurs.TypePlatObligatoire]);
                }
                else
                {
                    throw new ArgumentNullException(tMessagesErreurs[(int)CodesErreurs.TypePlatObligatoire]);
                }
            }
        }

        /// <summary>
        /// Obtient ou définit la date de livraison associé à la transaction.
        /// </summary>
        public DateTime DateLivraison
        {
            get { return dateLivraisonDateTime; }
            set 
            { 
                if (value >= DateTime.Now.AddDays(-15) && value <= DateTime.Now.AddDays(15))
                {
                    dateLivraisonDateTime = value;
                    datePaiementDateTime = value.AddDays(30);
                }
                else
                {
                    throw new ArgumentOutOfRangeException(tMessagesErreurs[(int)CodesErreurs.DateLivraisonInvalide]);
                }
                //dateLivraisonDateTime = value; 
            }
        }

        /// <summary>
        /// Obtient ou définit le prix associé à la transaction.
        /// </summary>
        public decimal Prix
        {
            get { return prixDecimal; }
            set 
            { 
                if (value > 0)
                {
                    if (!string.IsNullOrEmpty(repasStr) && !string.IsNullOrEmpty(accompagnementStr))
                    {
                        InitNomRepas();
                        InitAccompagnement();
                        InitPrix();

                        int indexRepas = Array.IndexOf(tNomRepas, repasStr.Trim());
                        int indexAccompagnement = Array.IndexOf(tAccompagnement, accompagnementStr.Trim());

                        if (tPrix[indexRepas, indexAccompagnement] == value)
                            prixDecimal = value;
                        else
                            throw new ArgumentException(tMessagesErreurs[(int)CodesErreurs.PrixCorrespondant]);
                    }
                    else
                    {
                        throw new ArgumentNullException(tMessagesErreurs[(int)CodesErreurs.RepasObligatoire] + " et " +
                                                        tMessagesErreurs[(int)CodesErreurs.AccompagnementObligatoire]);
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException(tMessagesErreurs[(int)CodesErreurs.PrixPositive]);
                }
            }
        }

        /// <summary>
        /// Obtient ou définit le repas associé à la transaction.
        /// </summary>
        public string Repas
        {
            get { return repasStr; }
            set
            {
                if (value != null)
                {
                    InitNomRepas();
                    value = value.Trim();

                    if (Array.IndexOf(tNomRepas, value) != -1)
                        repasStr = value;
                    else
                        throw new ArgumentOutOfRangeException(tMessagesErreurs[(int)CodesErreurs.RepasInvalid]);
                }
                else
                    throw new ArgumentNullException(tMessagesErreurs[(int)CodesErreurs.RepasObligatoire]);             
            }
        }

        /// <summary>
        /// Obtient ou définit l'accompagnement associé à la transaction.
        /// </summary>
        public string Accompagnement
        {
            get { return accompagnementStr; }
            set
            {
                if (value != null)
                {
                    InitAccompagnement();
                    value = value.Trim();

                    if (Array.IndexOf(tAccompagnement, value) != -1)
                        accompagnementStr = value;
                    else
                        throw new ArgumentOutOfRangeException(tMessagesErreurs[(int)CodesErreurs.AccompagnementInvalid]);
                }
                else
                    throw new ArgumentNullException(tMessagesErreurs[(int)CodesErreurs.AccompagnementObligatoire]);
            }
        }

        /// <summary>
        /// Obtient la date de paiement associé à la transaction.
        /// </summary>
        public DateTime DatePaiement
        {
            get { return datePaiementDateTime; }
        }

        #endregion

        #region Constructeur par paramètres

        /// <summary>
        /// Initialise une nouvelle instance de la classe Transaction avec le nom, prénom, adresse, code postal, téléphone, la date de livraison et le prix spécifiés.
        /// </summary>
        /// <param name="nomPrinc">Le nom associé à la transaction.</param>
        /// <param name="prenomPrinc">Le prénom associé à la transaction.</param>
        /// <param name="adressePrinc">L'adresse associé à la transaction.</param>
        /// <param name="codePostalPrinc">Le code postal associé à la transaction.</param>
        /// <param name="telephonePrinc">L'adresse téléphonique associé à la transaction.</param>
        /// <param name="typeCuisinePrinc">Le type de cuisine associé à la transaction.</param>
        /// <param name="typePlatPrinc">Le type de plat associé à la transaction.</param>
        /// <param name="dateLivraisonPrinc">La date de livraison associé à la transaction.</param>
        /// <param name="repasPrinc">Le repas associé à la transaction.</param>
        /// <param name="accompagnementPrinc">L'accompagnement associé à la transaction.</param>
        /// <param name="prixPrinc">Le prix associé à la transaction.</param>
        public Transaction(string nomPrinc, string prenomPrinc, string adressePrinc, string codePostalPrinc, string telephonePrinc, string typeCuisinePrinc, string typePlatPrinc, DateTime dateLivraisonPrinc, string repasPrinc, string accompagnementPrinc, decimal prixPrinc)
        {
            ID = NumeroTransaction;
            Nom = nomPrinc;
            Prenom = prenomPrinc;
            Adresse = adressePrinc;
            CodePostal = codePostalPrinc;
            Telephone = telephonePrinc;
            TypeCuisine = typeCuisinePrinc;
            TypePlat = typePlatPrinc;
            DateLivraison = dateLivraisonPrinc;
            Repas = repasPrinc;
            Accompagnement = accompagnementPrinc;
            Prix = prixPrinc;
        }

        #endregion

        #region Constructeur par défaut
        /// <summary>
        /// Initialise une nouvelle instance de la classe Transaction.
        /// </summary>
        public Transaction()
        {
            InitNomRepas();
            InitAccompagnement();
            InitPrix();
            InitMessagesErreurs();
        }

        #endregion

        #region Méthodes Enregistrer

        // Enregistrer() sans paramètre : Pour sauvegarder l’information dans un fichier (ultérieurement)

        /// <summary>
        /// Enregistre les données de transaction.
        /// </summary>
        public void Enregistrer()
        {
            Console.WriteLine(Nom, Prenom, Adresse, CodePostal, Telephone, TypeCuisine, TypePlat, DateLivraison, Repas, Accompagnement, Prix);

            NumeroTransaction++;
        }

        // Enregistrer() avec paramètre : 3ième façon de transmettre des données Projet principal  --> Projet Transaction

        /// <summary>
        /// Enregistre les données de transaction avec le nom, prénom, adresse, code postal, adresse téléphonique, la date de livraison et le prix spécifiés.
        /// </summary>
        /// <param name="nomPrinc">Le nom associé à la transaction.</param>
        /// <param name="prenomPrinc">Le prénom associé à la transaction.</param>
        /// <param name="adressePrinc">L'adresse associé à la transaction.</param>
        /// <param name="codePostalPrinc">Le code postal associé à la transaction.</param>
        /// <param name="telephonePrinc">L'adresse téléphonique associé à la transaction.</param>
        /// <param name="typeCuisinePrinc">Le type de cuisine associé à la transaction.</param>
        /// <param name="typePlatPrinc">Le type de plat associé à la transaction.</param>
        /// <param name="dateLivraisonPrinc">La date de livraison associé à la transaction.</param>
        /// <param name="repasPrinc">Le repas associé à la transaction.</param>
        /// <param name="accompagnementPrinc">L'accompagnement associé à la transaction.</param>
        /// <param name="prixPrinc">Le prix associé à la transaction.</param>
        public void Enregistrer(string nomPrinc, string prenomPrinc, string adressePrinc, string codePostalPrinc, string telephonePrinc, string typeCuisinePrinc, string typePlatPrinc, DateTime dateLivraisonPrinc, string repasPrinc, string accompagnementPrinc, decimal prixPrinc)
        {
            Nom = nomPrinc;
            Prenom = prenomPrinc;
            Adresse = adressePrinc;
            CodePostal = codePostalPrinc;
            Telephone = telephonePrinc;
            TypeCuisine = typeCuisinePrinc;
            TypePlat = typePlatPrinc;
            DateLivraison = dateLivraisonPrinc;
            Repas = repasPrinc;
            Accompagnement = accompagnementPrinc;
            Prix = prixPrinc;

            Enregistrer();
        }

        #endregion

        #region Initialisation des tableaux : Méthodes privées

        /// <summary>
        ///  Initialiser les repas
        /// </summary>
        private void InitNomRepas()
        {
            tNomRepas = new string[] { "Poulet rôti", "Lasagnes", "Ramen" };
        }

        /// <summary>
        /// Initialiser les accompagnements des repas
        /// </summary>
        private void InitAccompagnement()
        {
            tAccompagnement = new string[] { "Sauce gravy", "Pain à l’ail", "Œuf à la coque" };
        }

        /// <summary>
        /// Initialiser les prix
        /// </summary>
        private void InitPrix()
        {
            tPrix = new decimal[,] { { 20.99M, 22.78M, 21.88M }, { 25.50M, 26.90M, 27.99M }, { 15.90M, 16.10M, 17.99M } };
        }

        #endregion

        #region Méthodes publiques : Récupérer le nom du repas, l'accompagnement, et le prix

        #region Nom de repas

        /// <summary>
        /// Obtient les noms des repas.
        /// </summary>
        /// <returns>Les repas</returns>
        public string[] ObtenirNomRepas()
        {
            return tNomRepas;
        }

        #endregion

        #region Accompagnements

        /// <summary>
        /// Obtient les Accompagnements.
        /// </summary>
        /// <returns>les Accompagnements.</returns>
        public string[] ObtenirAccompagnement()
        {
            return tAccompagnement;
        }

        #endregion

        #region Prix du repas

        /// <summary>
        /// Obtient le prix pour un repas et un accompagnement donnés.
        /// </summary>
        /// <param name="repasInt">Index du repas.</param>
        /// <param name="accompagnementInt">Index de l'accompagnement.</param>
        /// <returns>Le prix du repas.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Lancé lorsque repasInt ou accompagnementInt est hors de portée.</exception>
        public decimal ObtenirPrix(int repasInt, int accompagnementInt)
        {
            if (repasInt < 0 || repasInt >= tNomRepas.Length)
            {
                throw new ArgumentOutOfRangeException("repasInt");
            }

            if (accompagnementInt < 0 || accompagnementInt >= tAccompagnement.Length)
            {
                throw new ArgumentOutOfRangeException("accompagnementInt");
            }

            return tPrix[repasInt, accompagnementInt];
        }

        /// <summary>
        /// Obtient le prix pour un repas et un accompagnement donnés.
        /// </summary>
        /// <param name="repasString">Index du repas.</param>
        /// <param name="accompagnementString">Index de l'accompagnement.</param>
        /// <returns>Le prix du repas.</returns>
        /// <exception cref="System.ArgumentException">Lancé lorsque le repas ou l'accompagnement n'est pas trouvé dans les tableaux respectifs.</exception>
        public decimal ObtenirPrix(string repasString, string accompagnementString)
        {
            int indexRepas = Array.IndexOf(tNomRepas, repasString);
            int indexAccompagnement = Array.IndexOf(tAccompagnement, accompagnementString);

            if (indexRepas < 0)
            {
                throw new ArgumentException("Repas invalid.", "repasString");
            }

            if (indexAccompagnement < 0)
            {
                throw new ArgumentException("Accompagnement invalid", "accompagnementString");
            }

            return tPrix[indexRepas, indexAccompagnement];
        }

        #endregion

        #endregion
    }
}
