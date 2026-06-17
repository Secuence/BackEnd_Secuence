using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;

namespace SecuenceBack.Utils
{
    class HTMLTemplateHelper
    {//Getting PDF 
        public static string parseFromFile(string file, Dictionary<string, string> words)
        {
            try
            {
                string data = File.ReadAllText(file);
                foreach (KeyValuePair<string, string> item in words)
                {
                    data = data.Replace(item.Key, item.Value);
                }
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "";
            }
        }
    }
    public class EmailManager
    {
        public static async Task<bool> SendEmail(string apikey, string from, string name, string to, string subject, string htmlbody, string fileName = null)
        {
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(from, name),
                Subject = subject,
                HtmlContent = htmlbody
            };
            msg.AddTo(to);

            var transportWeb = new SendGridClient(apikey);
            var response = await transportWeb.SendEmailAsync(msg);
            if (response.StatusCode != HttpStatusCode.Accepted) return false;
            else return true;
        }

    }
}
