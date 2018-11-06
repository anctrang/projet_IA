using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace Pluscourtchemin
{
    public partial class Form1 : Form
    {
        public List<String>[] reponses;
        public string noeudsFermes;
        public int etape;
        static public double[,] matrice;
        static public int nbnodes = 10;
        static public int numinitial;
        static public int numfinal;

        public Form1()
        {
            InitializeComponent();
            etape = 0;
            reponses = new List<String>[2];
            reponses[0] = new List<string>();
            reponses[1] = new List<string>();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            matrice = new double[nbnodes, nbnodes];
            for (int i = 0; i < nbnodes; i++)
                for (int j = 0; j < nbnodes; j++)
                    matrice[i, j] = -1;

            matrice[0, 1] = 3; matrice[1, 0] = 3;
            matrice[0, 2] = 5; matrice[2, 0] = 5;
            matrice[0, 3] = 7; matrice[3, 0] = 7;
            matrice[1, 4] = 8; matrice[4, 1] = 8;
            matrice[2, 4] = 3; matrice[4, 2] = 3;
            matrice[4, 5] = 7; matrice[5, 4] = 7;
            matrice[5, 6] = 4; matrice[6, 5] = 4;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            numinitial = Convert.ToInt32(textBox1.Text);
            numfinal = Convert.ToInt32(textBox2.Text);
            SearchTree g = new SearchTree();
            Node2 N0 = new Node2();
            N0.numero = numinitial;
            List<GenericNode> solution = g.RechercheSolutionAEtoile(N0);

            Node2 N1 = N0;
            for (int i = 1; i < solution.Count; i++)
            {
                Node2 N2 = (Node2)solution[i];
                listBox1.Items.Add(Convert.ToString(N1.numero)
                     + "--->" + Convert.ToString(N2.numero)
                     + "   : " + Convert.ToString(matrice[N1.numero, N2.numero]));
                N1 = N2;
            }

            g.GetSearchTree(treeView1);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            StreamReader monStreamReader = new StreamReader("graphe1.txt");

            // Lecture du fichier avec un while, évidemment !
            // 1ère ligne : "nombre de noeuds du graphe
            string ligne = monStreamReader.ReadLine();
            int i = 0;
            while (ligne[i] != ':') i++;
            string strnbnoeuds = "";
            i++; // On dépasse le ":"
            while (ligne[i] == ' ') i++; // on saute les blancs éventuels
            while (i < ligne.Length)
            {
                strnbnoeuds = strnbnoeuds + ligne[i];
                i++;
            }
            nbnodes = Convert.ToInt32(strnbnoeuds);

            matrice = new double[nbnodes, nbnodes];
            for (i = 0; i < nbnodes; i++)
                for (int j = 0; j < nbnodes; j++)
                    matrice[i, j] = -1;

            // Ensuite on a ls tructure suivante : 
            //  arc : n°noeud départ    n°noeud arrivée  valeur
            //  exemple 4 : 
            ligne = monStreamReader.ReadLine();
            while (ligne != null)
            {
                i = 0;
                while (ligne[i] != ':') i++;
                i++; // on passe le :
                while (ligne[i] == ' ') i++; // on saute les blancs éventuels
                string strN1 = "";
                while (ligne[i] != ' ')
                {
                    strN1 = strN1 + ligne[i];
                    i++;
                }
                int N1 = Convert.ToInt32(strN1);

                // On saute les blancs éventuels
                while (ligne[i] == ' ') i++;
                string strN2 = "";
                while (ligne[i] != ' ')
                {
                    strN2 = strN2 + ligne[i];
                    i++;
                }
                int N2 = Convert.ToInt32(strN2);

                // On saute les blancs éventuels
                while (ligne[i] == ' ') i++;
                string strVal = "";
                while ((i < ligne.Length) && (ligne[i] != ' '))
                {
                    strVal = strVal + ligne[i];
                    i++;
                }
                double val = Convert.ToDouble(strVal);

                matrice[N1, N2] = val;
                matrice[N2, N1] = val;
                listBoxgraphe.Items.Add(Convert.ToString(N1)
                   + "--->" + Convert.ToString(N2)
                   + "   : " + Convert.ToString(matrice[N1, N2]));

                ligne = monStreamReader.ReadLine();
            }
            // Fermeture du StreamReader (obligatoire) 
            monStreamReader.Close();



        }

       
        public bool verifierEntreeTextBox()
        {
            bool acceptable = true;    

            if (this.textBoxO.Text[0] != '{' || this.textBoxF.Text[0] != '{' || this.textBoxO.Text[textBoxO.Text.Count() - 1] != '}' ||
                this.textBoxF.Text[textBoxF.Text.Count() - 1] != '}') // On teste si les premiers et derniers caractères des textbox sont bien des accolades
            {
                acceptable = false;
            }


            if (this.textBoxO.Text.Count() > 3)
            {
                for (int i=2; i<textBoxO.Text.Count()-2; i=i+2)
                {
                    if(this.textBoxO.Text[i]!=44 && this.textBoxO.Text[i]!=46 && this.textBoxO.Text[i]!=58 && this.textBoxO.Text[i]!=59)
                    {
                        acceptable = false;
                    }
                }
            }

            if (this.textBoxF.Text.Count() > 3)
            {
                for (int i = 2; i < textBoxF.Text.Count()-2; i = i + 2)
                {
                    if (this.textBoxF.Text[i] != 44 && this.textBoxF.Text[i] != 46 && this.textBoxF.Text[i] != 58 && this.textBoxF.Text[i] != 59)
                    {
                        acceptable = false;
                    }
                }
            }


            for (int i = 1; i < textBoxO.Text.Count(); i = i + 2)
            {
                if (this.textBoxO.Text.Count() != 2) { // On vérifie la taille du stirng, s'il est "vide" pas besoin de faire la condition prochaine
                    if (this.textBoxO.Text[i] < 65 || this.textBoxO.Text[i] > 90 && this.textBoxO.Text[i] < 97 || this.textBoxO.Text[i] > 122 )
                    // On vérifie si tous les caractères impairs sont bien des lettres (gestion ascii + maj/min)
                    {
                        acceptable = false;               
                    }
                }
            }

            for (int i = 1; i < textBoxF.Text.Count(); i = i + 2)
            {
                if (this.textBoxF.Text.Count() != 2)
                { // On vérifie la taille du stirng, s'il est "vide" pas besoin de faire la condition prochaine
                    if (this.textBoxF.Text[i] < 65 || this.textBoxF.Text[i] > 90 && this.textBoxF.Text[i] < 97 || this.textBoxF.Text[i] > 122)
                    // On vérifie si tous les caractères impairs sont bien des lettres (gestion ascii + maj/min)
                    {
                        acceptable = false;
                    }
                }
            }
            return acceptable;
        }

        private string UniformisationReponse(string reponse)
        {

            char[] reponseEnTableauDeChar = reponse.ToCharArray(); // String immuable
            // passage obligatoire en tableau de char  
            if(reponse.Count() > 2)
            {
                for(int i=1; i < reponse.Count()-1; i++)
                {
                    if (i % 2 == 1) // Si on est sur une lettre
                    {
                        if (reponse[i] > 96) // Si minuscule
                        {
                            reponseEnTableauDeChar[i] = (char)((int)reponse[i] - 32); // Transformation en majuscule
                        }
                    }

                    else
                    {
                        if (reponse[i]!=44) // Si minuscule
                        {
                            reponseEnTableauDeChar[i] = (char)44; // Transformation en majuscule
                        }
                    }
                    
                    
                }
            }
            return new string(reponseEnTableauDeChar);
        }



        private void buttonNouvelleEtape_Click(object sender, EventArgs e)
        {

            if (verifierEntreeTextBox())
            {

                reponses[0].Add(UniformisationReponse(this.textBoxO.Text));
                reponses[1].Add(UniformisationReponse(this.textBoxF.Text));
                this.listBoxRep.Items.Add("F =" + reponses[0][etape] + "\t O=" + reponses[1][etape]);
                etape++;
                
            }

            else
            {
                MessageBox.Show("L'une des entrées est incorrecte ! Veuillez re-vérifer");
                this.textBoxO.Text = "{}";
                this.textBoxF.Text = "{}";
            }


        }

       
    }
}
