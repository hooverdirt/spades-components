using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace cardObjects {
    public enum Suit {
        Diamonds = 0, Clubs = 1, Hearts = 2, Spades = 3
    }

    public enum FaceValue {
        Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7,
        Eight = 8,
        Nine = 9, Ten = 10, Jack = 11, Queen = 12, King = 13, Ace = 14
    }

    public static class Constants {
        public const float ACEVALUE = 1.0F, KINGVALUE = 1.0F, QUEENVALUE = 1.0F,
            JACKVALUE = 0.75F, TENVALUE = 0.75F, NINEVALUE = 0.5F, EIGHTVALUE = 0.5F,
            SEVENVALUE = 0.25F, SIXVALUE = 0.1F, FIVEVALUE = 0.1F, FOURVALUE = 0.1F,
            THREEVALUE = 0.1F, TWOVALUE = 0.1F;

        public static float FaceValueToValue(FaceValue avalue) {
            float res = 0.0F;

            switch (avalue) {
                case FaceValue.Ace:
                    res = ACEVALUE;
                    break;
                case FaceValue.King:
                    res = KINGVALUE;
                    break;
                case FaceValue.Queen:
                    res = QUEENVALUE;
                    break;
                case FaceValue.Jack:
                    res = JACKVALUE;
                    break;
                case FaceValue.Ten:
                    res = TENVALUE;
                    break;
                case FaceValue.Nine:
                    res = NINEVALUE;
                    break;
                case FaceValue.Eight:
                    res = EIGHTVALUE;
                    break;
                case FaceValue.Seven:
                    res = SEVENVALUE;
                    break;
                case FaceValue.Six:
                    res = SIXVALUE;
                    break;
                case FaceValue.Five:
                    res = FIVEVALUE;
                    break;
                case FaceValue.Four:
                    res = FOURVALUE;
                    break;
                case FaceValue.Three:
                    res = THREEVALUE;
                    break;
                case FaceValue.Two:
                    res = TWOVALUE;
                    break;
            }

            return res;
        }

        public static int LoadXML(ref CardScores scores, string afilename) {
            int res = 0;

            XmlSerializer np = new XmlSerializer(typeof(CardScores));
            TextReader nr = new StreamReader(afilename);
            try {
                scores.FileName = afilename;

                scores = (CardScores)np.Deserialize(nr);
            }
            finally {
                nr.Close();
                nr.Dispose();
                res = scores.Count;
            }

            return res;
        }
    }

    public enum CardSortOptions {
        Ascending = 0,
        Descending = 1
    }

    public enum PlayerPosition {
        North = 1,
        East = 2,
        South = 3,
        West = 4
    }

    public class Card : IComparable<Card> {
        private FaceValue ffacevalue;
        private bool ffaceup = false;
        private bool fselected = false;
        private int fplayed = 0;
        private int fwon = 0;

        private Suit fsuit;

        private Rectangle fhitarea = new Rectangle();

        private int numericValue() {
            return (int)ffacevalue + ((int)fsuit * (int)FaceValue.Ace);
        }

        public Card() {
            fsuit = Suit.Clubs;
            ffaceup = false;
            ffacevalue = FaceValue.Ace;
        }

        public Card(Suit suit, FaceValue facevalue, bool faceup) {
            fsuit = suit;
            ffacevalue = facevalue;
            ffaceup = faceup;
        }

        public int Played {
            get { return fplayed; }
            set { fplayed = value; }
        }

        public int Won {
            get { return fwon; }
            set { fwon = value; }
        }

        public float Percentage {
            get { return ((float)fwon / (float)fplayed); }
        }

        public Suit Suit {
            get { return fsuit; }
            set { fsuit = value; }
        }

        public FaceValue Value {
            get { return ffacevalue; }
            set { ffacevalue = value; }
        }

        public int CalcValue {
            get { return numericValue(); }
        }

        public bool IsCardOpen {
            get { return ffaceup; }
            set { ffaceup = value; }
        }

        public Rectangle HitArea {
            get { return fhitarea; }
            set { fhitarea = value; }
        }

        public bool IsCardSelected {
            get { return fselected; }
            set { fselected = value; }
        }

        public override string ToString() {
            return "The " + ffacevalue.ToString() + " of " + fsuit.ToString();
        }

        public int CompareTo(Card acard) {
            return this.CalcValue.CompareTo(acard.CalcValue);
        }
    }

    public class HandComparer : IComparer<Card> {
        private CardSortOptions fsort = CardSortOptions.Ascending;

        public HandComparer(CardSortOptions order) {
            fsort = order;
        }

        public CardSortOptions SortOption {
            get { return fsort; }
            set { fsort = value; }
        }

        public int Compare(Card x, Card y) {
            if (fsort == CardSortOptions.Ascending) {
                return x.CompareTo(y);
            }
            else {
                return y.CompareTo(x);
            }
        }
    }

    // Hand filters..
    public class HandSuitFilter {
        private Suit fsuit;

        public HandSuitFilter(Suit asuit) {
            fsuit = asuit;
        }

        public bool FilterBySuit(Card acard) {
            return acard.Suit.Equals(fsuit);
        }
    }

    public class HandSelectedCardsFilter {
        private bool fselectedcards = false;

        public HandSelectedCardsFilter(bool selected) {
            fselectedcards = selected;
        }

        public bool FilterSelected(Card acard) {
            return acard.IsCardSelected.Equals(fselectedcards);
        }
    }
    // End hand filters


    public class Hand {
        private CardSortOptions forder = CardSortOptions.Ascending;

        protected List<Card> fhand = new List<Card>();

        protected void SwapCards(int index1, int index2) {
            Card card = fhand[index1];
            fhand[index1] = fhand[index2];
            fhand[index2] = card;
        }

        public void Clear() {
            fhand.Clear();
        }

        private void setCardOpen(bool IsOpen, int indexcard) {
            this[indexcard].IsCardOpen = IsOpen;
        }

        public void AllCardsOpen() {
            for (int i = 0; i < this.Count; i++) {
                setCardOpen(true, i);
            }
        }

        public void AllCardsClosed() {
            for (int i = 0; i < this.Count; i++) {
                setCardOpen(false, i);
            }
        }

        public int Count {
            get { return fhand.Count; }
        }

        public Card this[int aposition] {
            get { return fhand[aposition]; }
            set { fhand[aposition] = value; }
        }

        public int FindCardIndex(Suit suit, FaceValue aValue) {
            int res = -1;

            for (int i = 0; i < this.Count; i++) {
                Card n = this[i];

                if (n != null) {
                    if (n.Suit == suit) {
                        if (n.Value == aValue) {
                            res = i;
                            break;
                        }
                    }
                }

            }

            return res;
        }

        public int FindCardIndex(Card acard) {
            return fhand.IndexOf(acard);
        }

        public CardSortOptions SortOrder {
            get { return forder; }
            set { forder = value; }
        }

        public void AddCard(Card acard) {
            this.fhand.Add(acard);
        }

        public void Sort() {
            HandComparer fcomparer = new HandComparer(forder);
            fhand.Sort(fcomparer);
        }

        public List<Card> FindSelectedCards() {
            HandSelectedCardsFilter n = new HandSelectedCardsFilter(true);
            Predicate<Card> filtert = new Predicate<Card>(n.FilterSelected);
            return fhand.FindAll(filtert);
        }

        public List<Card> FindUnselectedCards() {
            HandSelectedCardsFilter n = new HandSelectedCardsFilter(false);
            Predicate<Card> filtert = new Predicate<Card>(n.FilterSelected);
            return fhand.FindAll(filtert);
        }

        public List<Card> FindCardsBySuit(Suit asuit) {
            HandSuitFilter n = new HandSuitFilter(asuit);
            Predicate<Card> filtert = new Predicate<Card>(n.FilterBySuit);
            return fhand.FindAll(filtert);
        }

        public int CountCardsSuit(Suit asuit) {
            int res = 0;

            for (int i = 0; i < fhand.Count; i++) {
                if (fhand[i].Suit == asuit) {
                    res++;
                }
            }

            return res;
        }

        public Card MinMaxCardOfSuit(Suit asuit, bool max) {
            Card res = null;

            List<Card> finnersuit = new List<Card>();

            finnersuit = FindCardsBySuit(asuit);

            if (finnersuit.Count > 0) {
                finnersuit.Sort(new HandComparer(CardSortOptions.Ascending));

                if (max) {
                    res = finnersuit[(finnersuit.Count - 1)];
                }
                else {
                    res = finnersuit[0];
                }
            }

            return res;
        }


    }

    public class CardPlayer {
        private Hand fhand = new Hand();

        private PlayerPosition fposition;
        private string fname = "";

        public Card this[int acard] {
            get { return fhand[acard]; }
            set { fhand[acard] = value; }
        }

        public Hand Hand {
            get { return fhand; }
            set { fhand = value; }
        }

        public PlayerPosition Seat {
            get { return fposition; }
            set { fposition = value; }
        }

        public string FirstName {
            get { return fname; }
            set { fname = value; }
        }

        public int HandCount {
            get { return fhand.Count; }
        }

        public void AddCard(Card acard) {
            this.fhand.AddCard(acard);
        }

        public void ClearHand() {
            this.fhand.Clear();
        }

        public int EstimateTricks() {
            int numhearts = 0, numclubs = 0, numdiamonds = 0, numspades = 0;
            int bids = 0, x = 0;
            Card c = null;

            numhearts = this.Hand.CountCardsSuit(Suit.Hearts);
            numclubs = this.Hand.CountCardsSuit(Suit.Clubs);
            numdiamonds = this.Hand.CountCardsSuit(Suit.Diamonds);
            numspades = this.Hand.CountCardsSuit(Suit.Spades);

            // Hearts
            if ((numhearts <= 6) && (Hand.FindCardIndex(Suit.Hearts, FaceValue.Ace) > -1)) {
                bids++;
            }

            if ((numhearts >= 2) && (numhearts <= 4) &&
                Hand.FindCardIndex(Suit.Hearts, FaceValue.King) > -1) {
                bids++;
            }

            c = Hand.MinMaxCardOfSuit(Suit.Hearts, false);
            if (c != null) {
                if (c.Value >= FaceValue.Six) {

                    bids++;
                }
            }


            // Clubs
            if ((numclubs <= 4) && (Hand.FindCardIndex(Suit.Clubs,
                FaceValue.Ace) > -1)) {
                bids++;
            }

            if ((numclubs == 3) && (Hand.FindCardIndex(Suit.Clubs,
                FaceValue.King) > -1)) {
                bids++;
            }

            c = Hand.MinMaxCardOfSuit(Suit.Clubs, false);
            if (c != null) {
                if (c.Value >= FaceValue.Six) {

                    bids++;
                }
            }

            // Diamonds...
            if ((numdiamonds <= 6) && (Hand.FindCardIndex(Suit.Diamonds, FaceValue.Ace) > -1)) {
                bids++;
            }

            if ((numdiamonds >= 2) && (numdiamonds <= 4)
                && (Hand.FindCardIndex(Suit.Diamonds, FaceValue.King) > -1)) {
                bids++;
            }

            c = Hand.MinMaxCardOfSuit(Suit.Diamonds, false);
            if (c != null) {
                if (c.Value >= FaceValue.Six) {

                    bids++;
                }
            }


            // Spades
            if (Hand.FindCardIndex(Suit.Spades, FaceValue.Ace) > -1) {
                bids++;
                numspades--;
            }

            if ((numspades >= 2) && (Hand.FindCardIndex(Suit.Spades, FaceValue.King) > -1)) {
                bids++;
                numspades--;
                numspades--;
            }

            // finish it off...
            if (numhearts < 3) {
                x = 3 - numhearts;
                /*while ((x > 0) && (numspades > 0))
                {
                    bids++; 
                    x--;
                    numspades--;
                }*/
                for (x = 3 - numhearts; x > 0 && numspades > 0; x--, numspades--) {
                    bids++;
                }

            }

            if (numclubs < 3) {
                x = 3 - numclubs;
                /*while ((x > 0) && (numspades > 0))
                {
                    bids++;
                    x--;
                    numspades--;
                }
                 */

                for (x = 3 - numclubs; x > 0 && numspades > 0; x--, numspades--) {
                    bids++;
                }

            }

            if (numdiamonds < 3) {
                x = 3 - numdiamonds;
                /* while ((x >0) && (numspades > 0))
                {
                    bids++;
                    x--;
                    numspades--;
                }
                 */
                for (x = 3 - numdiamonds; x > 0 && numspades > 0; x--, numspades--) {
                    bids++;
                }

            }

            return bids;
        }

        public int EstimateTricks1() {
            int bids = 0;

            for (int i = 0; i < Hand.Count; i++) {
                // Jack!
                if ((int)Hand[i].Value >= 11) {
                    bids++;
                }
            }

            return bids;
        }

        public int EstimateTricks2() {
            float bids = 0;
            for (int i = 0; i < Hand.Count; i++) {
                bids += Constants.FaceValueToValue(Hand[i].Value);
            }

            return (int)bids;
        }
    }

    public class CardPlayerList {
        private List<CardPlayer> flist = new List<CardPlayer>();


        public List<CardPlayer> PlayerList {
            get { return flist; }
            set { flist = value; }
        }

        public CardPlayer this[int anindex] {
            get { return flist[anindex]; }
            set { flist[anindex] = value; }
        }

        public int Count {
            get { return flist.Count; }
        }

        public void Clear() {
            flist.Clear();
        }

        public void AddPlayer(CardPlayer aplayer) {
            flist.Add(aplayer);
        }
    }

    public class CardScores {
        protected List<Card> fcards = new List<Card>();
        private string ffilename = "";

        public string FileName {
            get { return ffilename; }
            set { ffilename = value; }
        }

        protected void Init() {
            foreach (Suit suit in Enum.GetValues(typeof(Suit))) {
                foreach (FaceValue face in Enum.GetValues(typeof(FaceValue))) {
                    fcards.Add(new Card(suit, face, false));
                }
            }
        }

        public CardScores() {
            Init();
        }

        public Card this[int anindex] {
            get { return fcards[anindex]; }
            set { fcards[anindex] = value; }
        }

        [XmlArrayAttribute("CardList")]
        public List<Card> CardList {
            get { return fcards; }
            set { fcards = value; }
        }

        public int Count {
            get { return fcards.Count; }
        }

        public Card FindCard(Card acard) {

            Card res = null;
            foreach (Card card in fcards) {
                if (card.Equals(acard)) {
                    res = acard;
                    break;
                }
            }

            return res;
        }

        public void SaveToXMLFile() {

            XmlSerializer np = new XmlSerializer(typeof(CardScores));
            TextWriter tf = new StreamWriter(ffilename);
            try {
                if (ffilename != "") {
                    np.Serialize(tf, this);
                }
            }
            finally {
                tf.Dispose();
            }
        }
    }

    public class Deck {
        protected List<Card> fcards = new List<Card>();

        protected void SwapCards(int index1, int index2) {
            Card card = fcards[index1];
            fcards[index1] = fcards[index2];
            fcards[index2] = card;
        }

        protected void Init() {
            foreach (Suit suit in Enum.GetValues(typeof(Suit))) {
                foreach (FaceValue face in Enum.GetValues(typeof(FaceValue))) {
                    fcards.Add(new Card(suit, face, false));
                }
            }

        }


        public Deck() {
            Init();
            Shuffle();
        }

        public Card this[int anindex] {
            get { return fcards[anindex]; }
            set { fcards[anindex] = value; }
        }

        public int Count {
            get { return fcards.Count; }
        }

        public Card DrawCard() {
            Card carddrawn = fcards[0];
            fcards.RemoveAt(0);
            return carddrawn;
        }

        public Card DrawCard(Card acard) {
            Card s = null;
            int res = FindCardIndex(acard);

            if (res > -1) {
                s = fcards[res];
                fcards.RemoveAt(res);
            }

            return s;
        }

        public int FindCardIndex(Card acard) {
            return fcards.IndexOf(acard);
        }

        public void Shuffle() {
            Random random = new Random();
            for (int i = 0; i < fcards.Count; i++) {
                int index1 = i;
                int index2 = random.Next(fcards.Count);
                SwapCards(index1, index2);
            }
        }

        public void Clear() {
            fcards.Clear();
            Init();
            Shuffle();
        }

        public void Deal(CardPlayerList alist) {
            int p = 0;

            /* for (int i = 0; i < fcards.Count; i++)
            {
                if (p > 3)
                {
                    p = 0;
                }

                alist[p].AddCard(this.DrawCard());
                p++;
            }
             */

            while (fcards.Count > 0) {
                if (p > 3) {
                    p = 0;
                }

                alist[p].AddCard(this.DrawCard());
                p++;
            }
        }

    }

    public class BaseCardList {
        private List<Card> fcards = new List<Card>();

        public List<Card> CardList {
            get { return fcards; }
            set { fcards = value; }
        }

        public Card this[int anindex] {
            get { return fcards[anindex]; }
            set { fcards[anindex] = value; }
        }

        public void AddCard(Card acard) {
            fcards.Add(acard);
        }

        public void RemoveCard(int anindex) {
            fcards.RemoveAt(anindex);
        }

        public void Clear() {
            fcards.Clear();
        }

        public int Count {
            get { return fcards.Count; }
        }
    }


    public class GameTurn : BaseCardList {
        private int fmaxcardsperturn = 4;
        private PlayerPosition fplayerpos = PlayerPosition.North; // default!

        public int MaxCards {
            get { return fmaxcardsperturn; }
            set { fmaxcardsperturn = value; }
        }

        public PlayerPosition StartingPlayer {
            get { return fplayerpos; }
            set { fplayerpos = value; }
        }

    }

    public class CardTable {

        private List<GameTurn> fturns = new List<GameTurn>();


        //public List<TGameTurn> TurnList()
        //{
        //    return fturns;
        //}

        public GameTurn this[int anindex] {
            get { return fturns[anindex]; }
            set { fturns[anindex] = value; }
        }

        private GameTurn getLastTurn() {
            int i = this.fturns.Count - 1;

            GameTurn res = null;

            if (i > -1) {
                if (fturns[i].Count <= 4) {
                    res = fturns[i];
                }
            }
            else {
                GameTurn n = new GameTurn();
                fturns.Add(n);

                res = fturns[fturns.Count - 1];
            }

            return res;
        }

        public void AddCard(Card acard) {
            GameTurn n = getLastTurn();
            n.AddCard(acard);
        }

        public GameTurn LastTurn {
            get { return getLastTurn(); }
        }

        public void AddTurn(GameTurn aturn) {
            fturns.Add(aturn);
        }

        public void RemoveTurn(int anindex) {
            fturns.RemoveAt(anindex);
        }



        public void Clear() {
            fturns.Clear();
        }

        public int Count {
            get { return fturns.Count; }
        }

    }


}