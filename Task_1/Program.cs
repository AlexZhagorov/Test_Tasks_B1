using System;
using System.IO;
using System.Text;
using Task_1;

class Program
{
    static void Main()
    {
        //объединение файлов
        FileMerger.MergeFiles(); //D:\\7sem\\Test_Tasks\\Tasks\\Task_1\\files\\, D:\\7sem\\Test_Tasks\\Tasks\\Task_1\\compiled_file.txt

        //импорт файла в бд
        DataBaseImport.ImportData(); //D:\\7sem\\Test_Tasks\\Tasks\\Task_1\\files\\file_1.txt
    }
}