using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace FishGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int total = 0;
            Console.WriteLine("釣りゲームの開始！釣った魚の得点で競おう！！");
            Console.WriteLine("まずは名前の登録をしよう！");

            string name = Console.ReadLine();

            var list = new List<Fish>()
            {
                new Fish("マグロ", 100),
                new Fish("タイ", 120),
                new Fish("サケ", 80),
                new Fish("ブリ", 70),
                new Fish("イカ", 30),
                new Fish("ホタテ", 20),
                new Fish("アジ", 15),
                new Fish("メダカ", 3),
                new Fish("ゴミ", 0),
                new Fish("おじさん", -50),

            };

            while (true)
            {
                play();
                while (true)
                {
                    Console.WriteLine("再チャレンジしますか？　もう一回( Y ) / やめる( N )");
                    string ans = Console.ReadLine();
                    if (ans == "Y" || ans == "y")
                    {
                        break;
                    }
                    else if (ans == "N" || ans == "n")
                    {
                        Environment.Exit(0x8020);
                    } else
                    {
                        Console.WriteLine("指定されたコマンドを入力してください！");
                        continue;
                    }
                }
                
            }

            void play()
            {
                total = 0;
                for (int i = 0; i < 4; i++)
                {
                    int fishNumber;
                    list = list.OrderBy(a => Guid.NewGuid()).ToList();
                    Console.WriteLine("釣り場を選択してください！ 1～4の半角数字を入力してね!");
                    string spot = Console.ReadLine();
                    Random rnd = new Random();
                    if (spot == "1")
                    {
                        fishNumber = rnd.Next(0, 2);
                    }
                    else if (spot == "2")
                    {
                        fishNumber = rnd.Next(3, 5);
                    }
                    else if (spot == "3")
                    {
                        fishNumber = rnd.Next(6, 8);
                    }
                    else if (spot == "4")
                    {
                        fishNumber = rnd.Next(0, 9);
                    }
                    else
                    {
                        Console.WriteLine("入力が間違えてるよ！もう一度選択してね！");
                        i--;
                        continue;
                    }

                    Console.WriteLine(list[fishNumber].Name + "が釣れました!");
                    Console.WriteLine(list[fishNumber].Point + "ptゲット");
                    total += list[fishNumber].Point;
                }

                Console.WriteLine("あなたの合計点数は" + total);
                Console.WriteLine("=========================");

                insert(name, total);

                view();
            }

            



            void insert(string userName, int totalPoint)
            {
                var db = "Server=localhost;User ID=root;Password='';Database=fish";
                var sql = "INSERT INTO ranking VALUES(@name, @total)";

                using (var connection = new MySqlConnection(db))
                {
                    try
                    {
                        connection.Open();

                        using (var command = new MySqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@name", userName);
                            command.Parameters.AddWithValue("@total", totalPoint);

                            var result = command.ExecuteNonQuery();

                            if (result != 1)
                            {
                                Console.WriteLine("ランキングに入力出来ませんでした");
                            }
                        }

                        connection.Close();
                    }
                    catch (MySqlException m)
                    {
                        Console.WriteLine("ERROR: " + m.Message);
                    }

                }

                
            }

            void view()
            {
                int i = 1;
                var db = "Server=localhost;User ID=root;Password='';Database=fish";
                var sql = "SELECT * FROM ranking ORDER BY total DESC LIMIT 3";

                using (var connection = new MySqlConnection(db))
                {
                    connection.Open();

                    using (var command = new MySqlCommand(sql, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        Console.WriteLine("現在のランキングは！！");
                        while (reader.Read())
                        {
                            Console.WriteLine("第" + i + "位");
                            Console.WriteLine("名前：" + reader["name"] + "/合計点：" + reader["total"]);
                            Console.WriteLine("-------------");
                            i++;
                        }
                    }
                    connection.Close();
                }
            }


            
            

        }
    }
}
