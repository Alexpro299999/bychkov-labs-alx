using System;
using System.Collections;
using System.Collections.Generic;
using RealEstateAgency.EF.DataAccess.Models;
using Word = Microsoft.Office.Interop.Word;

namespace RealEstateAgency.EF.WPF.Services
{
    public class WordReportGenerator
    {
        public void GenerateAllEmployeesReport(List<Employee> employees)
        {
            var wordApp = new Word.Application { Visible = true };
            var doc = wordApp.Documents.Add();

            var p = doc.Paragraphs.Add();
            p.Range.Text = "Список сотрудников";
            p.Range.Font.Bold = 1;
            p.Range.InsertParagraphAfter();

            var table = doc.Tables.Add(p.Range, employees.Count + 1, 4);
            table.Borders.Enable = 1;

            table.Cell(1, 1).Range.Text = "ФИО";
            table.Cell(1, 2).Range.Text = "Должность";
            table.Cell(1, 3).Range.Text = "Телефон";
            table.Cell(1, 4).Range.Text = "Образование";

            for (int i = 0; i < employees.Count; i++)
            {
                table.Cell(i + 2, 1).Range.Text = employees[i].FullName;
                table.Cell(i + 2, 2).Range.Text = employees[i].Position;
                table.Cell(i + 2, 3).Range.Text = employees[i].Phone;
                table.Cell(i + 2, 4).Range.Text = employees[i].Education;
            }
        }

        public void GenerateEmployeeContractsReport(string employeeName, List<Contract> contracts)
        {
            var wordApp = new Word.Application { Visible = true };
            var doc = wordApp.Documents.Add();

            var p = doc.Paragraphs.Add();
            p.Range.Text = $"Договора сотрудника: {employeeName}";
            p.Range.Font.Bold = 1;
            p.Range.InsertParagraphAfter();

            if (contracts.Count > 0)
            {
                var table = doc.Tables.Add(p.Range, contracts.Count + 1, 5);
                table.Borders.Enable = 1;

                table.Cell(1, 1).Range.Text = "№";
                table.Cell(1, 2).Range.Text = "Дата";
                table.Cell(1, 3).Range.Text = "Клиент";
                table.Cell(1, 4).Range.Text = "Услуга";
                table.Cell(1, 5).Range.Text = "Цена";

                for (int i = 0; i < contracts.Count; i++)
                {
                    table.Cell(i + 2, 1).Range.Text = contracts[i].ContractNumber;
                    table.Cell(i + 2, 2).Range.Text = contracts[i].ContractDate.ToShortDateString();
                    table.Cell(i + 2, 3).Range.Text = contracts[i].ClientName;
                    table.Cell(i + 2, 4).Range.Text = contracts[i].Service.Name;
                    table.Cell(i + 2, 5).Range.Text = contracts[i].Service.Cost.ToString("C");
                }
            }
            else
            {
                doc.Paragraphs.Add().Range.Text = "Нет данных.";
            }
        }

        public void GenerateStatsReport(IEnumerable stats)
        {
            var wordApp = new Word.Application { Visible = true };
            var doc = wordApp.Documents.Add();

            var p = doc.Paragraphs.Add();
            p.Range.Text = "Статистика эффективности";
            p.Range.Font.Bold = 1;
            p.Range.InsertParagraphAfter();

            int count = 0;
            foreach (var item in stats) count++;

            var table = doc.Tables.Add(p.Range, count + 1, 3);
            table.Borders.Enable = 1;

            table.Cell(1, 1).Range.Text = "Сотрудник";
            table.Cell(1, 2).Range.Text = "Кол-во";
            table.Cell(1, 3).Range.Text = "Прибыль";

            int row = 2;
            foreach (dynamic item in stats)
            {
                table.Cell(row, 1).Range.Text = item.FullName;
                table.Cell(row, 2).Range.Text = item.ServicesCount.ToString();
                table.Cell(row, 3).Range.Text = item.TotalProfit.ToString("C");
                row++;
            }
        }
    }
}