using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JogoDaMemoria
{
    public partial class Form1 : Form
    {
        int movements, click, cardFound, tagIndex;
        Image[] img = new Image[5];
        List<string> lista = new List<string>();
        int[] tags = new int[2];


        private void label1_Click(object sender, EventArgs e)
        {

        }

        public Form1()
        {
            InitializeComponent();
            Start();
        }
        private void Start()
        {
            foreach (PictureBox item in Controls.OfType<PictureBox>())
            {
                tagIndex = int.Parse(String.Format("{0}", item.Tag));
                img[tagIndex] = item.Image;
                item.Image = Properties.Resources.verse;
                item.Enabled = true;

            }

            Random();
        }

        private void Random()
        {
            foreach (PictureBox item in Controls.OfType<PictureBox>())
            {
                Random rdn = new Random();

                int[] xP = { 27, 216, 405, 594, 783, 403 };
                int[] yP = { 26, 182, 184 };

            Repeat:
                var X = xP[rdn.Next(0, xP.Length)];
                var Y = yP[rdn.Next(0, yP.Length)];



                string verify = X.ToString() + Y.ToString();

                if (lista.Contains(verify))
                {
                    goto Repeat;
                }
                else
                {
                    item.Location = new Point(X, Y);
                    lista.Add(verify);
                }


            }
        }

        private void ImageClick(object sender, EventArgs e)
        {
            bool foundPair = false;

            PictureBox pic = (PictureBox)sender;
            click++;
            tagIndex = int.Parse(String.Format("{0}", pic.Tag));
            pic.Image = img[tagIndex];
            pic.Refresh();

            if (click == 1)
            {
                tags[0] = int.Parse(String.Format("{0}", pic.Tag));
            }
            else if (click == 2)
            {
                movements++;
                lblmovements.Text = "Movimentos: " + movements.ToString();
                tags[1] = int.Parse(String.Format("{0}", pic.Tag));
                foundPair = PairChecking();
                Untap(foundPair);
            }

        }

        private bool PairChecking()
        {
            click = 0;
            if (tags[0] == tags[1])
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Untap(bool check)
        {
            Thread.Sleep(500);
            foreach (PictureBox item in Controls.OfType<PictureBox>())
            {
                if (int.Parse(String.Format("{0}", item.Tag)) == tags[0] ||
                    int.Parse(String.Format("{0}", item.Tag)) == tags[1])
                {
                    if (check == true)
                    {
                        item.Enabled = false;
                        cardFound++;
                    }
                    else
                    {
                        item.Image = Properties.Resources.verse;
                        item.Refresh();
                    }
                }
            }
        }

    }
}
