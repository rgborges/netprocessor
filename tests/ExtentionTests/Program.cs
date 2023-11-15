// See https://aka.ms/new-console-template for more information


using BgSoftLab.Extentions;

var data = new DTO()
{
      ItemA = "Teste B",
      ItemB = "Teste A Hello"
};


System.Console.WriteLine(data.ToJson<DTO>());

record class DTO
{
      public string ItemA { get; set; }
      public string ItemB { get; set; }
}
