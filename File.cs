/* Edgar Ruiz
 * CS 431
 * December 5, 2016
 */

using System;
using System.Collections;

namespace CosmosKernel1
{
    public class File
    {
        String fileName;
        String extension;
        String date;
        Int32 size;
        Int32 line;
        ArrayList data;

        public File(String n, String e)
        {
            fileName = n;
            extension = e;
            date = "12/9/2016";
            size = 0;
            line = 0;
            data = new ArrayList();
        }

        public File(File f)
        {
            fileName = f.fileName;
            extension = f.extension;
            date = f.date;
            size = f.size;
            line = 0;
            data = f.data;
        }

        public void setData(String input)
        {
            data.Add(input);
            Char[] s = input.ToCharArray();
            Int32 si = s.Length;
            if (size == 0)
            {
                size += si;
            }
            else
            {
                size = size + si + 2; //2 bytes for new line and return 
            }
        }

        public String getName()
        {
            return fileName;
        }

        public String getExtension()
        {
            return extension;
        }

        public String getDate()
        {
            return date;
        }

        public Int32 getSize()
        {
            return size;
        }

        public Int32 getLine()
        {
            return line;
        }

        public ArrayList getData()
        {
            return data;
        }

        public void setLine(Int32 num)
        {
            line = num;
        }

        public void resetLine()
        {
            line = 0;
        }

        public void incrementLine()
        {
            line++;
        }

    }
}
