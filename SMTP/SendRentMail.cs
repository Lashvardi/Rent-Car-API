using SendGrid.Helpers.Mail;
using SendGrid;
using Org.BouncyCastle.Math.EC.Multiplier;

namespace RentCar.SMTP;

public class SendRentMail
{
    public async Task SendRentEmail(string recipientEmail, string CarModel, string CarMark, double PricePaid, int multiplier)
    {
        // SendGrid
        // https://app.sendgrid.com/guide/integrate/langs/csharp
        // https://app.sendgrid.com/settings/api_keys
        var client = new SendGridClient("<YOUR TOKEN HERE>");
        var from = new EmailAddress("<YOUR MAIL ADDRESS>", "Montez");
        var to = new EmailAddress(recipientEmail);
        var subject = "მანქანა გაქირავდა!";
        var plainTextContent = $"თქვენი მანქანა {CarMark} {CarModel}, {multiplier }დღით{PricePaid} Lari!";
        var htmlContent = $@"
    <html>
    <head>
        <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #f2f2f2;
            }}
            .container {{
                max-width: 600px;
                margin: 0 auto;
                padding: 20px;
                background-color: #fff;
                border-radius: 5px;
                box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
            }}
            h1 {{
                color: #333;
                text-align: center;
            }}
            .details {{
                margin-top: 30px;
                padding: 20px;
                background-color: #f9f9f9;
                border-radius: 5px;
            }}
            .details p {{
                margin: 0;
            }}
            .highlight {{
                font-weight: bold;
                color: #0066cc;
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <h1>Car Rental Confirmation</h1>
            <div class='details'>
                <p>თქვენ გააქირაავეთ</p>
                <p class='highlight'>{CarMark} {CarModel}</p>
                <p>დღით</p>
                <p class='highlight'>{multiplier} დღით</p>  
                <p>თქვენ მიიღებთ</p>
                <p class='highlight'>{PricePaid} ლარი</p>
            </div>
        </div>
    </body>
    </html>";


        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        var response = await client.SendEmailAsync(msg);
    }
}
