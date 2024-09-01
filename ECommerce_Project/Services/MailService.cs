using System.Net.Mail;
using System.Net;
using System.Windows;
using System.Collections.ObjectModel;
using ECommerce_Project.Entity.Models;
using System.Text;

namespace ECommerce_Project.Services;

public class MailService
{
    public static string OTPCode = "";
    public static void SendMail(string _destinationMail)
    {
        Random rand = new Random();
        OTPCode = rand.Next(1000, 10000).ToString();
        string body = $@"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>OTP Email</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f2f2f2;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 20px auto;
            padding: 20px;
            background-color: #fff;
            border-radius: 10px;
            box-shadow: 0px 0px 20px 0px rgba(0,0,0,0.1);
        }}
        .header {{
            background-color: #007bff;
            color: #fff;
            text-align: center;
            padding: 20px 0;
            border-radius: 10px 10px 0 0;
        }}
        h1 {{
            margin: 0;
            font-size: 24px;
        }}
        .content {{
            padding: 20px 0;
        }}
        p {{
            font-size: 16px;
            line-height: 1.6;
            margin-bottom: 15px;
            text-align: center;
        }}
        .otp {{
            font-size: 28px;
            font-weight: bold;
            color: #007bff;
            margin-bottom: 20px;
            text-align: center;
        }}
        .footer {{
            font-size: 14px;
            color: #888;
            text-align: center;
            padding-top: 20px;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <h1>OTP Code</h1>
        </div>
        <div class=""content"">
            <p>Your One-Time Password (OTP) is:</p>
            <p class=""otp"">{OTPCode}</p>
            <p>Please use this OTP to verify your identity.</p>
        </div>
        <div class=""footer"">
            This is an automated message. Please do not reply to this email.
        </div>
    </div>
</body>
</html>
";
        try
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("qasimov.vaqif512@gmail.com");
            mail.To.Add(_destinationMail);
            mail.Subject = "";
            mail.Body = body?.ToString() ?? "No content provided.";

            mail.IsBodyHtml = true;
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential("qasimov.vaqif512@gmail.com", "epma ppdx blox xgmv");
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }

        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    public static void SendMail(string _destinationMail, ObservableCollection<ProductView> productViews)
    {
        Func<int, string> generateRatingStars = rating =>
        {
            return new string('★', rating) + new string('☆', 5 - rating);
        };
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<html>");
        sb.AppendLine("<head>");
        sb.AppendLine("<style>");
        sb.AppendLine("body { font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; }");
        sb.AppendLine(".container { max-width: 800px; margin: 20px auto; background-color: #ffffff; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); }");
        sb.AppendLine("table { width: 100%; border-collapse: collapse; }");
        sb.AppendLine("th, td { padding: 12px; border: 1px solid #dddddd; text-align: left; }");
        sb.AppendLine("th { background-color: #f8f8f8; }");
        sb.AppendLine(".image-cell img { width: 60px; height: 60px; border-radius: 30px; }");
        sb.AppendLine(".rating { color: #FFD700; font-size: 16px; }");
        sb.AppendLine(".success-header { font-size: 24px; font-weight: bold; color: #28a745; margin-bottom: 20px; }");
        sb.AppendLine(".description { font-size: 16px; color: #333; margin-bottom: 20px; }");
        sb.AppendLine("</style>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");
        sb.AppendLine("<div class='container'>");
        sb.AppendLine("<div class='success-header'>Successful Purchase!</div>");
        sb.AppendLine("<div class='description'>Thank you for your purchase. Below is the list of products you have bought. We hope you enjoy your Products!</div>");
        sb.AppendLine("<table>");
        sb.AppendLine("<thead>");
        sb.AppendLine("<tr>");
        sb.AppendLine("<th>Image</th>");
        sb.AppendLine("<th>Product Details</th>");
        sb.AppendLine("<th>Rating</th>");
        sb.AppendLine("<th>Quantity</th>");
        sb.AppendLine("<th>Total Price</th>");
        sb.AppendLine("</tr>");
        sb.AppendLine("</thead>");
        sb.AppendLine("<tbody>");
        foreach (var p in productViews)
        {
            var ratingStars = generateRatingStars((int)p.Product.RatingView);
            sb.AppendLine("<tr>");
            sb.AppendLine($"<td class='image-cell'><img src='{p.Product.CoverImage}' alt='Product Image'/></td>");
            sb.AppendLine($"<td><strong>{p.Product.ProductName}</strong><br/><em>{p.Product.Category.Name}</em></td>");
            sb.AppendLine($"<td><div class='rating'>{ratingStars}</div></td>");
            sb.AppendLine($"<td>{p.Count}</td>"); sb.AppendLine($"<td>${p.TotalPrice}</td>");
            sb.AppendLine("</tr>");
        }
        sb.AppendLine("</tbody>");
        sb.AppendLine("</table>");
        sb.AppendLine("</div>");
        sb.AppendLine("<div class='footer'>");
        sb.AppendLine("<p>If you have any questions or need further assistance, please contact us:</p>");
        sb.AppendLine("<p>Email: <a href='qasimov.vaqif512@gmail.com'>qasimov.vaqif512@gmail.com</a></p>");
        sb.AppendLine("<p>Phone: +994 77 777 77 77");
        sb.AppendLine("<p>Address: Shamkir, Azerbaijan</p>");
        sb.AppendLine("</div>");
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");
        var body = sb.ToString();
        try
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("qasimov.vaqif512@gmail.com");
            mail.To.Add(_destinationMail);
            mail.Subject = "";
            mail.Body = body?.ToString() ?? "No content provided.";

            mail.IsBodyHtml = true;
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential("qasimov.vaqif512@gmail.com", "epma ppdx blox xgmv");
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }

        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}