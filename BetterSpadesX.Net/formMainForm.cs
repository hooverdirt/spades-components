using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using cardObjects;

namespace BetterSpades.Net {
    public partial class formMainForm : Form {
        private Deck fdeck = new Deck();
        private CardPlayerList fplayerlist = new CardPlayerList();
        private CardScores fcardscores = new CardScores();

        public formMainForm() {
            InitializeComponent();
        }

        private void formMainForm_Load(object sender, EventArgs e) {
            CardPlayer n = null;

            for (int i = 0; i < 4; i++) {
                n = new CardPlayer();
                fplayerlist.AddPlayer(n);
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            listBox5.Items.Clear();

            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;

            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;


            fdeck.Clear();

            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();

            for (int i = 0; i < 4; i++) {
                fplayerlist[i].ClearHand();
            }

            fdeck.Deal(fplayerlist);

            fplayerlist[0].Hand.AllCardsOpen();
            fplayerlist[0].Hand.Sort();
            fplayerlist[1].Hand.AllCardsOpen();
            fplayerlist[1].Hand.Sort();
            fplayerlist[2].Hand.AllCardsOpen();
            fplayerlist[2].Hand.Sort();
            fplayerlist[3].Hand.AllCardsOpen();
            fplayerlist[3].Hand.Sort();

            ListBox lb = null;

            for (int x = 0; x < 4; x++) {
                switch (x) {
                    case 0:
                        lb = listBox1;
                        break;
                    case 1:
                        lb = listBox2;
                        break;
                    case 2:
                        lb = listBox3;
                        break;
                    case 3:
                        lb = listBox4;
                        break;
                }

                for (int t = 0; t < fplayerlist[x].HandCount; t++) {
                    lb.Items.Add(fplayerlist[x][t].ToString());
                }
            }


            /* 
            tghCardHand1.CardHand = fplayerlist[0].Hand;
            tghCardHand1.Refresh();

            tghCardHand2.CardHand = fplayerlist[1].Hand;
            tghCardHand2.Refresh();

            tghCardHand3.CardHand = fplayerlist[2].Hand;
            tghCardHand3.Refresh();


            tghCardHand4.CardHand = fplayerlist[3].Hand;
            tghCardHand4.Refresh();
             */

            tghCardHand1.Player = fplayerlist[0];
            tghCardHand1.Refresh();

            tghCardHand2.Player = fplayerlist[1];
            tghCardHand2.Refresh();

            tghCardHand3.Player = fplayerlist[2];
            tghCardHand3.Refresh();

            tghCardHand4.Player = fplayerlist[3];
            tghCardHand4.Refresh();

            listBox5.Items.Add(String.Format("Player 1 bids {0} - {1} - {2}", fplayerlist[0].EstimateTricks(),
                fplayerlist[0].EstimateTricks1(), fplayerlist[0].EstimateTricks2()));
            listBox5.Items.Add(String.Format("Player 2 bids {0} - {1} - {2}", fplayerlist[1].EstimateTricks(),
                fplayerlist[1].EstimateTricks1(), fplayerlist[1].EstimateTricks2()));
            listBox5.Items.Add(String.Format("Player 3 bids {0} - {1} - {2}", fplayerlist[2].EstimateTricks(),
                fplayerlist[2].EstimateTricks1(), fplayerlist[2].EstimateTricks2()));
            listBox5.Items.Add(String.Format("Player 4 bids {0} - {1} - {2}", fplayerlist[3].EstimateTricks(),
                fplayerlist[3].EstimateTricks1(), fplayerlist[3].EstimateTricks2()));
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e) {
            if (!checkBox5.Checked) {
                tghCardHand1.Player.Hand.AllCardsOpen();
            }
            else {
                tghCardHand1.Player.Hand.AllCardsClosed();
            }

            tghCardHand1.Refresh();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            int player;
            ListBox lb = null;

            if (sender == checkBox1) {
                player = 0;
                lb = listBox1;
            }
            else {
                if (sender == checkBox2) {
                    player = 1;
                    lb = listBox2;
                }
                else {
                    if (sender == checkBox3) {
                        player = 2;
                        lb = listBox3;
                    }
                    else {
                        player = 3;
                        lb = listBox4;
                    }
                }
            }


            if ((sender as CheckBox).Checked) {
                fplayerlist[player].Hand.SortOrder = CardSortOptions.Ascending;
            }
            else {
                fplayerlist[player].Hand.SortOrder = CardSortOptions.Descending;
            }


            fplayerlist[player].Hand.Sort();
            tghCardHand1.Refresh();
            tghCardHand2.Refresh();
            tghCardHand3.Refresh();
            tghCardHand4.Refresh();

            lb.Items.Clear();



            for (int t = 0; t < fplayerlist[player].HandCount; t++) {
                lb.Items.Add(fplayerlist[player][t].ToString());
            }


        }

        private void button2_Click(object sender, EventArgs e) {

        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e) {
            if (!checkBox6.Checked) {
                tghCardHand2.Player.Hand.AllCardsOpen();
            }
            else {
                tghCardHand2.Player.Hand.AllCardsClosed();
            }
            tghCardHand2.Refresh();
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e) {
            if (!checkBox7.Checked) {
                tghCardHand3.Player.Hand.AllCardsOpen();
            }
            else {
                tghCardHand3.Player.Hand.AllCardsClosed();
            }

            tghCardHand3.Refresh();
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e) {
            if (!checkBox8.Checked) {
                tghCardHand4.Player.Hand.AllCardsOpen();
            }
            else {
                tghCardHand4.Player.Hand.AllCardsClosed();
            }

            tghCardHand4.Refresh();
        }

        private void button2_Click_1(object sender, EventArgs e) {

            GameTurn nt = new GameTurn();

            nt.StartingPlayer = PlayerPosition.South;
            nt.AddCard(fplayerlist[0].Hand[0]);
            nt.AddCard(fplayerlist[3].Hand[0]);
            nt.AddCard(fplayerlist[2].Hand[0]);
            nt.AddCard(fplayerlist[1].Hand[0]);

            tghCardTable1.CardTable.AddTurn(nt);

            tghCardTable1.Refresh();
        }

        private void listBox5_DoubleClick(object sender, EventArgs e) {
            int i = listBox5.SelectedIndex;

            fplayerlist[i].EstimateTricks1();
        }

        private void formMainForm_FormClosing(object sender, FormClosingEventArgs e) {
            fcardscores.FileName = Application.StartupPath + "\\" + "aiscores.xml";
            fcardscores.SaveToXMLFile();
        }

    }
}