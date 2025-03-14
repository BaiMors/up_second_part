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
using System.Collections.Generic;
using System.Linq;
using PdfSharp.Pdf;
using up_second_part.Models;
using MsBox.Avalonia;

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
            Order order = MainWindowViewModel.myConnection.Orders.Where(x => x.OrderId == MainWindowViewModel.myConnection.Orders.Max(x => x.OrderId)).First();

            var parentWindow = this.VisualRoot as Window;

            // Создаем PDF-документ
            PdfSharpCore.Pdf.PdfDocument document = new PdfSharpCore.Pdf.PdfDocument();
            PdfSharpCore.Pdf.PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Comic Sans MS", 12);
            XFont font1 = new XFont("Comic Sans MS", 20);

            // Добавляем данные
            string products = string.Join(", ", order.OrderProducts.Select(w => w.ProductArticleNumberNavigation.ProductName + " " + w.ProductCount.ToString() + " " + w.ProductArticleNumberNavigation.ProductMeasurementUnitNavigation.MeasurementUnitName));
            gfx.DrawString("Талон для получения", font, XBrushes.Black, new XPoint(50, 50));
            gfx.DrawString($"Дата заказа: {order.OrderDate}", font, XBrushes.Black, new XPoint(50, 70));
            gfx.DrawString($"Дата доставки: {order.OrderDeliveryDate}", font, XBrushes.Black, new XPoint(50, 90));
            gfx.DrawString($"Номер заказа: {order.OrderId}", font, XBrushes.Black, new XPoint(50, 110));
            gfx.DrawString($"Сумма заказа: {order.OrderSum}", font, XBrushes.Black, new XPoint(50, 130));
            gfx.DrawString($"Сумма скидки: {order.OrderDiscountSum}", font, XBrushes.Black, new XPoint(50, 150));
            gfx.DrawString($"Пункт выдачи: {order.OrderPickupPointNavigation.PickupPointAddress}", font, XBrushes.Black, new XPoint(50, 170));
            gfx.DrawString($"Состав заказа: {products}", font, XBrushes.Black, new XPoint(50, 190));
            gfx.DrawString($"Код получения: {order.OrderReceiptCode}", font1, XBrushes.Black, new XPoint(50, 240));

            // Диалог сохранения файла
            var saveDialog = new SaveFileDialog
            {
                Title = "Сохранить PDF",
                DefaultExtension = "pdf",
                Filters = { new FileDialogFilter { Name = "PDF-файлы", Extensions = { "pdf" } } },
                InitialFileName = "Талон.pdf"
            };

            
            var filePath = await saveDialog.ShowAsync(parentWindow);

            if (filePath != null)
            {
                document.Save(filePath);
                document.Close();
            }
        }
        catch (Exception ex)
        {
            MessageBoxManager.GetMessageBoxStandard("Ошибка", ex.Message, MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
        }
    }
}