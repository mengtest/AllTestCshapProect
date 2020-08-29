using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;

namespace Z_CShapCore {
    public static class ZipHelper {
        #region 压缩文件  
        /// <summary>  
        /// 压缩文件  
        /// </summary>  
        /// <param name="FileToZip">待压缩的文件</param>  
        /// <param name="ZipedFile">压缩后的文件</param>  
        /// <param name="password">压缩密码</param>  
        private static void ZipFile (string FileToZip , string ZipedFile , string password) {   //如果文件没有找到，则报错   
            if (!System.IO.File.Exists(FileToZip)) {
                throw new System.IO.FileNotFoundException("文件 " + FileToZip + "不存在！");
            }
            //取得待压缩文件流  
            System.IO.FileStream StreamToZip = new System.IO.FileStream(FileToZip , System.IO.FileMode.Open , System.IO.FileAccess.Read);
            //创建压缩后的文件  
            System.IO.FileStream ZipFile = System.IO.File.Create(ZipedFile);
            //创建新的ZIP输出流  
            ZipOutputStream ZipStream = new ZipOutputStream(ZipFile);
            // 每一个被压缩的文件都用ZipEntry表示，需要为每一个压缩后的文件设置名称    
            ZipEntry ZipEntry = new ZipEntry("ZippedFile");
            //设置每一个ZipEntry对象  
            ZipStream.PutNextEntry(ZipEntry);
            // 为后续的 DEFLATED 条目设置压缩级别。 0 -9  
            ZipStream.SetLevel(6);
            //设置解压密码  
            ZipStream.Password = password;
            //每次写入1024个字节  
            byte[] buffer = new byte[1024];
            int size = 0; //已写入压缩流的字节数  
            try {
                //如果没有写入完成  
                while (size < StreamToZip.Length) {
                    //将文件内容写入buffer  
                    int sizeRead = StreamToZip.Read(buffer , 0 , buffer.Length);
                    //将字节写入压缩流  
                    ZipStream.Write(buffer , 0 , sizeRead);
                    size += sizeRead;
                }
            } catch (System.Exception ex) {
                throw ex;
            }
            //完成写入 ZIP 输出流的内容，无需关闭底层流。  
            ZipStream.Finish();
            //关闭 ZIP 输出流和正在过滤的流。  
            ZipStream.Close();
            //关闭文件流  
            StreamToZip.Close();
        }
        #endregion 压缩文件  
        #region 压缩文件夹（私有不被外部调用）  
        /// <summary>  
        /// 压缩文件夹  
        /// </summary>  
        /// <param name="FileToZip">待压缩的文件夹</param>  
        /// <param name="ZipStream">压缩文件流</param>  
        /// <param name="ParentFolderName">压缩父目录</param>  
        private static void ZipFolder (string FolderToZip , ZipOutputStream ZipStream , string ParentFolderName) {   //如果文件夹没有找到，则报错   
            if (!System.IO.Directory.Exists(FolderToZip)) {
                throw new System.IO.FileNotFoundException("文件路径 " + FolderToZip + "不存在！");
            }
            //校验类  
            Crc32 crc = new Crc32();
            //取得路径下所有文件全路径  
            string[] filenames = Directory.GetFiles(FolderToZip);

            //循环压缩文件  
            foreach (string file in filenames) {    //打开要的压缩文件   
                if (!System.IO.File.Exists(file)) //判断文件是否存在  
                {
                    throw new System.IO.FileNotFoundException("待压缩的文件 " + file + "不存在！");
                }
                //取得待压缩文件流  
                FileStream fs = File.OpenRead(file);
                byte[] buffer = new byte[fs.Length];
                //将文件写入字节数组  
                fs.Read(buffer , 0 , buffer.Length);

                //压缩文件后的目录  
                string ZipPath = Path.Combine(ParentFolderName , Path.GetFileName(FolderToZip) + "/" + Path.GetFileName(file));
                ZipEntry entry = new ZipEntry(ZipPath);
                entry.DateTime = DateTime.Now;
                //设置size  
                entry.Size = fs.Length;
                //关闭文件流  
                fs.Close();
                // 将 CRC-32 重置为初始值。  
                crc.Reset();
                // 使用指定的字节数组更新校验和。  
                crc.Update(buffer);
                // 设置 CRC-32 值。  
                entry.Crc = crc.Value;
                //设置每一个ZipEntry对象  
                ZipStream.PutNextEntry(entry);
                //将字节写入压缩流  
                ZipStream.Write(buffer , 0 , buffer.Length);
            }

            //取得路径下所有文件夹全路径  
            string[] folders = Directory.GetDirectories(FolderToZip);
            //循环压缩文件夹下层目录的文件  
            foreach (string folder in folders) {
                ZipFolder(folder , ZipStream , Path.Combine(ParentFolderName , Path.GetFileName(FolderToZip)));
            }
        }
        #endregion 压缩文件夹  
        #region 压缩文件夹 （可被外部应用）  
        /// <summary>  
        /// 压缩文件夹  
        /// </summary>  
        /// <param name="FileToZip">待压缩的文件夹</param>  
        /// <param name="ZipStream">压缩后的文件</param>  
        /// <param name="password">压缩密码</param>  
        public static void ZipFolder (string FolderToZip , string ZipedFile , string password) {
            // 解决中文乱码  
            Encoding gbk = Encoding.GetEncoding("gbk");
            ZipConstants.DefaultCodePage = gbk.CodePage;

            //创建压缩后的文件  
            System.IO.FileStream zipFile = System.IO.File.Create(ZipedFile);
            //创建新的ZIP输出流  
            ZipOutputStream ZipStream = new ZipOutputStream(zipFile);
            //设置解压密码  
            ZipStream.Password = password;
            // 为后续的 DEFLATED 条目设置压缩级别。 0 -9  
            ZipStream.SetLevel(6);
            //压缩文件夹  
            ZipFolder(FolderToZip , ZipStream , "");
            //完成写入 ZIP 输出流的内容，无需关闭底层流。  
            ZipStream.Finish();
            //关闭 ZIP 输出流和正在过滤的流。  
            ZipStream.Close();
        }
        #endregion 压缩文件夹  
        #region 压缩文件（根据路径 自动判定是文件夹还是文件）  
        /// <summary>  
        /// 压缩文件  
        /// </summary>  
        /// <param name="FileToZip">待压缩的文件（夹）路径</param>  
        /// <param name="ZipStream">压缩后的文件</param>  
        /// <param name="password">压缩密码</param>  
        public static void ZipFileMain (string FileToZip , string ZipedFile , string password) {
            //为压缩文件夹  
            if (Directory.Exists(FileToZip)) {
                //压缩文件夹  
                ZipFolder(FileToZip , ZipedFile , password);
            }
            //为压缩文件  
            else if (File.Exists(FileToZip)) {
                // //压缩文件  
                ZipFile(FileToZip , ZipedFile , password);
            } else {
                throw new System.IO.FileNotFoundException("待压缩的文件目录 " + FileToZip + "出错！");
            }
        }
        #endregion 压缩文件（根据路径 自动判定是文件夹还是文件）  
        #region 解压文件  
        /// <summary>  
        /// 解压  
        /// </summary>  
        /// <param name="args">  
        //    args[0] = Server.MapPath("ZIP") + "\\f12Zip.zip"; //待解压的文件    
        //   args[1]=Server.MapPath("UPZIP\\");//解压后放置的目标目录   
        //  或  
        //    args[0] =  "D:\\f12Zip.zip"; //待解压的文件    
        //   args[1]="D:\\UPZIP\\");  //解压后放置的目标目录   
        //</param>  
        /// <summary>  
        /// 解压  
        /// </summary>  
        /// <param name="UpZipFile">待解压文件</param>  
        /// <param name="ZipToFile">解压后放置的目标目录</param>  
        /// <param name="password">解压密码</param>  
        public static void UnZip (string UpZipFile , string ZipToFile , string password = "") {
            if (!System.IO.File.Exists(UpZipFile)) {
                throw new System.IO.FileNotFoundException("文件 " + UpZipFile + "不存在！");
            }
            //创建新的ZIP输入流  
            ZipInputStream ZipStream = new ZipInputStream(File.OpenRead(UpZipFile));
            if (!password.Equals("")) {
                //设置解压密码  
                ZipStream.Password = password;
            }
            ZipEntry theEntry;
            while ((theEntry = ZipStream.GetNextEntry()) != null) {
                //取得解压后的目录  
                string directoryName = Path.GetDirectoryName(ZipToFile);
                //取得解压文件下的文件名   
                string fileName = Path.GetFileName(theEntry.Name);
                //取得子目录  
                string filepath = Path.GetDirectoryName(theEntry.Name);
                //取得解压文件名  
                //如 xxxxx.zip  
                string ZipFile = Path.GetFileName(UpZipFile);
                //去掉文件名后缀 xxxxx  
                string zipfile = ZipFile.Split('.')[0];
                //创建解压后文件目录  
                string filePath = directoryName + "\\" + zipfile + "\\" + filepath + "\\";
                Directory.CreateDirectory(filePath);
                if (fileName != String.Empty) {
                    //解压文件到指定的目录      
                    FileStream streamWriter = File.Create(filePath + "\\" + fileName);
                    int size = 2048;
                    byte[] data = new byte[2048];
                    while (true) {
                        size = ZipStream.Read(data , 0 , data.Length);
                        if (size > 0) {
                            streamWriter.Write(data , 0 , size);
                        } else { break; }
                    }
                    streamWriter.Close();
                }
            }
            ZipStream.Close();
        }
        #endregion 解压文件
    }
}