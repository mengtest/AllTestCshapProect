using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Z_CShapCore;

namespace ddzServer
{
    class Program
    {
        static void Main(string[] args)
        {

            //List<int> idxs = new List<int>();
            //List<int>[] players = new List<int>[3] {
            //    new List<int>(),
            //    new List<int>(),
            //    new List<int>()
            //};
            //for (int i = 0; i < 54; i++)
            //{
            //    idxs.Add(i);
            //}
            //int playerIdx = 0;
            //Random random = new Random();
            //while (idxs.Count > 3)
            //{
            //    int pokerIndex = random.Next(0, idxs.Count);
            //    players[playerIdx].Add(idxs[pokerIndex]);
            //    idxs.RemoveAt(pokerIndex);
            //    playerIdx = (++playerIdx) % 3;
            //}
            //
            //Func<int, string> action = (val) => {
            //    if (val <= 51)
            //    {
            //        int v = val / 4;
            //        int flg = val - v * 4;
            //        return "(" + (PokerValueType)(v) + "_" + (PokerType)(flg) + ")";
            //    }
            //    return val.ToString();
            //};
            //
            //List<PokerData> data2 = LitJson.JsonMapper.ToObject<List<PokerData>>(Tools.Read("poker.txt"));
            //Dictionary<string, PokerData> data3 = new Dictionary<string, PokerData>();
            //for (int i = 0; i < data2.Count; i++)
            //{
            //    data3.Add(i.ToString(), data2[i]);
            //}
            //Console.WriteLine(data3.ToJson());


            //List<PokerData> data2 = new List<PokerData>();
            //for (int i = 0; i < 54; i++)
            //{
            //    if (i <= 51)
            //    {
            //        int v = i / 4;
            //        int flg = i - v * 4;
            //        data2.Add(new PokerData()
            //        {
            //            pokerType = (PokerType)flg,
            //            valueType = (PokerValueType)(v)
            //        });
            //    }
            //    else if (i == 52)
            //    {
            //        data2.Add(new PokerData()
            //        {
            //            pokerType = PokerType.wang,
            //            valueType = PokerValueType.minJoker
            //        });
            //    }
            //    else if (i == 53)
            //    {
            //        data2.Add(new PokerData()
            //        {
            //            pokerType = PokerType.wang,
            //            valueType = PokerValueType.maxJoker
            //        });
            //    }
            //}
            //data2.Add(new PokerData()
            //{
            //    pokerType = PokerType.laizi,
            //    valueType = PokerValueType.laizi,
            //});
            //
            //Tools.SaveString("poker.txt", data2.ToJson());

            //3,3,3,3,
            //4,4,4,4,
            //5,5,5,5,
            //6,6,6,6,

            //string sql = @"select * from user where username='admin' and password='admin'";
            //string constr = @"server=localhost;port=3306;uid=root;pwd=root;database=ddzserver";
            //MySqlConnection conn = new MySqlConnection(constr);
            //conn.Open();
            //MySqlCommand cmd = new MySqlCommand(sql, conn);
            //MySqlDataReader dr = cmd.ExecuteReader();
            //
            //Console.Write(dr.GetType() + "\n");
            //int j = dr.FieldCount;
            //for (int i = 0; i < j; i++)
            //{
            //    Console.Write(dr.GetName(i));
            //    Console.Write("\t");
            //}
            //Console.Write("\n");
            //
            //while (dr.Read())
            //{
            //    if (dr.HasRows)
            //    {
            //        for (int i = 0; i < j; i++)
            //        {
            //            Console.Write(dr[i]);
            //            Console.Write("\t");
            //
            //        }
            //        Console.Write("\n");
            //    }
            //}
            //
            //dr.Close();
            //conn.Close();
            return;
            
            MessageManage.Self.Init();
            Server.OnClientConnent += (s, ip) =>
            {
                Console.WriteLine(ip.ToString() + " 进入！");
                
                ByteBuffer buff = new ByteBuffer();
                buff.WriteString("一则公告");
                MessageManage.Self.SendMsg(1, 0, buff,new List<Client>() {
                    Server.GetClient(ip)
                });
            };
            Server.OnClientQuit += (ips) =>
            {
                foreach (var ip in ips)
                {
                    Console.WriteLine(ip.ToString() + " 退出！");
                    if (Server.allClient.ContainsKey(ip))
                    {
                        Server.allClient.Remove(ip);
                    }
                }
                Console.WriteLine("Server.allClient.Count == " + Server.allClient.Count);
            };
            
            Server.Self.Start();
            SetConsoleCtrlHandler(cancelHandler, true);
            
            
            string txt = Tools.ReadFile("1.txt");
            string[] lines = txt.Split('\n');
            foreach (var line in lines)
            {
                string[] data = line.Split(new string[] { "<===>" },StringSplitOptions.RemoveEmptyEntries);
                if (data != null && data.Length == 2)
                {
                    GlobalData.allUser.Add(LitJson.JsonMapper.ToObject<GIPEndPoint>(data[0]), LitJson.JsonMapper.ToObject<User>(data[1]));
                }
            }

        }

        


        public delegate bool ControlCtrlDelegate(int CtrlType);
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ControlCtrlDelegate HandlerRoutine, bool Add);
        private static ControlCtrlDelegate cancelHandler = new ControlCtrlDelegate(HandlerRoutine);

        /// <summary>
        /// 关闭事件
        /// </summary>
        /// <param name="CtrlType"></param>
        /// <returns></returns>
        public static bool HandlerRoutine(int CtrlType)
        {
            switch (CtrlType)
            {
                case 0:
                    Console.WriteLine("0工具被强制关闭"); //Ctrl+C关闭
                    break;
                case 2:
                    Console.WriteLine("2工具被强制关闭");//按控制台关闭按钮关闭
                    
                    string result = "";
                    foreach (var item in GlobalData.allUser)
                    {
                        result += item.Key.ToJson() + "<===>" + item.Value.ToJson() + "\n";
                    }
                    if (result != "")
                    {
                        Tools.SaveFile("1.txt", result);
                    }
                    break;
            }
            return false;
        }
    }
}