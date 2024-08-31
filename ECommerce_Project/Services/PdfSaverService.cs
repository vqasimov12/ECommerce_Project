using ECommerce_Project.Entity.Models;
using PdfSharpCore.Drawing;
using System.Diagnostics;
using PdfSharpCore.Pdf;
using System.IO;

namespace ECommerce_Project.Services;
public class PdfSaverService
{
    public void ExportSelectedOrderToPdf(Order selectedOrder)
    {
        PdfDocument document = new PdfDocument();
        PdfPage page = document.AddPage();
        XGraphics Xgraph = XGraphics.FromPdfPage(page);
        XFont titleFont = new XFont("Verdana", 20, XFontStyle.Bold);
        XFont headerFont = new XFont("Verdana", 16, XFontStyle.Bold);
        XFont bodyFont = new XFont("Times New Roman", 12, XFontStyle.Regular);

        int yPosition = 50;

        Xgraph.DrawString("Order Summary", titleFont, XBrushes.Black, new XRect(0, yPosition, page.Width, page.Height), XStringFormats.TopCenter);
        yPosition += 40;

        Xgraph.DrawString($"Order ID: {selectedOrder.Id}", headerFont, XBrushes.Black, new XPoint(50, yPosition));
        yPosition += 30;
        Xgraph.DrawString($"User: {selectedOrder.User.Name} {selectedOrder.User.Surname}", headerFont, XBrushes.Black, new XPoint(50, yPosition));
        yPosition += 20;
        Xgraph.DrawString($"Email: {selectedOrder.User.Email}", bodyFont, XBrushes.Black, new XPoint(50, yPosition));
        yPosition += 40;

        Xgraph.DrawString($"Total Price: ", bodyFont, XBrushes.Black, new XPoint(50, yPosition));
        Xgraph.DrawString($"{selectedOrder.TotalPrice:C}", bodyFont, XBrushes.Black, new XPoint(130, yPosition));
        yPosition += 20;
        Xgraph.DrawString($"Order Date: ", bodyFont, XBrushes.Black, new XPoint(50, yPosition));
        Xgraph.DrawString($"{selectedOrder.OrderDate:dd/MM/yyyy}", bodyFont, XBrushes.Black, new XPoint(130, yPosition));
        yPosition += 20;
        Xgraph.DrawString($"Delivery Date:", bodyFont, XBrushes.Black, new XPoint(50, yPosition));
        Xgraph.DrawString($" {selectedOrder.DeliveryDate:dd/MM/yyyy}", bodyFont, XBrushes.Black, new XPoint(127, yPosition));
        yPosition += 40;

        // Products List
        Xgraph.DrawString("Products:", headerFont, XBrushes.Black, new XPoint(50, yPosition));
        yPosition += 30;
        int i = 1;

        foreach (var productView in selectedOrder.Products)
        {
            string s = (i < 10 ? "  " : " ");
            Xgraph.DrawString($"{i}.{s}Product Name:  {productView.Product.ProductName}", bodyFont, XBrushes.Black, new XPoint(50, yPosition));
            yPosition += 20;
            Xgraph.DrawString($"  Category:          {productView.Product.Category.Name}", bodyFont, XBrushes.Black, new XPoint(60, yPosition));
            yPosition += 20;
            Xgraph.DrawString($"  Quantity:           {productView.Count}", bodyFont, XBrushes.Black, new XPoint(60, yPosition));
            yPosition += 20;
            Xgraph.DrawString($"  Price:                 {productView.Product.Price:C}", bodyFont, XBrushes.Black, new XPoint(60, yPosition));
            yPosition += 30;
            i++;
        }

        string fileName = $"Order_{selectedOrder.Id}.pdf";
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
        document.Save(filePath);
        Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
    }

}
