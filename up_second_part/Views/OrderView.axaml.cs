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
            // ������� PDF-��������
            PdfSharpCore.Pdf.PdfDocument document = new PdfSharpCore.Pdf.PdfDocument();
            PdfSharpCore.Pdf.PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Arial", 12);

            // ��������� ������
            gfx.DrawString("������ ���������", font, XBrushes.Black, new XPoint(50, 50));
            gfx.DrawString($"����: {DateTime.Now}", font, XBrushes.Black, new XPoint(50, 70));

            // ������ ���������� �����
            var saveDialog = new SaveFileDialog
            {
                Title = "��������� PDF",
                DefaultExtension = "pdf",
                Filters = { new FileDialogFilter { Name = "PDF-�����", Extensions = { "pdf" } } },
                InitialFileName = "document.pdf"
            };

            var parentWindow = this.VisualRoot as Window;
            var filePath = await saveDialog.ShowAsync(parentWindow); // ����������� �������� �������

            if (filePath != null)
            {
                document.Save(filePath); // ��������� �� ���������� ����
                document.Close();

                // �����������: ����������� �� �������� ����������
                //var messageBox = new MessageBox("�����", "���� ������� ��������!");
                //await messageBox.ShowDialog(this);
            }
        }
        catch (Exception ex)
        {
            // ��������� ������ (��������, ������������ ����)
            //var errorBox = new MessageBox("������", $"�� ������� ��������� ����: {ex.Message}");
            //await errorBox.ShowDialog(this);
        }
    }

    //private async void GeneratePdf(object sender, RoutedEventArgs e)
    //{
    //    var parentWindow = this.VisualRoot as Window;
    //    var saveDialog = new SaveFileDialog
    //    {
    //        Title = "��������� PDF",
    //        DefaultExtension = "pdf",
    //        Filters = { new FileDialogFilter { Name = "PDF", Extensions = { "pdf" } } },
    //        InitialFileName = "report.pdf"
    //    };

    //    var filePath = await saveDialog.ShowAsync(parentWindow);

    //    if (filePath != null)
    //    {
    //        try
    //        {
    //            // ������� PDF-��������
    //            QuestPDF.Fluent.Document.Create(container =>
    //            {
    //                container.Page(page =>
    //                {
    //                    page.Content()
    //                        .Padding(20)
    //                        .Text("������ ���������")
    //                        .FontSize(14);
    //                });
    //            })
    //            .GeneratePdf(filePath); // ��������� �������� �� ���������� ����

    //            // ���������� ������������ �� �������� ����������
    //            //var messageBox = new MessageBox("�����", "���� ������� ��������!");
    //            //await messageBox.ShowDialog(this);
    //        }
    //        catch (Exception ex)
    //        {
    //            // ��������� ������
    //            Console.WriteLine(ex.Message);
    //            //var errorBox = new MessageBox("������", $"�� ������� ��������� ����: {ex.Message}");
    //            //await errorBox.ShowDialog(this);
    //        }
    //    }
    //}


}