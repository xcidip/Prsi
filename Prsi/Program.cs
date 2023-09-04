using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Prsi.Program;

namespace Prsi
{
    public class Program
    {
        public static void Main()
        {
            var npcList = new List<string>
            {
                "Norwyn", "Darwyn"
            };

            var Prsi = new Prsi("xcidip", npcList);


            Console.ReadLine();
        }

        public class Prsi
        {
            #region Variables

            public List<PrsiPlayer> PlayerList = new List<PrsiPlayer>();

            public List<Card> PlayedCards = new List<Card>();

            public List<Card> CardsInDeck = new List<Card>();
            public Card CurrentCard { get; set; }
            public PrsiPlayer RealPlayer { get; set; }

            private readonly List<string> _typeList = new List<string>
            {
                "kule", "srdce", "listy", "zaludy"
            };

            private readonly List<string> _nameList = new List<string>
            {
                "7", "8", "9", "10", "spodek", "svrsek", "eso"
            };

            public Prsi(string playerName, List<string> AiPlayers)
            {
                #region Create Cards

                foreach (var type in _typeList) // create card deck
                {
                    foreach (var name in _nameList)
                    {
                        CardsInDeck.Add(new Card(name, type));
                    }
                }

                #endregion

                #region NPC Players

                foreach (var aiPlayerName in AiPlayers)
                {
                    if (PlayerList != null) PlayerList.Add(new PrsiPlayer(aiPlayerName, "computer"));
                }

                #endregion

                // add person player
                PlayerList?.Add(new PrsiPlayer(playerName, "real"));

                // give cards
                GiveCards(4);

                var startingPlayer = WhoGoesFirst(PlayerList.Count);

                var currentPlayer = PlayerList[startingPlayer];

                ShowCards(currentPlayer);

                Card CurrentCard = null;

                RealPlayer = PlayerList.Find(a => a.Status == "real");


                while (true)
                {
                    Play(RealPlayer); // the current player plays
                }


            }
            #endregion

            public void ShowCards(PrsiPlayer player)
            {
                var i = 1;
                foreach (var card in player.Cards)
                {
                    Console.WriteLine($"({i}) {card.Name} - {card.Type}");
                    i++;
                }
            }

            public void Play(PrsiPlayer player)
            {
                if (player.Type == "computer")
                {
                    // AI things
                    
                }
                else
                {
                    Console.WriteLine("What card to play");
                    // validate if input = number
                    var choice = Int32.Parse(Console.ReadLine()); // todo use Choice.NumberRangeValidation() instead
                }
            }


            public void GiveCards(int howManyCardsToEachPlayer)
                {
                    var random = new Random();
                    foreach (var player in PlayerList)
                    {
                        for (var i = 0; i < howManyCardsToEachPlayer; i++)
                        {
                            
                            var index = random.Next(CardsInDeck.Count);
                            player.GiveCard(CardsInDeck[index]);
                            CardsInDeck.RemoveAt(index);
                        }
                    }
                }
            public static int WhoGoesFirst(int numberOfPlayers)
            {
                var random = new Random();
                return random.Next(numberOfPlayers);
            }
        }

        public class Card
        {
            public string Name { get; set; }// 7,8,9,10,spodek,svrsek,kral,eso
            public string Type { get; set; } // k,s,l,z

            public Card(string name, string type)
            {
                Name = name;
                Type = type;
            }
        }
        public class PrsiPlayer
        {
            public string Name { get; set; }
            public string Type { get; set; } // real/computer
            public List<Card> Cards = new List<Card>(); // what cards are in hand
            public string Status { get; set; } // 1st place, 2nd place, lost...
            public PrsiPlayer(string name,string type)
            {
                Name= name;
                Type = type;
            }

            public void GiveCard(Card card)
            {
                Cards.Add(card);
            }

            public void RemoveCard(Card card)
            {
                Cards.Remove(card);
            }
        }

    }
}
