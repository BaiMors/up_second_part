using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Reflection.Metadata;
using up_second_part.ViewModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using PdfSharpCore;
using PdfSharpCore.Drawing;
using System;
using PdfSharp.Pdf;

namespace up_second_part;

public partial class OrderView : UserControl
{
    public OrderView()
    {
        InitializeComponent();
        DataContext = new ProductsVM();
    }

    private async void GeneratePdf(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        try
        {
            // Создаем PDF-документ
            PdfSharpCore.Pdf.PdfDocument document = new PdfSharpCore.Pdf.PdfDocument();
            PdfSharpCore.Pdf.PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Arial", 12);

            // Добавляем данные
            gfx.DrawString("Пример документа", font, XBrushes.Black, new XPoint(50, 50));
            gfx.DrawString($"Дата: {DateTime.Now}", font, XBrushes.Black, new XPoint(50, 70));

            // Диалог сохранения файла
            var saveDialog = new SaveFileDialog
            {
                Title = "Сохранить PDF",
                DefaultExtension = "pdf",
                Filters = { new FileDialogFilter { Name = "PDF-файлы", Extensions = { "pdf" } } },
                InitialFileName = "document.pdf"
            };

            var parentWindow = this.VisualRoot as Window;
            var filePath = await saveDialog.ShowAsync(parentWindow); // Асинхронное открытие диалога

            if (filePath != null)
            {
                document.Save(filePath); // Сохраняем по выбранному пути
                document.Close();

                // Опционально: уведомление об успешном сохранении
                //var messageBox = new MessageBox("Успех", "Файл успешно сохранен!");
                //await messageBox.ShowDialog(this);
            }
        }
        catch (Exception ex)
        {
            // Обработка ошибок (например, недостаточно прав)
            //var errorBox = new MessageBox("Ошибка", $"Не удалось сохранить файл: {ex.Message}");
            //await errorBox.ShowDialog(this);
        }
    }

    //private async void GeneratePdf(object sender, RoutedEventArgs e)
    //{
    //    var parentWindow = this.VisualRoot as Window;
    //    var saveDialog = new SaveFileDialog
    //    {
    //        Title = "Сохранить PDF",
    //        DefaultExtension = "pdf",
    //        Filters = { new FileDialogFilter { Name = "PDF", Extensions = { "pdf" } } },
    //        InitialFileName = "report.pdf"
    //    };

    //    var filePath = await saveDialog.ShowAsync(parentWindow);

    //    if (filePath != null)
    //    {
    //        try
    //        {
    //            // Создаем PDF-документ
    //            QuestPDF.Fluent.Document.Create(container =>
    //            {
    //                container.Page(page =>
    //                {
    //                    page.Content()
    //                        .Padding(20)
    //                        .Text("Пример документа")
    //                        .FontSize(14);
    //                });
    //            })
    //            .GeneratePdf(filePath); // Сохраняем документ по выбранному пути

    //            // Уведомляем пользователя об успешном сохранении
    //            //var messageBox = new MessageBox("Успех", "Файл успешно сохранен!");
    //            //await messageBox.ShowDialog(this);
    //        }
    //        catch (Exception ex)
    //        {
    //            // Обработка ошибок
    //            Console.WriteLine(ex.Message);
    //            //var errorBox = new MessageBox("Ошибка", $"Не удалось сохранить файл: {ex.Message}");
    //            //await errorBox.ShowDialog(this);
    //        }
    //    }
    //}


}