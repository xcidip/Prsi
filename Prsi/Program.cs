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
            public Dictionary<int,PrsiPlayer> PositionPlayer { get; set; }

            private readonly List<string> _typeList = new List<string>
            {
                "kule", "srdce", "listy", "zaludy"
            };

            private readonly List<string> _nameList = new List<string>
            {
                "7", "8", "9", "10", "spodek", "svrsek", "eso"
            };
            #endregion
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

                #region Player initiation + giving cards
                // add person player
                PlayerList?.Add(new PrsiPlayer(playerName, "real"));

                // add npc players
                foreach (var aiPlayerName in AiPlayers)
                {
                    if (PlayerList != null) PlayerList.Add(new PrsiPlayer(aiPlayerName, "computer"));
                }

                // match players with their position in the queue
                PositionPlayer = new Dictionary<int, PrsiPlayer>();
                var i = 1;
                foreach (var player in PlayerList)
                {
                    PositionPlayer.Add(i, player);
                    i++;
                }

                // give cards to all players
                GiveCards(4);
                #endregion


                

                while (true)
                {
                    // played cards print
                    var playedCardsString = string.Join(", ", PlayedCards);

                    
                    Console.WriteLine($"Played cards: {playedCardsString}");
                    
                    foreach (var player in PlayerList)
                    {
                        Play(player);
                    }
                }
            }
            

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
                int choice;
                choice = player.Type == "computer" ? ComputerPlay(player) : PlayerPlay(player);


                if (choice == 0)
                {
                    Console.WriteLine($"Drawing one card for {player.Name}");
                    return;
                }

                // first card or if the last card played is the same type or name as the selected one
                if (PlayedCards.Count == 0 || player.Cards[choice].Type == PlayedCards.Last().Type || player.Cards[choice].Name == PlayedCards.Last().Name)
                {
                    PlayCard(player.Cards[choice],player);
                } 
                else
                {
                    Console.WriteLine("That card cant be played right now");
                    
                }
            }

            public int PlayerPlay(PrsiPlayer player)
            {
                Console.WriteLine("Player cards:");
                ShowCards(player);
                Console.Write("What card to play - (0 = Draw one): ");
                // validate if input = number
                var choice = Int32.Parse(Console.ReadLine()); // todo use Choice.NumberRangeValidation() instead
                return choice;
            }

            public int ComputerPlay(PrsiPlayer player)
            {
                Console.WriteLine("Enemy cards");
                ShowCards(player);
                var choice = 1;
                if (PlayedCards.Count == 0) return choice;
                foreach (var card in player.Cards)
                {
                    if (card.Type == PlayedCards.Last().Type || card.Name == PlayedCards.Last().Name)
                    {
                        Console.WriteLine($"Selected {card.Name} {card.Type}");
                        return choice;
                    }

                    choice++;
                }

                return 0;
            }
            public void PlayCard(Card card, PrsiPlayer player)
            {
                PlayedCards.Add(card);
                player.Cards.Add(card);
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
