/* Edgar Ruiz
 * CS 431
 * December 5, 2016
 */

using System;
using System.Collections;
using System.Collections.Generic;


namespace CosmosKernel1
{
    public class Commands
    {
        public static void interpret(String input)
        {
            Char[] inputC = input.ToCharArray();
            Boolean space = false;
            for (int i = 0; i < inputC.Length; i++)
            {
                if (inputC[i] == ' ')
                {
                    space = true;
                    break;
                }
            }
            if (space)
            {

                String command = "";
                Int32 index = 0;
                while (inputC[index] != ' ')
                {
                    command += inputC[index];
                    index++;
                }
                while (inputC[index] == ' ')
                {
                    index++;
                }
                Char[] afterC = new Char[inputC.Length - index];
                for (int i = 0; i < afterC.Length; i++)
                {
                    afterC[i] = inputC[index];
                    index++;
                }
                if (command.ToLower() == "create")
                {
                    Boolean sp = false;
                    for (int i = 0; i < afterC.Length; i++)
                    {
                        if (afterC[i] == ' ')
                        {
                            sp = true;
                        }
                    }
                    if (sp)
                    {
                        Console.WriteLine("Spaces are not allowed!");
                    }
                    else
                    {
                        create(afterC);
                    }

                }
                else if(command.ToLower() == "run")
                {
                    run(afterC);
                }
                else if(command.ToLower() == "runall")
                {
                    runAll(afterC);
                }
                else if (command.ToLower() == "set")
                {
                    Variable newVar = convertSetVariable(afterC);
                    LinkedListNode<Variable> temp = Kernel.variables.First;
                    Boolean found = false;
                    while (temp != null)
                    {
                        if (temp.Value.name == newVar.name)
                        {
                            temp.Value.setValue(newVar.value);
                            found = true;
                            break;
                        }
                        else
                            temp = temp.Next;
                    }
                    if (!found)
                        Kernel.variables.AddLast(newVar);
                }
                else if (command.ToLower() == "add" || command.ToLower() == "sub" || command.ToLower() == "mul" || command.ToLower() == "div")
                {
                    String com = command.ToLower();
                    Variable newVar = convertVariable(com, afterC);
                    LinkedListNode<Variable> temp = Kernel.variables.First;
                    Boolean found = false;
                    while (temp != null)
                    {
                        if (temp.Value.name == newVar.name)
                        {
                            temp.Value.setValue(newVar.value);
                            found = true;
                            break;
                        }
                        else
                            temp = temp.Next;
                    }
                    if (!found)
                        Kernel.variables.AddLast(newVar);
                }
                else if (command.ToLower() == "echo")
                {
                    String name = new String(afterC);
                    String var = output(afterC);
                    if (var == null)
                        Console.WriteLine(afterC);
                    else
                        Console.WriteLine(name + " = " + var);
                }
                else
                {
                    Console.WriteLine("Invalid Command");
                }
            }
            else
            {
                dirCommand(input);
            }
        }

        public static void dirCommand(String input)
        {
            if (input.ToLower() == "dir")
                Commands.dir();
            else
                Console.WriteLine("Invalid Command");
        }

        public static void create(Char[] full)
        {
            String name = "", ext = "";
            Boolean extension = false;
            for (int i = 0; i < full.Length; i++)
            {
                name += full[i];
                if (full[i] == '.')
                    extension = true;
                if (extension)
                    ext += full[i];
            }
            File file = new File(name, ext);
            String input = "";
            Int32 count = 1;
            while (input != "save")
            {
                Console.Write(count + ": ");
                input = Console.ReadLine();
                if (input != "save")
                {
                    file.setData(input);
                    count++;
                }
            }
            Console.WriteLine("File saved successfully!");
            Kernel.file_directory.AddLast(file);
        }

        public static void dir()
        {
            Console.WriteLine("Filename\tExtension\tDate\tSize");
            Console.WriteLine("****************************************");
            LinkedListNode<File> temp = Kernel.file_directory.First;
            while (temp != null)
            {
                Console.WriteLine(temp.Value.getName() + "\t" + temp.Value.getExtension() + "\t\t" + temp.Value.getDate() + "\t" + temp.Value.getSize() + " B");
                temp = temp.Next;
            }
            Console.WriteLine("Total Files: " + Kernel.file_directory.Count);
        }

