using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using cardObjects;

namespace BetterSpades.Net {

    public partial class GraphicsCardHand : System.Windows.Forms.PictureBox {

        // private THand fhand = null;
        private CardPlayer fcardplayer = null;
        private HandOrientiation fhandorientation = HandOrientiation.HandHorizontalUp;
        private CardSet fcardset = CardSet.Default;
        private CardBacks fback = CardBacks.Clouds;

        private void setCardSet(CardSet avalue) {
            fcardset = avalue;
        }


        public GraphicsCardHand() {
            InitializeComponent();
        }

        public CardBacks CardBack {
            get { return fback; }
            set {
                fback = value;
                this.Refresh();
            }
        }

        public CardPlayer Player {
            get { return fcardplayer; }
            set { fcardplayer = value; }
        }

        public HandOrientiation Orientation {
            get { return fhandorientation; }
            set { fhandorientation = value; }
        }

        public CardSet CardSet {
            get { return fcardset; }
            set { fcardset = value; }
        }

        private int findIndex(int xlocation, int ylocation) {

            int r = -1;


            for (int i = 0; i < fcardplayer.Hand.Count; i++) {
                if ((xlocation >= fcardplayer.Hand[i].HitArea.X) &&
                    (xlocation <= fcardplayer.Hand[i].HitArea.Right)) {
                    if ((ylocation >= fcardplayer.Hand[i].HitArea.Y) &&
                        (ylocation <= fcardplayer.Hand[i].HitArea.Bottom)) {
                        r = i;
                        break;
                    }
                }
            }
            return r;
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            if (fcardplayer.Hand != null) {
                if (e.Button == MouseButtons.Left) {
                    int i = findIndex(e.X, e.Y);

                    if (i > -1) {

                        this.fcardplayer.Hand[i].IsCardSelected =
                            !this.fcardplayer.Hand[i].IsCardSelected;
                        this.Refresh();
                    }
                }
            }

            base.OnMouseUp(e);
        }

        private int getHorizontalOverlap(int amt, int width) {
            // shows one / fourth of the cards
            return (amt * width) - (amt * (width / 2)) - (amt * (width / 4));
        }

        private int getVerticalOverlap(int amt, int height) {
            return (amt * height) - (amt * height / 2) - (amt * height / 4);
        }

        protected override void OnPaint(PaintEventArgs pe) {
            base.OnPaint(pe);

            if (fcardplayer != null) {
                if (fcardplayer.Hand != null) {
                    pe.Graphics.Clear(this.BackColor);
                    for (int i = 0; i < this.fcardplayer.Hand.Count; i++) {
                        Bitmap p = CardFunctions.GetBitmapCard(this.fcardplayer.Hand[i], fcardset, fback);
                        switch (this.Orientation) {
                            case HandOrientiation.HandHorizontalUp:
                                pe.Graphics.DrawImage(p,
                                    new Rectangle(getHorizontalOverlap(i, p.Width),
                                        (this.fcardplayer.Hand[i].IsCardSelected ? (this.Height - p.Height - 20) :
                                      (this.Height - p.Height)), p.Width, p.Height));

                                this.fcardplayer.Hand[i].HitArea = new Rectangle(
                                    getHorizontalOverlap(i, p.Width),
                                    (this.fcardplayer.Hand[i].IsCardSelected ? (this.Height - p.Height - 20) :
                                      (this.Height - p.Height)),
                                      (i < this.fcardplayer.Hand.Count - 1) ? getHorizontalOverlap(1, p.Width) : p.Width,
                                    p.Height);
                                break;
                            case HandOrientiation.HandVerticalLeft:
                                pe.Graphics.DrawImage(p,
                                    new Rectangle(this.fcardplayer.Hand[i].IsCardSelected ? (this.Width - p.Width - 20) :
                                    this.Width - p.Width,
                                    getVerticalOverlap(i, p.Height),
                                    p.Width,
                                    p.Height));

                                this.fcardplayer.Hand[i].HitArea =
                                    new Rectangle(
                                    this.fcardplayer.Hand[i].IsCardSelected ? (this.Width - p.Width - 20) : this.Width - p.Width,
                                    getVerticalOverlap(i, p.Height),
                                    p.Width,
                                    (i < this.fcardplayer.Hand.Count - 1) ? getVerticalOverlap(1, p.Height) : p.Height);
                                break;
                            case HandOrientiation.HandHorizontalDown:
                                pe.Graphics.DrawImage(p,
                                    new Rectangle(getHorizontalOverlap(i, p.Width),
                                        (this.fcardplayer.Hand[i].IsCardSelected ? 20 :
                                      0), p.Width, p.Height));

                                this.fcardplayer.Hand[i].HitArea = new Rectangle(
                                    getHorizontalOverlap(i, p.Width),
                                    (this.fcardplayer.Hand[i].IsCardSelected ? 20 :
                                      0),
                                      (i < this.fcardplayer.Hand.Count - 1) ? getHorizontalOverlap(1, p.Width) : p.Width,
                                    p.Height);
                                break;
                            case HandOrientiation.HandVericalRight:

                                pe.Graphics.DrawImage(p,
                                    new Rectangle(this.fcardplayer.Hand[i].IsCardSelected ? 20 : 0,
                                    getVerticalOverlap(i, p.Height),
                                    p.Width,
                                    p.Height));

                                this.fcardplayer.Hand[i].HitArea =
                                    new Rectangle(
                                    this.fcardplayer.Hand[i].IsCardSelected ? 20 : 0,
                                    getVerticalOverlap(i, p.Height),
                                    p.Width,
                                    (i < this.fcardplayer.Hand.Count - 1) ? getVerticalOverlap(1, p.Height) : p.Height);
                                break;
                        }

                    }
                }
            }
        }
    }

