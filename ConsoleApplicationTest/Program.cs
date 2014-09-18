using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcklenAvenue.Email;

namespace ConsoleApplicationTest
{
    class Program
    {
        static void Main(string[] args)
        {

            var client = AcklenSmtpClient.From("dante.dubon@gmail.com");
            client.Send("dante.dubon@gmail.com","texto plano","texto plano");

        }
    }
}
