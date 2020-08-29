using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_SQLiteHelper {
    class Program {
        static void Main (string[] args) {

            using (SQLiteConnection conn = new SQLiteConnection("data source=c;version=3;")) {
                using (SQLiteCommand cmd = new SQLiteCommand()) {
                    cmd.Connection = conn;
                    conn.Open();

                    SQLiteHelper sh = new SQLiteHelper(cmd);

                    DataTable dt = sh.GetTableList();

                    //遍历DataTable对象,转换成List
                    foreach (DataRow row in dt.Rows) {
                        Console.WriteLine(row["sql"]);
                    }

                    // do something...

                    conn.Close();
                }
            }

            while (true) {

            }
        }
    }
}
