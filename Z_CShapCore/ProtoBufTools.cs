﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z_CShapCore {
    public static class ProtoBufTools {

        /// <summary>
        /// 将消息序列化为二进制的方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static byte[] Serialize<T> (T model) {
            try {
                //涉及格式转换，需要用到流，将二进制序列化到流中
                using (MemoryStream ms = new MemoryStream()) {
                    //使用ProtoBuf工具的序列化方法
                    ProtoBuf.Serializer.Serialize<T>(ms , model);
                    //定义二级制数组，保存序列化后的结果
                    byte[] result = new byte[ms.Length];
                    //将流的位置设为0，起始点
                    ms.Position = 0;
                    //将流中的内容读取到二进制数组中
                    ms.Read(result , 0 , result.Length);
                    return result;
                }
            } catch (Exception ex) {
                Console.WriteLine("序列化失败: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 将收到的消息反序列化成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static T DeSerialize<T> (byte[] msg) {
            try {
                using (MemoryStream ms = new MemoryStream()) {
                    ms.Write(msg , 0 , msg.Length);                     //将消息写入流中
                    ms.Position = 0;                                    //将流的位置归0
                    T result = ProtoBuf.Serializer.Deserialize<T>(ms);  //使用工具反序列化对象
                    return result;
                }
            } catch (Exception ex) {
                Console.WriteLine("反序列化失败: " + ex.ToString());
                return default(T);
            }
        }
    }
}
