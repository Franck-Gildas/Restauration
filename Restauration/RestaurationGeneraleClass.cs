/*
        Programmeur(s):      Mohamed ESSANHAJI,
                             Alioune sarr,
                             Mbengue El Hadji Cisse,
                             Franck Gildas M. K.

        Date:                Novembre

        Solution:            Restauration.sln
        Projet:              Restauration.csproj
        Classe:              RestaurationGeneraleClass.cs

        But:                 •  Créer un tableau de messages d'erreurs


        Info:                Couche de présentation (Classe générale).    
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ce = Restauration.RestaurationGeneraleClass.CodeErreurs;

namespace Restauration
{
    /// <summary>
    /// Classe générale de la couche de présentation
    /// </summary>
    internal class RestaurationGeneraleClass
    {
        #region Énumération

        /// <summary>
        /// Énumération pour les messages d'erreurs
        /// </summary>
        public enum CodeErreurs
        {
            CEIndexOutOfRange,
            CENullReference,
            CECastingInvalid,
            CEArgumentException,
            CEErreurChargementFichier,
            CEErreurInconnue
        }

        #endregion

        #region Messages d'erreurs

        public static string[] tMessagesErreursStr = new string[6];

        /// <summary>
        /// Initialiser les messages d'erreurs
        /// </summary>
        public static void InitMessagesErreurs()
        {
            tMessagesErreursStr[(int)ce.CEIndexOutOfRange] = "Selection invalide. Veuillez sélectionner un repas et un accompagnement valides.";
            tMessagesErreursStr[(int)ce.CENullReference] = "Une erreur s'est produite lors du traitement de votre demande. Veuillez réessayer.";
            tMessagesErreursStr[(int)ce.CECastingInvalid] = "Entrée invalide. S'il vous plait, entrez un nombre valide.";
            tMessagesErreursStr[(int)ce.CEArgumentException] = "Argument invalide. Veuillez saisir les informations dans le format correct.";
            tMessagesErreursStr[(int)ce.CEErreurChargementFichier] = "Impossible de charger le fichier ou l'assembly.";
            tMessagesErreursStr[(int)ce.CEErreurInconnue] = "Erreur inconue. Veillez contacter le bureau d'aide.";
        }

        #endregion
    }
}
