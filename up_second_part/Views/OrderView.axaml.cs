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

            // ������� PDF-��������
            PdfSharpCore.Pdf.PdfDocument document = new PdfSharpCore.Pdf.PdfDocument();
            PdfSharpCore.Pdf.PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Comic Sans MS", 12);
            XFont font1 = new XFont("Comic Sans MS", 20);

            // ��������� ������
            string products = string.Join(", ", order.OrderProducts.Select(w => w.ProductArticleNumberNavigation.ProductName + " " + w.ProductCount.ToString() + " " + w.ProductArticleNumberNavigation.ProductMeasurementUnitNavigation.MeasurementUnitName));
            gfx.DrawString("����� ��� ���������", font, XBrushes.Black, new XPoint(50, 50));
            gfx.DrawString($"���� ������: {order.OrderDate}", font, XBrushes.Black, new XPoint(50, 70));
            gfx.DrawString($"���� ��������: {order.OrderDeliveryDate}", font, XBrushes.Black, new XPoint(50, 90));
            gfx.DrawString($"����� ������: {order.OrderId}", font, XBrushes.Black, new XPoint(50, 110));
            gfx.DrawString($"����� ������: {order.OrderSum}", font, XBrushes.Black, new XPoint(50, 130));
            gfx.DrawString($"����� ������: {order.OrderDiscountSum}", font, XBrushes.Black, new XPoint(50, 150));
            gfx.DrawString($"����� ������: {order.OrderPickupPointNavigation.PickupPointAddress}", font, XBrushes.Black, new XPoint(50, 170));
            gfx.DrawString($"������ ������: {products}", font, XBrushes.Black, new XPoint(50, 190));
            gfx.DrawString($"��� ���������: {order.OrderReceiptCode}", font1, XBrushes.Black, new XPoint(50, 240));

            // ������ ���������� �����
            var saveDialog = new SaveFileDialog
            {
                Title = "��������� PDF",
                DefaultExtension = "pdf",
                Filters = { new FileDialogFilter { Name = "PDF-�����", Extensions = { "pdf" } } },
                InitialFileName = "�����.pdf"
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
            MessageBoxManager.GetMessageBoxStandard("������", ex.Message, MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowAsync();
        }
    }
}