        public static Variable convertVariable(String oper, Char[] var)
        {
            String name = "";
            Int32 index = var.Length - 1;
            while (var[index] != ' ')
            {
                index--;
            }
            index++;
            for (int i = index; i < var.Length; i++)
            {
                name += var[i];
            }
            Char[] sub = new Char[index - 1];
            for (int j = 0; j < sub.Length; j++)
            {
                sub[j] = var[j];
            }
            String value = evaluateOper(oper, sub);
            Variable v = new Variable(name, value);
            return v;
        }

        public static Variable convertSetVariable(Char[] var)
        {
            String name = "";
            Int32 index = 0;
            while (var[index] != ' ')
            {
                name += var[index];
                index++;
            }
            while (var[index] == ' ')
            {
                index++;
            }
            Char[] vals = new Char[var.Length - index];
            for (int i = 0; i < vals.Length; i++)
            {
                vals[i] = var[index];
                index++;
            }
            String value = evaluate(vals);
            Variable v = new Variable(name, value);
            return v;
        }

        public static String evaluateOper(String oper, Char[] vals)
        {
            String temp1 = "";
            String temp2 = "";
            String val1 = null;
            String val2 = null;
            Int32 index = 0;
            Int32 iTemp1 = 0;
            Int32 iTemp2 = 0;
            while (vals[index] != ' ')
            {
                temp1 += vals[index++];
            }
            Char[] cTemp1 = temp1.ToCharArray();
            index++;
            while (index < vals.Length)
            {
                temp2 += vals[index++];
            }
            Char[] cTemp2 = temp2.ToCharArray();
            if (checkIfContainsString(cTemp1))
            {
                val1 = output(cTemp1);

                if (val1 == null)
                {
                    Kernel.variables.AddLast(new Variable(temp1, "0"));
                    iTemp1 = 0;
                }
                else
                {
                    iTemp1 = Int32.Parse(val1);
                }
            }
            else
            {
                iTemp1 = Int32.Parse(temp1);
            }

            if (checkIfContainsString(cTemp2))
            {
                val2 = output(cTemp2);

                if (val2 == null)
                {
                    Kernel.variables.AddLast(new Variable(temp2, "0"));
                    iTemp2 = 0;
                }
                else
                {
                    iTemp2 = Int32.Parse(val2);
                }
            }
            else
            {
                iTemp2 = Int32.Parse(temp2);
            }

            String total = operate(oper, iTemp1, iTemp2);
            return total;
        }

        public static void run(Char[] numFile)
        {
            Int32 count = 1;
            Int32 counter = 0;
            String strCount = "";
            String name = "";
            String ext = "";
            Boolean e = false;
            Boolean space = false;
            for(int i = 0; i < numFile.Length; i++)
            {
                if(numFile[i] == ' ')
                {
                    space = true;
                    break;
                }
            }

            if (space)
            {
                while (numFile[counter] != ' ')
                {
                    strCount += numFile[counter];
                    counter++;
                }
                count = Int32.Parse(strCount);
                counter++;
            }
            for(int i = counter; i < numFile.Length; i++)
            {
                name += numFile[i];
                if (numFile[i] == '.')
                    e = true;
                if (e)
                    ext += numFile[i];
            }

            if(ext != ".bat")
            {
                Console.WriteLine("Must be a file with a .bat extension");
                return;
            }

            if (fileExists(name, ext) == null)
            {
                Console.WriteLine(name + "." + ext + " does not exist");
                return;
            }

            File file = new File(fileExists(name, ext));
            for(int j = 0; j < count; j++)
            {
                Kernel.readyQueue.AddLast(file);
                runFile();
            }
            Console.WriteLine("Run command successful");
        }

        public static void runFile()
        {
            File current = Kernel.readyQueue.First.Value;
            Kernel.readyQueue.RemoveFirst();
            ArrayList data = current.getData();
            for (int i = 0; i < data.Count; i++)
            { 
                interpret(data[i] as String);
            }           
        }

        public static File fileExists(String name, String ext)
        {
            LinkedListNode<File> temp = Kernel.file_directory.First;
            while(temp != null)
            {
                if(temp.Value.getName() == name)
                {
                    return temp.Value;
                }
                temp = temp.Next;
            }
            return null;
        }

