using Game.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Player.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintGrid();
            Thread.Sleep(1000);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n                    1.START            2.EXIT    ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("\n                            Press Key: ");
            Console.ForegroundColor = ConsoleColor.White;
            int key = Convert.ToInt16(Console.ReadLine());
            Thread.Sleep(1000);
            if (key == 1)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("\n                     Enter Player Name: ");
                Console.ForegroundColor = ConsoleColor.White;
                string name = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("\n                      COMPUTER V/S " + name.ToUpper());
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\nloading.................");
                var player1 = ActorProxy.Create<IPlayer>(ActorId.CreateRandom(), "fabric:/ActorTicTacToeApplication");
                var player2 = ActorProxy.Create<IPlayer>(ActorId.CreateRandom(), "fabric:/ActorTicTacToeApplication");
                var gameId = ActorId.CreateRandom();
                var game = ActorProxy.Create<IGame>(gameId, "fabric:/ActorTicTacToeApplication");
                var rand = new Random();

                var result1 = player1.JoinGameAsync(gameId, "Computer");
                var result2 = player2.JoinGameAsync(gameId, name);

                if (!result1.Result || !result2.Result)
                {
                    Console.WriteLine("Failed to join game.");
                    return;
                }
                var player1Task = Task.Run(() => { MakeMove(player1, game, gameId); });
                var player2Task = Task.Run(() => { MakeMove(player2, game, gameId); });
                var gameTask = Task.Run(() =>
                {
                    string winner = "";


                    while (winner == "")
                    {
                        var board = game.GetGameBoardAsync().Result;
                        PrintBoard(board);
                        winner = game.GetWinnerAsync().Result;
                        Task.Delay(1000).Wait();
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\n     Winner is: ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(winner.ToUpper());
                });

                gameTask.Wait(); Console.Read();
            }
            else
            {
                Console.Write("THANKYOU ");
            }

        }

        private static async void MakeMove(IPlayer player, IGame game, ActorId gameId)
        {
            Random rand = new Random();
            while (true)
            {
                await player.MakeMoveAsync(gameId, rand.Next(0, 3), rand.Next(0, 3));
                await Task.Delay(rand.Next(500, 2000));
            }
        }

        private static void PrintGrid()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("_______ _____ _______    _______ _______ _______     _______  _____  _______");
            Console.WriteLine("   |      |   |      ___    |    |_____| |        ___   |    |     | |______");
            Console.WriteLine("   |    __|__ |______       |    |     | |______        |    |_____| |______");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("                         ╔═══╦═══╦═══╗");
            Console.WriteLine("                         ║ X ║   ║ 0 ║");
            Console.WriteLine("                         ╠═══╬═══╬═══╣");
            Console.WriteLine("                         ║   ║ X ║   ║");
            Console.WriteLine("                         ╠═══╬═══╬═══╣");
            Console.WriteLine("                         ║ 0 ║   ║ X ║");
            Console.WriteLine("                         ╚═══╩═══╩═══╝");
        }

        private static void PrintBoard(int[] board)
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("---------------");
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] == -1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("| X |");
                }
                else if (board[i] == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("| O |");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("| . |");
                }
                Console.ForegroundColor = ConsoleColor.Black;
                if ((i + 1) % 3 == 0) Console.WriteLine();
            }
            Console.WriteLine("---------------");
        }
    }
}
