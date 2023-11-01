// See https://aka.ms/new-console-template for more information
using System;
using NetProcessor.ResultStructure;

Console.WriteLine("Hello, World!");


var result = new TestResult();

result.Start();

for (int i = 0; i < 1000; i++)
{

}
result.Finish();


if (result.Success)
{
      System.Console.WriteLine("It was a success");

      return;
}

System.Console.WriteLine("It has faild");

foreach (string s in result.Errors)
{
      System.Console.WriteLine(s);
}
