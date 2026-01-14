namespace DSFactory.Core
{
    public interface IDataStructure<T>
    {
        void Add(T item);
        T Remove();
        T Peek();
        int Count { get; }
        string TypeName { get; } 

        string Display();
        bool Contains(T item);
        void Clear();
    };
}