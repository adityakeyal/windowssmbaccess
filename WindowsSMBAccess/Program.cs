using System;

namespace WindowsSMBAccess
{
    class Program
    {
        static void Main(string[] args)
        {

            var options =XmlOptions.Read();

            Console.WriteLine("Select...");


            options.Ops.ForEach(action: x => {
                Console.WriteLine(x.Op + " : ");
            });


            string userInput = Console.ReadLine();


            var UserSelection = options.Ops.Find(x => x.Op.Equals(userInput));

            if (UserSelection == null)
            {
                Console.WriteLine("Invalid Selection..");
            }
            else {
                //process

                SMBAccess access = new SMBAccess();
                access.Fetch(UserSelection);

            }





        }
    }
}
