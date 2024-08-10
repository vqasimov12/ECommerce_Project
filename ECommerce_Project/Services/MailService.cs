using System.Net.Mail;
using System.Net;
using System.Windows;

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
}