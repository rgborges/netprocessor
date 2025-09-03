# A library for .NET file processing

**IMPORTANT**: This project has not any functional release yet. The development is a work in progress.

The goal of this library is providing help functions and classes to make file importing and exporting easy.

## Last developments
- Import of CSV file based in readall content method.

## Use cases

DataFrames is the way to store and work with data. I developed a very light version with the minimum need to have some work. Microsoft has a large implementation of a dataframe in ML.Net repository.

You can easily create a new dataframe:

```csharp
var df = new DataFrame("names", ["Jhon", "Elena"])
                .AddColumn("ages", [12, 23]);
```
### Importing csv file to DataFrame 


Import a csv file into a data frame using `DataFrame.FromCSV(path:<your-path>)`:

```csharp
var df = DataFrame.FromCSV(".\uae_used_cars_10k.csv");
```
# Documentação de Uso: Classe `DataFrame` (NetProcessor)

Esta documentação descreve como utilizar a classe `DataFrame` da biblioteca `NetProcessor.Data.DataFrame`, com base nos exemplos fornecidos no código de teste `DataFrameTests.cs`.

A classe `DataFrame` oferece uma estrutura de dados tabular bidimensional, semelhante aos DataFrames encontrados em bibliotecas como Pandas (Python) ou R, permitindo manipulação e análise de dados de forma eficiente.

## 1. Criação de um DataFrame

Você pode criar um DataFrame fornecendo um nome e os dados para a primeira coluna. Colunas subsequentes podem ser adicionadas usando o método `AddColumn`.

```csharp
using NetProcessor.Data.DataFrame;
using NetProcessor.Data; // Necessário para AddColumn e outros métodos de extensão, talvez

// Cria um DataFrame com uma coluna inicial "names"
var df = new DataFrame("names", ["John Doe", "Maria", "Stella"]);

// Adiciona uma segunda coluna "ages"
df.AddColumn("ages", [12, 23, 15]);

// O DataFrame 'df' agora contém:
// | names    | ages |
// |----------|------|
// | John Doe | 12   |
// | Maria    | 23   |
// | Stella   | 15   |
```

# 2. Data Frame

## 2.1. Acessar Colunas por Nome

Você pode recuperar os dados de uma coluna específica acessando o dicionário Data do DataFrame com o nome da coluna.

```csharp

// Supondo que 'df' foi criado como no exemplo anterior
var agesColumnData = df.Data["ages"]; // Retorna um array ou lista com [12, 23, 15]

Console.WriteLine($"Número de elementos na coluna 'ages': {agesColumnData.Length}"); // Output: 3
```
## 2.2. Acessar Linhas por Índice
É possível acessar uma linha específica do DataFrame usando indexação numérica (baseada em zero). O retorno é um objeto que representa a linha (o tipo exato pode variar, mas permite acesso aos dados daquela linha).
```csharp
// Acessa a primeira linha (índice 0)
var firstRow = df[0];

// Você pode então acessar os dados dessa linha (a forma exata dependerá da implementação do objeto de linha)
// Exemplo hipotético:
// Console.WriteLine($"Nome: {firstRow["names"]}, Idade: {firstRow["ages"]}");
```

## 2.3. Obter Múltiplas Linhas por Intervalo

Use o método GetRows para obter um subconjunto de linhas especificando um índice inicial e final (ou um índice inicial e uma contagem, dependendo da assinatura exata do método).

```csharp
// Obtém as linhas dos índices 0 e 1
var firstTwoRows = df.GetRows(0, 1).ToArray(); // Retorna um array com as duas primeiras linhas

// Obtém as N primeiras linhas (head)
var top1Row = df.Top(1).ToArray(); // Retorna um array com a primeira linha
```

## 2.4. Selecionar Colunas Específicas

Para criar um novo DataFrame contendo apenas um subconjunto das colunas originais, use o método GetColumns.

```csharp
// Cria um novo DataFrame contendo apenas a coluna "ages"
var agesOnlyDf = df.GetColumns(new[] { "ages" });

// 'agesOnlyDf' contém:
// | ages |
// |------|
// | 12   |
// | 23   |
// | 15   |
```

## 3. Obtendo Informações do DataFrame

### 3.1. Dimensões (Shape)

A propriedade Shape retorna uma tupla contendo o número de linhas e o número de colunas do DataFrame.

```csharp
// Obtém as dimensões (linhas, colunas)
var shape = df.Shape; // Retorna (3, 2) para o exemplo inicial

Console.WriteLine(<span class="math-inline">"Linhas\: \{shape\.Item1\}, Colunas\: \{shape\.Item2\}"\); // Output\: Linhas\: 3, Colunas\: 2
// Ou usando a propriedade Rows diretamente\:
Console\.WriteLine\(</span>"Linhas: {df.Rows}"); // Output: 3
```