        public static void runAll(Char[] files)
        {
            String strCount = "";
            Int32 count = 0;
            Int32 counter = 0;
            Int32 numFiles = 0;
            String name = "";
            String ext = "";
            Boolean e = false;
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i] == ' ')
                {
                    counter++;
                    break;
                }
                else
                {
                    strCount += files[i];
                    counter++;
                }
            }
            count = Int32.Parse(strCount);

            for (int j = counter; j < files.Length; j++)
            {
                if (files[j] != ' ')
                {
                    name += files[j];
                }

                if (files[j] == '.')
                {                   
                    e = true;
                }

                if (e)
                {
                    if (files[j] != ' ')
                    {
                        ext += files[j];                       
                    }
                    else
                    {
                        File file = new File(fileExists(name, ext));
                        Kernel.readyQueue.AddLast(file);
                        Kernel.masterQueue.AddLast(file);
                        numFiles++;
                        name = "";
                        ext = "";
                        e = false;
                    }
                }              
            }
            File lastFile = new File(fileExists(name, ext));
            Kernel.readyQueue.AddLast(lastFile);
            Kernel.masterQueue.AddLast(lastFile);
            numFiles++;
            executeAll(count, numFiles);
            Console.WriteLine("Completed runall command successfully");
        }

        public static void executeAll(Int32 count, Int32 numFiles)
        {
            Int32 lineCount = 0;
            Int32 currentFileNum = numFiles;
            Int32 deleteFileNum = 0;
            Int32 masterCount = Kernel.masterQueue.Count;
            Boolean cont = true;
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine("Round " + (i + 1));
                while (cont)
                {
                    for (int j = 0; j < currentFileNum; j++)
                    {
                        File current = Kernel.readyQueue.First.Value;
                        Kernel.readyQueue.RemoveFirst();
                        ArrayList data = current.getData();
                        if (lineCount < data.Count)
                        {
                            Console.WriteLine("Executing line " + lineCount + " in " + current.getName());                  
                            interpret(data[lineCount] as String);
                            Kernel.readyQueue.AddLast(current);
                        }
                        else
                        {
                            Console.WriteLine("Completed " + current.getName());
                            deleteFileNum++;
                        }
                    }
                    lineCount++;
                    currentFileNum -= deleteFileNum;
                    deleteFileNum = 0;
                    if(Kernel.readyQueue.Count == 0)
                    {
                        cont = false;
                    }
                }
                //reset all items to redo based on count value

                currentFileNum = numFiles;
                lineCount = 0;
                deleteFileNum = 0;
                cont = true;
                for (int k = 0; k < masterCount; k++)
                {
                    Kernel.readyQueue.AddLast(Kernel.masterQueue.First.Value);
                    Kernel.masterQueue.AddLast(Kernel.masterQueue.First.Value);
                    Kernel.masterQueue.RemoveFirst();
                }
            }
            
            //clear both queues
            for (int l = 0; l < masterCount; l++)
            {
                Kernel.readyQueue.RemoveFirst();
                Kernel.masterQueue.RemoveFirst();
            }
        }

        public static String operate(String oper, Int32 iTemp1, Int32 iTemp2)
        {
            Int32 total = 0;
            if (oper == "add")
            {
                total = iTemp1 + iTemp2;
            }
            else if (oper == "sub")
            {
                total = iTemp1 - iTemp2;
            }
            else if (oper == "mul")
            {
                total = iTemp1 * iTemp2;
            }
            else if (oper == "div")
            {
                if (iTemp2 != 0)
                {
                    total = iTemp1 / iTemp2;
                }
                else
                {
                    Console.Write("Cannot divide by 0. Value returned to variable is -1");
                    total = -1;
                }
            }
            return total.ToString();
        }
        public static String evaluate(Char[] vals)
        {
            String value;
            if (checkIfContainsString(vals))
            {
                value = evaluateString(vals);
            }
            else
            {
                value = evaluateNumeric(vals);
            }
            return value;
        }

        public static String evaluateString(Char[] vals)
        {
            String name = "";
            for (int i = 0; i < vals.Length; i++)
            {
                name += vals[i];
            }
            LinkedListNode<Variable> temp = Kernel.variables.First;
            while (temp != null)
            {
                if (temp.Value.name == name)
                    return temp.Value.getValueString();
                else
                    temp = temp.Next;
            }
            Kernel.variables.AddLast(new Variable(name, "0"));
            return "0";
        }

        public static String evaluateNumeric(Char[] vals)
        {
            String val = "";
            for (int i = 0; i < vals.Length; i++)
            {
                val += vals[i];
            }
            return val;
        }

        public static Boolean checkIfContainsString(Char[] input)
        {
            Char[] num = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < num.Length; j++)
                {
                    if (input[i] == num[j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static String output(Char[] var)
        {
            String varName = new String(var);
            LinkedListNode<Variable> temp = Kernel.variables.First;
            while (temp != null)
            {
                if (temp.Value.name == varName)
                    break;
                else
                    temp = temp.Next;
            }
            if (temp == null)
                return null;
            else
                return temp.Value.getValueString();
        }
    }
}
