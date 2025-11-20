using System;

namespace DAL
{
    public interface IDataProvider<T> where T : Person
    {
        List<T> Load(string filePath);
        void Save(string filePath, List<T> data);
    }
}