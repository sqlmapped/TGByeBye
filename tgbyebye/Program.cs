using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

class Program
{
    static readonly HttpClient client = new HttpClient();

    static Dictionary<int, Tuple<string, List<string>>> violations = new Dictionary<int, Tuple<string, List<string>>>
    {
        { 1, new Tuple<string, List<string>>("Spam", new List<string>
            {
                "Dear Support Team, please note the activity of user {username}, who is sending a large amount of unwanted advertisements and messages in Telegram chats and groups. Please take measures to stop this spam.",
                "User {username} is actively abusing spam sending, violating the courtesy and rules of using Telegram. Please check and take appropriate actions."
            })
        },
        { 2, new Tuple<string, List<string>>("Fraud", new List<string>
            {
                "Dear Support Team, please pay attention to the account of user {username}, who is offering participation in potentially fraudulent schemes. This behavior raises concerns and requires investigation.",
                "User {username} may be involved in fraudulent activities, and their behavior and actions should be reviewed in detail."
            })
        },
        { 3, new Tuple<string, List<string>>("Pornography", new List<string>
            {
                "Dear Support Team, I am a Telegram user and have noticed violations in the content of user {username}, which contains pornographic material. Please take action to remove this content and hold the user accountable.",
                "User {username} is actively distributing adult materials, which contradicts Telegram's rules and goals as a safe messaging platform."
            })
        },
        { 4, new Tuple<string, List<string>>("Rule Violation", new List<string>
            {
                "Dear Support Team, please note the account of {username}, who systematically violates Telegram platform rules. Please take action against this user to ensure compliance with community rules.",
                "User {username} is provoking conflicts and posting unacceptable content in Telegram chats and channels, which is not allowed and requires intervention. Please check and take appropriate actions."
            })
        }
    };

    /// by sqlmapped

    static List<string> phoneNumbersTemplates = new List<string>
    {
        "+7917**11**2", "+7926**386**", "+7952**99*63", "+7903**76*82", "+7914**237*7*",
        "+7937**61***", "+7978**42***", "+7982**89***", "+7921**57***", "+7991**34***",
        "+7910**68***", "+7940**15***", "+7961**72***", "+7985**49***", "+7951**27***",
        "+7916**83***", "+7932**95***", "+7975**44***", "+7989**78***", "+7993**64***",
        "+7923**58***", "+7970**30***", "+7960**17***", "+7995**48***", "+7953**25***",
        "+7919**77***", "+7938**36***", "+7986**62***", "+7907**81*7*", "+7947**53*6*",
        "+7971**29***"
    };

    static string GenerateComplaint(string username, int violation)
    {
        var messages = violations[violation].Item2;
        var random = new Random();
        var complaintTemplate = messages[random.Next(messages.Count)];
        return complaintTemplate.Replace("{username}", username);
    }

    static async Task SendComplaintTelegramSupportAsync(string complaint, string phoneNumber)
    {
        var url = "https://telegram.org/support";
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("complaint", complaint),
            new KeyValuePair<string, string>("phone_number", phoneNumber)
        });

        try
        {
            var response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Complaint sent from '{phoneNumber}' successfully.");
            }
            else
            {
                Console.WriteLine("Error sending complaint.");
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Error sending complaint.");
        }
    }

    static async Task Main(string[] args)
    {
        Console.Write("Please enter the password: ");
        string password = Console.ReadLine();
        if (password != "sqlmapped")
        {
            Console.WriteLine("Invalid password. Access denied.");
            return;
        }

        Console.WriteLine("TGByeBye - by sqlmapped");

        Console.Write("Enter the username: ");
        string username = Console.ReadLine();

        Console.WriteLine("Choose the type of complaint:");
        Console.WriteLine("1 - Spam");
        Console.WriteLine("2 - Fraud");
        Console.WriteLine("3 - Pornography");
        Console.WriteLine("4 - Rule Violation");
        Console.Write("Enter the complaint type number: ");
        int violation = int.Parse(Console.ReadLine());

        Console.Write("Enter the number of complaints to send: ");
        int numComplaints = int.Parse(Console.ReadLine());

        var random = new Random();
        var phoneNumbers = new List<string>();
        for (int i = 0; i < numComplaints; i++)
        {
            var tpl = phoneNumbersTemplates[random.Next(phoneNumbersTemplates.Count)];
            var phoneNumber = "";
            foreach (var c in tpl)
            {
                phoneNumber += (c == '*') ? random.Next(10).ToString() : c.ToString();
            }
            phoneNumbers.Add(phoneNumber);
        }

        Console.WriteLine("Sending complaints...");
        foreach (var phoneNumber in phoneNumbers)
        {
            var complaint = GenerateComplaint(username, violation);
            await SendComplaintTelegramSupportAsync(complaint, phoneNumber);
            await Task.Delay(500);
        }
    }
}
