using System;
using System.Dynamic;

namespace NetProcessor.Data.DataFrame;

public class DataFrameColumn
{
      /// <summary>
      /// Return the name of the column.
      /// </summary>
      /// <value>the name of the column as a string</value>
      public string Name
      {
            get
            {
                  return _data.Key;
            }
      }

      /// <summary>
      /// Retrieves the number of values of the vector.
      /// </summary>
      /// <value>The number of items as integer</value>
      public int Size { get; private set;}
      public string Type { get; private set;}

      private KeyValuePair<string, object[]> _data;
      public DataFrameColumn(string name, object[] values)
      {
            string columnName = !string.IsNullOrEmpty(name) ? name : String.Empty;    
            _data = new KeyValuePair<string, object[]>(columnName, values);
            Size = values.Length;
            
            if (values.Count() == 0)
            {
                  return;
            }

            var referenceValue = values[0];
            var type = referenceValue.GetType();
            Type = type.Name;
      }
      /// <summary>
      /// /// return the Data of the DataFrameColumn structure.
      /// </summary>
      /// <value>A keyvalue pair of a string as key and an array of object as Data</value>
      public KeyValuePair<string, object[]> Data
      {
            get { return _data; }
      }
      public object[] GetValues()
      {
            return _data.Value;
      }
      public T[] GetValues<T>(IDataFrameColumnConverter<T> converter)
      {
            var vector = _data.Value;
            T[] result = new T[vector.Length];
            for (int i = 0; i < vector.Length; i++)
            {
                  result[i] = converter.ConvertFromObject(vector[i]);
            }
            return result;
      }
      public DataFrameColumn Apply<T>(Func<T, T> expression)
      {
            var vector = Data.Value;
            for (int i = 0; i < vector.Length; i++)
            {
                  vector[i] = expression((T)vector[i]);
            }
            _data = new KeyValuePair<string, object[]>(Name, vector);

            return this;
      }
      
      public DataFrameColumn Apply<T>(Func<T, T> expression, Predicate<T> predicate)
      {
            var vector = Data.Value;
            for (int i = 0; i < vector.Length; i++)
            {
                  var v = (T)vector[i];
                  if (predicate(v))
                  {
                        vector[i] = expression(v);
                  }
            }
            _data = new KeyValuePair<string, object[]>(Name, vector);

            return this;
      }
}

public interface IDataFrameColumnConverter<T>
{
    T ConvertFromObject(object v);
}