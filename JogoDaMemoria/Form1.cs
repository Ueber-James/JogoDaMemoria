﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JogoDaMemoria
{
    public partial class Form1 : Form
    {

        public class CardInfo
        {
            public int Tag { get; set; }
            public Image Image { get; set; }
            public SoundPlayer Sound { get; set; }
        }

        int movements, click, cardFound, tagIndex;
        Image[] img = new Image[9];
        List<string> lista = new List<string>();
        List<Point> positions = new List<Point>();
        int[] tags = new int[2];

        List<CardInfo> cards = new List<CardInfo>();


       

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

                // Crie um novo objeto CardInfo para representar esta carta
                CardInfo card = new CardInfo
                {
                    Tag = tagIndex,
                    Image = img[tagIndex]
                };

                // Associe cada carta a um som específico
                switch (tagIndex)
                {
                    case 0:
                        card.Sound = new SoundPlayer(Properties.Resources._1);
                        break;
                    case 1:
                        card.Sound = new SoundPlayer(Properties.Resources._2);
                        break;
                    case 2:
                        card.Sound = new SoundPlayer(Properties.Resources._4);
                        break;
                    case 3:
                        card.Sound = new SoundPlayer(Properties.Resources._3);
                        break;
                    case 4:
                        card.Sound = new SoundPlayer(Properties.Resources._0);
                        break;

                    case 5:
                        card.Sound = new SoundPlayer(Properties.Resources._18);
                        break;
                    case 6:
                        card.Sound = new SoundPlayer(Properties.Resources._15);
                        break;
                    case 7:
                        card.Sound = new SoundPlayer(Properties.Resources._10);
                        break;
                    case 8:
                        card.Sound = new SoundPlayer(Properties.Resources._8);
                        break;
                   
                }

                cards.Add(card);

            }

            Random();
        }

        private void Random()
        {
            Random rdn = new Random();
            

            // Defina as posições possíveis para as cartas
            int[] xP = { 12, 130, 248, 366, 484, 602, 720, 838, 956 };
            int[] yP = { 26, 124 };

            foreach (int x in xP)
            {
                foreach (int y in yP)
                {
                    positions.Add(new Point(x, y));
                }
            }

            // Embaralhe as posições aleatoriamente
            positions = positions.OrderBy(p => rdn.Next()).ToList();

            int cardIndex = 0;
            foreach (PictureBox item in Controls.OfType<PictureBox>())
            {
                if (cardIndex >= positions.Count)
                {
                    // Se não houver mais posições disponíveis, saia do loop.
                    break;
                }

                item.Location = positions[cardIndex];
                cardIndex++;
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

            // Encontre o CardInfo correspondente com base na tag
            CardInfo clickedCard = cards.FirstOrDefault(card => card.Tag == tagIndex);

            // Reproduza o som associado à carta quando ela for clicada
            if (clickedCard != null && clickedCard.Sound != null)
            {
                clickedCard.Sound.Play();
            }

            

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
            Thread.Sleep(1000);
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
            EndGame();
        }

        private void EndGame()
        {
            if (cardFound == (img.Length * 2))
            {
                MessageBox.Show($"Parabens, você fez {movements.ToString()} pontos");
              DialogResult msg =  MessageBox.Show("Deseja continual jogando?", "Caixa de Pergunta", MessageBoxButtons.YesNo);

                if (msg == DialogResult.Yes)
                {
                    click = 0; movements = 0; cardFound = 0;
                    lista.Clear();
                    Start();

                    
                }else if (msg == DialogResult.No)
                {
                    MessageBox.Show("Obrigado por jogar");
                    Application.Exit();
                }
            }
        }

    }
}
