using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Office.Interop.Word;
using RealEstateAgency.DataAccess.Models;
using Word = Microsoft.Office.Interop.Word;

namespace RealEstateAgency.WPF.Services
{
    public class WordReportGenerator
    {
        //Список всех сотрудников
        public void GenerateAllEmployeesReport(List<Employee> employees)
        {
            var wordApp = new Word.Application();
            wordApp.Visible = true;
            var doc = wordApp.Documents.Add();

            var paragraph = doc.Paragraphs.Add();
            paragraph.Range.Text = "Отчет: Список сотрудников";
            paragraph.Range.Font.Bold = 1;
            paragraph.Range.InsertParagraphAfter();

            var table = doc.Tables.Add(paragraph.Range, employees.Count + 1, 4);
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

        // Договора конкретного сотрудника
        public void GenerateEmployeeContractsReport(string employeeName, List<Contract> contracts)
        {
            var wordApp = new Word.Application();
            wordApp.Visible = true;
            var doc = wordApp.Documents.Add();

            var p = doc.Paragraphs.Add();
            p.Range.Text = $"Отчет по договорам сотрудника: {employeeName}";
            p.Range.Font.Bold = 1;
            p.Range.InsertParagraphAfter();

            if (contracts.Count > 0)
            {
                var table = doc.Tables.Add(p.Range, contracts.Count + 1, 5);
                table.Borders.Enable = 1;

                table.Cell(1, 1).Range.Text = "№ Договора";
                table.Cell(1, 2).Range.Text = "Дата";
                table.Cell(1, 3).Range.Text = "Клиент";
                table.Cell(1, 4).Range.Text = "Услуга";
                table.Cell(1, 5).Range.Text = "Стоимость";

                for (int i = 0; i < contracts.Count; i++)
                {
                    table.Cell(i + 2, 1).Range.Text = contracts[i].ContractNumber;
                    table.Cell(i + 2, 2).Range.Text = contracts[i].ContractDate.ToShortDateString();
                    table.Cell(i + 2, 3).Range.Text = contracts[i].ClientName;
                    table.Cell(i + 2, 4).Range.Text = contracts[i].ServiceName;
                    table.Cell(i + 2, 5).Range.Text = contracts[i].ServiceCost.ToString("C");
                }
            }
            else
            {
                doc.Paragraphs.Add().Range.Text = "Договоров не найдено.";
            }
        }

        // Группировка и итоги (Статистика сотрудников)
        public void GenerateStatsReport(System.Data.DataTable dt)
        {
            var wordApp = new Word.Application();
            wordApp.Visible = true;
            var doc = wordApp.Documents.Add();

            var p = doc.Paragraphs.Add();
            p.Range.Text = "Отчет: Статистика эффективности сотрудников за текущий месяц";
            p.Range.Font.Bold = 1;
            p.Range.InsertParagraphAfter();

            var table = doc.Tables.Add(p.Range, dt.Rows.Count + 2, 3);
            table.Borders.Enable = 1;

            table.Cell(1, 1).Range.Text = "Сотрудник";
            table.Cell(1, 2).Range.Text = "Кол-во услуг";
            table.Cell(1, 3).Range.Text = "Общая прибыль";

            decimal totalProfit = 0;
            int totalServices = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                table.Cell(i + 2, 1).Range.Text = row["FullName"].ToString();
                table.Cell(i + 2, 2).Range.Text = row["ServicesCount"].ToString();
                table.Cell(i + 2, 3).Range.Text = Convert.ToDecimal(row["TotalProfit"]).ToString("C");

                totalServices += Convert.ToInt32(row["ServicesCount"]);
                totalProfit += Convert.ToDecimal(row["TotalProfit"]);
            }

            int lastRow = dt.Rows.Count + 2;
            table.Cell(lastRow, 1).Range.Text = "ИТОГО:";
            table.Cell(lastRow, 1).Range.Font.Bold = 1;
            table.Cell(lastRow, 2).Range.Text = totalServices.ToString();
            table.Cell(lastRow, 3).Range.Text = totalProfit.ToString("C");
        }
    }
}