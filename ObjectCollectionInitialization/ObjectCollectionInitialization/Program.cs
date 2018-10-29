using System;
using System.Collections.Generic;

namespace ObjectCollectionInitialization
{
    class Program
    {
        static void Main(string[] args)
        {
            //Traditional way of creating the object

            //Employee empObj = new Employee();
            //empObj.EmpId = 1;
            //empObj.EmpName = "Raj Pawar";
            //empObj.EmpAddress = "Udhna, Surat";

            //New way of creating the object

            Employee empObj = new Employee { EmpId=1, EmpName="Raj Pawar", EmpAddress="Udhna, Surat"};

            //Traditional way of creating Collection of objects is 

            //Employee empObj1 = new Employee();
            //empObj.EmpId = 1;
            //empObj.EmpName = "Raj Pawar";
            //empObj.EmpAddress = "Udhna, Surat";

            //Employee empObj2 = new Employee();
            //empObj.EmpId = 2;
            //empObj.EmpName = "Urvesh Mishra";
            //empObj.EmpAddress = "Vijalpore, Navsari";

            //Employee empObj3 = new Employee();
            //empObj.EmpId = 3;
            //empObj.EmpName = "Sahani Rishikesh";
            //empObj.EmpAddress = "Vijalpore, Navsari";

            //Employee empObj4 = new Employee();
            //empObj.EmpId = 4;
            //empObj.EmpName = "Sheikh Jelani";
            //empObj.EmpAddress = "Dakha, Bangladesh";

            //List<Employee> objList = new List<Employee>();
            //objList.Add(empObj1);
            //objList.Add(empObj2);
            //objList.Add(empObj3);
            //objList.Add(empObj4);

            //New way of initializing the collection of object list

            List<Employee> newListObject = new List<Employee>
            {
                new Employee { EmpId=1, EmpName="Raj Pawar", EmpAddress="Udhna, Surat"},
                new Employee { EmpId=2, EmpName="Urvesh Mishra", EmpAddress="Vijalpore, Navsari"},
                new Employee { EmpId=3, EmpName="Sahani Rishikesh", EmpAddress="Vijalpore, Navsari"},
                new Employee { EmpId=4, EmpName="Sheikh Jelani", EmpAddress="Dakha, Bangladesh"}
            };

            foreach(Employee emp in newListObject)
            {
                Console.WriteLine(emp.EmpId+" - "+emp.EmpName+" - "+emp.EmpAddress);
            }


            //Console.WriteLine(empObj.EmpId + " - " + empObj.EmpName);
            Console.ReadLine();


        }
    }
}
