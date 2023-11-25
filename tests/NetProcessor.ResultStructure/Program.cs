// See https://aka.ms/new-console-template for more information
using System;
using BgSoftLab.Results;
using NetProcessor.ResultStructure;

Console.WriteLine("Hello, World!");

var result = Execute();

if (result.Success)
{
      // System.Console.WriteLine("It was a success");
      // var dataResult = result.GetResult<int[]>().GetData(); 
      // return;
}

System.Console.WriteLine("It has faild");

foreach (string s in result.Errors)
{
      System.Console.WriteLine(s);
}
static TestResult Execute()
{
      var result = new TestResult();
      
      int[] resultData = new int[1000];
      for (int i = 0; i < 1000; i++)
      {
            resultData[i] = i;
      }
      result.Finish(resultData);
      return result;
}