## 4. Manipulação de Dados

### 4.1. Filtragem de Linhas

O método Filter permite criar um novo DataFrame contendo apenas as linhas que satisfazem uma determinada condição em uma coluna específica. A condição é definida por uma função (lambda) que recebe o valor da célula como object e retorna true se a linha deve ser incluída.

```csharp
// Cria um DataFrame filtrado contendo apenas indivíduos com idade superior a 21
var dfFiltered = df.Filter("ages", (object ageValue) => {
    // É necessário fazer o cast para o tipo correto dentro da função
    if (ageValue is int age)
    {
        return age > 21;
    }
    return false; // Ou lançar uma exceção se o tipo for inesperado
});

// 'dfFiltered' agora contém:
// | names | ages |
// |-------|------|
// | Maria | 23   |

Console.WriteLine($"Número de linhas após filtro: {dfFiltered.Rows}"); // Output: 1
```

**Observação**: O teste original tem um //TODO: Implment filter function, mas o código abaixo dele já implementa a filtragem. A documentação reflete a implementação mostrada.

## 5. Carregando Dados de Arquivos

### 5.1. Importar de CSV

É possível carregar dados diretamente de um arquivo CSV usando o método estático DataFrame.FromCSV.

```csharp
string csvFilePath = "..\\..\\..\\..\\Data\\Admission_Predict_Ver1.1.csv"; // Caminho relativo ao local de execução

try
{
    var dfFromCsv = DataFrame.FromCSV(csvFilePath);

    Console.WriteLine("DataFrame carregado com {dfFromCsv.Rows} linhas.");
// Faça operações com dfFromCsv
}
catch (FileNotFoundException)
{
Console.WriteLine("Erro: Arquivo CSV não encontrado em '{csvFilePath}'");
}
catch (Exception ex)
{
    Console.WriteLine($"Erro ao carregar CSV: {ex.Message}");
}
```

## 6. Gerenciamento de Recursos (IDisposable)

A classe DataFrame implementa a interface IDisposable. Isso significa que ela pode gerenciar recursos não gerenciados (como handles de arquivo ou grandes blocos de memória) que precisam ser liberados explicitamente.

É altamente recomendado usar a instrução using ao trabalhar com instâncias de DataFrame para garantir que o método Dispose() seja chamado automaticamente, liberando os recursos adequadamente.

```csharp
using (var df = new DataFrame("names", ["John Doe", "Maria", "Stella"])
                    .AddColumn("ages", [12, 23, 15]))
{
    // Use o DataFrame 'df' aqui dentro...
    Console.WriteLine($"Shape dentro do using: {df.Shape}");

} // df.Dispose() é chamado automaticamente aqui ao sair do bloco using

// Tentar usar 'df' aqui fora resultaria em erro ou comportamento indefinido,
// pois seus recursos podem ter sido liberados.
```

## 7. Trabalhando com DataFrameColumn (Opcional)

Embora geralmente você interaja com colunas através do DataFrame, a classe DataFrameColumn pode ser usada diretamente se necessário.

```csharp
// Criar uma coluna diretamente
var cityColumn = new DataFrameColumn("city", new[] { "São Paulo", "Rio de Janeiro", "Bahia" });
Console.WriteLine("Nome da Coluna: {cityColumn.Name}"); // Output\: city
Console\.WriteLine("Tamanho da Coluna: {cityColumn.Size}"); // Output: 3

// Criar outra coluna
var agesColumn = new DataFrameColumn("ages", [12, 15, 26, 24]);

// Aplicar uma função a todos os elementos da coluna (modifica in-place!)
// A função recebe um elemento e DEVE retornar o valor modificado.
agesColumn.Apply<int>((int input) => {
    return input + 2; // Incrementa cada idade em 2
});

// Obter os valores modificados
var modifiedAges = agesColumn.GetValues(); // Retorna os dados como um array ou lista
// modifiedAges conteria [14, 17, 28, 26] (assumindo que Apply retorna o novo valor corretamente)
```

Nota: A implementação de Apply no teste parece tentar modificar o parâmetro input diretamente (input = input + 2), o que não alteraria o valor original fora do escopo da lambda em C# para tipos de valor como int. A documentação assume que Apply foi corrigida para usar o valor retornado pela lambda para atualizar a coluna, como é comum nesses tipos de operação. Se Apply realmente modificar in-place de outra forma (ex: acessando um array interno por referência), a lógica funciona, mas a lambda como escrita no teste não teria o efeito desejado por si só.

## Maintainer

- Rafael Borges (rborges@datawiden.net, rgborges96@gmail.com)
