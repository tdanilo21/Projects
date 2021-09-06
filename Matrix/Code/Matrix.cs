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
        protected abstract T OperatorDevide(T a);
        protected abstract T OperatorAdd(T a);
        protected abstract T OperatorSubtract(T a);
        #endregion
        #region Operators
        public static T operator *(T a, AbstractElement<T> b)
        {
            return a.OperatorMultiply((T)b);
        }
        public static T operator /(T a, AbstractElement<T> b)
        {
            return a.OperatorDevide((T)b);
        }
        public static T operator +(T a, AbstractElement<T> b)
        {
            return a.OperatorAdd((T)b);
        }
        public static T operator -(T a, AbstractElement<T> b)
        {
            return a.OperatorSubtract((T)b);
        }
        #endregion
    }
    class MyDouble : AbstractElement<MyDouble>
    {
        private static Random rand = new Random();
        public double val;

        #region Constructors
        public MyDouble()
        {
            this.val = 0;
        }
        public MyDouble(double new_val)
        {
            this.val = new_val;
        }
        public MyDouble(MyDouble a)
        {
            this.val = a.val;
        }
        #endregion
        #region Funcitons
        public override void Random()
        {
            this.val = MyDouble.rand.Next();
        }
        protected override MyDouble OperatorMultiply(MyDouble a)
        {
            return new MyDouble(this.val * a.val);
        }
        protected override MyDouble OperatorDevide(MyDouble a)
        {
            return new MyDouble(this.val / a.val);
        }
        protected override MyDouble OperatorAdd(MyDouble a)
        {
            return new MyDouble(this.val + a.val);
        }
        protected override MyDouble OperatorSubtract(MyDouble a)
        {
            return new MyDouble(this.val - a.val);
        }
        #endregion
    }
    class Matrix<T> where T : AbstractElement<T>, new()
    {
        private T[,] data;
        private int rows, cols;

        #region Constructors
        public Matrix()
        {
            Construct(1, 1, new T());
        }
        public Matrix(int rows, int cols)
        {
            Construct(rows, cols, new T());
        }
        public Matrix(int rows, int cols, T defaultData)
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
        private void Construct(int rows, int cols, T defaultData)
        {
            Resize(rows, cols);
            Fill(defaultData);
        }
        public void Resize(int rows = 1, int cols = 1)
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
            for (int i = 0; i < this.rows; i++)
                for (int j = 0; j < this.cols; j++)
                    this.data[i, j] = data;
        }
        public void SetData(T[,] data)
        {
            for (int i = 0; i < this.rows; i++)
                for (int j = 0; j < this.cols; j++)
                    this.data[i, j] = data[i, j];
        }
        public void Randomize()
        {
            for (int i = 0; i < this.rows; i++)
                for (int j = 0; j < this.cols; j++)
                    this.data[i, j].Random();
        }
        public T[] GetRow(int index)
        {
            Debug.Assert(index < this.rows);
            T[] result = new T[this.cols];
            for (int i = 0; i < this.cols; i++)
                result[i] = this.data[index, i];
            return result;
        }
        public T[] GetCol(int index)
        {
            Debug.Assert(index < this.cols);
            T[] result = new T[this.rows];
            for (int i = 0; i < this.rows; i++)
                result[i] = this.data[i, index];
            return result;
        }
        public static T[] ToArray(Matrix<T> m)
        {
            T[] result = new T[m.rows * m.cols];
            for (int i = 0; i < m.rows; i++)
                for (int j = 0; j < m.cols; j++)
                    result[i * m.cols + j] = m.data[i, j];
            return result;
        }
        public static Matrix<T> FromArray(T[] arr)
        {
            Matrix<T> result = new Matrix<T>(arr.Length, 1);
            for (int i = 0; i < arr.Length; i++)
                result.data[i, 1] = arr[i];
            return result;
        }
        #endregion
        #region Functionalities
        public static Matrix<T> Product(Matrix<T> a, Matrix<T> b)
        {
            Debug.Assert(a.cols == b.rows);
            Matrix<T> result = new Matrix<T>(a.rows, b.cols);
            for (int i = 0; i < a.rows; i++)
            {
                for (int j = 0; j < b.cols; j++)
                {
                    T sum = new T();
                    T[] r = a.GetRow(i), c = b.GetCol(j);
                    for (int t = 0; t < a.cols; t++)
                        sum += r[t] * c[t];
                    result.data[i, j] = sum;
                }
            }
            return result;
        }
        public static Matrix<T> Transpose(Matrix<T> m)
        {
            Matrix<T> result = new Matrix<T>(m.cols, m.rows);
            for (int i = 0; i < m.rows; i++)
                for (int j = 0; j < m.cols; j++)
                    result.data[j, i] = m.data[i, j];
            return result;
        }
        public static Matrix<T> ElementWise(Matrix<T> a, Matrix<T> b, Func<T, T, T> func)
        {
            Debug.Assert(a.rows == b.rows && a.cols == b.cols);
            Matrix<T> result = new Matrix<T>(a.rows, a.cols);
            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < a.cols; j++)
                    result.data[i, j] = func(a.data[i, j], b.data[i, j]);
            return result;
        }
        public void Scale(Func<T, T> func)
        {
            for (int i = 0; i < this.rows; i++)
                for (int j = 0; j < this.cols; j++)
                    this.data[i, j] = func(this.data[i, j]);
        }
        #endregion

        public struct Dims
        {
            public int rows, cols;
            public Dims(int rows = 1, int cols = 1) { this.rows = rows; this.cols = cols; }
        }
    }
    class LambdaFunctions
    {
        // Activation function is currently sigmoid
        public static Func<MyDouble, MyDouble> ActivationFunction = x => { return new MyDouble(Math.Tanh(x.val)); };
        public static Func<MyDouble, MyDouble> ActivationFunctionDerivative = x => { return new MyDouble(1 - x.val * x.val); };

        public static Func<MyDouble, MyDouble, MyDouble> Multiply = (x, y) => { return new MyDouble(x * y); };
        public static Func<MyDouble, MyDouble, MyDouble> Devide = (x, y) => { return new MyDouble(x / y); };
        public static Func<MyDouble, MyDouble, MyDouble> Add = (x, y) => { return new MyDouble(x + y); };
        public static Func<MyDouble, MyDouble, MyDouble> Subtract = (x, y) => { return new MyDouble(x - y); };
    }
    class Layer
    {
        private int size, next_size;
        private Matrix<MyDouble> values, weights, bias;

        #region Constructors
        public Layer()
        {
            this.size = this.next_size = 0;
        }
        public Layer(int size, int next_size)
        {
            Resize(size, next_size);
        }
        #endregion
        #region Getters and Setters
        public void Resize(int size, int next_size)
        {
            this.size = size;
            this.next_size = next_size;
            this.values = new Matrix<MyDouble>(size, 1);
            this.weights = new Matrix<MyDouble>(size, next_size);
            this.weights.Randomize();
            this.bias = new Matrix<MyDouble>(next_size, 1);
            this.bias.Randomize();
        }
        public int Size()
        {
            return this.size;
        }
        public void SetValues(Matrix<MyDouble> new_values)
        {
            Debug.Assert(new_values.Dimensions().rows == this.size && new_values.Dimensions().cols == 1);
            this.values = new Matrix<MyDouble>(new_values);
        }
        public Matrix<MyDouble> GetValues()
        {
            return new Matrix<MyDouble>(this.values);
        }
        public void SetWeights(Matrix<MyDouble> new_weights)
        {
            Debug.Assert(new_weights.Dimensions().rows == this.size && new_weights.Dimensions().cols == this.next_size);
            this.weights = new Matrix<MyDouble>(new_weights);
        }
        public Matrix<MyDouble> GetWeights()
        {
            return new Matrix<MyDouble>(this.weights);
        }
        public void SetBias(Matrix<MyDouble> new_bias)
        {
            Debug.Assert(new_bias.Dimensions().rows == this.next_size && new_bias.Dimensions().cols == 1);
            this.bias = new Matrix<MyDouble>(new_bias);
        }
        public Matrix<MyDouble> GetBias()
        {
            return new Matrix<MyDouble>(this.bias);
        }
        #endregion
        #region Funcitons
        public Matrix<MyDouble> Propagate()
        {
            Matrix<MyDouble> weights_t = Matrix<MyDouble>.Transpose(this.weights);
            Matrix<MyDouble> next_values = Matrix<MyDouble>.Product(weights_t, this.values);
            next_values = Matrix<MyDouble>.ElementWise(next_values, this.bias, LambdaFunctions.Add);
            next_values.Scale(LambdaFunctions.ActivationFunction);
            return next_values;
        }
        public void Reset()
        {
            this.values.Fill(new MyDouble());
        }
        #endregion
    }
    class NeuralNetwork
    {
        private int size;
        private Layer[] layers;
        private Matrix<MyDouble> maxInput, maxOutput;

        public NeuralNetwork() { }
        public NeuralNetwork(int size, int[] layout, double[] maxInputData, double[] maxOutputData)
        {

        }

        private void Resize(int size)
        {
            this.size = size;
            this.layers = new Layer[size];
        }
        private void Setlayout(int[] layout)
        {
            for (int i = 0; i < layout.Length; i++)
            {
                int next_size = (i == layout.Length - 1 ? 0 : layout[i + 1]);
                this.layers[i] = new Layer(layout[i], next_size);
            }
        }
        private void SetMaxData(double[] maxInputData, double[] maxOutputData)
        {
            Debug.Assert(this.layers[0].Size() == maxInputData.Length);
            Debug.Assert(this.layers[this.size - 1].Size() == maxOutputData.Length);
            this.maxInput = new Matrix<MyDouble>(maxInputData.Length, 1);
        }
    }
    class Program
    {
        static void Print(Matrix<MyDouble> m)
        {
            for (int i = 0; i < m.Dimensions().rows; i++)
            {
                MyDouble[] row = m.GetRow(i);
                for (int j = 0; j < m.Dimensions().cols; j++)
                    Console.Write($"{row[j].val} ");
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        static void Main(string[] args)
        {
            Matrix<MyDouble> m = new Matrix<MyDouble>(2, 3);
            m.Scale(x => new MyDouble(2 + x.val));
            Print(m);
            m.Scale(x => new MyDouble(3 * x.val));
            Print(m);
            m = Matrix<MyDouble>.Transpose(m);
            Print(m);

            Matrix<MyDouble> a = new Matrix<MyDouble>(2, 3, new MyDouble(2));
            Matrix<MyDouble> b = new Matrix<MyDouble>(3, 3, new MyDouble(3));
            Matrix<MyDouble> p = Matrix<MyDouble>.Product(a, b);
            Print(p);

            Matrix<MyDouble> c = new Matrix<MyDouble>(3, 2, new MyDouble(5));
            Matrix<MyDouble> e = Matrix<MyDouble>.ElementWise(m, c, LambdaFunctions.Multiply);
            Print(m);

            Matrix<MyDouble> r = new Matrix<MyDouble>(2, 2);
            r.Randomize();
            Print(r);
        }
    }
}
