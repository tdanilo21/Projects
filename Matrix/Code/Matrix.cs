using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace ConsoleApp1
{
    abstract class AbstractElement<T> where T : AbstractElement<T>
    {
        #region Abstract
        public abstract void Random();
        protected abstract T OperatorMultiply(T a);
        protected abstract T OperatorAdd(T a);
        #endregion
        #region Operators
        public static T operator *(T a, AbstractElement<T> b)
        {
            return a.OperatorMultiply((T)b);
        }
        public static T operator+(T a, AbstractElement<T> b)
        {
            return a.OperatorAdd((T)b);
        }
        #endregion
    }
    class MyInt : AbstractElement<MyInt>
    {
        private static Random rand = new Random();
        public int val = 0;

        #region Operators
        public MyInt() { }
        public MyInt(int new_val) { this.val = new_val; }
        public MyInt(MyInt a) { this.val = a.val; }
        #endregion
        #region Funcitons
        public override void Random()
        {
            this.val = MyInt.rand.Next();
        }
        protected override MyInt OperatorMultiply(MyInt a)
        {
            return new MyInt(this.val * a.val);
        }
        protected override MyInt OperatorAdd(MyInt a)
        {
            return new MyInt(this.val + a.val);
        }
        #endregion
    }
    class Matrix<T> where T : AbstractElement<T>, new()
    {

        private T[,] data;
        private uint rows, cols;

        #region Constructors
        public Matrix()
        {
            Construct(1, 1, new T());
        }
        public Matrix(uint rows, uint cols)
        {
            Construct(rows, cols, new T());
        }
        public Matrix(uint rows, uint cols, T defaultData)
        {
            Construct(rows, cols, defaultData);
        }
        public Matrix(Matrix<T> other)
        {
            Resize(other.rows, other.cols);
            SetData(other.data);
        }
        #endregion
        #region Helper function
        private void Construct(uint rows, uint cols, T defaultData)
        {
            Resize(rows, cols);
            Fill(defaultData);
        }
        public void Resize(uint rows = 1, uint cols = 1)
        {
            this.rows = rows;
            this.cols = cols;
            this.data = new T[rows, cols];
        }
        public Dims Dimensions()
        {
            return new Dims(this.rows, this.cols);
        }
        public void Fill(T data)
        {
            for (uint i = 0; i < this.rows; i++)
                for (uint j = 0; j < this.cols; j++)
                    this.data[i, j] = data;
        }
        public void SetData(T[,] data)
        {
            for (uint i = 0; i < this.rows; i++)
                for (uint j = 0; j < this.cols; j++)
                    this.data[i, j] = data[i, j];
        }
        public void Randomize()
        {
            for (uint i = 0; i < this.rows; i++)
                for (uint j = 0; j < this.cols; j++)
                    this.data[i, j].Random();
        }
        public T[] GetRow(uint index)
        {
            Debug.Assert(index < this.rows);
            T[] result = new T[this.cols];
            for (uint i = 0; i < this.cols; i++)
                result[i] = this.data[index, i];
            return result;
        }
        public T[] GetCol(uint index)
        {
            Debug.Assert(index < this.cols);
            T[] result = new T[this.rows];
            for (uint i = 0; i < this.rows; i++)
                result[i] = this.data[i, index];
            return result;
        }
        public static T[] ToArray(Matrix<T> m)
        {
            T[] result = new T[m.rows * m.cols];
            for (uint i = 0; i < m.rows; i++)
                for (uint j = 0; j < m.cols; j++)
                    result[i * m.cols + j] = m.data[i, j];
            return result;
        }
        public static Matrix<T> FromArray(T[] arr)
        {
            Matrix<T> result = new Matrix<T>(Convert.ToUInt32(arr.Length), 1);
            for (uint i = 0; i < arr.Length; i++)
                result.data[i, 1] = arr[i];
            return result;
        }
        #endregion
        #region Functionalities
        public static Matrix<T> Product(Matrix<T> a, Matrix<T> b)
        {
            Debug.Assert(a.cols == b.rows);
            Matrix<T> result = new Matrix<T>(a.rows, b.cols);
            for (uint i = 0; i < a.rows; i++)
            {
                for (uint j = 0; j < b.cols; j++)
                {
                    T sum = new T();
                    T[] r = a.GetRow(i), c = b.GetCol(j);
                    for (uint t = 0; t < a.cols; t++)
                        sum += r[t] * c[t];
                    result.data[i, j] = sum;
                }
            }
            return result;
        }
        public static Matrix<T> Transpose(Matrix<T> m)
        {
            Matrix<T> result = new Matrix<T>(m.cols, m.rows);
            for (uint i = 0; i < m.rows; i++)
                for (uint j = 0; j < m.cols; j++)
                    result.data[j, i] = m.data[i, j];
            return result;
        }
        public static Matrix<T> ElementWise(Matrix<T> a, Matrix<T> b, Func<T, T, T> func)
        {
            Debug.Assert(a.rows == b.rows && a.cols == b.cols);
            Matrix<T> result = new Matrix<T>(a.rows, a.cols);
            for (uint i = 0; i < a.rows; i++)
                for (uint j = 0; j < a.cols; j++)
                    result.data[i, j] = func(a.data[i, j], b.data[i, j]);
            return result;
        }
        public void Scale(Func<T, T> func)
        {
            for (uint i = 0; i < this.rows; i++)
                for (uint j = 0; j < this.cols; j++)
                    this.data[i, j] = func(this.data[i, j]);
        }
        #endregion

        public struct Dims
        {
            public uint rows, cols;
            public Dims(uint rows = 1, uint cols = 1) { this.rows = rows; this.cols = cols; }
        }
    }
    class Program
    {
        static void Print(Matrix<MyInt> m)
        {
            for (uint i = 0; i < m.Dimensions().rows; i++)
            {
                MyInt[] row = m.GetRow(i);
                for (uint j = 0; j < m.Dimensions().cols; j++)
                    Console.Write($"{row[j].val} ");
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        static void Main(string[] args)
        {
            Matrix<MyInt> m = new Matrix<MyInt>(2, 3);
            m.Scale(x => new MyInt(2 + x.val));
            Print(m);
            m.Scale(x => new MyInt(3 * x.val));
            Print(m);
            m = Matrix<MyInt>.Transpose(m);
            Print(m);

            Matrix<MyInt> a = new Matrix<MyInt>(2, 3, new MyInt(2));
            Matrix<MyInt> b = new Matrix<MyInt>(3, 3, new MyInt(3));
            Matrix<MyInt> p = Matrix<MyInt>.Product(a, b);
            Print(p);

            Func<MyInt, MyInt, MyInt> Multiply = (x, y) => { return new MyInt(x.val * y.val); };
            Matrix<MyInt> c = new Matrix<MyInt>(3, 2, new MyInt(5));
            Matrix<MyInt> e = Matrix<MyInt>.ElementWise(m, c, Multiply);
            Print(e);

            Matrix<MyInt> r = new Matrix<MyInt>(2, 2);
            r.Randomize();
            Print(r);
        }
    }
}
