using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Task_2.Context;
using Task_2.Models;
using System.Data;
using Microsoft.IdentityModel.Tokens;

namespace Task_2.Controllers
{
    public class FileUploadController : Controller      //класс контроллера, отвечающий за работу с файлами
    {
        private readonly AppDbContext _context;

        public FileUploadController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()                    //действие, возвращающее информацию о файлах из БД
        {
            var files = _context.FileRecords.ToList();
            return View(files);
        }

        [HttpPost]
        public IActionResult UploadFile(IFormFile file) //действие, загружающее новый файл в БД
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);

                var tempFilePath = Path.Combine(Path.GetTempPath(), fileName);
                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                Application excel = new Application();
                Workbook workbook = excel.Workbooks.Open(tempFilePath);
                Worksheet worksheet = workbook.Sheets[1];

                var cells = worksheet.Cells;            //получение ячеек из Excel-таблицы файла

                var fileRecord = new FileRecord()       //формирование объекта для записи информации о файле в БД
                {
                    FileName = fileName,
                    BankName = cells[1, 1].Value.ToString(),
                    Period = cells[3, 1].Value.ToString(),
                    Date = cells[6, 1].Value,
                    Currency = cells[6, 7].Value.ToString()
                };
                _context.FileRecords.Add(fileRecord);
                _context.SaveChanges();

                Microsoft.Office.Interop.Excel.Range range = worksheet.UsedRange;
                for (int row = 10; row < range.Rows.Count; row++)       //цикл считывания информации из ячеек Excel-таблицы файла
                {
                    if (cells[row, 1].Value is string && cells[row, 1].Value.Contains("КЛАСС") && cells[row, 1].Value.ToString() != "ПО КЛАССУ")
                    {
                        continue;
                    }
                    else if (cells[row, 1].Value.ToString() == "ПО КЛАССУ")
                    {
                        var classAccount = new ClassAccountRecord()
                        {
                            FileId = fileRecord.FileId,
                            AccountNumber = Convert.ToInt32(worksheet.Cells[row - 1, 1].Value) / 10,        //итог по классу нумеруем однозначными числами
                            ActiveIn = worksheet.Cells[row, 2].Value,
                            PassiveIn = worksheet.Cells[row, 3].Value,
                            Debet = worksheet.Cells[row, 4].Value,
                            Credit = worksheet.Cells[row, 5].Value,
                            ActiveOut = worksheet.Cells[row, 6].Value,
                            PassiveOut = worksheet.Cells[row, 7].Value
                        };
                        _context.ClassAccountRecords.Add(classAccount);
                    }
                    else if (cells[row, 1].Value.ToString() == "БАЛАНС")
                    {
                        var classAccount = new ClassAccountRecord()
                        {
                            FileId = fileRecord.FileId,
                            AccountNumber = 0,                          //для итогового баланса присваиваем номер счёта 0, объединяя с итогами по классам
                            ActiveIn = worksheet.Cells[row, 2].Value,
                            PassiveIn = worksheet.Cells[row, 3].Value,
                            Debet = worksheet.Cells[row, 4].Value,
                            Credit = worksheet.Cells[row, 5].Value,
                            ActiveOut = worksheet.Cells[row, 6].Value,
                            PassiveOut = worksheet.Cells[row, 7].Value
                        };
                        _context.ClassAccountRecords.Add(classAccount);
                    }
                    else if (Convert.ToInt32(cells[row, 1].Value) >= 1000 && Convert.ToInt32(cells[row, 1].Value) < 10000)
                    {
                        var account = new AccountRecord()
                        {
                            FileId = fileRecord.FileId,
                            AccountNumber = Convert.ToInt32(worksheet.Cells[row, 1].Value),         //4- и 2-значные счета получают свой номер из файла
                            ActiveIn = worksheet.Cells[row, 2].Value,
                            PassiveIn = worksheet.Cells[row, 3].Value,
                            Debet = worksheet.Cells[row, 4].Value,
                            Credit = worksheet.Cells[row, 5].Value,
                            ActiveOut = worksheet.Cells[row, 6].Value,
                            PassiveOut = worksheet.Cells[row, 7].Value
                        };
                        _context.AccountRecords.Add(account);
                    }
                    else if (Convert.ToInt32(cells[row, 1].Value) >= 10 && Convert.ToInt32(cells[row, 1].Value) < 100)
                    {
                        var totalAccount = new TotalAccountRecord()
                        {
                            FileId = fileRecord.FileId,
                            AccountNumber = Convert.ToInt32(worksheet.Cells[row, 1].Value),
                            ActiveIn = worksheet.Cells[row, 2].Value,
                            PassiveIn = worksheet.Cells[row, 3].Value,
                            Debet = worksheet.Cells[row, 4].Value,
                            Credit = worksheet.Cells[row, 5].Value,
                            ActiveOut = worksheet.Cells[row, 6].Value,
                            PassiveOut = worksheet.Cells[row, 7].Value
                        };
                        _context.TotalAccountRecords.Add(totalAccount);
                    }
                }
                _context.SaveChanges();

                // Закрываем файл и очищаем ресурсы
                workbook?.Close(false);
                excel.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);

                // Удаляем временный файл
                System.IO.File.Delete(tempFilePath);
            }    

            return RedirectToAction("Index");
        }

        public IActionResult ViewFile(int id) //действие для просмотра содержимого файла
        {
            var accountsData = _context.AccountRecords
                            .Where(f => f.FileId == id)
                            .OrderBy(f => f.AccountNumber)
                            .ToList();
            var totalAccountsData = _context.TotalAccountRecords
                            .Where(f => f.FileId == id)
                            .OrderBy(f => f.AccountNumber)
                            .ToList();
            var classAccountsData = _context.ClassAccountRecords
                            .Where(f => f.FileId == id)
                            .OrderBy(f => f.AccountNumber)
                            .ToList();
            var fileRecordData = _context.FileRecords
                            .Where(f => f.FileId == id)
                            .ToList();

            List<string> classHeaders = new List<string>() 
            {   
                "КЛАСС  1  Денежные средства, драгоценные металлы и межбанковские операции",
                "КЛАСС  2  Кредитные и иные активные операции с клиентами",
                "КЛАСС  3  Счета по операциям клиентов",
                "КЛАСС  4  Ценные бумаги",
                "КЛАСС  5  Долгосрочные финансовые вложения в уставные фонды юридических лиц, основные средства и прочее имущество",
                "КЛАСС  6  Прочие активы и прочие пассивы",
                "КЛАСС  7  Собственный капитал банка",
                "КЛАСС  8  Доходы банка",
                "КЛАСС  9  Расходы банка"
            };

            return View((accountsData, totalAccountsData, classAccountsData, classHeaders, fileRecordData));
        }
    }
}
