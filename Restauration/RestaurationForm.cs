/*
        Programmeur(s):      Franck Gildas M. K.
                             Mohamed ESSANHAJI,
                             Alioune Sarr,
                             Mbengue El Hadji Cisse,
                             

        Date:                Novembre

        Solution:            Restauration.sln
        Projet:              Restauration.csproj
        Classe:              RestaurationForm.cs

        But:                 •	Créer et manipuler les tableaux à une et à deux dimensions sous .Net. 
                             •	Créer et documenter les membres de la classe de l'objet métier Transaction (Couche Métier / Business Tier - Bibliothèque de classes Transaction.Dll).
                             •	Créer un objet métier de la classe Transaction et s'en servir à partir du formulaire initial de la couche Présentation (VentesPneus.Exe).
                             •	Utiliser le diagramme hiérarchique remis pour écrire le code.
                             •	Insérer le diagramme de classe pour chaque projet.
                             •	Valider les données 
                             •	Se servir de la structure DateTime pour manipuler les dates en .Net. 
                             •	Valider les dates 



        Info:                Couche de présentation.    
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TransactionNS;
using TypesNS;

using ce = Restauration.RestaurationGeneraleClass.CodeErreurs;
using g = Restauration.RestaurationGeneraleClass;

namespace Restauration
{
    /// <summary>
    /// La classe RestaurationForm est la classe principale de la couche de présentation.
    /// Saisie une commande (transaction)
    /// </summary>
    /// <remarks>Tous droits réservés : Franky G. 2035</remarks>
    public partial class RestaurationForm : Form
    {
        #region Déclaration

        private Transaction oTrans;
        private Types oTypes;

        #endregion

        #region Constructeur
        public RestaurationForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Initialisation

        private void RestaurationForm_Load(object sender, EventArgs e)
        {
            // Initialisation des messages d'erreur
            g.InitMessagesErreurs();

            try
            {
                // Créer une nouvelle instance de Transaction
                oTrans = new Transaction();

                // Remplir le repasComboBox
                repasComboBox.Items.AddRange(oTrans.ObtenirNomRepas());
                repasComboBox.SelectedIndex = 0;

                // Remplir l'accompagnementComboBox
                accompagnementComboBox.Items.AddRange(oTrans.ObtenirAccompagnement());
                accompagnementComboBox.SelectedIndex = 0;
            }
            catch(Exception)
            {
                MessageBox.Show(g.tMessagesErreursStr[(int)ce.CENullReference]);
            }

            ChargerTypesCuisinePlat();
        }

        #endregion

        #region Méthodes privées

        private void ChargerTypesCuisinePlat()
        {
            try
            {
                oTypes = new Types();

                // Remplir les ComboBox
                typeCuisineComboBox.Items.AddRange(oTypes.GetTypes(Types.CodeTypes.TypeCuisine));
                typePlatComboBox.Items.AddRange(oTypes.GetTypes(Types.CodeTypes.TypePlat));
            }
            catch (ArgumentOutOfRangeException) 
            {
                MessageBox.Show(g.tMessagesErreursStr[(int)ce.CEIndexOutOfRange]);
            }
            catch (Exception)
            {
                MessageBox.Show(g.tMessagesErreursStr[(int)ce.CEErreurInconnue]);
            }
        }

        

        #endregion

        #region Obtenir le prix

        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            if (repasComboBox.SelectedIndex != -1 && accompagnementComboBox.SelectedIndex != -1)
            {
                try
                {
                    prixLabel.Text = "$" + oTrans.ObtenirPrix(repasComboBox.SelectedIndex, accompagnementComboBox.SelectedIndex).ToString();
                    //OR
                    //prixLabel.Text = "$" + oTrans.ObtenirPrix(repasComboBox.SelectedItem.ToString(), accompagnementComboBox.SelectedItem.ToString()).ToString();

                }
                catch (ArgumentOutOfRangeException)
                {
                    MessageBox.Show(g.tMessagesErreursStr[(int)ce.CEIndexOutOfRange]);
                }
                catch(ArgumentException)
                {
                    MessageBox.Show(g.tMessagesErreursStr[(int)ce.CEArgumentException]);
                }
                catch(Exception) 
                {
                    MessageBox.Show(g.tMessagesErreursStr[(int)ce.CEErreurInconnue]);
                }
            }
        }

        #endregion

        #region SelectAll() - Le texte est sélectionné à chaque fois qu’une zone de texte reçoit le focus

        private void MaskedTextBox_Enter(object sender, EventArgs e)
        {
            MaskedTextBox textBox = sender as MaskedTextBox;
            textBox.SelectAll();
        }

        #endregion

        #region Enregistrer

        private void enregistrerToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        #endregion

        #region Validation de la date de livraison

        private void dateCommandeDateTimePicker_Validating(object sender, CancelEventArgs e)
        {
            // Si l'utilisateur n'a pas saisi de date valide ou s'il n'a pas saisi de date, remplacez la date du jour
            DateTime dateValue;
            bool estParse = DateTime.TryParse(dateCommandeDateTimePicker.Text, out dateValue);

            if (!estParse)
            {
                dateCommandeDateTimePicker.Value = DateTime.Now;
            }
            else
            {
                dateCommandeDateTimePicker.Text = dateValue.ToLongDateString();
            }
        }

        #endregion

        #region Information sur l'entreprise
        private void aProposDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                RestaurationAboutBox oAboutBox = new RestaurationAboutBox();
                oAboutBox.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show(g.tMessagesErreursStr[(int)ce.CEErreurInconnue]);
            }
        }

        #endregion

        #region Quitter

        private void Quitter_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        #endregion

        private void CommanderEnregistrer_Click(object sender, EventArgs e)
        {
            try
            {
                // Déclencher toutes les validations 
                if (this.ValidateChildren())
                {
                    string prixText = prixLabel.Text.Replace("$", string.Empty);
                    // Technique 1 : Transmettre les données Projet principal --> Transaction du projet via le constructeur
                    Transaction oTrans = new Transaction(nomMaskedTextBox.Text, prenomMaskedTextBox.Text, adresseMaskedTextBox.Text,
                                                         codePostalMaskedTextBox.Text, telephoneMaskedTextBox.Text, typeCuisineComboBox.Text,
                                                         typePlatComboBox.Text, DateTime.Parse(dateCommandeDateTimePicker.Text),
                                                          repasComboBox.Text, accompagnementComboBox.Text, Decimal.Parse(prixText));

                    oTrans.Enregistrer();

                    // Afficher la date d'échéance du paiement
                    dueDateLabel.Text = "Paiement dû le:      " + oTrans.DatePaiement.ToLongDateString();

                    // Technique 2 : Transmettre données Projet Principal --> Projet Transaction en passant par les propriétés
                    //Transaction oTrans2 = new Transaction();

                    //oTrans2.Nom = nomMaskedTextBox.Text;
                    //oTrans2.Prenom = prenomMaskedTextBox.Text;
                    //oTrans2.Adresse = adresseMaskedTextBox.Text;
                    //oTrans2.CodePostal = codePostalMaskedTextBox.Text;
                    //oTrans2.Telephone = telephoneMaskedTextBox.Text;
                    //oTrans2.DateLivraison = DateTime.Parse(dateCommandeDateTimePicker.Text);
                    //oTrans2.Prix = Decimal.Parse(prixLabel.Text);

                    //oTrans2.Enregistrer();

                    // Technique 3 : Transmettre données Projet Principal --> Projet Transaction en passant par Enregistrer avec paramètres
                    //Transaction oTrans3 = new Transaction();

                    //oTrans3.Enregistrer(nomMaskedTextBox.Text, prenomMaskedTextBox.Text, adresseMaskedTextBox.Text,
                    //                    codePostalMaskedTextBox.Text, telephoneMaskedTextBox.Text, typeCuisineComboBox.Text,
                    //                    typePlatComboBox.Text, DateTime.Parse(dateCommandeDateTimePicker.Text),
                    //                    repasComboBox.Text, accompagnementComboBox.Text, Decimal.Parse(prixLabel.Text));
                }
                else
                {
                    MessageBox.Show(g.tMessagesErreursStr[(int)ce.CEErreurChargementFichier]);
                }

            }
            catch (Exception)
            {
                MessageBox.Show(g.tMessagesErreursStr[(int)ce.CEErreurInconnue]);
            }

        }
    }
}
