using Xunit;
using DSFactory.Core;
using System;

namespace DSFactory.Tests
{
    public class StructureTests
    {
        [Fact]
        public void Stack_Behaves_Like_LIFO()
        {
            IDataStructure<int> stack = StructureFactory.Create<int>(StructureType.Stack);
            
            stack.Add(1);
            stack.Add(2);
            stack.Add(3);
            int popped = stack.Remove();

            Assert.Equal(3, popped);
            Assert.Equal(2, stack.Count);
        }

        [Fact]
        public void Queue_Behaves_Like_FIFO()
        {
            IDataStructure<int> queue = StructureFactory.Create<int>(StructureType.Queue);

            queue.Add(1);
            queue.Add(2);
            int removed = queue.Remove();

            Assert.Equal(1, removed);
        }

        [Fact]
        public void DLL_Links_Correctly()
        {
            var dll = new DoublyLinkedList<string>();

            dll.Add("A");
            dll.Add("B");
            
            string visual = dll.Display();
            Assert.Contains("[A] <-> [B]", visual);
        }

        [Fact]
        public void Stack_Remove_FromEmpty_ThrowsException()
        {
            var stack = StructureFactory.Create<int>(StructureType.Stack);

            var ex = Assert.Throws<InvalidOperationException>(() => stack.Remove());
            
            Assert.Equal("Stack is empty.", ex.Message);
        }

        [Fact]
        public void Queue_Peek_FromEmpty_ThrowsException()
        {
            var queue = StructureFactory.Create<int>(StructureType.Queue);
            Assert.Throws<InvalidOperationException>(() => queue.Peek());
        }

        [Fact]
        public void DLL_Contains_FoundAndNotFound()
        {
            var dll = new DoublyLinkedList<int>();
            dll.Add(10);
            dll.Add(20);

            Assert.True(dll.Contains(10));
            Assert.False(dll.Contains(99));
        }

        [Fact]
        public void Clear_Resets_Count_To_Zero()
        {
            var stack = StructureFactory.Create<int>(StructureType.Stack);
            stack.Add(1);
            stack.Add(2);
            
            stack.Clear();
            
            Assert.Equal(0, stack.Count);
            Assert.Throws<InvalidOperationException>(() => stack.Remove());
        }
    }
}