    public partial class GraphicsCardTable : System.Windows.Forms.PictureBox {

        private CardTable ftable = new CardTable();


        private CardSet fcardset = CardSet.Default;
        private CardBacks fback = CardBacks.Clouds;



        public GraphicsCardTable() {
            InitializeComponent();
        }

        public CardSet CardSet {
            get { return fcardset; }
            set { fcardset = value; }
        }

        public CardTable CardTable {
            get { return ftable; }
            set { ftable = value; }
        }

        public CardBacks CardBack {
            get { return fback; }
            set {
                fback = value;
                this.Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs pe) {

            Size rsize = new Size();
            Point rpos = new Point();

            base.OnPaint(pe);

            pe.Graphics.Clear(this.BackColor);

            GameTurn n = this.ftable.LastTurn;

            if (n != null) {
                int startpos = ((int)n.StartingPlayer) - 1;

                foreach (Card i in n.CardList) {
                    Bitmap p = CardFunctions.GetBitmapCard(i, fcardset, fback);
                    Rectangle nrect = new Rectangle();

                    rpos.X = (this.Width / 2) - (p.Width / 2);
                    rpos.Y = 0;

                    rsize.Height = p.Height;
                    rsize.Width = p.Width;

                    if (p != null) {

                        if (startpos == 4) {
                            startpos = 0;
                        }

                        nrect = CardFunctions.RectangleTable(startpos, rpos, rsize);

                        pe.Graphics.DrawImage(p, nrect);
                        startpos++;
                    }

                }
            }

        }
    }

    public enum HandOrientiation {
        HandHorizontalUp = 0,
        HandVerticalLeft = 1,
        HandHorizontalDown = 2,
        HandVericalRight = 3
    }

    public enum CardSet {
        Default = 0,
        Roman = 1
    }

    public enum CardBacks {
        Clouds = 0,
        Concentric = 1,
        BlueCross = 2,
        Varchar = 3,
        Green = 4,
        MarbleStone = 5
    }

    public static class CardFunctions {
        public static string GetCardChar(Card acard) {
            string s = "";

            switch (acard.Suit) {
                case Suit.Clubs:
                    s = "c";
                    break;
                case Suit.Diamonds:
                    s = "d";
                    break;
                case Suit.Hearts:
                    s = "h";
                    break;
                case Suit.Spades:
                    s = "s";
                    break;
            }

            return ((int)acard.Value).ToString() + s;
        }

        public static string GetCardBack(CardBacks acardback) {
            return acardback.ToString();
        }

        public static Rectangle RectangleTable(int index, Point startpoint, Size asize) {
            Rectangle n = new Rectangle();

            //  Draw like:    []
            //              []  []
            //                [] 
            //  (With overlap)...

            switch (index) {
                case 0:
                    // North;
                    n.X = startpoint.X;
                    n.Y = startpoint.Y;
                    n.Width = asize.Width;
                    n.Height = asize.Height;
                    break;
                case 1:
                    // East
                    n.X = startpoint.X + (asize.Width / 2);
                    n.Y = startpoint.Y + (asize.Height / 2);
                    n.Width = asize.Width;
                    n.Height = asize.Height;
                    break;
                case 2:
                    // South
                    n.X = startpoint.X;
                    n.Y = startpoint.Y + (asize.Height / 2) * 2;
                    n.Width = asize.Width;
                    n.Height = asize.Height;
                    break;
                case 3:
                    // West
                    n.X = startpoint.X - (asize.Width / 2);
                    n.Y = startpoint.Y + (asize.Height / 2);
                    n.Width = asize.Width;
                    n.Height = asize.Height;
                    break;
            }

            return n;
        }

        public static Bitmap GetBitmapCard(Card acard, CardSet acardset, CardBacks acardback) {
            string s = "_" + CardFunctions.GetCardChar(acard);

            Bitmap nbitmap = null;

            if (acard.IsCardOpen) {
                switch (acardset) {

                    case CardSet.Default:
                        nbitmap = (Bitmap)
                            BetterSpades.card_defaultset.ResourceManager.GetObject(s);
                        break;
                    case CardSet.Roman:
                        nbitmap = (Bitmap)
                            BetterSpades.card_romanset.ResourceManager.GetObject(s);
                        break;
                }
            }
            else {
                nbitmap = (Bitmap)BetterSpades.card_backs.ResourceManager.GetObject(
                    CardFunctions.GetCardBack(acardback)
                    );
            }

            return nbitmap;
        }

    }



}
