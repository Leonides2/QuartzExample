using Quartz;
using System;

namespace QuartzProject
{
    public class TelefonoGenerator : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"Un numero de telefono aleatorio es {RandomPhone()}");
            await Task.CompletedTask;
        }

        private string RandomPhone() {

            string[] phone = new string [8];

            Random random = new Random();

            for (int i = 0; i < phone.Length; i++)
            {
                phone[i] = random.Next(0, 9).ToString(); // Genera un número aleatorio entre 0 y 9
            }

            string a = "";

            foreach(string s in phone)
            {
                a += s;
            }


            return a;
        }
    }
}
