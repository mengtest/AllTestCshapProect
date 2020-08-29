using System;
using System.Collections.Generic;
using System.IO;

namespace Z
{
	class Program {
        static void Main (string[] args) {
            string saveRoot = "GenFiles/";
            if (!Directory.Exists(saveRoot)) {
                Directory.CreateDirectory(saveRoot);
            }
            Tools.stopwatch.Start();

            int fileNum = Tools.RandomNum(1 , 4); //文件的数量
            int classNum = Tools.RandomNum(1 , 4);//类的数量 
            int varNum = Tools.RandomNum(2 , 7);//声明的变量数量
            int funNum = Tools.RandomNum(2 , 5);//方法的数量
            int funCodeNum = Tools.RandomNum(3 , 10);//方法里插的代码行数
            int chindNum = Tools.RandomNum(0 , 5);//子方法层级嵌套的数量，如if里嵌套的层数

            for (int ii = 0 ; ii < 3 ; ii++) {
                Znamespace code = new Znamespace();
                for (int i = 0 ; i < classNum ; i++) {
                    Zchunk zclass = code.AddChunk(Zchunk.New<Zclass>());

                    for (int j = 0 ; j < varNum ; j++) {
                        int idx = Tools.RandomNum(0 , 100); //是普通的变量声明，还是属性的声明
                        if (idx > 50)
                            zclass.AddChunk(Zchunk.New<Zvar>());
                        else
                            zclass.AddChunk(Zchunk.New<Zattribute>());
                    }

                    for (int j = 0 ; j < funNum ; j++) {
                        Zchunk zfun = new Zfun();
                        zclass.AddChunk(zfun);
                        for (int k = 0 ; k < funCodeNum ; k++)
                            AddChildChunk(zfun , chindNum);
                    }
                }


                //Console.WriteLine(code);
                string fileName = Tools.GetBase64();
                File.WriteAllText($"{saveRoot}{fileName}.cs" , code.ToString());

                if (false)
                {
                    //序列化格式化输出
                    Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
                    settings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;  //不序列自我循环引用，循环引用的字段会丢失
                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                    TextReader tr = new StringReader(Newtonsoft.Json.JsonConvert.SerializeObject(code , settings));
                    Newtonsoft.Json.JsonTextReader jtr = new Newtonsoft.Json.JsonTextReader(tr);
                    object obj = serializer.Deserialize(jtr);
                    if (obj != null) {
                        StringWriter textWriter = new StringWriter();
                        Newtonsoft.Json.JsonTextWriter jsonWriter = new Newtonsoft.Json.JsonTextWriter(textWriter) {
                            Formatting = Newtonsoft.Json.Formatting.Indented ,
                            Indentation = 4 ,
                            IndentChar = ' '
                        };
                        serializer.Serialize(jsonWriter , obj);
                        Console.WriteLine(textWriter.ToString());
                        File.WriteAllText($"a.cs" , textWriter.ToString());
                    }
                }
            }
        }

        static List<Type> randomTypeList = new List<Type>()
        {
            typeof(Zfor),
            typeof(Zwhile),
            typeof(Zcode),
            typeof(Zcode),
            typeof(Zif),
            typeof(Zif),
            typeof(Zif)
        };

        static void AddChildChunk (Zchunk zfun , int chindMaxNum , int childCurCount = 0) {
            Type randomType = randomTypeList[Tools.RandomNum(0 , randomTypeList.Count)];   //随机类型
            Zchunk newChunk = Activator.CreateInstance(randomType) as Zchunk;
            zfun.AddChunk(newChunk);
            int randomIdx = 0;
            if (randomType.Name == "Zif") {
                Zif newIf = newChunk as Zif;
                randomIdx = Tools.RandomNum(0 , 100);
                while (randomIdx > 70) {
                    randomIdx = Tools.RandomNum(0 , 100);
                    if (randomIdx > 50) {
                        ZelseIf newElseIf = newIf.AddElseIf(new ZelseIf());
                        randomIdx = Tools.RandomNum(0 , 3); //嵌套的elseif有多少个代码行
                        for (int ii = 0 ; ii < randomIdx ; ii++)
                            newElseIf.AddChunk(Zchunk.New<Zcode>());
                    } else {
                        Zelse newElse = newIf.AddElseIf(new Zelse());
                        randomIdx = Tools.RandomNum(0 , 3); //嵌套的else有多少个代码行
                        for (int ii = 0 ; ii < randomIdx ; ii++)
                            newElse.AddChunk(Zchunk.New<Zcode>());
                        break;   //if的嵌套到else结束
                    }
                    randomIdx = Tools.RandomNum(0 , 100);
                }
            }
            if (randomType.Name != "Zcode") {
                randomIdx = Tools.RandomNum(0 , 3); //语句块里嵌套的有多少个代码行
                for (int ii = 0 ; ii < randomIdx ; ii++)
                    newChunk.AddChunk(Zchunk.New<Zcode>());
                randomIdx = Tools.RandomNum(0 , 100);   // 有多少的几率会继续嵌套
                if (childCurCount < chindMaxNum && randomIdx > 40)
                    AddChildChunk(newChunk , chindMaxNum , childCurCount + 1);
            }
        }
    }
}