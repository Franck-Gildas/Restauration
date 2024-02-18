/*
        Programmeur(s):      Mohamed ESSANHAJI,
                             Alioune sarr,
                             Mbengue El Hadji Cisse,
                             Franck Gildas M. K.

        Date:                Novembre

        Solution:            Restauration.sln
        Projet:              Types.csproj
        Classe:              Types.cs
        
        Namespace:           {TypesnNS}

        But:                 •	Créer et manipuler les tableaux à une et à deux dimensions sous .Net. 
                             •	Créer et documenter les membres de la classe de l'objet métier Transaction (Couche Métier / Business Tier - Bibliothèque de classes Transaction.Dll).
                             •	Créer un objet métier de la classe Types et s'en servir à partir du formulaire initial de la couche Présentation (Restauration.Exe).
                             •	Utiliser le diagramme hiérarchique remis pour écrire le code.


        Info:                Couche de métier.    
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypesNS
{
    /// <summary>
    /// Objet métier : Classe de Types
    /// La classe Types encapsule les règles métier et opérations.
    /// </summary>
    /// <remarks>Tous droits réservés : Franky Gildas Inc. 2035</remarks>

    public class Types
    {
        #region Énumération
        /// <summary>
        /// Énumération des types de données.
        /// </summary>
        public enum CodeTypes
        {
            TypeCuisine,
            TypePlat
        }

        #endregion

        #region Déclaration des tableaux

        private string[] tTypeCuisine;
        private string[] tTypePlat;

        #endregion

        #region Constructeur pour la classe Types.
        /// <summary>
        /// Constructeur pour la classe Types. Initialise les types de plat et les cuisine.
        /// </summary>
        public Types() 
        {
            InitTypeCuisine();
            InitTypePlat();
        }

        #endregion

        #region Initialisation

        /// <summary>
        /// Initialise le tableau types de cuisines
        /// </summary>
        private void InitTypeCuisine()
        {
            tTypeCuisine = new string[] { "Africaine", "Italienne", "Japonaise" };
        }

        /// <summary>
        /// Initialise le tableau types de plats.
        /// </summary>
        private void InitTypePlat()
        {
            tTypePlat = new string[] { "Entrée", "Principal", "Dessert" };
        }

        #endregion

        #region Obtenir les types de cuisine ou plat

        /// <summary>
        /// Obtient les types de cuisine ou plat en fonction de la valeur CodesTypes fournie.
        /// </summary>
        /// <param name="types">Valeur CodesTypes indiquant s'il faut obtenir des types de plat ou de cuisine.</param>
        /// <returns>Un tableau de chaînes de types de cuisine ou de plat</returns>
        /// <exception cref="ArgumentOutOfRangeException">Déclenché lorsque la valeur CodesTypes fournie n’est pas valide.</exception>
        public string[] GetTypes(CodeTypes types)
        {
            switch(types)
            {
                case CodeTypes.TypeCuisine:
                    return tTypeCuisine;
                case CodeTypes.TypePlat:
                    return tTypePlat;
                default:
                    throw new ArgumentOutOfRangeException(nameof(types), "Type invalide. Il doit s'agir de Types de cuisine ou plat.");
            }
        }

        #endregion
    }
}
