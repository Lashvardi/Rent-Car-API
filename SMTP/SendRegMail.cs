using SendGrid;
using SendGrid.Helpers.Mail;


namespace SMTP
{
    public class SendMessage
    {
        public async Task SendRegistrationEmail(string recipientEmail, string firstName, string lastName)
        {
            // SendGrid
            // https://app.sendgrid.com/guide/integrate/langs/csharp
            // https://app.sendgrid.com/settings/api_keys
            var client = new SendGridClient("<YOUR TOKEN HERE>");
            var from = new EmailAddress("<YOUR MAIL ADDRESS>", "Morent");
            var to = new EmailAddress(recipientEmail);
            var subject = "Welcome to Our Website";
            var plainTextContent = $"მოგესალმებით {firstName} {lastName}";
            var htmlContent = $@"
    <html>
    <head>
<link rel=""preconnect"" href=""https://fonts.googleapis.com"">
<link rel=""preconnect"" href=""https://fonts.gstatic.com"" crossorigin>
<link href=""https://fonts.googleapis.com/css2?family=Bruno+Ace&family=Bungee+Spice&family=Montserrat:ital,wght@0,100;0,200;0,300;0,400;0,500;0,600;0,700;0,800;0,900;1,100;1,200;1,300;1,400;1,500;1,600;1,700;1,800;1,900&family=Noto+Sans+Georgian:wght@100;200;300;400;500;600;700;800;900&display=swap"" rel=""stylesheet"">
        <style>
            body {{
                background-color: #f2f2f2;
font-family: 'Noto Sans Georgian', sans-serif !important;
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
            .message {{
                margin-top: 30px;
                padding: 20px;
                background-color: #f9f9f9;
                border-radius: 5px;
            }}
            .message p {{
                margin: 0;
                font-size: 16px;
                line-height: 1.5;
            }}
            .highlight {{
                font-weight: bold;
                color: #0066cc;
font-family: 'Montserrat', sans-serif;
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <h1>მოგესალმებით</h1>
            <div class='message'>
                <p>ძვირფასო <span class='highlight'>{firstName} {lastName}</span>,</p>
                <p>მოგესალმბებით, ჩვენ გვიახირია რომ გვენდობით. თქვენ წარმატებით გაიარეთ რეგისტრაცია</p>
                <p>კითხვების შემთხვევაში დაუკავშირდით მხარდაჭერის გუნდს.</p>
                <p>საუკეთესო სურვილებით,</p>
                <p class='highlight'>Morent</p>
            </div>
        </div>
    </body>
    </html>";


            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
